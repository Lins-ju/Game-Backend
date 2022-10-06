using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Backend.Domain;
using Backend.Models;
using Microsoft.Extensions.Options;
using Amazon.CloudFront;
using Backend.Models.S3;
using Amazon.Runtime;

namespace Backend.Persistence
{
    public class DynamoDatastore
    {
        private readonly Table _tableLB;
        private readonly Table _tableGU;
        // public DynamoDatastore(IAmazonDynamoDB client, IOptions<DynamoOptions> options)
        // {
        //     this._tableLB = Table.LoadTable(client, options.Value.TableName);
        // }

        public DynamoDatastore()
        {
            var creds = new BasicAWSCredentials("fakeMyKeyId", "fakeSecretAccessKey");
            var clientConfigDynamo = new AmazonDynamoDBConfig
            {
                ServiceURL = "http://localhost:4566",
                AuthenticationRegion = "us-east-1"
            };
            var client = new AmazonDynamoDBClient(creds, clientConfigDynamo);
            _tableLB = Table.LoadTable(client, "Leaderboard");
            _tableGU = Table.LoadTable(client, "Users");
        }

        public async Task<bool> Insert(LeaderboardData leaderboardData)
        {
            var response = await _tableLB.PutItemAsync(LeaderboardData.ToDocument(leaderboardData));
            return response == null;
        }

        public async Task<LeaderboardData?> GetItem(string partitionKey, string rangeKey)
        {
            var result = await _tableLB.GetItemAsync(partitionKey, rangeKey);
            if (result != null)
            {
                return LeaderboardData.FromDocument(result);
            }
            else
            {
                return null;
            }
        }
        public async Task<List<LeaderboardData>> LeadeboardDataListByTrackId(string partitionKey)
        {
            QueryFilter queryFilter = new QueryFilter();

            queryFilter.AddCondition("TrackId", QueryOperator.Equal, partitionKey);

            var search = _tableLB.Query(queryFilter);

            var retrievedDocuments = new List<Document>();

            do
            {
                var searchResult = await search.GetNextSetAsync();
                retrievedDocuments.AddRange(searchResult);
            } while (!search.IsDone);

            List<LeaderboardData> leaderboardData = new List<LeaderboardData>();

            foreach (var item in retrievedDocuments)
            {
                var fromDocument = LeaderboardData.FromDocument(item);
                leaderboardData.Add(fromDocument);
            }

            return leaderboardData;
        }

        public List<LeaderboardDetail> DisplayFullLeaderboard(Leaderboard leaderboardEntries, List<LeaderboardData> leaderboardDataList)
        {
            List<LeaderboardDetail> leaderboardDetailList = new List<LeaderboardDetail>();
            LeaderboardDetail leaderboardDetail = new LeaderboardDetail();

            foreach (var entryItem in leaderboardEntries.Leaderboards)
            {
                foreach (var dataItem in leaderboardDataList)
                {
                    if (entryItem == null)
                    {
                        leaderboardDetailList.Add(new LeaderboardDetail(dataItem));
                    }
                    else
                    {
                        if (entryItem.UserId == dataItem.UserId && !leaderboardDetailList.Contains(new LeaderboardDetail(dataItem, entryItem)))
                        {
                            leaderboardDetailList.Add(new LeaderboardDetail(dataItem, entryItem));
                        }
                    }
                }
            }
            return leaderboardDetailList;
        }
        public async Task<bool> InsertUser(string id, string userName, CarCollectionList carCollection)
        {
            var result = await _tableGU.PutItemAsync(GameUser.ToDocument(new GameUser(id, userName, carCollection)));
            return result == null;
        }

        public async Task<GameUser> GetGameUserInfo(string userName)
        {
            var gameUserDocument = await _tableGU.GetItemAsync(userName);
            var gameUser = GameUser.FromDocument(gameUserDocument);
            return gameUser;
        }
    }
}