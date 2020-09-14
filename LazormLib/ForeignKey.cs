
namespace Lazorm
{
    public class ForeignKey
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string ReferencedTableName { get; set; }

        public string ReferencedColumnName { get; set; }

    }
   

}