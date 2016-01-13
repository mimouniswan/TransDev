using SQLite.Net.Attributes;

namespace AppAndroid.Data
{
    public class Table : ITable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public override string ToString()
        {
            return "Table";
        }
    }
}