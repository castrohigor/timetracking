using SQLite;
using TimeTracking.Models;

namespace TimeTracking.Data
{
    public class DatabaseService
    {
        private readonly SQLiteConnection _connection;
        private readonly string _databasePath;

        public DatabaseService()
        {
            _databasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TimeTracking",
                "timetracking.db"
            );

            // Criar diretório se não existir
            var directory = Path.GetDirectoryName(_databasePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            _connection = new SQLiteConnection(_databasePath);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            _connection.CreateTable<Project>();
            _connection.CreateTable<TimeEntry>();
        }

        // Métodos para Projetos
        public void AddProject(Project project)
        {
            _connection.Insert(project);
        }

        public void UpdateProject(Project project)
        {
            _connection.Update(project);
        }

        public void DeleteProject(int projectId)
        {
            _connection.Delete<Project>(projectId);
            // Deletar todas as entradas de tempo deste projeto
            _connection.Execute("DELETE FROM TimeEntries WHERE ProjectId = ?", projectId);
        }

        public List<Project> GetAllProjects()
        {
            return _connection.Table<Project>().OrderBy(p => p.Name).ToList();
        }

        public Project GetProject(int projectId)
        {
            return _connection.Table<Project>().FirstOrDefault(p => p.Id == projectId);
        }

        // Métodos para Time Entries
        public void AddTimeEntry(TimeEntry entry)
        {
            _connection.Insert(entry);
        }

        public void UpdateTimeEntry(TimeEntry entry)
        {
            _connection.Update(entry);
        }

        public void DeleteTimeEntry(int entryId)
        {
            _connection.Delete<TimeEntry>(entryId);
        }

        public List<TimeEntry> GetTimeEntriesByProject(int projectId)
        {
            return _connection.Table<TimeEntry>()
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.StartTime)
                .ToList();
        }

        public List<TimeEntry> GetTimeEntriesByProjectAndMonth(int projectId, int year, int month)
        {
            var allEntries = _connection.Table<TimeEntry>()
                .Where(t => t.ProjectId == projectId)
                .ToList();

            return allEntries
                .Where(t => t.StartTime.Year == year && t.StartTime.Month == month)
                .OrderByDescending(t => t.StartTime)
                .ToList();
        }

        public decimal GetTotalHoursByProjectAndMonth(int projectId, int year, int month)
        {
            var entries = GetTimeEntriesByProjectAndMonth(projectId, year, month);
            return entries.Sum(e => e.GetHours());
        }

        public void CloseConnection()
        {
            _connection?.Close();
        }
    }
}
