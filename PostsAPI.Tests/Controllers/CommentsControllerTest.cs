using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostsAPI.Controllers;
using DataAccess.Models;
using System.Web.Http.Routing;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Web.Helpers;
using System.Net.Http.Headers;
using System.Net;
using DataAccess;

namespace PostsAPI.Tests.Controllers
{
    /// <summary>
    /// Summary description for CommentsControllerTest
    /// </summary>
    [TestClass]
    public class CommentsControllerTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task PostAsync()
        {
            // Arrange
            CommentsController controller = new CommentsController();
            controller.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/comments");

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            var filter = new AuthenticationAttribute();
            filter.OnAuthorization(controller.ActionContext);

            controller.Configuration = new HttpConfiguration();

            // Act
            CommentRecieved comment = new CommentRecieved() { Body = "Comment", PostId = 1 };
            var response = controller.Post(comment);

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual(json.Body, comment.Body);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PostSetsLocationHeaderAsync()
        {
            // Arrange
            CommentsController controller = new CommentsController();

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/comments"),
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
                values: new HttpRouteValueDictionary { { "controller", "comments" } });

            // Act
            CommentRecieved comment = new CommentRecieved() { Body = "Comment", PostId = 1 };
            var response = controller.Post(comment);

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual("http://localhost/api/posts/" + json.PostId, response.Headers.Location.AbsoluteUri);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PutAsync()
        {
            // Arrange
            CommentsController controller = new CommentsController();
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
                var lastComment = entities.Comments.ToList().Last();

                CommentRecieved comment = new CommentRecieved() { Body = "Comment" };
                var response = controller.Put(lastComment.Id, comment);

                // Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

                string responseBody = await response.Content.ReadAsStringAsync();
                var json = Json.Decode(responseBody);

                // Assert
                Assert.AreEqual(json.Body, comment.Body);
            }
        }

        [TestMethod]
        public void Delete()
        {
            CommentsController controller = new CommentsController();
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
                var lastComment = entities.Comments.ToList().Last();
                var response = controller.Delete(lastComment.Id);

                // Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [TestMethod]
        public void Delete_DoesntExist()
        {
            CommentsController controller = new CommentsController();
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
