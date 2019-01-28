using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }
        
        public Post Post { get; set; }
        
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CommentRecieved
    {
        public String Body { get; set; }
        public object PostId { get; set; }
    }

    public class CommentReturned
    {
        public int Id { get; set; }
        public string Body { get; set; }

        public int PostId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public static explicit operator CommentReturned(Comment v)
        {
            CommentReturned commentReturned = new CommentReturned();

            commentReturned.Id = v.Id;
            commentReturned.Body = v.Body;
            commentReturned.PostId = v.Post.Id;
            commentReturned.UserId = v.User.Id;
            commentReturned.CreatedAt = v.CreatedAt;
            
            return commentReturned;
        }
    }
}
