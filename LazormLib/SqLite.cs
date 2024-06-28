using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lazorm.Attributes;
using Microsoft.Data;

using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Lazorm
{
    /// <summary>
    /// SQL Serverのデータベースを表すクラス。
    /// </summary>
    public class SqLiteDb: Database
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">接続文字列を指定します。</param>
        public SqLiteDb(string connectionString)
            : base(connectionString)
        {
        }

        private string _filePath;

        private string _schema = "main";
        /// <summary>
        /// データベーススキーマ名を取得します。
        /// </summary>
        /// <value>データベーススキーマ名</value>
        public override string Schema
        {
            get {
                if (_filePath == null)
                {
                    var dbFind = new Regex("DataSource *= *(?<schema>.+?) *;", RegexOptions.IgnoreCase);
                    var match = dbFind.Match(this.ConnectionString);
                    _filePath = match.Groups["schema"].Value;
                    if(System.IO.File.Exists(_filePath))
                    {
                        Trace.WriteLine($"SQLite database file not exists: {_filePath}");
                        Trace.WriteLine("Provider will create a new database file.");
                    }
                }
                return _schema; }
            set {
                _schema = value;
            }
        }
        

        internal override DatabaseType Type
        {
            get { return DatabaseType.SqLite; }
        }

        Dictionary<string, IDbConnection> connectionPool = new Dictionary<string, IDbConnection>();
        Dictionary<string, IDbConnection> ConnectionPool
        {
            get { return connectionPool; }
        }


        /// <summary>
        /// データベースへの接続を生成します。
        /// </summary>
        /// <returns>生成したコネクションを返却します。</returns>
        public override IDbConnection CreateConnection()
        {
            return new Microsoft.Data.Sqlite.SqliteConnection (this.ConnectionString);
        } 

        public override string GetForeignKeySql()
        {
            return string.Empty;
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
            return string.Empty;
        }

        internal override string GetTableListSql()
        {
            return string.Format(@"SELECT 
    name as TableName, '' as Remarks 
FROM 
    sqlite_schema
WHERE 
    type ='table' AND 
    name NOT LIKE 'sqlite_%';");
        }

        internal override string GetColumnListSql(string tableName)
        {
            var sql = string.Format($@"WITH RECURSIVE
  a AS (
    SELECT name, lower(REPLACE (replace(replace(sql, char(13), ' '), char(10), ' '), char(9), ' ')) AS sql
    FROM sqlite_master
    WHERE lower(sql) LIKE '%integer% autoincrement%'
  ),
  b AS (
    SELECT name, trim(substr(sql, instr(sql, '(') + 1)) AS sql
    FROM a
  ),
  c AS (
    SELECT b.name, sql, '' AS col
    FROM b
    UNION ALL
    SELECT 
      c.name, 
      trim(substr(c.sql, ifnull(nullif(instr(c.sql, ','), 0), instr(c.sql, ')')) + 1)) AS sql, 
      trim(substr(c.sql, 1, ifnull(nullif(instr(c.sql, ','), 0), instr(c.sql, ')')) - 1)) AS col
    FROM c JOIN b ON c.name = b.name
    WHERE c.sql != ''
  ),
  d AS (
    SELECT name, substr(col, 1, instr(col, ' ') - 1) AS col
    FROM c
    WHERE col LIKE '%autoincrement%'
  )
SELECT 
  name as ColumnName,
  type as TypeName,
  0 as Length,
  pk as IsPrimaryKey,
  (SELECT COUNT(*)
	FROM d
	WHERE d.name = '{tableName}' and d.col = t.name) as IsAutoNumber,
  (CASE [notnull] WHEN 0 THEN 1 WHEN 1 THEN 0 END) as Nullable,
  0 as DecimalPlace,
  '' as Remarks 
              FROM pragma_table_info('{tableName}') t");

            return sql;
           }

        public override Type GetProgramType(ColumnDef column)
        {
            var typenameWithoutLength = Regex.Replace(column.TypeName, "\\(.*\\)", string.Empty);
            //Console.WriteLine($"replacing : " + typenameWithoutLength);
            var date = "date";
            switch (typenameWithoutLength.ToUpper())
            {
                case "INTEGER":
                case "INT":
                    return  column.Nullable || column.IsAutoNumber ? typeof(Nullable<Int64>) : typeof(Int64);
                case "NUMERIC":
                case "REAL":
                    return column.Nullable ? typeof(Nullable<double>) : typeof(double);
                case "TEXT":
                    return column.Name.EndsWith(date, StringComparison.CurrentCultureIgnoreCase)? typeof(DateTime) : typeof(string);
                case "BLOB":
                    return typeof(byte[]);
                default:
                    throw new Exception("対応外の型を使用しています。" + column.TypeName);
            }
        }



        /// <summary>
        /// SQL Serverのカラムに使用するデータ型を取得します。
        /// </summary>
        /// <param name="typeName">SQL Serverで使用する、データ型を文字列で指定します。</param>
        /// <returns>SqliteTypeクラスの型を返却します。</returns>
        public Microsoft.Data.Sqlite.SqliteType GetSqlDbType(string typeName)
        {
            switch (typeName)
            {
                case "INTEGER":
                case "INT":
                    return Microsoft.Data.Sqlite.SqliteType.Integer;
                case "NUMERIC":
                case "REAL":
                    return Microsoft.Data.Sqlite.SqliteType.Real;
                case "TEXT":
                    return Microsoft.Data.Sqlite.SqliteType.Text;
               case "BLOB":
                    return Microsoft.Data.Sqlite.SqliteType.Blob;
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
            return "SqLite";
        }

        internal override IDbDataParameter CreateDataParameter(string parameterName, object value, string typeName)
        {
            Microsoft.Data.Sqlite.SqliteParameter parameter = new Microsoft.Data.Sqlite.SqliteParameter(parameterName, this.GetSqlDbType(typeName));
            if (value == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value;
            return parameter;
        }

        internal override string GetParameterName(string columnName)
        {
            return "$" + columnName;
        }

        /// <summary>
        /// データアダプターを生成します。
        /// </summary>
        /// <param name="sql">SQLを指定します。</param>
        /// <returns>生成したデータアダプターを返却します。</returns>
        public override IDbDataAdapter CreateDataAdapter(string sql)
        {
            throw new NotImplementedException("SqLite provider does not support DataAdapter neither DataTable.");
        }

        internal override string AutoNumberGetSql { get {return "; SELECT last_insert_rowid();"; } }
    }
}
