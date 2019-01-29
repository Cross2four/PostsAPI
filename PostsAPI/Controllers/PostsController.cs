using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess;
using DataAccess.Models;

namespace PostsAPI.Controllers
{
    // [Authorize]
    public class PostsController : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get(string search = "")
        {
            using (DataModel entities = new DataModel())
            {
                IEnumerable<PostReturned> list;

                List<string> terms = search.Split(' ').ToList();

                if (!search.Equals(""))
                {
                    list = entities.Posts.Include("User").Where(p => terms.All(t => p.Body.Contains(t))).Select(s => new PostReturned { Id = s.Id, Body = s.Body, UserId = s.User.Id, CreatedAt = s.CreatedAt }).ToList();
                } else
                {
                    list = entities.Posts.Select(s => new PostReturned { Id = s.Id, Body = s.Body, UserId = s.User.Id, CreatedAt = s.CreatedAt }).ToList();
                }

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {

            using (DataModel entities = new DataModel())
            {
                var post = entities.Posts.Include("User").Include("Comments").FirstOrDefault(e => e.Id.Equals(id));

                if (post != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, (PostReturned)post);
                } else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Post with ID: {id.ToString()} was not found");
                }
            }
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]PostRecieved postRecieved)
        {
            try
            {
                using (DataModel entities = new DataModel())
                {
                    Post postEntity = new Post();

                    postEntity.Body = postRecieved.Body;
                    postEntity.CreatedAt = DateTime.Now;

                    var user = DataAccess.Models.AppUser.AuthedUser;

                    if (user == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }

                    postEntity.User = entities.Users.FirstOrDefault(u => u.Id == user.Id);

                    entities.Posts.Add(postEntity);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, (PostReturned)postEntity);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + postEntity.Id.ToString());

                    return message;
                }
            } catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
        }

        // PUT api/values/5
        public HttpResponseMessage Put(int id, [FromBody]PostRecieved post)
        {
            try
            {
                using (DataModel entities = new DataModel())
                {
                    var postEntity = entities.Posts.FirstOrDefault(p => p.Id.Equals(id));

                    if (postEntity != null)
                    {
                        postEntity.Body = post.Body;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, postEntity);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Unable to update. Post with ID: {id.ToString()} was not found");
                    }
                }
            } catch (Exception ex)
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
                    var post = entities.Posts.FirstOrDefault(p => p.Id == id);

                    if (post != null)
                    {
                        entities.Posts.Remove(post);
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    } else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Unable to delete. Post with ID: {id.ToString()} was not found");
                    }
                }
            } catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
