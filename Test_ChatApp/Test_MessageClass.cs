using ChatApp;
using System;
using Xunit;
using FluentAssertions;

namespace Test_ChatApp
{
    public class Test_MessageClass
    {
        [Fact]
        public void test_Message_Creation()
        {
            //Arrange
            Message newMessage;

            //Act
            newMessage = new Message("Hello!");

            //Assert
            Assert.Equal("Hello!", newMessage.message);
            Assert.Equal(DateTime.Now.Hour, newMessage.timeStamp.Hour);
            Assert.Equal(DateTime.Now.Minute, newMessage.timeStamp.Minute);
        }

        [Theory]
        [InlineData("I","I")]
        [InlineData("Hello!", "Hello!")]
        [InlineData("!@#$%^&*", "!@#$%^&*")]
        [InlineData("This is a big string.", "This is a big string.")]
        public void test_Message_Creation_Differently(string message, string expected)
        {
            //Arrange
            Message newMessage;

            //Act
            newMessage = new Message(message);

            //Assert
            newMessage.message.Should().Be(expected);
            newMessage.timeStamp.Hour.Should().Be(DateTime.Now.Hour);
            newMessage.timeStamp.Minute.Should().Be(DateTime.Now.Minute);
        }
    }
}
