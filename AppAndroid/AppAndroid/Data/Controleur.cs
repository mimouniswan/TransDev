using SQLite.Net.Attributes;

namespace AppAndroid.Data
{
    public class Controleur
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string MdP { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}