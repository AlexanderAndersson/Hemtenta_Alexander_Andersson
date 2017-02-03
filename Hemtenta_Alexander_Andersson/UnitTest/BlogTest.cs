using NUnit.Framework;
using HemtentaTdd2017.blog;
using HemtentaTdd2017;
using System;
using Moq;

namespace UnitTest
{
    [TestFixture]
    public class BlogTest
    {
        Mock<IAuthenticator> auth;
        private Blog b;
        private User defaultUser;

        [SetUp]
        public void SetUp()
        {
            auth = new Mock<IAuthenticator>();          
            b = new Blog(auth.Object);
            defaultUser = new User("Alex");
        }

        [Test]
        [TestCase(null, "password")]
        [TestCase("Username", null)]
        public void LogInUser_IncorrectValues_Throws(string name, string password)
        {
            User u = new User(name);
            u.Password = password;

            Assert.Throws<UserIsNullException>(() => b.LoginUser(u), "user is null");
        }

        [Test]
        [TestCase("", "password")]
        [TestCase("Username", "")]
        [TestCase(" ", "password")] //Whitespace
        [TestCase("Username", " ")] //Whitespace
        public void LogInUser_IncorrectValues_Failed(string name, string password)
        {
            User u = new User(name);
            u.Password = password;
            b.LoginUser(u);

            Assert.IsFalse(b.UserIsLoggedIn, "Successfull login");
        }

        [Test]
        [TestCase("Username")]
        public void LogInUser_Success(string name)
        {
            User u = new User(name);
            auth.Setup(x => x.GetUserFromDatabase(name)).Returns(new User(name));
            b.LoginUser(u);

            Assert.IsTrue(b.UserIsLoggedIn, "Login failed");
        }

        [Test]
        [TestCase(null)]
        public void LogOutUser_UserIsNull_Throw(string name)
        {
            Assert.Throws<UserIsNullException>(() => b.LogoutUser(null)); 
        }
       
        [Test]
        [TestCase("Username")]
        public void LogOutUser_Failed_NotSignedIn(string name)
        {
            User u = new User(name);
            b.LogoutUser(u);

            Assert.IsFalse(b.UserIsLoggedIn);
        }

        [Test]
        public void LogOutUser_Success()
        {
            auth.Setup(x => x.GetUserFromDatabase(defaultUser.Name)).Returns(new User(defaultUser.Name));
            b.LoginUser(defaultUser);
            b.LogoutUser(defaultUser);

            Assert.IsFalse(b.UserIsLoggedIn);
        }

        [Test]
        [TestCase("Title", null)]
        [TestCase(null, "Content")]
        public void PublishPage_IncorrectValues_Throws(string title, string content)
        {
            auth.Setup(x => x.GetUserFromDatabase(defaultUser.Name)).Returns(new User(defaultUser.Name));
            b.LoginUser(defaultUser);
            Page p = new Page
            {
                Title = title,
                Content = content             
            };
            Assert.Throws<PageContentIsNotValidException>(() => b.PublishPage(p));
        }

        [Test]
        [TestCase("Title", "Content")]
        public void PublishPage_Failed_NotSignedIn(string title, string content)
        {
            Page p = new Page
            {
                Title = title,
                Content = content
            };
            Assert.IsFalse(b.PublishPage(p), "Page was published");
        }

        [Test]
        [TestCase("Title", "Content")]
        public void PublishPage_Success(string title, string content)
        {
            auth.Setup(x => x.GetUserFromDatabase(defaultUser.Name)).Returns(new User(defaultUser.Name));
            b.LoginUser(defaultUser);          
            Page p = new Page
            {
                Title = title,
                Content = content
            };
            Assert.IsTrue(b.PublishPage(p), "Publish failed");
        }

        [Test]
        [TestCase("", "caption", "some text")]
        [TestCase("alex@hotmail.com", "", "some text")]
        [TestCase("alex@hotmail.com", "caption", "")]
        [TestCase(null, "caption", "some text")]
        [TestCase("alex@hotmail.com", null, "some text")]
        [TestCase("alex@hotmail.com", "caption", null)]
        [TestCase(" ", "caption", "some text")] //Whitespace
        [TestCase("alex@hotmail.com", " ", "some text")] //Whitespace
        [TestCase("alex@hotmail.com", "caption", " ")] //Whitespace
        public void SendEmail_IncorrectValues_Failed(string address, string caption, string body)
        {
            auth.Setup(x => x.GetUserFromDatabase(defaultUser.Name)).Returns(new User(defaultUser.Name));
            b.LoginUser(defaultUser);
            int result = b.SendEmail(address, caption, body);

            Assert.AreEqual(0, result, "The message was sent");
        }

        [Test]
        [TestCase("alex@hotmail.com", "caption", "some text")]
        public void SendEmail_Failed_NotSignedIn(string address, string caption, string body)
        {
            int result = b.SendEmail(address, caption, body);
            Assert.AreEqual(0, result, "The message was sent");
        }

        [Test]
        [TestCase("alex@hotmail.com", "caption", "some text")]
        public void SendEmail_Success(string address, string caption, string body)
        {
            auth.Setup(x => x.GetUserFromDatabase(defaultUser.Name)).Returns(new User(defaultUser.Name));
            b.LoginUser(defaultUser);
            int result = b.SendEmail(address, caption, body);
            
            Assert.AreEqual(1, result, "Failed to send email");
        }
    }
}
