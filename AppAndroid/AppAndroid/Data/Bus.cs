using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace AppAndroid.Data
{
    public class Bus
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Model))]
        public int IdModel { get; set; }

        public int Number { get; set; }
        public string Color { get; set; }

        [ManyToMany(typeof(BusIncident))]
        public List<BusIncident> BusIncident { get; set; }
        [ManyToOne]
        public Model Model { get; set; }

        public override string ToString()
        {
            return "Bus";
        }
    }
}