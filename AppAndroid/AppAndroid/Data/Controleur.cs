using SQLite.Net.Attributes;

namespace AppAndroid.Data
{
    public class Controleur : Table
    {
        //[PrimaryKey, AutoIncrement]
        //public int Id { get; set; }

        public string Nom { get; set; }

        public override string ToString()
        {
            return "Controleur";
        }
    }
}