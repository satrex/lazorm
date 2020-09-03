using System;
using System.Collections.Generic;
using System.Text;

namespace Lazorm.Attributes
{
    /// <summary>
    /// データベースのカラムを表す属性です。
    /// </summary>
    public class DbColumnAttribute : Attribute
    {
        private string name = string.Empty;
        private bool isPrimaryKey = false;
        private bool nullable = false;
        private string typeName;
        private bool isAutoNumber = false;
        private int length;
        private int decimalPlace;
        private string remarks;

        /// <summary>
        /// マッピングするテーブルの列名
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// 型名
        /// </summary>
        public string TypeName
        {
            get { return this.typeName; }
            set { this.typeName = value; }
        }

        /// <summary>
        /// 主キーかどうか
        /// </summary>
        public bool IsPrimaryKey
        {
            get { return this.isPrimaryKey; }
            set { this.isPrimaryKey = value; }
        }

        /// <summary>
        /// Null許可列かどうか
        /// </summary>
        public bool Nullable
        {
            get { return this.nullable; }
            set { this.nullable = value; }
        }

        /// <summary>
        /// オートナンバー列かどうか
        /// </summary>
        public bool IsAutoNumber
        {
            get { return this.isAutoNumber; }
            set { this.isAutoNumber = value; }
        }

        /// <summary>
        /// 長さ
        /// </summary>
        public int Length
        {
            get { return this.length; }
            set { this.length = value; }
        }

        /// <summary>
        /// 少数点以下桁数
        /// </summary>
        public int DecimalPlace
        {
            get { return this.decimalPlace; }
            set { this.decimalPlace = value; }
        }

        /// <summary>
        /// 列の備考を取得または設定します。
        /// </summary>
        public string Remarks
        {
            get { return this.remarks; }
            set { this.remarks = value; }
        }
    }
}
