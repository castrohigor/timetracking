using SQLite;

namespace TimeTracking.Models
{
    [Table("TimeEntries")]
    public class TimeEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ProjectId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Description { get; set; }

        public TimeEntry()
        {
            Description = string.Empty;
        }

        public TimeSpan GetDuration()
        {
            return EndTime - StartTime;
        }

        public decimal GetHours()
        {
            return (decimal)GetDuration().TotalHours;
        }
    }
}
