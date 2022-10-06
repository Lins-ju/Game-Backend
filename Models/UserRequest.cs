namespace Backend.Models
{
    public class UserRequest
    {
        public string UserId { get; set; }

        public UserRequest()
        {

        }
        public UserRequest(string userId)
        {
            UserId = userId;
        }
    }
}