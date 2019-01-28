namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading;
    using ApplicationPrincipal;

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
                var name = ApplicationPrincipal.Current.Identity.Name;
                var user = entities.Users.FirstOrDefault(u => u.UserName.Equals(name));

                if (user == null)
                {
                    throw new Exception("Loged in User not found");
                }

                return user;
            }
        }
    }
}