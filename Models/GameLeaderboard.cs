namespace backend.Models
{
    public class GameLeaderboards
    {
        public Dictionary<string, LeaderboardFormation> Leaderboards { get; set; }
        public GameLeaderboards()
        {
            Leaderboards = new Dictionary<string, LeaderboardFormation>();
        }
         public void addLeaderboard(string trackGiven, string nameGiven, long timeGiven)
        {
            if(Leaderboards.ContainsKey(trackGiven)) {
                var newEntryInFormation = new LeaderboardEntry(nameGiven, timeGiven);
                Leaderboards[trackGiven].BestTimes.Add(newEntryInFormation);
            } 
            else
            {
                var newEntry = new LeaderboardFormation(trackGiven, new LeaderboardEntry(nameGiven, timeGiven));
                Leaderboards.Add(trackGiven, newEntry);
            }
            
        }
    }

    public class LeaderboardFormation
    {
        public string Track { get; set; }
        public List<LeaderboardEntry> BestTimes { get; set; }

        public LeaderboardFormation(string track_name, LeaderboardEntry entry)
        {
            this.Track = track_name;
            BestTimes = new List<LeaderboardEntry>();
            BestTimes.Add(entry);
        }
    }

    public class LeaderboardEntry
    {
        public string name { get; set; }
        public long time { get; set; }

        public LeaderboardEntry(string name, long time)
        {
            this.name = name;
            this.time = time;
        }
    }

}