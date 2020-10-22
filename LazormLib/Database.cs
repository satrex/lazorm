using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Data.Common;
using System.Reflection;
using System.Linq;
using System.Data.SqlClient;
using Lazorm.Attributes;
using System.Threading.Tasks;

namespace Lazorm
{
    /// <summary>
    /// specifies kind of database product.
    /// データベースの製品を表します。
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// SQL Server
        /// </summary>
        SqlServer,
        /// <summary>
        /// Oracle
        /// </summary>
        Oracle,
        /// <summary>
        /// MySql
        /// </summary>
        MySql
    }
    
    /// <summary>
    /// Abstrace class for manipulating database. 
    /// Implementation of each product is written in child classes.
    /// データベースを表します。
    /// </summary>
    public abstract class Database
    {
        /// <summary>
        /// A traceSource.
        /// トレースソース
        /// </summary>
        public static TraceSource ts = new TraceSource("Lazorm");

        /// <summary>
        /// Gets or Sets database schema name.
        /// データベーススキーマ名を取得します。
        /// </summary>
        public virtual string Schema {get; set;}

        /// <summary>
        /// A constructor
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionString">connectionString for target database 接続文字列を指定します。</param>
        public Database(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private static Dictionary<string, DatabaseType> types = new Dictionary<string, DatabaseType>()
        {
            {"SqlServer", DatabaseType.SqlServer},
            {"sqlServer", DatabaseType.SqlServer},
            {"sqlserver", DatabaseType.SqlServer},
            {"mssql", DatabaseType.SqlServer},
            {"Mssql", DatabaseType.SqlServer},
            {"MsSql", DatabaseType.SqlServer},
            {"Oracle", DatabaseType.Oracle},
            {"oracle", DatabaseType.Oracle},
            {"MySql", DatabaseType.MySql},
            {"mySql", DatabaseType.MySql},
            {"mysql", DatabaseType.MySql}
        };

        /// <summary>
        /// Gets Database type enum by name string.
        /// 文字列を元に、データベースタイプ列挙体を取得します。
        /// </summary>
        /// <param name="typeName">a string that specifies type of database. データベースタイプを表す文字列を指定します。</param>
        /// <returns>DatabaseType Enum データベースタイプ列挙体</returns>
        public static DatabaseType GetDatabaseType(string typeName) 
        {

            return types[typeName];
        }

        internal abstract DatabaseType Type { get; }

        /// <summary>
        /// 接続文字列を取得または設定します。
        /// </summary>
        public string ConnectionString { get; set; }

        private int commandTimeout = 300;
        /// <summary>
        /// コマンドのタイムアウトを秒数で指定します。
        /// </summary>
        public int CommandTimeout 
        {
            get { return this.commandTimeout; }
            set { this.commandTimeout = value; }
        }

        /// <summary>
        /// データベーススキーマから、C#のインスタンスを生成します。
        /// </summary>
        /// <param name="type">データベースの製品を指定します。 </param>
        /// <param name="connectionString">接続文字列を指定します。</param>
        /// <returns></returns>
        public static Database CreateInstance(DatabaseType type, string connectionString)
        {
            switch (type)
            {
                case DatabaseType.Oracle:
                    return new Oracle(connectionString);
                case DatabaseType.SqlServer:
                    return new SqlServer(connectionString);
                case DatabaseType.MySql:
                    return new MySqlDb(connectionString);
                default:
                    throw new ApplicationException("対応外のデータベースです");
            }
        }

                /// <summary>
        /// データベーススキーマから、C#のインスタンスを生成します。
        /// </summary>
        /// <param name="type">データベースの製品を指定します。 </param>
        /// <param name="connectionString">接続文字列を指定します。</param>
        /// <returns></returns>
        public static Database CreateInstance(string type, string connectionString)
        {
            DatabaseType enumType = GetDatabaseType(type);
            return CreateInstance(enumType, connectionString);
        }


        /// <summary>
        /// IDataRecordから取得されるオブジェクト型の値をキャストしなおして
        /// オブジェクト型で返す。PropertyInfo.SetValue用
        /// DBNullをnullに変換する。
        /// string, int, decimal, DateTime, bool, double, byte[] まで対応
        /// </summary>
        /// <param name="val">IDataRecordから取得された値</param>
        /// <param name="type">変換先の型</param>
        /// <returns>変換されたオブジェクト型の値</returns>
        public static object Cast(object val, Type type)
        {
            if (val == null || val == DBNull.Value)
                return null;

            if (type == typeof(int) || type == typeof(int?))
                return Convert.ToInt32(val);

            if (type == typeof(decimal) || type == typeof(decimal?))
                return Convert.ToDecimal(val);

            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return Convert.ToDateTime(val);

            if (type == typeof(bool) || type == typeof(bool?))
                return Convert.ToBoolean(val);

            if (type == typeof(double) || type == typeof(double?))
                return Convert.ToDouble(val);

            if (type == typeof(float) || type == typeof(float?))
                return Convert.ToDouble(val);

            if (type == typeof(byte[]))
                return (byte[])val;

            return val;
        }

        /// <summary>
        /// データベースへの接続を生成します。
        /// </summary>
        /// <returns>データベースへの接続を返却します。</returns>
        public abstract IDbConnection CreateConnection();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName">@項目名とか:項目名とか</param>
        /// <param name="value">パラメータに入れる値</param>
        /// <param name="typeName">データベース項目の型名</param>
        /// <returns>SqlParameterとかOracleParameterとか</returns>
        internal abstract IDbDataParameter CreateDataParameter(string parameterName, object value, string typeName);
        internal abstract string GetTableListSql();
        internal abstract string GetColumnListSql(string tableName);
        public abstract Type GetProgramType(ColumnDef column);

        /// <summary>
        /// SqlServerなら@+項目名、Oracleなら:+項目名が返る
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        internal abstract string GetParameterName(string columnName);
        
        /// <summary>
        /// テーブルの全データを、エンティティのリストで取得します。
        /// </summary>
        /// <typeparam name="T">指定するテーブルのエンティティを指定します。</typeparam>
        /// <returns>エンティティのリストを返却します。</returns>
        public IEnumerable<T> SelectAll<T>() where T : DataEntity<T>, new()
        {
            DbTableAttribute table = DataEntity<T>.GetTableAttribute();
            return this.ExecuteQuery<T>("SELECT * FROM " + table.Name);
        }

         public IEnumerable<T> SelectMany<T>(Func<T, bool> predicate) where T : DataEntity<T>, new()
        {
            DbTableAttribute table = DataEntity<T>.GetTableAttribute();

            return this.ExecuteQuery<T>("SELECT * FROM " + table.Name).Where(predicate);
        }
                
        /// <summary>
        /// Databaseプロパティを設定し、IDataRecordからプロパティのｺﾋﾟｰを
        /// 行なったDataEntityをインスタンス化する。
        /// reader内に合致する項目が無い場合無視する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public T CreateInstance<T>(IDataReader reader) where T : DataEntity<T>, new()
        {
            T instance = new T();

            var elements = DataEntity<T>.GetElements();

            foreach (var element in elements)
            {
                var column = element.Key;
                var property = element.Value;

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i) != column.Name)
                        continue;

                    object val = reader[i];
                    val = Cast(val, property.PropertyType);
                    property.SetValue(instance, val, null);
                    break;
                }
            }

            return instance;
        }

        /// <summary>
        /// 任意のSQLでいろいろ処理したいときに使う
        /// readerからエンティティの生成はCreateInstanseメソッドを使えば可能です。
        /// ※このメソッドを呼ぶ際に
        /// IDataReaderは絶対にusingして使ってください
        /// CommandBehavior.CloseConnectionにより
        /// ConnectionはIDataReaderが閉じ次第閉じるようになっているそうです
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql)
        {
            IDbConnection connection = this.CreateConnection();
            using(IDbCommand command = connection.CreateCommand())
            {
                command.CommandTimeout = this.CommandTimeout;
                command.CommandText = sql;
                connection.Open();
                ts.TraceInformation(sql);
                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        /// <summary>
        /// SQLの結果から指定した型のエンティティを返す。
        /// SQLの列名と属性に指定されているNameが同じならば値が入る
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteQuery<T>(string sql, Func<T, bool> predicate = null) where T : DataEntity<T>, new()
        {
            using (IDbConnection connection = this.CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandText = sql;
                    ts.TraceInformation(sql);

                    using (IDataReader reader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        if(predicate == null)
                        {
                            while (reader.Read())
                            {
                                yield return this.CreateInstance<T>(reader);
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                var instance = this.CreateInstance<T>(reader);
                                if(predicate(instance))
                                {
                                    yield return instance;
                                }
                            }

                        }
                   }
                }
            }
        }
        /// <summary>
        /// sqlを発行する
        /// 戻りの値がNullまたはDBNull.Valueの場合nullを返す。
        /// sqlが返す結果セットの最初の行の最初の列の値を返す。
        /// </summary>
        /// <param name="sql">発行するSQL</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            return this.ExecuteScalar(sql, new List<IDbDataParameter>());
        }

        /// <summary>
        /// sqlを発行する。
        /// 戻りの値がNullまたはDBNull.Valueの場合nullを返す。
        /// sqlが返す結果セットの最初の行の最初の列の値を返す。
        /// </summary>
        /// <param name="sql">発行するSQL</param>
        /// <param name="parameters">SQLのパラメータリスト</param>
        /// <returns>値</returns>
        public object ExecuteScalar(string sql, List<IDbDataParameter> parameters)
        {
            using (IDbConnection connection = this.CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandText = sql;
                    foreach (IDbDataParameter p in parameters)
                    {
                        command.Parameters.Add(p);
                    }

                    ts.TraceInformation(sql);
                    object val = command.ExecuteScalar();
                    if (val == DBNull.Value)
                        return null;

                    return val;
                }
            }
        }

        /// <summary>
        /// 更新系SQL発行用
        /// </summary>
        /// <param name="sql"></param>
        public void ExecuteNonQuery(string sql)
        {
            this.ExecuteNonQuery(sql, new List<IDbDataParameter>());
        }

        public Task ExecuteNonQueryAsync(string sql)
        {
            return Task.Run(() => {
                ExecuteNonQuery(sql);
            });
        }

        /// <summary>
        /// 更新系SQL発行用
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public void ExecuteNonQuery(string sql, List<IDbDataParameter> parameters)
        {
            using (IDbConnection connection = this.CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = this.CommandTimeout;
                    foreach (IDbDataParameter p in parameters)
                    {
                        command.Parameters.Add(p);
                    }

                    command.CommandText = sql;
                    ts.TraceInformation(sql);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Task ExecuteNonQueryAsync(string sql, List<IDbDataParameter> parameters)
        {
            return Task.Run(() => {
                ExecuteNonQuery(sql, parameters);
            });
        }

        /// <summary>
        /// SQLの結果をデータテーブルで返す
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            var dataSet = new DataSet();

            IDbDataAdapter adapter = this.CreateDataAdapter(sql);
            adapter.SelectCommand.CommandTimeout = this.CommandTimeout;
            adapter.Fill(dataSet);

            if (dataSet.Tables.Count == 0)
                return new DataTable();

            return dataSet.Tables[0];
        }

        /// <summary>
        /// データアダプターを生成します。
        /// </summary>
        /// <param name="sql">SQLを指定します。</param>
        /// <returns>生成したデータアダプターを返却します。</returns>
        public abstract IDbDataAdapter CreateDataAdapter(string sql);

        /// <summary>
        /// 主キーをエンティティから抜いてDBから取得する。なければnullを返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        public T Select<T>(DataEntity<T> dataEntity) where T : DataEntity<T>, new()
        {
            DbTableAttribute attributes = DataEntity<T>.GetTableAttribute();
            var keys = DataEntity<T>.GetKeyElements();

            if (keys.Count == 0)
                throw new ApplicationException("主キーの無いテーブルに選択クエリを発行しようとしました。");

            string sql = @" SELECT * FROM " + attributes.Name + " " + CreateWhereClause<T>(keys, dataEntity);
            
            using (IDbConnection connection = this.CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandText = sql;
                    ts.TraceInformation(sql);
                    using (IDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!reader.Read())
                            return default(T);

                        return this.CreateInstance<T>(reader);
                    }
                }
            }
        }
        public Task<T> SelectAsync<T>(DataEntity<T> dataEntity) where T : DataEntity<T>, new()
        {
            return (Task<T>)Task.Run(() =>
            {
                Select<T>(dataEntity);
            });
        }


        /// <summary>
        /// オートナンバーIDを取得するためのSQL文を取得します。
        /// </summary>
        /// <value>オートナンバーID取得用のSQL断片</value>
        internal virtual string AutoNumberGetSql { get; }

        /// <summary>
        /// 主キーをエンティティから抜いてDBに存在のチェックをかける。なければnullを返す。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        public bool Exists<T>(DataEntity<T> dataEntity) where T : DataEntity<T>, new()
        {
            if (this.Select<T>(dataEntity) == null)
                return false;

            return true;
        }

        /// <summary>
        /// INSERT文を自動生成し、実行する
        /// 新しく発行されたオートナンバーがある場合、
        /// 新しく発行した値をエンティティに入れる。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataEntity"></param>
        public void Insert<T>(DataEntity<T> dataEntity) where T : DataEntity<T>, new()
        {
            DbTableAttribute table = DataEntity<T>.GetTableAttribute();

            //オラクルで";"を後ろにつけると動かないらしい
            string sql = "INSERT INTO {0} ({1}) VALUES ({2})";
            string items = string.Empty; //項目羅列部
            string values = string.Empty; //値羅列部
            List<IDbDataParameter> parameters = new List<IDbDataParameter>(); //パラメーターリスト

            var elements = DataEntity<T>.GetElements();

            foreach (var element in elements)
            {
                var column = element.Key;
                if (column.IsAutoNumber)
                    continue;

                items += column.Name + ", ";

                var property = element.Value;
                object val = property.GetValue(dataEntity, null);

                //byte[]の更新のみパラメータークエリを使用する
                if (property.PropertyType == typeof(byte[]))
                {
                    string parameterName = this.GetParameterName(column.Name);
                    values += parameterName + ", ";
                    parameters.Add(this.CreateDataParameter(parameterName, val, column.TypeName));
                }
                else
                    values += this.ConvertToSqlValue(val, property.PropertyType, element.Key.Nullable) + ", ";
            }

            items = items.Substring(0, items.Length - 2);
            values = values.Substring(0, values.Length - 2);
            sql = string.Format(sql, new object[] { table.Name, items, values });

            if (!elements.Exists(p => p.Key.IsAutoNumber))
            {
                this.ExecuteScalar(sql, parameters);
                return;
            }

            //オートナンバー取得用、自動INCREMENT列がある場合に値が返る
            //SqlServerのみ対応した。OracleSEQUENCEはだれかやってもらいたい
            sql += this.AutoNumberGetSql;

            object id = this.ExecuteScalar(sql, parameters);

            //自動INCREMENT列が無い場合、nullが返る
            if (id == null)
                return;

            //採番された番号をエンティティに設定する
            var key = elements.Find(p => p.Key.IsPrimaryKey);
            var propertyInfo = key.Value;
            id = Cast(id, propertyInfo.PropertyType);
            propertyInfo.SetValue(dataEntity, id, null);
        }

        /// <summary>
        /// 主キーでレコードを特定し、
        /// 主キー以外の列をエンティティの値で更新する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataEntity"></param>
        public void Update<T>(DataEntity<T> dataEntity) where T : DataEntity<T>, new()
        {
            DbTableAttribute table = DataEntity<T>.GetTableAttribute();
            
            List<IDbDataParameter> parameters = new List<IDbDataParameter>();
            string sql = @"UPDATE " + table.Name + " SET ";

            var elements = DataEntity<T>.GetElements();
            var keys = elements.FindAll(p => p.Key.IsPrimaryKey);
            if (keys.Count == 0)
                throw new ApplicationException("主キーの無いテーブルへの更新は許可されていません。");

            if (elements.Count == 0)
                return;

            foreach (var element in elements)
            {
                var column = element.Key;

                if (column.IsPrimaryKey)
                    continue;

                var property = element.Value;
                
                object val = property.GetValue(dataEntity, null);
                if(property.PropertyType == typeof(byte[]))
                {
                    string parameterName = this.GetParameterName(column.Name);
                    sql += column.Name + " = " + parameterName + ", ";
                    parameters.Add(this.CreateDataParameter(parameterName, val, column.TypeName));
                }
                else
                    sql += column.Name + " = " + this.ConvertToSqlValue(val, property.PropertyType, element.Key.Nullable) + ", ";
            }

            sql = sql.Substring(0, sql.Length - 2) + " ";
            sql += CreateWhereClause<T>(keys, dataEntity);

            this.ExecuteNonQuery(sql, parameters);
        }

        /// <summary>
        /// エンティティの主キーを元に、テーブルの列を削除します。
        /// </summary>
        /// <typeparam name="T">エンティティを指定します。</typeparam>
        /// <param name="dataEntity">データエンティティを指定します。</param>
        public void Delete<T>(DataEntity<T> dataEntity) where T : DataEntity<T>, new()
        {
            var keys = DataEntity<T>.GetKeyElements();

            if(keys.Count == 0)
                throw new ApplicationException("主キーの無いテーブルへのDELETE文の発行は許可されていません。");

            DbTableAttribute table = DataEntity<T>.GetTableAttribute();
            string sql = @"DELETE FROM " + table.Name + " " + CreateWhereClause<T>(keys, dataEntity);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 文字型ならシングルコート囲い　と文字内の'置き換え
        /// 日付型もシングルコート囲い
        /// ブール型の"0"、"1"への変換
        /// nullは"NULL"に
        /// </summary>
        /// <param name="val">エンティティから抜いた値</param>
        /// <param name="type">プロパティの型</param>
        /// <param name="isNullable"></param>
        /// <returns>SQLのパラメタとなる文字</returns>
        internal virtual string ConvertToSqlValue(object val, Type type, bool isNullable)
        {
            if (val == null)
            {
                //NULL不許可でも空文字列で通そうと思う。SQLのシステム例外は発生させてはいかん
                if (type == typeof(string) && !isNullable)
                {
                    return "''";
                }
                
                return "NULL";
            }

            if (type == typeof(string))
                return "'" + val.ToString().Replace("'", "''") + "'";

            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                DateTime time = (type == typeof(DateTime)) ? (DateTime)val : ((DateTime?)val).Value;

                //なんかオーバーライドするよりもここで書いたほうがいいようなきがしました。
                if (this is Oracle)
                    return "TO_DATE('" + time.ToString("yyyy/MM/dd HH:mm:ss") + "','yyyy/mm/dd hh24:mi:ss')";
                if (this is MySqlDb)
                    return "'" + time.ToString("yyyy/MM/dd HH:mm:ss") + "'";

                return "'" + val.ToString() + "'";
            }

            if (type == typeof(bool))
                return ((bool)val) ? "1" : "0";

            if (type == typeof(bool?))
                return ((bool?)val).Value ? "1" : "0";

            return val.ToString();
        }       

        /// <summary>
        /// 主キーで存在チェックを行い、追加・更新を自動で判断して実行する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataEntity"></param>
        public void Merge<T>(DataEntity<T> dataEntity) where T : DataEntity<T>, new()
        {
            if (this.Exists<T>(dataEntity))
                this.Update<T>(dataEntity);
            else
                this.Insert<T>(dataEntity);
        }

        /// <summary>
        /// 主キーを指定するWHERE句を自動生成する。
        /// NULLを指定する事は無いものとする。
        /// elementsが0個の場合空文字列を返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="dataEntity"></param>
        /// <returns></returns>
        internal virtual string CreateWhereClause<T>(List<KeyValuePair<DbColumnAttribute, PropertyInfo>> elements, DataEntity<T> dataEntity) where T : DataEntity<T>, new()
        {
            if (elements.Count == 0)
                return string.Empty;

            string sql = " WHERE ";
            foreach (var element in elements)
            {
                var column = element.Key;
                var property = element.Value;
                object val = property.GetValue(dataEntity, null);
                sql += column.Name + " = " + this.ConvertToSqlValue(val, property.PropertyType, element.Key.Nullable) + " AND ";
            }

            sql = sql.Substring(0, sql.Length - 5);
            return sql;
        }

        /// <summary>
        /// ColumnsプロパティにColumnの情報を入れて
        /// テーブル定義を取得する
        /// </summary>
        /// <returns></returns>
        public List<TableDef> GetTableDefs()
        {
            List<TableDef> tables = new List<TableDef>();
            using (IDbConnection connection = this.CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandText = this.GetTableListSql();
                    ts.TraceInformation(command.CommandText);
                    using (IDataReader tableReader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        while (tableReader.Read())
                        {
                            TableDef table = new TableDef();
                            table.Name = (string)Cast(tableReader["TableName"], typeof(string));
                            table.Remarks = (string)Cast(tableReader["Remarks"], typeof(string));
                            var columnDefs = this.GetColumnDefs(table.Name);
                            table.Columns.AddRange(columnDefs);
                            tables.Add(table);
                        }
                    }
                }
            }

            return tables;
        }

        /// <summary>
        /// 列名を取得します。
        /// </summary>
        /// <param name="tableName">テーブル名を指定します。</param>
        /// <returns>列定義のリストを返却します。</returns>
        protected List<ColumnDef> GetColumnDefs(string tableName)
        {
            List<ColumnDef> columns = new List<ColumnDef>();
            using (IDbConnection connection = this.CreateConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = this.CommandTimeout;
                    command.CommandText = this.GetColumnListSql(tableName);
                    ts.TraceInformation(command.CommandText);
                    using (IDataReader columnReader = command.ExecuteReader(CommandBehavior.SingleResult))
                    {
                        while (columnReader.Read())
                        {
                            ColumnDef column = new ColumnDef();
                            column.Name = (string)Cast(columnReader["ColumnName"], typeof(string));
                            column.TypeName = (string)Cast(columnReader["TypeName"], typeof(string));
                            column.Length = (int)Cast(columnReader["Length"], typeof(int));
                            column.IsPrimaryKey = (bool)Cast(columnReader["IsPrimaryKey"], typeof(bool));
                            column.IsAutoNumber = (bool)Cast(columnReader["IsAutoNumber"], typeof(bool));
                            column.Nullable = (bool)Cast(columnReader["Nullable"], typeof(bool));
                            column.DecimalPlace = (int)Cast(columnReader["DecimalPlace"], typeof(int));
                            column.Remarks = (string)Cast(columnReader["Remarks"], typeof(string));
                            columns.Add(column);
                        }
                    }
                }
            }

            return columns;
        }

        /// <summary>
        /// specifies a sql to get foreign keys of schema.
        /// </summary>
        /// <returns>sql string if implemented. Else returns string.empty</returns>
        public virtual string GetForeignKeySql()
        {
            return string.Empty;
        }
    }
}
