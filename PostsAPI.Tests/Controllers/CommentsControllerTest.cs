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
            controller.Request = new HttpRequestMessage();

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            controller.Configuration = new HttpConfiguration();

            // Act
            CommentRecieved comment = new CommentRecieved { Body = "Product1" };
            var response = controller.Post(comment);

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual(json, comment);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PostSetsLocationHeaderAsync()
        {
            // Arrange
            CommentsController controller = new CommentsController();

            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/products")
            };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary { { "controller", "posts" } });

            // Act
            CommentRecieved comment = new CommentRecieved() { Body = "Product1" };
            var response = controller.Post(comment);

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual("http://localhost/api/products/42", response.Headers.Location.AbsoluteUri);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task PutAsync()
        {
            // Arrange
            CommentsController controller = new CommentsController();
            controller.Request = new HttpRequestMessage();

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            controller.Configuration = new HttpConfiguration();

            // Act
            CommentRecieved comment = new CommentRecieved() { Body = "Product1" };
            var response = controller.Put(3, comment);

            string responseBody = await response.Content.ReadAsStringAsync();
            var json = Json.Decode(responseBody);

            // Assert
            Assert.AreEqual(json, comment);
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            CommentsController controller = new CommentsController();
            controller.Request = new HttpRequestMessage();

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
            // Arrange
            CommentsController controller = new CommentsController();
            controller.Request = new HttpRequestMessage();

            controller.Request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(
                    string.Format("{0}:{1}", "user1", "password1"))));

            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.Delete(300);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
