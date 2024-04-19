using DAL.Interfaces;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SocialServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SocialController : ControllerBase
    {
        private readonly ILogger<SocialController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserLogic _userLogic;

        public SocialController(ILogger<SocialController> logger, IUserRepository userRepository, IUserLogic userLogic)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userLogic = userLogic;
        }

        [HttpPost("SendFriendRequest/{userid}/{friendid}")]
        public ActionResult SendFriendRequest(string userid, string friendid)
        {
            if (_userRepository.SendFriendRequest(userid, friendid))
            {
                return Ok();
            } else
            {
                return NotFound();
            }
        }

        [HttpPost("RespondFriendRequest/{responderId}/{senderId}/{accept}")]
        public ActionResult RespondToFriendRequest(string responderId, string senderId, bool accept)
        {
            if (_userLogic.RespondToFriendRequest(responderId, senderId, accept))
            {
                return Ok();
            } else
            {
                return NotFound();
            }
        }

        [HttpGet("GetFriends/{id}")]
        public ActionResult<List<string>> GetFriends(string id)
        {
            var result = _userRepository.GetFriends(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
