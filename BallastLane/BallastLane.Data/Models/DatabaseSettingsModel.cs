namespace BallastLane.Data.Models
{
    public class DatabaseSettingsModel : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
