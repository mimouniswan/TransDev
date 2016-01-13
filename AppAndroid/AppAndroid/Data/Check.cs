using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AppAndroid.Data
{
    public class Check : Table
    {
        //[PrimaryKey, AutoIncrement]
        //public int Id { get; set; }

        [ForeignKey(typeof(Bus))]
        public int IdBus { get; set; }
        [ForeignKey(typeof(Controleur))]
        public int IdControleur { get; set; }
        [ForeignKey(typeof(Conducteur))]

        public int IdConducteur { get; set; }
        public string Date { get; set; }

        [ManyToOne]
        public Bus Bus { get; set; }
        [ManyToOne]
        public Controleur Controleur { get; set; }
        [ManyToOne]
        public Conducteur Conducteur { get; set; }

        public override string ToString()
        {
            return "Check";
        }
    }
}