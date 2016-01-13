using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AppAndroid.Data
{
    public class BusIncident : Table
    {
        //[PrimaryKey, AutoIncrement]
        //public int Id { get; set; }

        [ForeignKey(typeof(Bus))]
        public int BusId { get; set; }

        [ForeignKey(typeof(Incident))]
        public int IncidentId { get; set; }

        public override string ToString()
        {
            return "BusIncident";
        }
    }
}