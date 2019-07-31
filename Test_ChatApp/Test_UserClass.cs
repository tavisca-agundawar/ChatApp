using ChatApp;
using System;
using Xunit;
using FluentAssertions;

namespace Test_ChatApp
{
    public class Test_UserClass
    {
        [Theory]
        [InlineData("arnaw@1.1.1.1:1234", "arnaw")]
        [InlineData("paresh@1.1.1.1:1234", "paresh")]
        [InlineData("shravan@1.1.1.1:1234", "shravan")]
        [InlineData("dharna@1.1.1.1:1234", "dharna")]
        [InlineData("aditi_123@1.1.1.1:1234", "aditi_123")]
        public void test_User_Creation(string address, string username)
        {
            //Arrange
            User testUser;

            //Act
            testUser = new User(address);

            //Assert
            testUser.userAddress.Should().Be(address);
            testUser.userName.Should().Be(username);
        }
    }
}
