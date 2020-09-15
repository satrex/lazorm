using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Lazorm.Attributes;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Lazorm
{
    /// <summary>
    /// エンティティ抽象ジェネリッククラス
    /// エンティティ自体がデータベースにアクセスして
    /// 更新できる機能を持たせた。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class DataEntity<T> :INotifyPropertyChanged where T : DataEntity<T>, new()
    {
        /// <summary>
        /// プロパティ変更イベントハンドラ
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// プロパティの変更を通知します。
        /// </summary>
        /// <param name="propertyName">プロパティ名を設定します。</param>
        protected void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataEntity()
        {
        }

        /// <summary>
        /// データベースを操作するインスタンスを返す
        /// </summary>
        /// <returns></returns>
        public static Database GetDatabase()
        {
            DbTableAttribute table = DataEntity<T>.GetTableAttribute();
            var db = table.GetDatabase();
            db.CommandTimeout = CommandTimeout;
            return db;
        }

        /// <summary>
        /// DbTable属性が無い場合nullが返る
        /// </summary>
        /// <returns>DbTable属性</returns>
        internal static DbTableAttribute GetTableAttribute()
        {
            Type type = typeof(T);
            object[] attributes = type.GetCustomAttributes(typeof(DbTableAttribute), true);
            if (attributes.Length == 0)
                return null;

            DbTableAttribute attr = attributes[0] as DbTableAttribute;
            return attr;
        }
        
        internal static List<KeyValuePair<DbColumnAttribute, PropertyInfo>> GetKeyElements()
        {
            return GetElements(true);
        }

        internal static List<KeyValuePair<DbColumnAttribute, PropertyInfo>> GetElements()
        {
            return GetElements(false);
        }

        /// <summary>
        /// DbColumn属性のある属性だけ抽出する
        /// </summary>
        /// <param name="primaryKeyOnly"></param>
        /// <returns></returns>
        private static List<KeyValuePair<DbColumnAttribute, PropertyInfo>> GetElements(bool primaryKeyOnly)
        {
            var elements = new List<KeyValuePair<DbColumnAttribute, PropertyInfo>>();

            Type type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(typeof(DbColumnAttribute), true);
                if (attributes.Length == 0)
                    continue;

                var column = attributes[0] as DbColumnAttribute;

                if (primaryKeyOnly && !column.IsPrimaryKey)
                    continue;

                elements.Add(new KeyValuePair<DbColumnAttribute, PropertyInfo>(column, property));
            }

            return elements;
        }

        /// <summary>
        /// SQLの結果からエンティティを返す。
        /// SQLの列名と属性に指定されているName同じならば値が入る
        /// </summary>
        /// <typeparam name="U">エンティティの型を指定します。</typeparam>
        /// <param name="sql">エンティティを取得するSQL文を指定します。</param>
        /// <returns>取得したエンティティを返却します。</returns>
        public static IEnumerable<U> ExecuteQuery<U>(string sql) where U : DataEntity<U>, INotifyPropertyChanged, new()
        {
            var db = GetDatabase();
            return db.ExecuteQuery<U>(sql);
        }
       /// <summary>
        /// SQLの結果からエンティティを返す。
        /// SQLの列名と属性に指定されているNameが同じ場合、プロパティに値が入ります。
        /// </summary>
        /// <param name="whereExpression">Where句を指定します(WHEREキーワードは暗黙的に設定されます)。</param>
        /// <returns>エンティティのリストを返却します。</returns>
        public static IEnumerable<T> GetWhere(string whereExpression) 
        {
            var table = (DbTableAttribute) Attribute.GetCustomAttribute( typeof(T), typeof(DbTableAttribute));
            var sql = string.Format("SELECT * FROM {0} WHERE {1};", table.Name, whereExpression);
            var db = GetDatabase();
            return db.ExecuteQuery<T>(sql);
        }
        /// <summary>
        /// 型パラメータで指定したエンティティを、Where句を指定して取得します。
        /// </summary>
        /// <param name="whereExpression">SQLのWhere句を指定します。</param>
        /// <param name="args">Where句をstring.Formatする際のパラメータを指定します。</param>
        /// <returns>型パラメータで指定したエンティティのリストを返却します。</returns>
        public static IEnumerable<T> GetWhere(string whereExpression, object[] args)
        {
            var query = string.Format(whereExpression, args);
            return GetWhere(query);
        }

        /// <summary>
        /// Gets multiple entities with condition
        /// </summary>
        /// <param name="predicate">specifies condition (if not specified, then get all rows)</param>
        /// <returns>entities</returns>
        public static IEnumerable<T> Get(Func<T, bool> predicate)
        {
            var db = GetDatabase();
            return  db.SelectMany<T>(predicate);
        }

        /// <summary>
        /// SQLの結果からエンティティを取得します。
        /// SQLの列名と属性に指定されているNameが同じ場合、プロパティに値が入ります。
        /// /// </summary>
        /// <param name="sql">SQL文を指定します。</param>
        /// <returns>エンティティのリストを返却します。</returns>
        public static IEnumerable<T> GetBySql(string sql) 
        {
            var db = GetDatabase();
            return db.ExecuteQuery<T>(sql);
        }

        /// <summary>
        /// SQLの結果からエンティティを取得します。
        /// SQLの列名と属性に指定されているNameが同じ場合、プロパティに値が入ります。
        /// /// </summary>
        /// <param name="sql">SQL文を指定します。</param>
        /// <param name="args">SQLをstring.Formatする際のパラメータを指定します。</param>
        /// <returns></returns>
        public static IEnumerable<T> GetBySql(string sql, object[] args)
        {
            var query = string.Format(sql, args);
            return GetBySql(query);
        }

        /// <summary>
        /// テーブルのデータを全行取得します。
        /// </summary>
        /// <returns>テーブル全行ぶんのエンティティリストを返却します。</returns>
        public static IEnumerable<T> GetAll()
        {
            Database db = GetDatabase();
            return db.SelectAll<T>();
        }

        /// <summary>
        /// 主キー項目に値をいれてから呼べば
        /// DBから値を抜いてプロパティを更新してくれる。
        /// DBに無い場合falseが帰る
        /// </summary>
        public virtual bool Fill()
        {
            T dataEntity = GetDatabase().Select<T>(this);
            if (dataEntity == null)
                return false;
            this.Fill(dataEntity);
            return true;
        }

        public virtual Task<bool> FillAsync()
        {
            return Task.Run(() => Fill());
        }
        

        /// <summary>
        /// sourceに与えられたエンティティのプロパティを
        /// 自分に入れ込む DbColumn属性があるものだけ
        /// </summary>
        /// <param name="source"></param>
        public void Fill(DataEntity<T> source)
        {
            var elements = GetElements();
            
            foreach (var element in elements)
            {
                if (!element.Value.CanWrite)
                    continue;

                object val = element.Value.GetValue(source, null);
                element.Value.SetValue(this, val, null);

                //if (property.PropertyType.IsValueType || property.PropertyType.Equals(typeof(string)) || property.PropertyType == typeof(byte[]))
                //{
                    
                //}
            }
        }
        
        /// <summary>
        /// 主キー項目がおなじものを削除する
        /// 対象が無い場合は何もしない
        /// </summary>
        public virtual void Drop()
        {
            GetDatabase().Delete<T>(this);
        }

        /// <summary>
        /// 追加、更新のどちらかをおこなう
        /// 追加時に主キーがオートナンバーの場合、
        /// 新しい値を発行後にこのエンティティの主キーが更新される。
        /// </summary>
        public virtual void Store()
        {
            GetDatabase().Merge<T>(this);
        }

        /// <summary>
        /// オーバーライド不可メソッド
        /// 1つのエンティティを単純にUPSERTする
        /// オーバーライド可能なStoreと区別するため追加した
        /// </summary>
        public void StoreSimply()
        {
            GetDatabase().Merge<T>(this);
        }

        /// <summary>
        /// 主キーが同じものがDBに存在するかどうか
        /// </summary>
        /// <returns></returns>
        public virtual bool Exists()
        {
            return GetDatabase().Exists<T>(this);
        }

        /// <summary>
        /// DbColumn属性のあるプロパティをすべて比較し、
        /// すべて同じならtrueを返す
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual bool Equals(DataEntity<T> target)
        {
            var elements = DataEntity<T>.GetElements();

            foreach (var element in elements)
            {
                object thisValue = element.Value.GetValue(this, null);
                object targetValue = element.Value.GetValue(target, null);

                if (thisValue == null)
                {
                    if (targetValue != null)
                        return false;

                    continue;
                }

                if (!thisValue.Equals(targetValue))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 中身のおなじインスタンスをメモリの別の場所に新しく作り上げて返す
        /// BinaryFormatterによるシリアライズ＋デシリアライズ
        /// </summary>
        /// <returns></returns>
        public virtual T Clone()
        {
            T clone = default(T);

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, this);

                stream.Position = 0;

                clone = (T)formatter.Deserialize(stream);
            }

            return clone;
        }

        /// <summary>
        /// クラスをXml文字列化して返す
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            using (var memStream = new MemoryStream())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(memStream, this);
                memStream.Position = 0;
                using (var reader = new StreamReader(memStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// コマンドタイムアウトの秒数を取得または設定します。
        /// </summary>
        /// <value>コマンドタイムアウトの秒数</value>
        public static int CommandTimeout { get; set; }
    }
}
