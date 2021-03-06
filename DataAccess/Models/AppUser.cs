﻿namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ApplicationPrincipal;

    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }

        [StringLength(100)]
        [Index(IsUnique = true)]
        public String UserName { get; set; }
        public String Password { get; set; }
        
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public static AppUser AuthedUser
        {
            get
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

    public class AppUserDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public static explicit operator AppUserDTO(AppUser v)
        {
            AppUserDTO userDTO = new AppUserDTO();

            userDTO.Id = v.Id;
            userDTO.Name = v.Name;

            return userDTO;
        }
    }
}