using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Threading.Tasks;

namespace Lazorm
{   
    /// <summary>
    /// データベースにマッピングされるエンティティクラスのcsファイルを
    /// 自動で生成するクラス。
    /// </summary>
    public partial class DataValidationGenerator
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
        public DataValidationGenerator(
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
        public DataValidationGenerator(
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

        private string GetClassName(string tableName)
        {
            return new Pluralize.NET.Pluralizer().Singularize(tableName) + "Validation";
        }

        /// <summary>
        /// 最後のsをとりのぞく　iesをyにする
        /// </summary>
        private string GetModelClassName(string tableName)
        {
            return new Pluralize.NET.Pluralizer().Singularize(tableName) ;
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
    
        #region コード生成
        /// <summary>
        /// Generates an entity class file
        /// </summary>
        /// <param name="tableName">specitying target table name</param>
        /// <param name="outDirectory">specifying output directory</param>
        public void Generate(string tableName, string outDirectory)
        {
            string modelClassName = this.GetModelClassName(tableName);
            string className = this.GetClassName(tableName);
            string filePath = Path.Combine(outDirectory, string.Format("{0}.cs", className));

            // Delete file if exists
            if(File.Exists(filePath)){ File.Delete(filePath);}

            var table = this.Tables.Find(table => table.Name == tableName);
            if(table == null ) throw new NullReferenceException(string.Format("Couldn't find table {0} on database", tableName));

            // Write down namespaces
            var namespaceDef = new CodeNamespace(this.NameSpace);
            namespaceDef.Imports.Add(new CodeNamespaceImport("System"));
            namespaceDef.Imports.Add(new CodeNamespaceImport("System.Linq"));
            // namespaceDef.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            if (this.UseWCF)
                namespaceDef.Imports.Add(new CodeNamespaceImport("System.Runtime.Serialization"));
            // namespaceDef.Imports.Add(new CodeNamespaceImport("Lazorm"));
            namespaceDef.Imports.Add(new CodeNamespaceImport("Lazorm.Attributes"));
            namespaceDef.Imports.Add(new CodeNamespaceImport("System.ComponentModel.DataAnnotations"));

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
            var singular = this.GetClassName(table.Name);
            var cls = new CodeTypeDeclaration(singular);

            //クラスの属性作成
            cls.CustomAttributes.Add(new CodeAttributeDeclaration("Serializable"));
            if(this.UseWCF)
                cls.CustomAttributes.Add(new CodeAttributeDeclaration("DataContract"));

            cls.IsPartial = true;
            cls.IsClass = true;

            foreach (ColumnDef column in table.Columns)
            {
                cls.Members.Add(this.GenerateField(column));
                cls.Members.Add(this.GenerateProperty(table, column));
            }
            cls.Members.Add( GenerateDefaultConstructor());
            cls.Members.Add( GenerateConstructorByEntity(table));
            cls.Members.Add( GenerateToEntityMethod(table));
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

        private CodeTypeReference GetGenericListType(string typeName)
        {
            var listType = new CodeTypeReference(typeof(List<>));
            listType.TypeArguments.Add(typeName);
            return listType;
        }


        private CodeMemberField GenerateField(ColumnDef column)
        {
            var field = new CodeMemberField();
            field.Name = this.GetFieldName(column);
            field.Type = new CodeTypeReference(this.db.GetProgramType(column));
            field.Attributes = MemberAttributes.Private;
            field.InitExpression = new CodeDefaultValueExpression(field.Type);
            if(!string.IsNullOrEmpty(column.Remarks))
                field.Comments.Add(new CodeCommentStatement(column.Remarks));
            return field;
        }

        private CodeMemberProperty GenerateProperty(TableDef table, ColumnDef column)
        {
            var property = new CodeMemberProperty();
           if (!column.Nullable)
            {
                var requiredAttribute = new CodeAttributeDeclaration("Required");
                requiredAttribute.Arguments.Add(
                    new CodeAttributeArgument(
                     "ErrorMessage", new CodePrimitiveExpression($"{column.Remarks}は必須項目です。")
                       )); 
                property.CustomAttributes.Add(requiredAttribute);
            }

            property.Name = this.GetPropertyName(table.Name, column.Name);
            property.Type = new CodeTypeReference(this.db.GetProgramType(column));
            property.Attributes = MemberAttributes.Public;
            property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(column))));
            property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), this.GetFieldName(column)), new CodePropertySetValueReferenceExpression()));

            return property;
        }

        private CodeMemberMethod GenerateToEntityMethod(TableDef table)
        {
            var className = this.GetModelClassName(table.Name);
            var method = new CodeMemberMethod();
            method.Name = $"To{className}";

            method.ReturnType =  new CodeTypeReference(className);
            method.Attributes = MemberAttributes.Public;

            //ClassName entity = new ClassName()　みたいなコードをつくる
            method.Statements.Add(new CodeVariableDeclarationStatement(
                new CodeTypeReference(this.GetModelClassName(table.Name)), "entity", new CodeObjectCreateExpression(new CodeTypeReference(this.GetModelClassName(table.Name)))));

            //entity.Id = this.Id みたいなコードをすべてに対してつくる
            foreach (var column in table.Columns)
            {
                CodeAssignStatement assign = new CodeAssignStatement();
                assign.Left = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("entity"), this.GetPropertyName(table.Name, column.Name));
                assign.Right = new CodeVariableReferenceExpression(this.GetFieldName(column));
                method.Statements.Add(assign);
            }

            //return entity;
            method.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("entity")));

            return method;
        }

        private CodeMemberMethod GenerateDefaultConstructor()
        {
            var defaultCtor = new CodeConstructor();
            defaultCtor.Attributes = MemberAttributes.Public;
            return defaultCtor;
        }

        private CodeMemberMethod GenerateConstructorByEntity(TableDef table)
        {
            string modelClassName = this.GetModelClassName(table.Name);
            
            var ctorFromEntity = new CodeConstructor();

            ctorFromEntity.Attributes = MemberAttributes.Public;
            var constArg = new CodeParameterDeclarationExpression(new CodeTypeReference(modelClassName), "entity"); 
            ctorFromEntity.Parameters.Add(constArg );

            //entity.Id = _Id みたいなコードを主キーすべてに対してつくる
            foreach (var column in table.Columns)
            {
                CodeAssignStatement assign = new CodeAssignStatement();
                assign.Left = new CodeVariableReferenceExpression(this.GetFieldName(column));
                assign.Right = new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("entity"), this.GetPropertyName(table.Name, column.Name));

                ctorFromEntity.Statements.Add(assign);
            }

            return ctorFromEntity;
        }

        #endregion

    }
}
