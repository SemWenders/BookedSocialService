using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private IAsyncSession _session;
        private ILogger<UserRepository> _logger;
        private string _database;

        public UserRepository(IDriver driver, ILogger<UserRepository> logger, IOptions<ApplicationSettings> appSettingsOptions)
        {
            _logger = logger;
            _database = appSettingsOptions.Value.Neo4jDatabase ?? "neo4j";
            _session = driver.AsyncSession(o => o.WithDatabase(_database));
        }
        public List<string> GetFriends(string id)
        {
            try
            {
                var friendIDs = new List<string>();
                var query = @"MATCH (u:User)-[:FRIENDS_WITH]-(p:User) WHERE u.id = $id RETURN p.id";
                var reader = _session.RunAsync(query, new { id = id }).Result;

                while (reader.FetchAsync().Result)
                {
                    var temp = reader.Current[0].ToString();
                    friendIDs.Add(temp);
                }
                return friendIDs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }
        
        public bool SendFriendRequest(string userid, string friendid)
        {
            try
            {
                var query = @"MATCH (u:User {id:$userid}), (p:User {id:$friendid}) MERGE (u)-[r:SEND_FRIEND_REQUEST]->(p) RETURN r";
                
                var result = _session.ExecuteWriteAsync(tx => tx.RunAsync(query, new { userid = userid, friendid = friendid}).Result.ConsumeAsync()).Result;
                if (result.Counters.RelationshipsCreated == 0)
                {
                    return false;
                } else
                {
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }        

        public bool AcceptFriendRequest(string responderId, string senderId)
        {
            try
            {
                var query = @"MATCH (n:User {id:$senderId})-[r:SEND_FRIEND_REQUEST]->(p:User {id: $responderId}) "
                                + "DELETE r "
                                + "MERGE (n)-[friends:FRIENDS_WITH]-(p) "
                                + "RETURN friends";
                var result = _session.ExecuteWriteAsync(tx => tx.RunAsync(query, new { responderId = responderId, senderId = senderId }).Result.ConsumeAsync()).Result;
                if (result.Counters.RelationshipsCreated  == 1 && result.Counters.RelationshipsDeleted == 1)
                {
                    return true;
                } else
                {
                    return false;
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }

        public bool DenyFriendRequest(string responderId, string senderId)
        {
            try
            {
                var query = @"MATCH (n:User {id:$responderId})-[r:SEND_FRIEND_REQUEST]-(p:User {id: $senderId}) DELETE r RETURN n, p";
                var result = _session.ExecuteWriteAsync(tx => tx.RunAsync(query, new { responderId = responderId, senderId = senderId }).Result.ConsumeAsync()).Result;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was a problem while executing database query");
                throw;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await _session.DisposeAsync();
        }
    }
}
