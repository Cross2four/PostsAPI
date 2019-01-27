using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            PostsController controller = new PostsController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            PostsController controller = new PostsController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            PostsController controller = new PostsController();

            // Act
            controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void PostSetsLocationHeader()
        {
            // Arrange
            PostsController controller = new PostsController();

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
            Post post = new Post() { Id = 42, Body = "Product1" };
            var response = controller.Post(post);

            // Assert
            Assert.AreEqual("http://localhost/api/products/42", response.Headers.Location.AbsoluteUri);
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            PostsController controller = new PostsController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            PostsController controller = new PostsController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
