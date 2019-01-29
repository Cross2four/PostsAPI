using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }

        public AppUser User { get; set; }
        
        public ICollection<Comment> Comments { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class PostRecieved
    {
        public String Body { get; set; }
    }

    public class PostReturned
    {
        public int Id { get; set; }
        public String Body { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public static explicit operator PostReturned(Post v)
        {
            PostReturned postReturned = new PostReturned();

            postReturned.Id = v.Id;
            postReturned.Body = v.Body;
            postReturned.UserId = v.User.Id;
            postReturned.CreatedAt = v.CreatedAt;

            return postReturned;
        }
    }
}