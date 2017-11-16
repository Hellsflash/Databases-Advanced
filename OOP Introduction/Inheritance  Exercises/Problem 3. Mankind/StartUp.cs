using System;

public class StartUp
{
    public static void Main(string[] args)
    {
        try
        {
            var studentArgs = Console.ReadLine().Split();
            var workerArgs = Console.ReadLine().Split();

            var studentFirstName = studentArgs[0];
            var studentLastName = studentArgs[1];
            var studentFacNum = studentArgs[2];

            var workerFirstName = workerArgs[0];
            var workerLastName = workerArgs[1];
            var workerWeekSalary = double.Parse(workerArgs[2]);
            var workerHoursPerDay = double.Parse(workerArgs[3]);

            var student = new Student(studentFirstName, studentLastName, studentFacNum);
            var worker = new Worker(workerFirstName, workerLastName, workerWeekSalary, workerHoursPerDay);

            Console.WriteLine(student + Environment.NewLine + Environment.NewLine + worker);
        }
        catch (ArgumentException ae)
        {
            Console.WriteLine(ae.Message);
            return;
        }
    }
}