namespace P01_StudentSystem.Data
{
    public class Configuration
    {
        public static string Connection { get; set; } = $"Server.;Database=StudentSystem;Integrated Security=true";
    }
}