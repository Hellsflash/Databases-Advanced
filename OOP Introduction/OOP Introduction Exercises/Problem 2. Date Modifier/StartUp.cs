using System;
using System.Globalization;

public class StartUp
{
    public static void Main(string[] args)
    {
        var firstDate = Console.ReadLine().Split();
        var secondDate = Console.ReadLine().Split();

        var firstDateExact = DateTime.ParseExact($"{firstDate[0]}-{firstDate[1]}-{firstDate[2]}", "yyyy-MM-dd",
            CultureInfo.InvariantCulture);
        var secondDateExact = DateTime.ParseExact($"{secondDate[0]}-{secondDate[1]}-{secondDate[2]}", "yyyy-MM-dd",
            CultureInfo.InvariantCulture);
        var dateModifier = new DateModifier(firstDateExact, secondDateExact);

        Console.WriteLine(dateModifier.CalculateDiferance());
    }
}