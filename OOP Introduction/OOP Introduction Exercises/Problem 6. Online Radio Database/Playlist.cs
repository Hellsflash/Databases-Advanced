using System;
using System.Collections.Generic;

public class Playlist
{
    private const string SongsAddedMessage = "Songs added: ";
    private const string SongAddedMessage = "Song added.";
    private const string PlaylistLengthMessage = "Playlist length: {0}h {1}m {2}s";
    private const long SecondsInAMinute = 60;
    private const long MinutesInAHour = 60;
    private long hours;
    private long minutes = 0;
    private long seconds = 0;

    public Playlist()
    {
        this.Songs = new List<Song>();
    }

    private ICollection<Song> Songs { get; set; }

    public void AddSong(Song song)
    {
        this.Songs.Add(song);
        this.seconds += song.Seconds;
        this.minutes += this.seconds / SecondsInAMinute;
        this.seconds %= SecondsInAMinute;
        this.minutes += song.Minutes;
        this.hours += this.minutes / MinutesInAHour;
        this.minutes %= MinutesInAHour;
        Console.WriteLine(SongAddedMessage);
    }

    public override string ToString()
    {
        return $"{SongsAddedMessage}{this.Songs.Count}{Environment.NewLine}"
               + string.Format(PlaylistLengthMessage, this.hours, this.minutes, this.seconds);
    }
}