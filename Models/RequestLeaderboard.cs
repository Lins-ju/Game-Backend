namespace Backend.Models
{
    public class RequestLeaderboard
    {
        public string UserName { get; set; }
        public double Score { get; set; }
        public string CarName { get; set; }
        public string CarImg { get; set; }

        public RequestLeaderboard()
        {
            
        }

        public RequestLeaderboard(string userName, double score, string carName, string carImg)
        {
            UserName = userName;
            Score = score;
            CarName = carName;
            CarImg = carImg;
        }
    }
}