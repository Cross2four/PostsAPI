using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostsAPI.Controllers;
using DataAccess;
using System.Linq;

namespace PostsAPI.Tests.Controllers
{
    [TestClass]
    public class PostsControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            var controller = new PostsController();
            controller.Request = new HttpRequestMessage();

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get();

            // Assert
            List<PostReturned> posts;
            Assert.IsTrue(response.TryGetContentValue<List<PostReturned>>(out posts));

            using (DataModel entities = new DataModel())
            {
                int postsCount = entities.Posts.Count();
                Assert.AreEqual(postsCount, posts.Count);
            }
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            var controller = new PostsController();
            controller.Request = new HttpRequestMessage();

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Get(10);

            // Assert
            PostReturned post = new PostReturned { Id = 10 };
            Assert.IsTrue(response.TryGetContentValue<PostReturned>(out post));
            Assert.AreEqual(10, post.Id);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PostAsync()
        {
            // Arrange
            PostsController controller = new PostsController();
            controller.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/posts");
            
            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            var filter = new AuthenticationAttribute();
            filter.OnAuthorization(controller.ActionContext);

            controller.Configuration = new HttpConfiguration();

            // Act
            PostRecieved post = new PostRecieved() { Body = "Product1" };
            var response = controller.Post(post);
            
            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual(json.Body, post.Body);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PostSetsLocationHeaderAsync()
        {
            // Arrange
            PostsController controller = new PostsController();

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/posts"),
                Method = HttpMethod.Post
            };

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            var filter = new AuthenticationAttribute();
            filter.OnAuthorization(controller.ActionContext);

            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "posts" } });

            // Act
            PostRecieved post = new PostRecieved() { Body = "Product1" };
            var response = controller.Post(post);

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual("http://localhost/api/posts/" + json.Id, response.Headers.Location.AbsoluteUri);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PutAsync()
        {
            // Arrange
            PostsController controller = new PostsController();
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Put
            };

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            var filter = new AuthenticationAttribute();
            filter.OnAuthorization(controller.ActionContext);

            controller.Configuration = new HttpConfiguration();

            // Act
            using (DataModel entities = new DataModel())
            {
                var posts = entities.Posts.ToList();

                PostRecieved post = new PostRecieved() { Body = "Product1" };
                var response = controller.Put(posts.Last().Id, post);

                // Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                string responseBody = await response.Content.ReadAsStringAsync();
                var json = Json.Decode(responseBody);

                // Assert
                Assert.AreEqual(json.Body, post.Body);
            }
        }

        [TestMethod]
        public void Delete()
        {
            PostsController controller = new PostsController();
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete
            };

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            var filter = new AuthenticationAttribute();
            filter.OnAuthorization(controller.ActionContext);

            controller.Configuration = new HttpConfiguration();

            // Act

            using (DataModel entities = new DataModel())
            {
                var posts = entities.Posts.ToList();
                var response = controller.Delete(posts.Last().Id);

                // Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [TestMethod]
        public void Delete_DoesntExist()
        {
            PostsController controller = new PostsController();
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete
            };

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            var filter = new AuthenticationAttribute();
            filter.OnAuthorization(controller.ActionContext);

            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Delete(1000);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
