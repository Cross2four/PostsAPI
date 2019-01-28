using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Routing;
using DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostsAPI.Controllers;

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
            Assert.AreEqual(8, posts.Count);
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
            controller.Request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            controller.Configuration = new HttpConfiguration();

            // Act
            PostRecieved post = new PostRecieved() { Body = "Product1" };
            var response = controller.Post(post);
            
            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual(json, post);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PostSetsLocationHeaderAsync()
        {
            // Arrange
            PostsController controller = new PostsController();

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/products"),
                Method = HttpMethod.Post
            };

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

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
            Assert.AreEqual("http://localhost/api/products/", response.Headers.Location.AbsoluteUri);
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

            controller.Configuration = new HttpConfiguration();

            // Act
            PostRecieved post = new PostRecieved() { Body = "Product1" };
            var response = controller.Put(3, post);

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual(json, post);
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


            controller.Configuration = new HttpConfiguration();

            // Act

            var response = controller.Delete(3);
            
            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
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


            controller.Configuration = new HttpConfiguration();

            // Act

            var response = controller.Delete(100);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
