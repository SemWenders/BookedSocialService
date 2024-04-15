using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUserRepository : IAsyncDisposable
    {
        List<string> GetFriends(string id);
        bool SendFriendRequest(string userid, string friendid);
        bool AcceptFriendRequest(string responderId, string senderId);
        bool DenyFriendRequest(string responderId, string senderId);
    }
}
