using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }
        
        public Post Post { get; set; }
        public AppUser User { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CommentRecieved
    {
        public String Body { get; set; }
        public int PostId { get; set; }
    }

    public class CommentDTOListItem
    {
        public int Id { get; set; }
        public string Body { get; set; }

        public AppUserDTO User { get; set; }

        public DateTime CreatedAt { get; set; }

        public static explicit operator CommentDTOListItem(Comment c)
        {
            CommentDTOListItem commentReturned = new CommentDTOListItem();

            commentReturned.Id = c.Id;
            commentReturned.Body = c.Body;
            commentReturned.User = (AppUserDTO)c.User;
            commentReturned.CreatedAt = c.CreatedAt;
            
            return commentReturned;
        }
    }

    public class CommentDTODetail
    {
        public int Id { get; set; }
        public string Body { get; set; }

        public int PostId { get; set; }
        public AppUserDTO User { get; set; }

        public DateTime CreatedAt { get; set; }

        public static explicit operator CommentDTODetail(Comment c)
        {
            CommentDTODetail commentReturned = new CommentDTODetail();

            commentReturned.Id = c.Id;
            commentReturned.Body = c.Body;
            commentReturned.PostId = c.Post.Id;
            commentReturned.User = (AppUserDTO)c.User;
            commentReturned.CreatedAt = c.CreatedAt;

            return commentReturned;
        }
    }
}
