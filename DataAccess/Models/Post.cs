using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

    public class PostDTOListItem
    {
        public int Id { get; set; }
        public String Body { get; set; }

        public AppUserDTO User { get; set; }

        public int CommentCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class PostDTODetail
    {
        public int Id { get; set; }
        public String Body { get; set; }

        public AppUserDTO User { get; set; }

        public List<CommentDTOListItem> Comments { get; set; }

        public DateTime CreatedAt { get; set; }

        public static explicit operator PostDTODetail(Post p)
        {
            PostDTODetail postReturned = new PostDTODetail();

            postReturned.Id = p.Id;
            postReturned.Body = p.Body;
            postReturned.User = (AppUserDTO)p.User;

            List<CommentDTOListItem> commentDTOs = new List<CommentDTOListItem>();

            if (p.Comments != null)
            {
                foreach (Comment c in p.Comments.ToList())
                {
                    commentDTOs.Add((CommentDTOListItem)c);
                }
            }

            postReturned.Comments = commentDTOs;
            postReturned.CreatedAt = p.CreatedAt;

            return postReturned;
        }
    }
}