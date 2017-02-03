using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.blog
{
    public class Blog : IBlog
    {
        private IAuthenticator auth;

        public Blog(IAuthenticator auth)
        {
            this.auth = auth;
        }

        public IAuthenticator Auth
        {
            get { return auth; }
            set { auth = value; }
        }

        public bool UserIsLoggedIn { get; set; }

        public void LoginUser(User u)
        {
            bool IsNull = u.Name == null || u.Password == null;
            bool IsEmpty = u.Name == "" || u.Password == "";

            if (IsNull)
                throw new UserIsNullException("User is null");

            else if (u.Name.All(char.IsWhiteSpace) || u.Password.All(char.IsWhiteSpace) || IsEmpty)
                UserIsLoggedIn = false;

            else
            {
                var getUser = auth.GetUserFromDatabase(u.Name);

                if (u.Name == getUser.Name && u.Password == getUser.Password)
                    UserIsLoggedIn = true;
                else
                    UserIsLoggedIn = false;                           
            }              
        }

        public void LogoutUser(User u)
        {
            if (u == null)
                throw new UserIsNullException("User is null");
            else
                UserIsLoggedIn = false;
        }

        public bool PublishPage(Page p)
        {
            bool NullOrEmpty = string.IsNullOrEmpty(p.Title) || string.IsNullOrEmpty(p.Content);
            bool NullOrWhiteSpace = string.IsNullOrWhiteSpace(p.Title) || string.IsNullOrWhiteSpace(p.Content);

            if (UserIsLoggedIn == false)
                return false;

            else if (NullOrEmpty || NullOrWhiteSpace)
                throw new PageContentIsNotValidException("Page is not a valid object");
      
            else
                return true;
        }

        public int SendEmail(string address, string caption, string body)
        {
            bool NullOrEmpty = string.IsNullOrEmpty(address) || string.IsNullOrEmpty(caption) || string.IsNullOrEmpty(body);
            bool NullOrWhiteSpace = string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(caption) || string.IsNullOrWhiteSpace(body);

            if (NullOrEmpty || NullOrWhiteSpace || UserIsLoggedIn == false)
                return 0;
            return 1;
        }
    }
}
