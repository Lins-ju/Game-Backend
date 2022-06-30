using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Backend.Domain;
using Backend.Models;
using Microsoft.Extensions.Options;

namespace Backend.Persistence
{
    public class DynamoDatastore
    {
        private readonly Table _table;
        public DynamoDatastore(IAmazonDynamoDB client, IOptions<DynamoOptions> options)
        {
            this._table = Table.LoadTable(client, options.Value.TableName);
        }

        public async Task<bool> Insert(LeaderboardData leaderboardData)
        {
            var response = await _table.PutItemAsync(leaderboardData.ToDocument());
            return response != null;
        }

        public async Task<LeaderboardData?> GetItem(string partitionKey, string rangeKey)
        {
            var result = await _table.GetItemAsync(partitionKey, rangeKey);
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

            var search = _table.Query(queryFilter);

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

            foreach (var dataItem in leaderboardDataList)
            {
                foreach (var entryItem in leaderboardEntries.Leaderboards)
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

    }
}