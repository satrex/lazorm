using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Lazorm.Attributes;
using System.Text.RegularExpressions;

namespace Lazorm
{
    /// <summary>
    /// SQL Serverのデータベースを表すクラス。
    /// </summary>
    public class SqlServer : Database
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列を指定します。</param>
        public SqlServer(string connectionString)
            : base(connectionString)
        {
        }
        private string _schema;
        /// <summary>
        /// データベースのスキーマ名を取得します。
        /// </summary>
        /// <value>データベースのスキーマ名</value>
        public override string Schema
        {
            get {
                if(_schema == null){
                    System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(this.ConnectionString);
                    _schema = builder.DataSource;
                }
                 return _schema; }
            set {
                _schema = value;
            }
        }
        internal override DatabaseType Type
        {
            get { return DatabaseType.SqlServer; }
        }

        /// <summary>
        /// データベースへの接続を生成します。
        /// </summary>
        /// <returns>生成したコネクションを返却します。</returns>
        public override IDbConnection CreateConnection()
        {
            return new SqlConnection(this.ConnectionString);
        }

        private string dbVersion;
        /// <summary>
        /// データベースのバージョン番号を取得します。
        /// </summary>
        /// <value>データベースのバージョン番号</value>
        public string DbVersion
        {
            get
            {
                if (string.IsNullOrEmpty(dbVersion))
                {
                    dbVersion = this.GetDbVersion();
                }
                return dbVersion;
            }
        }

        private string GetDbVersion()
        {
            var sql = @"SELECT SERVERPROPERTY('productversion')";
            return this.ExecuteScalar(sql).ToString();
        }

        internal override string GetTableListSql()
        {
            // TODO: Logic to get remarks
            return @"SELECT name as TableName ,
            '' as Remarks
            FROM sysobjects WHERE xtype = 'U' AND name <> 'dtproperties'";
        }

        internal override string GetColumnListSql(string tableName)
        {
            var oldSql =
            #region before 2005
 @"SELECT 
    c.name AS ColumnName,
    TYPE_NAME(c.xusertype) AS TypeName,
    CASE WHEN ik.colid IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey,
    CASE WHEN c.scale IS NOT NULL THEN c.scale ELSE 0 END AS DecimalPlace,
    c.isnullable AS Nullable,
    CASE WHEN c.autoval IS NOT NULL THEN 1 ELSE 0 END AS IsAutoNumber,
    c.length AS Length,
    p.value AS Remarks
FROM 	sysobjects o
JOIN 	syscolumns c ON c.id=o.id
LEFT JOIN sysobjects pk ON 
    pk.parent_obj=o.id and 
    pk.xtype='PK'
LEFT JOIN sysindexes ix ON 
    ix.name=pk.name
LEFT JOIN sysindexkeys ik ON 
    ik.id=o.id and ik.indid=ix.indid and 
    ik.colid=c.colid
LEFT JOIN sysproperties p ON
	c.id = p.id AND
	c.colid = p.smallid AND
	p.name = 'MS_Description'
WHERE 
    o.type='U' AND 
    o.name = '" + tableName + @"'
ORDER BY c.colorder";
            #endregion
            var newSql =
            #region 2005 or newer
 @$"
SELECT 
    c.name AS ColumnName,
    TYPE_NAME(c.user_type_id) AS TypeName,
    CASE WHEN ik.column_id  IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey,
    CASE WHEN c.scale IS NOT NULL THEN c.scale ELSE 0 END AS DecimalPlace,
    c.is_nullable  AS Nullable,
    c.is_identity  AS IsAutoNumber,
    c.max_length  AS Length,
    p.value AS Remarks
FROM 	sys.objects o
JOIN 	sys.columns c ON c.object_id=o.object_id
LEFT JOIN sys.objects pk ON 
    pk.parent_object_id = o.object_id and 
    pk.[type] ='PK'
LEFT JOIN sys.indexes ix ON 
    ix.name=pk.name
LEFT JOIN sys.index_columns ik ON 
    ik.object_id=o.object_id and ik.index_id =ix.index_id  and 
    ik.column_id =c.column_id 
LEFT JOIN sys.extended_properties p ON
	c.object_id  = p.major_id AND
	c.column_id  = p.minor_id AND
	p.name = 'MS_Description'
WHERE 
    o.type='U' AND 
    o.name = '{tableName}'
ORDER BY c.key_ordinal 
";
            #endregion

            int majorVersion = int.Parse(this.DbVersion.Split(new string[]{"."}, StringSplitOptions.RemoveEmptyEntries)[0]);
            if (majorVersion < 10){
                return oldSql;
            }
            else {
                return newSql;
            }

        }

        public override Type GetProgramType(ColumnDef column)
        {
            switch (column.TypeName)
            {
                case "bit":
                    return column.Nullable ? typeof(Nullable<bool>) : typeof(bool);
                case "numeric":
                case "decimal":
                case "money":
                    return column.Nullable ? typeof(Nullable<decimal>) : typeof(decimal);
                case "float":
                    return column.Nullable ? typeof(Nullable<double>) : typeof(double);
                case "smalldatetime":
                case "datetime":
                case "date":
                    return column.Nullable ? typeof(Nullable<DateTime>) : typeof(DateTime);
                case "int":
                case "tinyint":
                case "smallint":
                    return column.Nullable || column.IsAutoNumber ? typeof(Nullable<int>) : typeof(int);
                case "bigint":
                    return  column.Nullable ? typeof(Nullable<Int64>) : typeof(Int64);
                case "char":
                case "nvarchar":
                case "ntext":
                case "text":
                case "varchar":
                    return typeof(string);
                case "varbinary":
                case "image":
                    return typeof(byte[]);
                default:
                    throw new Exception("対応外の型を使用しています。" + column.TypeName);
            }
        }

        /// <summary>
        /// SQL Serverのカラムに使用するデータ型を取得します。
        /// </summary>
        /// <param name="typeName">SQL Serverで使用する、データ型を文字列で指定します。</param>
        /// <returns>OracleTypeクラスの型を返却します。</returns>
        public SqlDbType GetSqlDbType(string typeName)
        {
            switch (typeName)
            {
                case "bit":
                    return SqlDbType.Bit;
                case "numeric":
                case "decimal":
                    return SqlDbType.Decimal;
                case "money":
                    return SqlDbType.Money;
                case "smalldatetime":
                case "datetime":
                case "date":
                    return SqlDbType.DateTime;
                case "int":
                    return SqlDbType.Int;
                case "tinyint":
                    return SqlDbType.TinyInt;
                case "smallint":
                    return SqlDbType.SmallInt;
                case "bigint":
                    return SqlDbType.BigInt;
                case "float":
                    return SqlDbType.Float;
                case "char":
                    return SqlDbType.Char;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "text":
                    return SqlDbType.Text;
                case "ntext":
                    return SqlDbType.NText;
                case "varchar":
                    return SqlDbType.VarChar;
                case "varbinary":
                    return SqlDbType.VarBinary;
                case "image":
                    return SqlDbType.Image;
                default:
                    throw new Exception("対応外の型を使用しています。" + typeName);
            }
        }

        /// <summary>
        /// このクラスを表す文字列を取得します。
        /// </summary>
        /// <returns>SqlServerを返却します。</returns>
        public override string ToString()
        {
            return "SqlServer";
        }

        internal override IDbDataParameter CreateDataParameter(string parameterName, object value, string typeName)
        {
            SqlParameter parameter = new SqlParameter(parameterName, this.GetSqlDbType(typeName));
            if (value == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value;
            return parameter;
        }

        internal override string GetParameterName(string columnName)
        {
            return "@" + columnName;
        }

        /// <summary>
        /// データアダプターを生成します。
        /// </summary>
        /// <param name="sql">SQLを指定します。</param>
        /// <returns>生成したデータアダプターを返却します。</returns>
        public override IDbDataAdapter CreateDataAdapter(string sql)
        {
            return new SqlDataAdapter(sql, ConnectionString);
        }

        internal override string AutoNumberGetSql { get {return "; SELECT SCOPE_IDENTITY();";} }
   }
}
