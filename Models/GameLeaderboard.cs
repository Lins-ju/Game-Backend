using System.Collections.Generic;

namespace Backend.Models
{
    public class GameLeaderboards
    {
        public Dictionary<string, LeaderboardFormation> Leaderboards { get; set; }
        public GameLeaderboards()
        {
            Leaderboards = new Dictionary<string, LeaderboardFormation>();
        }
        public void AddLeaderboard(string TrackGiven, string NameGiven, long TimeGiven)
        {
            if(Leaderboards.ContainsKey(TrackGiven)) {
                var newEntryInFormation = new LeaderboardEntry(NameGiven, TimeGiven);
                Leaderboards[TrackGiven].BestTimes.Add(newEntryInFormation);
            } 
            else
            {
                var newEntry = new LeaderboardFormation(TrackGiven, new LeaderboardEntry(NameGiven, TimeGiven));
                Leaderboards.Add(TrackGiven, newEntry);
            }
            
        }
    }

    public class LeaderboardFormation
    {
        public string Track { get; set; }
        public List<LeaderboardEntry> BestTimes { get; set; }

        public LeaderboardFormation(string TrackName, LeaderboardEntry entry)
        {
            this.Track = TrackName;
            BestTimes = new List<LeaderboardEntry>();
            BestTimes.Add(entry);
        }
    }

    public class LeaderboardEntry
    {
        public string Name { get; set; }
        public long Time { get; set; }

        public LeaderboardEntry(string Name, long Time)
        {
            this.Name = Name;
            this.Time = Time;
        }
    }

}