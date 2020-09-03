using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Lazorm.Attributes;
using System.Text.RegularExpressions;

namespace Lazorm
{
    /// <summary>
    /// SQL Serverのデータベースを表すクラス。
    /// </summary>
    public class MySqlDb: Database
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列を指定します。</param>
        public MySqlDb(string connectionString)
            : base(connectionString)
        {
        }

        private string _schema;
        /// <summary>
        /// データベーススキーマ名を取得します。
        /// </summary>
        /// <value>データベーススキーマ名</value>
        public override string Schema
        {
            get {
                if(_schema == null){
                    var dbFind = new Regex("Database=(?<schema>.+?);", RegexOptions.IgnoreCase);
                    var match = dbFind.Match(this.ConnectionString);
                    _schema = match.Groups["schema"].Value;
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
            return new MySqlConnection(this.ConnectionString);
        }

        private string dbVersion;
        /// <summary>
        /// データベースのバージョンを取得します。
        /// </summary>
        /// <value>データベースのバージョン</value>
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
            var sql = @"select version();";
            return this.ExecuteScalar(sql).ToString();
        }

        internal override string GetTableListSql()
        {
            return string.Format(@"SELECT TABLE_NAME as TableName	
FROM `INFORMATION_SCHEMA`.`TABLES` 
WHERE `TABLE_SCHEMA`='{0}' ", this.Schema);
        }

        internal override string GetColumnListSql(string tableName)
        {
            var sql = string.Format(@"SELECT 
    COLUMN_NAME AS ColumnName,
    DATA_TYPE AS TypeName,
    CASE WHEN COLUMN_KEY IS NOT NULL THEN 1 ELSE 0 END AS IsPrimaryKey,
    CASE WHEN NUMERIC_SCALE IS NOT NULL THEN NUMERIC_SCALE ELSE 0 END AS DecimalPlace,
    CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END AS Nullable,
    CASE WHEN EXTRA = 'auto_increment' THEN 1 ELSE 0 END AS IsAutoNumber,
    LEAST(IFNULL(CHARACTER_MAXIMUM_LENGTH, 0), 	2147483647) AS Length,
    '' AS Remarks
FROM `INFORMATION_SCHEMA`.`COLUMNS` 
WHERE `TABLE_SCHEMA`='satrex_yingyang' 
    AND `TABLE_NAME`='{0}';", tableName);
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
 string.Format(@"
SELECT 
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
LEFT JOIN sys.extended_properties p ON
	c.id = p.major_id AND
	c.colid = p.minor_id AND
	p.name = 'MS_Description'
WHERE 
    o.type='U' AND 
    o.name = '{0}'
ORDER BY c.colorder
", tableName);
            #endregion

            return sql;
           }

        internal override Type GetProgramType(ColumnDef column)
        {
            switch (column.TypeName)
            {
                case "bit":
                    return column.Nullable ? typeof(Nullable<bool>) : typeof(bool);
                case "decimal":
                case "numeric":
                    return column.Nullable ? typeof(Nullable<decimal>) : typeof(decimal);
                case "float":
                case "double":
                    return column.Nullable ? typeof(Nullable<double>) : typeof(double);
                case "timestamp":
                case "datetime":
                case "date":
                    return column.Nullable ? typeof(Nullable<DateTime>) : typeof(DateTime);
                case "integer":
                case "int":
                case "smallint":
                case "tinyint":
                case "mediumint":
                    return column.Nullable ? typeof(Nullable<int>) : typeof(int);
                case "bigint":
                    return  column.Nullable ? typeof(Nullable<Int64>) : typeof(Int64);
                case "char":
                case "varchar":
                case "text":
                case "longtext":
                case "enum":
                case "set":
                    return typeof(string);
                case "binary":
                case "varbinary":
                case "blob":
                case "longblob":
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
        public MySqlDbType GetSqlDbType(string typeName)
        {
            switch (typeName)
            {
                case "bit":
                    return MySqlDbType.Bit;
                case "numeric":
                case "decimal":
                    return MySqlDbType.Decimal;
                case "timestamp":
                    return MySqlDbType.Timestamp;
                case "datetime":
                case "date":
                    return MySqlDbType.DateTime;
                case "time":
                    return MySqlDbType.Time;
                case "int":
                    return MySqlDbType.Int32;
                case "tinyint":
                    return MySqlDbType.Int16;
                case "smallint":
                    return MySqlDbType.Int24;
                case "bigint":
                    return MySqlDbType.Int64;
                case "float":
                    return MySqlDbType.Float;
                case "double":
                    return MySqlDbType.Double;
                case "char":
                case "varchar":
                    return MySqlDbType.VarChar;
                case "text":
                case "enum":
                case "set":
                    return MySqlDbType.Text;
                case "longtext":
                    return MySqlDbType.LongText;
                case "binary":
                case "varbinary":
                    return MySqlDbType.VarBinary;
                case "blob":
                    return MySqlDbType.Blob;
                case "longblob":
                    return MySqlDbType.LongBlob;
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
            return "MySql";
        }

        internal override IDbDataParameter CreateDataParameter(string parameterName, object value, string typeName)
        {
            MySqlParameter parameter = new MySqlParameter(parameterName, this.GetSqlDbType(typeName));
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
            return new MySqlDataAdapter(sql, ConnectionString);
        }

        internal override string AutoNumberGetSql { get {return "; SELECT LAST_INSERT_ID();";} }
    }
}
