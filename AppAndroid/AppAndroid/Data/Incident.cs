using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace AppAndroid.Data
{
    public class Incident
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Conducteur))]
        public int IdConducteur { get; set; }
        [ForeignKey(typeof(Controleur))]
        public int IdCreationCotroleur { get; set; }
        [ForeignKey(typeof(Controleur))]
        public int IdMaJControleur { get; set; }

        public string Type { get; set; }
        public int Gravite { get; set; }
        public int Etat { get; set; }
        public string DateCreation { get; set; }
        public string DateMaJ { get; set; }
        public string Observation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Picture { get; set; }

        [ManyToMany(typeof(BusIncident))]
        public List<BusIncident> BusIncident { get; set; }

        [ManyToOne]
        public Conducteur Conducteur { get; set; }
        [ManyToOne]
        public Controleur CreationCotroleur { get; set; }
        [ManyToOne]
        public Controleur MaJControleur { get; set; }

        public override string ToString()
        {
            return "Incident";
        }
    }
}