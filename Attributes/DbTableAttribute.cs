using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Lazorm.Attributes
{
    /// <summary>
    /// データベースのテーブルを表す属性です。
    /// </summary>
    public class DbTableAttribute : Attribute
    {
        private DatabaseType databaseType;
        private string connectionSettingKeyName;
        private string tableName = string.Empty;

        /// <summary>
        /// データベースがSQLServerであるか、Oracleであるかを取得または設定します。
        /// </summary>
        public DatabaseType DatabaseType
        {
            get { return this.databaseType; }
            set { this.databaseType = value; }
        }

        /// <summary>
        /// Comfigファイル内で指定する、ConnectionSettingのキー名を取得または設定します。
        /// </summary>
        public string ConnectionSettingKeyName
        {
            get { return this.connectionSettingKeyName; }
            set { this.connectionSettingKeyName = value; }
        }

        /// <summary>
        /// テーブル名を取得または設定します。
        /// </summary>
        public string Name
        {
            get { return this.tableName; }
            set { this.tableName = value; }
        }

        /// <summary>
        /// データベースを取得します。
        /// </summary>
        /// <returns>データベースを返却します。</returns>
        public Database GetDatabase()
        {
            ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings[this.ConnectionSettingKeyName];
            if (settings == null)
                throw new ApplicationException(@"構成ファイルに接続文字列の設定がされていないか、設定ファイルのキー名が間違っています。");

            return Database.CreateInstance(this.DatabaseType, settings.ConnectionString);
        }
    }
}
