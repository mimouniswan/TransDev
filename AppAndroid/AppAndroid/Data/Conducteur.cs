using SQLite.Net.Attributes;

namespace AppAndroid.Data
{
    public class Conducteur
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nom { get; set; }

        public override string ToString()
        {
            return "Conducteur";
        }
    }
}