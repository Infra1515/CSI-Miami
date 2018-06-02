using System.Collections.Generic;
using System.Linq;
using CSI_Miami.DTO.MovieService;
using CSI_Miami.Infrastructure.Providers.Contracts;
using CSI_Miami.Services.Internal.Contracts;
using CSI_Miami.Web.Controllers;
using CSI_Miami.Web.Models.HomeViewModels.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CSI_Miami.Web.Tests.HomeControllerTests
{
    [TestClass]
    public class LoadNextShould
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
        public void ReturnCorrectView_WhenMoviesAreLessThenTotalCount()
        {
            // Arrange
            this.movieServiceMock.Setup(x => x.GetMoviesPerPage())
                .Returns(5);

            this.movieServiceMock.Setup(x => x.GetTotalMoviesCount())
                .Returns(100);

            this.movieServiceMock.Setup(x => x.GetAllMovies(5))
                .Returns(new List<MovieDto>());

            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x["moviesToSkip"])
                .Returns(0);

            var movieViewListToReturn = new List<ResultsMoviesViewModel>()
            {
                new ResultsMoviesViewModel(),
                new ResultsMoviesViewModel(),
                new ResultsMoviesViewModel()
            };

            this.mapperMock.Setup(x =>
            x.EnumerableProjectTo<MovieDto, ResultsMoviesViewModel>(
                It.IsAny<IEnumerable<MovieDto>>()))
                .Returns(movieViewListToReturn);

            var sut = new HomeController(mapperMock.Object, userManagerMock.Object,
             movieServiceMock.Object, exporterProviderMock.Object,
             memoryCacheMock.Object)
            {
                TempData = tempDataMock.Object
            };

            // Act
            var result = sut.LoadNext() as PartialViewResult;
            var model = result.Model as IEnumerable<ResultsMoviesViewModel>;

            // Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.AreEqual(3, model.Count());
            Assert.AreEqual(result.ViewName, "_MoviesListPartial");

        }

        [TestMethod]
        public void ReturnCorrectView_WhenMoviesAreGreaterThenTotalCount()
        {
            // Arrange
            this.movieServiceMock.Setup(x => x.GetMoviesPerPage())
                .Returns(5);

            this.movieServiceMock.Setup(x => x.GetTotalMoviesCount())
                .Returns(104);

            this.movieServiceMock.Setup(x => x.GetAllMovies(5))
                .Returns(new List<MovieDto>());

            var tempDataMock = new Mock<ITempDataDictionary>();
            tempDataMock.Setup(x => x["moviesToSkip"])
                .Returns(100);

            var sut = new HomeController(mapperMock.Object, userManagerMock.Object,
             movieServiceMock.Object, exporterProviderMock.Object,
             memoryCacheMock.Object)
            {
                TempData = tempDataMock.Object
            };


            // Act
            var result = sut.LoadNext() as PartialViewResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
            Assert.AreEqual(result.ViewName, "_NoMoreMoviesPartial");
        }
    }
}