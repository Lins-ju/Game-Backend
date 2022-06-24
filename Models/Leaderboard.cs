using StackExchange.Redis;

namespace Backend.Models
{
    public class Leaderboard
    {
        public List<LeaderboardEntry> Leaderboards = new List<LeaderboardEntry>();
        public Leaderboard(SortedSetEntry[] entries)
        {
            Leaderboards = new List<LeaderboardEntry>();
            foreach(var obj in entries)
            {
                Leaderboards.Add(new LeaderboardEntry(obj));
            }
        }

        public Leaderboard()
        {
            
        }
    }
}