using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using System.Data;
using System.Text.RegularExpressions;

namespace Lazorm
{
    /// <summary>
    /// Oracleのデータベースを表すクラスです。
    /// </summary>
    public class Oracle : Database
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionstring">接続文字列を指定します。</param>
        public Oracle(string connectionstring)
            : base(connectionstring)
        {
        }

        internal override DatabaseType Type
        {
            get { return DatabaseType.Oracle; }
        }

        private string _schema;
        /// <summary>
        /// スキーマのテーブル定義一覧を取得します。
        /// </summary>
        /// <value>テーブル定義一覧</value>
        public override string Schema
        {
            get {
                if(_schema == null){
                    var dbFind = new Regex("Data Source[%s*]=[%s*](?<schema>.+?)[%s*];", RegexOptions.IgnoreCase);
                    var match = dbFind.Match(this.ConnectionString);
                    _schema = match.Groups["schema"].Value;
                }
                 return _schema; }
            set {
                _schema = value;
            }
        }

        /// <summary>
        /// データベースへのコネクションを生成します。
        /// </summary>
        /// <returns>データベースへのコネクションを返却します。</returns>
        public override System.Data.IDbConnection CreateConnection()
        {
            return new OracleConnection(this.ConnectionString);
        }

        internal override string GetTableListSql()
        {
            return "select table_name as TableName from user_tables";
        }

        internal override string GetColumnListSql(string tableName)
        {
            return @"
SELECT 
    C.COLUMN_NAME AS ColumnName, 
    C.DATA_TYPE AS TypeName, 
    DECODE(C.DATA_SCALE, NULL, 0, C.DATA_SCALE) AS DecimalPlace, 
    DECODE(C.NULLABLE, 'Y', 1, 0) AS Nullable, 
    DECODE(D.COLUMN_NAME, NULL, 0, 1) AS IsPrimaryKey, 
    0 AS IsAutoNumber, 
    C.DATA_LENGTH As Length,
    E.COMMENTS As Remarks
FROM USER_TAB_COLUMNS C, 
(
	SELECT B.COLUMN_NAME 
	FROM USER_CONSTRAINTS A, USER_CONS_COLUMNS B
	WHERE 
		A.TABLE_NAME = B.TABLE_NAME AND
		A.CONSTRAINT_NAME = B.CONSTRAINT_NAME AND
		A.TABLE_NAME = '" + tableName + @"' AND
		A.CONSTRAINT_TYPE = 'P'
) D, 
(
	SELECT COLUMN_NAME, COMMENTS FROM USER_COL_COMMENTS WHERE TABLE_NAME = '" + tableName + @"'
) E
WHERE C.TABLE_NAME = '" + tableName + @"' AND C.COLUMN_NAME = D.COLUMN_NAME(+) AND C.COLUMN_NAME = E.COLUMN_NAME(+)
ORDER BY C.COLUMN_ID
";
        }

        internal override Type GetProgramType(ColumnDef column)
        {
            switch (column.TypeName)
            {
                case "DATE":
                case "TIMESTAMP(6)":
                    return column.Nullable ? typeof(Nullable<DateTime>) : typeof(DateTime);
                case "NUMBER":
                    if (!string.IsNullOrEmpty(column.Remarks) && 
                        column.Remarks.Length >= 4 && 
                        column.Remarks.Substring(0, 4) == "bool")
                    {
                        if (column.Nullable)
                            return typeof(Nullable<bool>);
                        return typeof(bool);
                    }

                    if (column.DecimalPlace == 0)
                        return column.Nullable ? typeof(Nullable<int>) : typeof(int);
                    else
                        return column.Nullable ? typeof(Nullable<decimal>) : typeof(decimal);
                case "CHAR":
                case "NVARCHAR2":
                case "VARCHAR2":
                    return typeof(string);
                case "LONG RAW":
                    return typeof(byte[]);
                default:
                    throw new Exception("未対応の型を使用しています。" + column.TypeName);
            }
        }

        /// <summary>
        /// Oracleのカラムに使用するデータ型を取得します。
        /// </summary>
        /// <param name="typeName">Oracleで使用する、データ型を文字列で指定します。</param>
        /// <returns>OracleTypeクラスの型を返却します。</returns>
        public OracleType GetOracleType(string typeName)
        {
            switch (typeName)
            {
                case "NUMBER":
                    return OracleType.Number;
                case "CHAR":
                    return OracleType.Char;
                case "NVARCHAR2":
                    return OracleType.NVarChar;
                case "VARCHAR2":
                    return OracleType.VarChar;
                case "DATE":
                    return OracleType.DateTime;
                case "TIMESTAMP(6)":
                    return OracleType.Timestamp;
                case "LONG RAW":
                    return OracleType.LongRaw;
                default:
                    throw new ApplicationException("未対応の型を使用しています。" + typeName);
            }
        }

        /// <summary>
        /// このクラスを表す文字列を取得します。
        /// </summary>
        /// <returns>Oracleを返却します。</returns>
        public override string ToString()
        {
            return "Oracle";
        }

        /// <summary>
        /// オートナンバーIDを取得します（未実装）。
        /// </summary>
        /// <value>空文字列が返却されます。</value>
        internal override string AutoNumberGetSql { get{return "";} }

        internal override IDbDataParameter CreateDataParameter(string parameterName, object value, string typeName)
        {
            OracleParameter parameter = new OracleParameter(parameterName, this.GetOracleType(typeName));
            if (value == null)
                parameter.Value = DBNull.Value;
            else
                parameter.Value = value;
            return parameter;
        }

        internal override string GetParameterName(string columnName)
        {
            return ":" + columnName;
        }

        /// <summary>
        /// データアダプターを生成します。
        /// </summary>
        /// <param name="sql">SQLを指定します。</param>
        /// <returns>生成したデータアダプターを返却します。</returns>
        public override IDbDataAdapter CreateDataAdapter(string sql)
        {
            return new OracleDataAdapter(sql, ConnectionString);
        }
    }
}
