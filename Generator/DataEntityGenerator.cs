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

        private string GetFieldName(string propertyName)
        {
            return "_" + propertyName;
        }

        private string GetFieldName(ColumnDef column)
        {
            return "_" + column.Name;
        }
                
        private string GetPropertyName(string tableName, string columnName)
        {
            if (tableName == columnName)
                return columnName + "1";

            return columnName;
        }
    
        private List<ForeignKey> foreignKeys = new List<ForeignKey>();

        /// <summary>
        /// Gets all foreign keys of database schema
        /// </summary>
        /// <value>List of foreign keys obhect.</value>
        public List<ForeignKey> ForeignKeys { 
            get
            {
                if (this.foreignKeys.Count == 0)
                {
                    CollectForeignKeys();
                }
                return foreignKeys;
            }
        }

        /// <summary>
        /// Collects foreign keys of this schema from database.
        /// </summary>
       public void CollectForeignKeys()
        {
            var db = this.db;
            var foreignKeySql = this.db.GetForeignKeySql();
            if(string.IsNullOrEmpty( foreignKeySql)) return;
            using (var reader = this.db.ExecuteReader(foreignKeySql))
            {
                while (reader.Read())
                {
                    ForeignKey fk = new ForeignKey();

                    fk.TableName = (string)Database.Cast(reader["TableName"], typeof(string));
                    fk.ColumnName = (string)Database.Cast(reader["ColumnName"], typeof(string));
                    fk.ReferencedTableName = (string)Database.Cast(reader["ReferencedTableName"], typeof(string));
                    fk.ReferencedColumnName = (string)Database.Cast(reader["ReferencedColumnName"], typeof(string));
                    
                    this.foreignKeys.Add(fk);
               }
            }
        }

        #region コード生成
        /// <summary>
        /// Generates an entity class file
        /// </summary>
        /// <param name="tableName">specitying target table name</param>
        /// <param name="outDirectory">specifying output directory</param>
        public void Generate(string tableName, string outDirectory)
        {
            string className = this.GetClassName(tableName);
            string filePath = Path.Combine(outDirectory, string.Format("{0}.cs", className));

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
            if(table.Name != className)
            {
                namespaceDef.Types.Add(this.GenerateClass(table, true));
            }

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

        private CodeTypeDeclaration GenerateClass(TableDef table, bool isPrural = false)
        {
            var singular = this.GetClassName(table.Name);
            var cls = isPrural ? 
                 new CodeTypeDeclaration(table.Name) : new CodeTypeDeclaration(singular);

            //クラスの属性作成
            cls.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            if(this.UseWCF)
                cls.CustomAttributes.Add(new CodeAttributeDeclaration("DataContract"));

            var attribute = new CodeAttributeDeclaration("DbTable");
            attribute.Arguments.Add(new CodeAttributeArgument("Name", new CodePrimitiveExpression(table.Name)));
            attribute.Arguments.Add(new CodeAttributeArgument("DatabaseType", new CodeSnippetExpression("DatabaseType." + this.db.ToString())));
            attribute.Arguments.Add(new CodeAttributeArgument("ConnectionSettingKeyName", new CodePrimitiveExpression(this.ConnectionSettingKeyName)));
            cls.CustomAttributes.Add(attribute);

            cls.BaseTypes.Add("DataEntity<" + singular + ">");
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

            // Get Foreign Keys
            var parentForeignKeys = this.ForeignKeys.FindAll(f => f.TableName == table.Name);
            foreach(var parentForeignKey in parentForeignKeys)
            {
                var parentClassName = this.GetClassName(parentForeignKey.ReferencedTableName);
                if(AlreadyAdded(cls.Members, parentClassName))
                { continue;}
                cls.Members.Add(this.GenerateParentField(parentClassName));
                cls.Members.Add(this.GenerateParentProperty(parentClassName));
            }

            var childrenForeignKeys =this.ForeignKeys.FindAll(f => f.ReferencedTableName == table.Name); 
            foreach(var childrenForeignKey in childrenForeignKeys)
            {
                if(AlreadyAdded(cls.Members, childrenForeignKey.TableName))
                { continue;}

                cls.Members.Add(this.GenerateChildrenField(childrenForeignKey.TableName));
                cls.Members.Add(this.GenerateChildrenProperty(childrenForeignKey.TableName));
            }

            return cls;
        }

        private bool AlreadyAdded(CodeTypeMemberCollection collection, string name)
        {
            foreach(CodeTypeMember type in collection)
            {
                if(type.Name == name){
                    return true;
                }
            }
            return false;
        }


        private CodeTypeMember GenerateChildrenField(string childTableName)
        {
            var childClassName = this.GetClassName(childTableName);
             var field = new CodeMemberField();
            if (this.UseWCF)
                field.CustomAttributes.Add(new CodeAttributeDeclaration("DataMember"));
            field.Name = this.GetFieldName(childTableName);
            field.Type = GetGenericListType(childTableName);
            field.Attributes = MemberAttributes.Private;
            
            return field;
        }

        private CodeTypeReference GetGenericListType(string typeName)
        {
            var listType = new CodeTypeReference(typeof(List<>));
            listType.TypeArguments.Add(typeName);
            return listType;
        }

        private CodeTypeMember GenerateChildrenProperty(string childTableName)
        {
            var childClassName = this.GetClassName(childTableName);
             var property = new CodeMemberProperty();
            if (this.UseWCF)
                property.CustomAttributes.Add(new CodeAttributeDeclaration("DataMember"));
            property.Name = childTableName;
            property.Type = GetGenericListType(childClassName);
            property.Attributes = MemberAttributes.Public;
             property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(childTableName))));
            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(childTableName)), new CodePropertySetValueReferenceExpression()));
           
            return property;
        }

        private CodeTypeMember GenerateParentField(string className)
        {
            var field = new CodeMemberField();
            if (this.UseWCF)
                field.CustomAttributes.Add(new CodeAttributeDeclaration("DataMember"));
            field.Name = GetFieldName(className);
            field.Type = new CodeTypeReference(className);
            field.Attributes = MemberAttributes.Private;
            
            return field;
        }
        private CodeTypeMember GenerateParentProperty(string className)
        {
            var property = new CodeMemberProperty();
            if (this.UseWCF)
                property.CustomAttributes.Add(new CodeAttributeDeclaration("DataMember"));
            property.Name = className;
            property.Type = new CodeTypeReference(className);
            property.Attributes = MemberAttributes.Public;
             property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(className))));
            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(className)), new CodePropertySetValueReferenceExpression()));
           
            return property;
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
            property.Name = this.GetPropertyName(table.Name, column.Name);
            property.Type = new CodeTypeReference(this.db.GetProgramType(column));
            property.Attributes = MemberAttributes.Public;
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(column))));
            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(column)), new CodePropertySetValueReferenceExpression()));

            var parentForeignKeys = this.ForeignKeys.FindAll(f => f.TableName == table.Name && f.ColumnName == column.Name);

            foreach(var fk in parentForeignKeys)
            {
                var fkAttribute = new CodeAttributeDeclaration("ForeignKey");
                fkAttribute.Arguments.Add(new CodeAttributeArgument("ReferencedTableName", new CodePrimitiveExpression(fk.ReferencedTableName)));
                fkAttribute.Arguments.Add(new CodeAttributeArgument("ReferencedColumnName", new CodePrimitiveExpression(fk.ReferencedColumnName)));
                property.CustomAttributes.Add(fkAttribute);

            }

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
                assign.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("entity"), this.GetPropertyName(table.Name, column.Name));
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
