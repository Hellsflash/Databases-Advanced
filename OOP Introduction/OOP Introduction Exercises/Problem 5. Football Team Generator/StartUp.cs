using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        var command = Console.ReadLine();
        List<Team> teams = new List<Team>();

        while (command != "END")
        {
            var args = command.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            switch (args[0])
            {
                case "Team":
                    teams.Add(new Team(args[1]));
                    break;

                case "Add":
                    var teamName = args[1];
                    var playerName = args[2];
                    var endurance = int.Parse(args[3]);
                    var sprint = int.Parse(args[4]);
                    var dribble = int.Parse(args[5]);
                    var passing = int.Parse(args[6]);
                    var shooting = int.Parse(args[7]);

                    bool teamExists = teams.Any(t => t.Name == teamName);
                    Team team;
                    if (!teamExists)
                    {
                        Console.WriteLine($"Team {teamName} does not exist.");
                    }
                    else
                    {
                        try
                        {
                            team = teams.First(t => t.Name == teamName);
                            var player = new Player(playerName, endurance, sprint, dribble, passing, shooting);
                            team.AddPlayer(player);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    break;

                case "Remove":

                    teamName = args[1];
                    team = teams.First(t => t.Name == teamName);
                    playerName = args[2];

                    try
                    {
                        team.RemovePlayer(playerName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "Rating":
                    teamName = args[1];
                    teamExists = teams.Any(t => t.Name == teamName);

                    if (!teamExists)
                    {
                        Console.WriteLine($"Team {teamName} does not exist.");
                    }
                    else
                    {
                        team = teams.First(t => t.Name == teamName);
                        Console.WriteLine($"{team.Name} - {team.Rating()}");
                    }
                    break;
            }

            command = Console.ReadLine();
        }
    }
}