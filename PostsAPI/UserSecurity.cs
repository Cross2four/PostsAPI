namespace PostsAPI
{
    using System;
    using System.Linq;
    using DataAccess;
    using PasswordHasher;

    public class UserSecurity
    {
        public static Boolean Login(string username, string password)
        {
            using(DataModel entities = new DataModel())
            {
                var user = entities.Users.FirstOrDefault(u => u.UserName.Equals(username));

                if (user != null)
                {
                    return PasswordHasher.Verify(password, user.Password);
                }

                return false;
            }
        }
    }
}