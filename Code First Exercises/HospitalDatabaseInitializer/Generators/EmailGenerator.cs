using System;

namespace HospitalDatabaseInitializer.Generators
{
    internal class EmailGenerator
    {
        private static readonly Random rnd = new Random();

        private static readonly string[] domains =
            {"mail.bg", "abv.bg", "gmail.com", "hotmail.com", "softuni.bg", "students.softuni.bg"};
        //private static string[] domains = File.ReadAllLines("<INSERT DIR HERE>");

        internal static string NewEmail(string name)
        {
            var domain = domains[rnd.Next(domains.Length)];
            var number = rnd.Next(1, 2000);

            return $"{name.ToLower()}{number}@{domain}";
        }
    }
}