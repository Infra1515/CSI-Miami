using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using CSI_Miami.DTO.MovieService;
using CSI_Miami.Infrastructure.Providers.Contracts;
using CSI_Miami.Services.Internal.Contracts;
using CSI_Miami.Web.Controllers;
using CSI_Miami.Web.Models.HomeViewModels.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CSI_Miami.Web.Tests.HomeControllerTests
{
    [TestClass]
    public class ResultsShould
    {

        private Mock<IMovieService> movieServiceMock;
        private Mock<IMappingProvider> mapperMock;
        private Mock<IUserManagerProvider> userManagerMock;
        private Mock<IExporterProvider> exporterProviderMock;
        private Mock<IMemoryCache> memoryCacheMock;

        [TestInitialize]
        public void TestInitialize()
        {
            this.movieServiceMock = new Mock<IMovieService>();
            this.mapperMock = new Mock<IMappingProvider>();
            this.userManagerMock = new Mock<IUserManagerProvider>();
            this.exporterProviderMock = new Mock<IExporterProvider>();
            this.memoryCacheMock = new Mock<IMemoryCache>();
        }

        [TestMethod]
        public void ReturnAViewResult_WithCorrectCountOfMovies()
        {
            // http context mock
            var fakeHttpContext = new Mock<HttpContext>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            // in ASP.Core cannot mock Controller context 
            //   controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            // because controller context is not virtual
            var context = new ControllerContext
            {
                HttpContext = fakeHttpContext.Object
            };

            var tempDataMock = new Mock<ITempDataDictionary>();


            this.userManagerMock.Setup(x => x.GetUserName(It.IsAny<ClaimsPrincipal>()))
                .Returns("testName");

            this.movieServiceMock.Setup(x => x.GetAllMovies(0))
                .Returns(new List<MovieDto>());

            var fakeMovieVMListToReturn = new List<ResultsMoviesViewModel>
            {
                new ResultsMoviesViewModel(),
                new ResultsMoviesViewModel()
            };

            this.mapperMock.Setup(
                x => x.EnumerableProjectTo<MovieDto, ResultsMoviesViewModel>(
                    It.IsAny<IEnumerable<MovieDto>>()))
                    .Returns(fakeMovieVMListToReturn);


            var sut = new HomeController(mapperMock.Object, userManagerMock.Object,
             movieServiceMock.Object, exporterProviderMock.Object,
             memoryCacheMock.Object)
            {
                ControllerContext = context,
                TempData = tempDataMock.Object
            };

            // Act
            var result = sut.Results() as ViewResult;
            var model = result.Model as ResultsViewModel;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Movies.Count());

        }

    }
}