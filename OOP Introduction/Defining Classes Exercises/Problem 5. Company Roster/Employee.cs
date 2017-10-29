public class Employee
{
    public Employee(string name, double salary, string position, string department, string email, int age)
    {
        Name = name;
        Salary = salary;
        Position = position;
        Department = department;
        Email = email;
        Age = age;
    }

    public Employee(string name, double salary, string position, string department)
    {
        Name = name;
        Salary = salary;
        Position = position;
        Department = department;
        Email = "n/a";
        Age = -1;
    }

    public Employee(string name, double salary, string position, string department, string email)
    {
        Name = name;
        Salary = salary;
        Position = position;
        Department = department;
        Email = email;
        Age = -1;
    }

    public Employee(string name, double salary, string position, string department, int age)
    {
        Name = name;
        Salary = salary;
        Position = position;
        Department = department;
        Email = "n/a";
        Age = age;
    }

    public string Name { get; set; }

    public double Salary { get; set; }

    public string Position { get; set; }

    public string Department { get; set; }

    public string Email { get; set; }

    public int Age { get; set; }

    public override string ToString()
    {
        return $"{Name} {Salary:F2} {Email} {Age}";
    }
}