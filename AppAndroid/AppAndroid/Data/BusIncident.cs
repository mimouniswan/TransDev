using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace AppAndroid.Data
{
    public class BusIncident
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(CheckUp))]
        public int IdCheck { get; set; }
        [ForeignKey(typeof(Bus))]
        public int IdBus { get; set; }
        [ForeignKey(typeof(Incident))]
        public int IdIncident { get; set; }

        public override string ToString()
        {
            return "BusIncident";
        }
    }
}