namespace Backend.Models.S3
{
    public class ObjectKey
    {
        public string Key { get; set; }
        public int Id { get; set; }

        public ObjectKey(string key)
        {
            Key = key;
            Id = 0;
        }

        public ObjectKey(int id)
        {
            Id = id;
            Key = "";
        }
    }
}