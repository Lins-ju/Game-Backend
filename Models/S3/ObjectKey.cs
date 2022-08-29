namespace Backend.Models.S3
{
    public class ObjectKey
    {
        public string Key { get; set; }

        public ObjectKey(string key)
        {
            Key = key;
        }
    }
}