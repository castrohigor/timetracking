using SQLite;

namespace TimeTracking.Models
{
    [Table("Projects")]
    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Name { get; set; }

        public decimal HourlyRate { get; set; }

        public DateTime CreatedAt { get; set; }

        public Project()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
