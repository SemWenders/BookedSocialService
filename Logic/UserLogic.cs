using DAL.Interfaces;
using Logic.Interfaces;

namespace Logic
{
    public class UserLogic : IUserLogic
    {
        IUserRepository _userRepository;
        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public bool RespondToFriendRequest(string responderId, string senderId, bool accept)
        {
            if (accept)
            {
                return _userRepository.AcceptFriendRequest(responderId, senderId);
            } else
            {
                return _userRepository.DenyFriendRequest(responderId, senderId);
            }
        }
    }
}
