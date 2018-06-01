using System.Collections.Generic;
using System.Linq;
using CSI_Miami.Data.Models;
using CSI_Miami.Data.Repository;
using CSI_Miami.Data.UnitOfWork;
using CSI_Miami.DTO.MovieService;
using CSI_Miami.Infrastructure.Providers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CSI_Miami.Services.Internal.MovieServiceTests
{
    [TestClass]
    public class GetAllMoviesShould
    {
        private Mock<IMappingProvider> mapperMock { get; set; }
        private Mock<IDataRepository<Movie>> movieRepoMock { get; set; }
        private Mock<IDataSaver> dataSaverMock { get; set; }
        private Mock<IConfiguration> configurationMock { get; set; }
        private Mock<IExporterProvider> jsonExporterProviderMock { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.mapperMock = new Mock<IMappingProvider>();
            this.movieRepoMock = new Mock<IDataRepository<Movie>>();
            this.dataSaverMock = new Mock<IDataSaver>();
            this.configurationMock = new Mock<IConfiguration>();
            this.jsonExporterProviderMock = new Mock<IExporterProvider>();
        }

        [TestMethod]
        public void ReturnCorrectData_WhenInvoked()
        {
            // Arrange
            this.configurationMock.Setup(x => x["MoviesPerPage"])
                .Returns("5");

            var movieListDto = new List<MovieDto>();

            for (int i = 0; i < 10; i++)
            {
                movieListDto.Add(new MovieDto());
            }

            var movieDomainList = new List<Movie>();

            for (int i = 0; i < 10; i++)
            {
                movieDomainList.Add(new Movie());
            }


            this.movieRepoMock.Setup(x => x.All)
                .Returns(movieDomainList.AsQueryable());

            this.mapperMock.Setup(x => x.ProjectTo<MovieDto>(
                It.IsAny<IQueryable<Movie>>()))
                .Returns(movieListDto.AsQueryable());


            var sut = new MovieService(mapperMock.Object, movieRepoMock.Object,
                dataSaverMock.Object, configurationMock.Object,
                jsonExporterProviderMock.Object);

            // Act
            var result = sut.GetAllMovies(5);

            // Assert
            Assert.AreEqual(5, result.Count());
        }

        [TestMethod]
        public void ReturnsCorrectCount_WhenMovieToSkipAndMoviesPerPageAreGreaterThenAllMoviesCount()
        {
            // Arrange
            this.configurationMock.Setup(x => x["MoviesPerPage"])
                .Returns("5");

            var movieListDto = new List<MovieDto>();

            for (int i = 0; i < 8; i++)
            {
                movieListDto.Add(new MovieDto());
            }

            var movieDomainList = new List<Movie>();

            for (int i = 0; i < 8; i++)
            {
                movieDomainList.Add(new Movie());
            }


            this.movieRepoMock.Setup(x => x.All)
                .Returns(movieDomainList.AsQueryable());

            this.mapperMock.Setup(x => x.ProjectTo<MovieDto>(
                It.IsAny<IQueryable<Movie>>()))
                .Returns(movieListDto.AsQueryable());


            var sut = new MovieService(mapperMock.Object, movieRepoMock.Object,
                dataSaverMock.Object, configurationMock.Object,
                jsonExporterProviderMock.Object);

            // Act
            var result = sut.GetAllMovies(5);

            // Assert
            Assert.AreEqual(3, result.Count());
        }
    }
}
