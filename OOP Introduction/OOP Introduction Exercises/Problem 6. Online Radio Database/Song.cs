public class Song
{
    private const long MinPerformerNameLength = 3;
    private const long MaxPerformerNameLength = 20;
    private const long MinSongNameLength = 3;
    private const long MaxSongNameLength = 30;
    private const long MinSongMinutes = 0;
    private const long MaxSongMinutes = 14;
    private const long MinSongSeconds = 0;
    private const long MaxSongSeconds = 59;

    private string performer;
    private string title;
    private long minutes;
    private long seconds;

    public Song(string performer, string title, long minutes, long seconds)
    {
        this.Performer = performer;
        this.Title = title;
        this.Minutes = minutes;
        this.Seconds = seconds;
    }

    public string Performer
    {
        get
        {
            return this.performer;
        }

        set
        {
            if (string.IsNullOrEmpty(value) || value.Length < MinPerformerNameLength
                || value.Length > MaxPerformerNameLength)
            {
                throw new InvalidArtistNameException("Artist name should be between 3 and 20 symbols.");
            }

            this.performer = value;
        }
    }

    public string Title
    {
        get
        {
            return this.title;
        }

        set
        {
            if (string.IsNullOrEmpty(value) || value.Length < MinSongNameLength
                || value.Length > MaxSongNameLength)
            {
                throw new InvalidSongNameException("Song name should be between 3 and 30 symbols.");
            }

            this.title = value;
        }
    }

    public long Minutes
    {
        get
        {
            return this.minutes;
        }

        set
        {
            if (value < MinSongMinutes
                || value > MaxSongMinutes)
            {
                throw new InvalidSongMinutesException("Song minutes should be between 0 and 14.");
            }

            this.minutes = value;
        }
    }

    public long Seconds
    {
        get
        {
            return this.seconds;
        }

        set
        {
            if (value < MinSongSeconds
                || value > MaxSongSeconds)
            {
                throw new InvalidSongSecondsException("Song seconds should be between 0 and 59.");
            }

            this.seconds = value;
        }
    }
}