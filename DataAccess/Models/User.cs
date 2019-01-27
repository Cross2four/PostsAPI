using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace DataAccess.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public static User GetAuthedUser()
        {
            using (DataModel entities = new DataModel())
            {
                var user = entities.Users.FirstOrDefault(u => u.UserName.Equals(Thread.CurrentPrincipal.Identity.Name));

                if (user == null)
                {
                    throw new Exception("Loged in User not found");
                }

                return user;
            }
        }
    }
}