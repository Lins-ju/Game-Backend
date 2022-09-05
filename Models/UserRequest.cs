namespace Backend.Models
{
    public class UserRequest
    {
        public string UserName { get; set; }

        public UserRequest(string userName)
        {
            UserName = userName;
        }
    }
}