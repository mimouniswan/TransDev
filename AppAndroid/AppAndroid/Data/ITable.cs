using SQLite.Net.Attributes;

namespace AppAndroid.Data
{
    public interface ITable
    {
        [PrimaryKey, AutoIncrement]
        int Id { get; set; }

        string ToString();
    }
}