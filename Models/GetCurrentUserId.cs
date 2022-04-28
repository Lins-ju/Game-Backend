namespace Backend.Models
{
    public class GetCurrentUserId
    {
        public Cars cars = new Cars();
        public string currentUserId { get; set; }

        public GetCurrentUserId(Leaderboard leaderboard)
        {

            foreach(var item in leaderboard.Leaderboards)
            {
                this.currentUserId = item.UserId;
            }
        }
    }
}