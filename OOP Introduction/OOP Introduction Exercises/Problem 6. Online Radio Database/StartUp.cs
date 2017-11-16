using System;

public class Program
{
    public static void Main(string[] args)
    {

        var songsCount = long.Parse(Console.ReadLine());
        var playlist = new Playlist();
        for (var i = 0; i < songsCount; i++)
        {
            try
            {
                var input = Console.ReadLine().Split(new[] { ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (input.Length != 4)
                {
                    continue;
                }

                var performer = input[0];
                var title = input[1];
                if (!long.TryParse(input[2], out long minutes))
                {
                    throw new InvalidSongLengthException("Invalid song length.");
                };
                if (!long.TryParse(input[3], out long seconds))
                {
                    throw new InvalidSongLengthException("Invalid song length.");
                };
                var song = new Song(performer, title, minutes, seconds);
                playlist.AddSong(song);
            }
            catch (InvalidSongException ise)
            {
                Console.WriteLine(ise.Message);
            }
        }

        Console.WriteLine(playlist);
    }
}