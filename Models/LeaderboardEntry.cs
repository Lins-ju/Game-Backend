using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace Backend.Models
{
    public class LeaderboardEntry
    {
        [JsonPropertyName("userId")]   
        public string UserId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        public LeaderboardEntry(SortedSetEntry sortedSet)
        {
            this.UserId = sortedSet.Element;
            this.Score = sortedSet.Score;
        }

        public LeaderboardEntry()
        {
            
        }
    }
}