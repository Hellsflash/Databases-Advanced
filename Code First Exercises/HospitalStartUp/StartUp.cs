using HospitalDatabaseInitializer;

public class StartUp
{
    public static void Main(string[] args)
    {
        DatabaseInitializer.ResetDatabase();
    }
}