
namespace ContactList.Models
{
    public class Sensordata
    {
        public int Id { get; set; }

        public string SensorId { get; set; }

        public string KlantId { get; set; }
        public string Value { get; set; }

        public string Timestamp { get; set; }

        public bool Active { get; set; }
    }

    public class Agendadata
    {
        public int MessageId { get; set; }
        public string Titel { get; set; }
        public int CostumerId { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Priority { get; set; }
        public string State { get; set; }
    }
}