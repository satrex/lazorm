using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Lazorm
{   
    /// <summary>
    /// データベースにマッピングされるエンティティクラスのcsファイルを
    /// 自動で生成するクラス。
    /// </summary>
    public partial class DataEntityGenerator
    {
        private Database db = null;
        private string connectionSettingKeyName;
        private string nameSpace;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nameSpace">名前空間を指定します。</param>
        /// <param name="database">データベースを指定します。</param>
        /// <param name="connectionSettingKeyName">接続のキー名を指定します。</param>
        public DataEntityGenerator(
            string nameSpace, 
            Database database,
            string connectionSettingKeyName)
        {
            this.nameSpace = nameSpace;
            this.db = database;
            this.connectionSettingKeyName = connectionSettingKeyName;
            this._tables = this.GetTables();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="nameSpace">生成するエンティティの名前空間を指定します。</param>
        /// <param name="databaseType">接続するデータベースタイプを指定します。</param>
        /// <param name="connectionString">接続文字列を指定します。</param>
        public DataEntityGenerator(
            string nameSpace, 
            DatabaseType databaseType,
            string connectionString)
        {
            this.nameSpace = nameSpace;
            switch(databaseType)
            {
                case DatabaseType.SqlServer:
                this.db = new SqlServer(connectionString);
                break;
                case DatabaseType.Oracle:
                this.db = new Oracle(connectionString);
                break;
                case DatabaseType.MySql:
                this.db = new MySqlDb(connectionString);
                break;

            }
            this.connectionSettingKeyName = this.db.Schema;
            this._tables = this.GetTables();
        }

        /// <summary>
        /// ConnectionSettingのキー名を取得または設定します。
        /// </summary>
        public string ConnectionSettingKeyName
        {
            get { return this.connectionSettingKeyName; }
            set { this.connectionSettingKeyName = value; }
        }

        /// <summary>
        /// 名前空間を取得または設定します。
        /// </summary>
        public string NameSpace
        {
            get { return this.nameSpace; }
            set { this.nameSpace = value; }
        }

        /// <summary>
        /// WCFを使用するかどうかを、取得または設定します。
        /// </summary>
        public bool UseWCF { get; set; }

        /// <summary>
        /// テーブル定義一覧を取得します。
        /// </summary>
        /// <returns>テーブル定義一覧を返却します。</returns>
        public List<TableDef> GetTables()
        {
            List<TableDef> tables = this.db.GetTableDefs();
            return tables;
        }

        private List<TableDef> _tables = null;
        /// <summary>
        /// スキーマのテーブル定義一覧を取得します。
        /// </summary>
        /// <value>テーブル定義一覧</value>
        public List<TableDef> Tables
        {
            get { 
                if(_tables == null){
                    _tables = GetTables();
                }
                return _tables; 
            }
        }
        

        /// <summary>
        /// クラス名＋cs
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string GetFileName(TableDef table)
        {
            return this.GetClassName(table.Name) + ".cs";
        }

        /// <summary>
        /// 最後のsをとりのぞく　iesをyにする
        /// </summary>
        private string GetClassName(string tableName)
        {
            return new Pluralize.NET.Pluralizer().Singularize(tableName);
        }

        private string GetFieldName(ColumnDef column)
        {
            return "_" + column.Name;
        }
                
        private string GetPropertyName(TableDef table, ColumnDef column)
        {
            if (table.Name == column.Name)
                return column.Name + "1";

            return column.Name;
        }

        #region コード生成
        /// <summary>
        /// Generates an entity class file
        /// </summary>
        /// <param name="tableName">specitying target table name</param>
        /// <param name="outDirectory">specifying output directory</param>
        public void Generate(string tableName, string outDirectory)
        {
            string filePath = Path.Combine(outDirectory, string.Format("{0}.cs", tableName));

            // Delete file if exists
            if(File.Exists(filePath)){ File.Delete(filePath);}

            var table = this.Tables.Find(table => table.Name == tableName);
            if(table == null ) throw new NullReferenceException(string.Format("table {0} is null", tableName));

            // Write down namespaces
            var namespaceDef = new CodeNamespace(this.NameSpace);
            namespaceDef.Imports.Add(new CodeNamespaceImport("System"));
            namespaceDef.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            if (this.UseWCF)
                namespaceDef.Imports.Add(new CodeNamespaceImport("System.Runtime.Serialization"));
            namespaceDef.Imports.Add(new CodeNamespaceImport("Lazorm"));
            namespaceDef.Imports.Add(new CodeNamespaceImport("Lazorm.Attributes"));

            // Generate target class
            namespaceDef.Types.Add(this.GenerateClass(table));

            var compileUnit = new CodeCompileUnit();
            compileUnit.Namespaces.Add(namespaceDef);
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            //option.BlankLinesBetweenMembers = false;
            var provider = new CSharpCodeProvider();
            using(StreamWriter writer = new StreamWriter(filePath)){
                provider.GenerateCodeFromCompileUnit(compileUnit, writer, options);
            }

            Console.WriteLine("Entity file generated: {0}", filePath);
        }

        private CodeTypeDeclaration GenerateClass(TableDef table)
        {
            var cls = new CodeTypeDeclaration(this.GetClassName(table.Name));

            //クラスの属性作成
            cls.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            if(this.UseWCF)
                cls.CustomAttributes.Add(new CodeAttributeDeclaration("DataContract"));

            var attribute = new CodeAttributeDeclaration("DbTable");
            attribute.Arguments.Add(new CodeAttributeArgument("Name", new CodePrimitiveExpression(table.Name)));
            attribute.Arguments.Add(new CodeAttributeArgument("DatabaseType", new CodeSnippetExpression("DatabaseType." + this.db.ToString())));
            attribute.Arguments.Add(new CodeAttributeArgument("ConnectionSettingKeyName", new CodePrimitiveExpression(this.ConnectionSettingKeyName)));
            cls.CustomAttributes.Add(attribute);

            cls.BaseTypes.Add("DataEntity<" + this.GetClassName(table.Name) + ">");
            cls.IsPartial = true;
            cls.IsClass = true;
            foreach (ColumnDef column in table.Columns)
            {
                cls.Members.Add(this.GenerateField(column));
                cls.Members.Add(this.GenerateProperty(table, column));
            }

            if(table.Columns.Exists(p => p.IsPrimaryKey))
            {
                cls.Members.Add(this.GenerateGetMethod(table));
            }

            return cls;
        }

        private CodeMemberField GenerateField(ColumnDef column)
        {
            var field = new CodeMemberField();
            field.Name = this.GetFieldName(column);
            field.Type = new CodeTypeReference(this.db.GetProgramType(column));
            field.Attributes = MemberAttributes.Private;
            if(!string.IsNullOrEmpty(column.Remarks))
                field.Comments.Add(new CodeCommentStatement(column.Remarks));
            return field;
        }

        private CodeMemberProperty GenerateProperty(TableDef table, ColumnDef column)
        {
            var attribute = new CodeAttributeDeclaration("DbColumn");
            attribute.Arguments.Add(new CodeAttributeArgument("Name", new CodePrimitiveExpression(column.Name)));
            attribute.Arguments.Add(new CodeAttributeArgument("TypeName", new CodePrimitiveExpression(column.TypeName)));
            if (column.IsPrimaryKey)
                attribute.Arguments.Add(new CodeAttributeArgument("IsPrimaryKey", new CodePrimitiveExpression(true)));
            if (column.Nullable)
                attribute.Arguments.Add(new CodeAttributeArgument("Nullable", new CodePrimitiveExpression(true)));
            if (column.IsAutoNumber)
                attribute.Arguments.Add(new CodeAttributeArgument("IsAutoNumber", new CodePrimitiveExpression(true)));
            if (column.DecimalPlace != 0)
                attribute.Arguments.Add(new CodeAttributeArgument("DecimalPlace", new CodePrimitiveExpression(column.DecimalPlace)));
            attribute.Arguments.Add(new CodeAttributeArgument("Length", new CodePrimitiveExpression(column.Length)));
            if(!string.IsNullOrEmpty(column.Remarks))
                attribute.Arguments.Add(new CodeAttributeArgument("Remarks", new CodePrimitiveExpression(column.Remarks)));
            
            var property = new CodeMemberProperty();
            if (this.UseWCF)
                property.CustomAttributes.Add(new CodeAttributeDeclaration("DataMember"));
            property.CustomAttributes.Add(attribute);
            property.Name = this.GetPropertyName(table, column);
            property.Type = new CodeTypeReference(this.db.GetProgramType(column));
            property.Attributes = MemberAttributes.Public;
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(column))));
            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(column)), new CodePropertySetValueReferenceExpression()));
            return property;
        }

        private CodeMemberMethod GenerateGetMethod(TableDef table)
        {
            var method = new CodeMemberMethod();
            method.Name = "Get";
            method.ReturnType = new CodeTypeReference(this.GetClassName(table.Name));
            method.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            foreach (var column in table.Columns)
            {
                if (!column.IsPrimaryKey)
                    continue;
                method.Parameters.Add(new CodeParameterDeclarationExpression(this.db.GetProgramType(column), this.GetFieldName(column)));
            }

            //ClassName entity = new ClassName()　みたいなコードをつくる
            method.Statements.Add(new CodeVariableDeclarationStatement(
                new CodeTypeReference(this.GetClassName(table.Name)), "entity", new CodeObjectCreateExpression(new CodeTypeReference(this.GetClassName(table.Name)))));

            //entity.Id = _Id みたいなコードを主キーすべてに対してつくる
            foreach (var column in table.Columns)
            {
                if (!column.IsPrimaryKey)
                    continue;
                CodeAssignStatement assign = new CodeAssignStatement();
                assign.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("entity"), this.GetPropertyName(table, column));
                assign.Right = new CodeVariableReferenceExpression(this.GetFieldName(column));
                method.Statements.Add(assign);
            }

            //if(!entity.Fill())
            //  return null; って感じのものをつくる
            var bunki = new CodeConditionStatement();
            bunki.Condition = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("!entity"), "Fill");
            bunki.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(null)));
            method.Statements.Add(bunki);

            //return entity;
            method.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("entity")));

            return method;
        }
        #endregion

        #region 旧コード生成コード
        /*
        /// <summary>
        /// ゴリゴリ書いた奴
        /// CodoDom使った記法に書き換えたので未使用
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string Generate(TableDef table)
        {
            string file = @"using System;
using System.Collections.Generic;
using Lazorm;
using Lazorm.Attributes;

namespace " + this.NameSpace + @"
{
    " + this.GenerateClass(table) + @"
}";
            return file;
        }

        private string GenerateClass(TableDef table)
        {
            string classString = @"[DbTable(DatabaseType = DatabaseType." + this.db.ToString() + @", ConnectionSettingKeyName = """ + this.ConnectionSettingKeyName + @""", Name = """ + table.Name + @""")]
    public partial class " + this.GetClassName(table) + @" : DataEntity<" + this.GetClassName(table) + @">
    {
        " + this.GenerateFields(table) + @"
        
        " + this.GenerateProperties(table) + @"
        " + this.GenerateMethods(table) + @"
    }
    ";
            return classString;
        }

        private string GenerateMethods(TableDef table)
        {
            if (!table.HasPrimaryKey())
                return string.Empty;

            string methods = @"//Methods

        /// <summary>
        /// テーブルの主キーを指定して単一のEntityを返す。
        /// 存在しない場合はnullを返す。
        /// </summary>
        public static " + this.GetClassName(table) + @" Get(" + this.GetParams(table) + @")
        {
            " + this.GetClassName(table) + @" entity = new " + this.GetClassName(table) + @"();";
 
            foreach(ColumnDef column in table.Columns)
            {
                if (column.IsPrimaryKey)
                    methods += @"
            entity." + this.GetPropertyName(table, column) + @" = " + this.GetFieldName(column) + @";";
            }

            methods += @"
            if (!entity.Exists())
                return null;
            entity.Fill();
            return entity;
        }
        ";

            return methods;
        }

        private string GetParams(TableDef table)
        {
            string parameters = string.Empty;
            foreach (ColumnDef column in table.Columns)
            {
                if(column.IsPrimaryKey)
                    parameters += this.db.GetProgramTypeName(column) + " " + this.GetFieldName(column) + ", ";
            }

            if (parameters == string.Empty)
                return string.Empty;

            parameters = parameters.Substring(0, parameters.Length - 2);

            return parameters;
        }

        private string GenerateFields(TableDef table)
        {
            string f = @"//Fields";
            foreach (ColumnDef column in table.Columns)
            {
                f += @"
        private " + this.db.GetProgramTypeName(column) + " " + this.GetFieldName(column) + @";";
            }

            return f;
        }

        private string GenerateProperties(TableDef table)
        {
            string p = @"//Properties";
            foreach (ColumnDef column in table.Columns)
            {
                p += @"
        [DbColumn(";
                p += @"Name = """ + column.Name + @""", TypeName = """ + column.TypeName + @""", ";
                if (column.IsPrimaryKey)
                    p += "IsPrimaryKey = true, ";
                if(column.Nullable)
                    p += "Nullable = true, ";
                if (column.IsAutoNumber)
                    p += "IsAutoNumber = true, ";
                p = p.Substring(0, p.Length - 2);
                p += ")]";
                p += @"
        public " + this.db.GetProgramTypeName(column) + @" " + this.GetPropertyName(table, column) + @"
        {
            get{ return this." + this.GetFieldName(column) + @"; }
            set{ this." + this.GetFieldName(column) + @" = value; }
        }
        ";
            }

            return p;
        }
*/
        #endregion
    }
}
