using System;
using System.Collections.Generic;
using System.Text;

namespace Lazorm.Attributes
{
    /// <summary>
    /// データベースの外部キー列を表す属性です。
    /// </summary>
    public class ForeignKeyAttribute: Attribute
    {
        public string ReferencedTableName { get; set; }
        public string ReferencedColumnName { get; set; }

    }


}
