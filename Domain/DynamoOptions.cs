namespace Backend.Domain
{
    public class DynamoOptions
    {
        public const string Section = "Dynamo";

        public string TableName { get; set; }

        public string ServiceURL { get; set; }

        public bool LocalMode { get; set; }
    }
}