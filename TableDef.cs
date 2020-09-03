using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lazorm.Attributes;

namespace Lazorm
{
    /// <summary>
    /// クラス自動生成用
    /// データベースから取得したテーブルの定義を保持する。
    /// </summary>
    public class TableDef : DbTableAttribute
    {
        private List<ColumnDef> columns = new List<ColumnDef>();

        /// <summary>
        /// テーブルの列を表すインスタンスを取得します。
        /// </summary>
        public List<ColumnDef> Columns
        {
            get { return this.columns; }
        }
    }
}
