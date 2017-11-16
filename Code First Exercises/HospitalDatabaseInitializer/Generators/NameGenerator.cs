using System;

namespace HospitalDatabaseInitializer.Generators
{
    //using System.IO;

    public class NameGenerator
    {
        private static readonly string[] firstNames =
            {"Petur", "Ivan", "Georgi", "Alexander", "Stefan", "Vladimir", "Svetoslav", "Kaloyan", "Mihail", "Stamat"};

        //private static string[] firstNames = File.ReadAllLines("<INSERT DIR HERE>");
        private static readonly string[] lastNames =
            {"Ivanov", "Georgiev", "Stefanov", "Alexandrov", "Petrov", "Stamatkov"};
        //private static string[] lastNames = File.ReadAllLines("<INSERT DIR HERE>");

        public static string FirstName()
        {
            return GenerateName(firstNames);
        }

        public static string LastName()
        {
            return GenerateName(lastNames);
        }

        private static string GenerateName(string[] names)
        {
            var rnd = new Random();

            var index = rnd.Next(0, names.Length);

            var name = names[index];

            return name;
        }
    }
}