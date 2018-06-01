using System;
using System.Collections.Generic;
using System.Linq;
using CSI_Miami.Data.Models;
using CSI_Miami.Data.Repository;
using CSI_Miami.Data.UnitOfWork;
using CSI_Miami.DTO.MovieService;
using CSI_Miami.Infrastructure.Providers.Contracts;
using CSI_Miami.Services.Internal.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CSI_Miami.Services.Internal.MovieServiceTests
{
    [TestClass]
    public class EditMovieShould
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
        public void SucessfullyEditMovie_WhenInvokedWithValidParameters()
        {
            // Arrange
            var movieDtoArgument = new MovieDto()
            {
                Title = "Horatio is king baby"
            };

            var movieDomain = new Movie();

            this.movieRepoMock.Setup(x => x.All)
                .Returns(new List<Movie> { movieDomain }.AsQueryable());

            var sut = new MovieService(mapperMock.Object, movieRepoMock.Object,
                dataSaverMock.Object, configurationMock.Object,
                jsonExporterProviderMock.Object);

            // Act
            var result = sut.EditMovie(movieDtoArgument);

            // Assert
            Assert.AreEqual(movieDtoArgument.Title, movieDomain.Title);
        }

        [TestMethod]
        public void ThrowArgumentNullException_WhenInvokedWithInvalidParameters()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new MovieService(mapperMock.Object, movieRepoMock.Object,
                dataSaverMock.Object, configurationMock.Object,
                jsonExporterProviderMock.Object).CreateMovie(null));
        }
    }
}
