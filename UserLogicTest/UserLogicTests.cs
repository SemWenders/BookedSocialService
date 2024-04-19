using DAL.Interfaces;
using Logic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;
using Moq;
using System.Diagnostics;

namespace UserLogicTest
{
    public class UserLogicTests
    {
        UserLogic userLogic;
        List<string> friends;

        public UserLogicTests()
        {
            var userRepoMock = new Mock<IUserRepository>();
            friends = new List<string>();            
            userRepoMock.Setup(m => m.AcceptFriendRequest(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((s, p) => friends.Add(s)).Returns(true);
            userLogic = new UserLogic(userRepoMock.Object);
        }

        [Fact]
        public void Test_RecipientAcceptsFriendRequest()
        {
            //arrange

            //act
            var result = userLogic.RespondToFriendRequest("responder", "sender", true);

            //arrange
            Assert.Contains("responder", friends);
        }

        [Fact]
        public void Test_RecipientDeniesFriendRequest()
        {
            //arrange

            //act
            var result = userLogic.RespondToFriendRequest("responder", "sender", false);

            //assert
            Assert.DoesNotContain("responder", friends);
        }
    }
}