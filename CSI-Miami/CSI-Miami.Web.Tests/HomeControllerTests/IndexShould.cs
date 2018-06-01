using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using CSI_Miami.Infrastructure.Providers.Contracts;
using CSI_Miami.Services.Internal.Contracts;
using CSI_Miami.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CSI_Miami.Web.Tests.HomeControllerTests
{
    [TestClass]
    public class IndexShould
    {

        private Mock<IMovieService> movieServiceMock;
        private Mock<IMappingProvider> mapperMock;
        private Mock<IUserManagerProvider> userManagerMock;
        private Mock<IExporterProvider> exporterProviderMock;
        private Mock<IMemoryCache> memoryCacheMock;
        private Mock<IConfiguration> configurationMock;

        [TestInitialize]
        public void TestInitialize()
        {
            this.movieServiceMock = new Mock<IMovieService>();
            this.mapperMock = new Mock<IMappingProvider>();
            this.userManagerMock = new Mock<IUserManagerProvider>();
            this.exporterProviderMock = new Mock<IExporterProvider>();
            this.memoryCacheMock = new Mock<IMemoryCache>();
            this.configurationMock = new Mock<IConfiguration>();
        }


        [TestMethod]
        public void CorrectlyRedirectToAccountControllerAuthorize_WhenUserIsAuthenticated()
        {
            // Arrange

            // http context mock
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User)
                .Returns(principal);

            fakeHttpContext.Setup(t => t.User.Identity)
                .Returns(It.IsAny<IIdentity>);

            fakeHttpContext.Setup(t => t.User.Identity.IsAuthenticated)
                .Returns(false);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };


            var sut = new HomeController(mapperMock.Object, userManagerMock.Object,
                movieServiceMock.Object, exporterProviderMock.Object,
                memoryCacheMock.Object, configurationMock.Object)
            {
                ControllerContext = context
            };

            // Act
            var result = sut.Index() as RedirectToActionResult;

            // Assert
            Assert.AreEqual(result.ControllerName, "Account");
            Assert.AreEqual(result.ActionName, "Authorize");
        }

        [TestMethod]
        public void CorrectlyRedirectToHomeControllerResultsAction_WhenUserIsAuthenticated()
        {
            // Arrange

            // http context mock
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User)
                .Returns(principal);

            fakeHttpContext.Setup(t => t.User.Identity)
                .Returns(It.IsAny<IIdentity>);

            fakeHttpContext.Setup(t => t.User.Identity.IsAuthenticated)
                .Returns(true);

            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };


            var sut = new HomeController(mapperMock.Object, userManagerMock.Object,
              movieServiceMock.Object, exporterProviderMock.Object,
              memoryCacheMock.Object, configurationMock.Object)
            {
                ControllerContext = context
            };

            // Act
            var result = sut.Index() as RedirectToActionResult;

            // Assert
            Assert.AreEqual(result.ControllerName, "Home");
            Assert.AreEqual(result.ActionName, "Results");
        }
    }
}
