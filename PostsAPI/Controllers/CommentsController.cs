using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PostsAPI.Controllers
{
    public class CommentsController : ApiController
    {
        // POST api/values
        public HttpResponseMessage Post([FromBody]CommentRecieved commentRecieved)
        {
            try
            {
                using (DataModel entities = new DataModel())
                {
                    Comment commentEntity = new Comment();

                    commentEntity.Body = commentRecieved.Body;
                    commentEntity.CreatedAt = DateTime.Now;

                    AppUser user = AppUser.AuthedUser;

                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }

                    Post post = entities.Posts.FirstOrDefault(p => p.Id == commentRecieved.PostId);

                    if (post == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, $"Unable to update. Post with ID: { commentRecieved.PostId.ToString() }was not found");
                    }

                    commentEntity.User = entities.Users.FirstOrDefault(u => u.Id == user.Id);
                    commentEntity.Post = post;

                    entities.Comments.Add(commentEntity);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, (CommentDTODetail)commentEntity);
                    message.Headers.Location = new Uri(Request.RequestUri.OriginalString.Replace("comments", "posts") + "/" + commentEntity.Post.Id.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // PUT api/values/5
        public HttpResponseMessage Put(int id, [FromBody]CommentRecieved comment)
        {
            try
            {
                using (DataModel entities = new DataModel())
                {
                    var commentEntity = entities.Comments.FirstOrDefault(p => p.Id == id);

                    if (commentEntity != null)
                    {
                        commentEntity.Body = comment.Body;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, commentEntity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Unable to update. Post with ID: {id.ToString()} was not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/values/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (DataModel entities = new DataModel())
                {
                    var comment = entities.Comments.FirstOrDefault(p => p.Id == id);

                    if (comment != null)
                    {
                        entities.Comments.Remove(comment);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Unable to delete. Comment with ID: {id.ToString()} was not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
