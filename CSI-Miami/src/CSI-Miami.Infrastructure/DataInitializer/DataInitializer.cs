using System;
using System.Linq;
using CSI_Miami.Data.Models;
using CSI_Miami.Data.Repository;
using CSI_Miami.Data.UnitOfWork;

namespace CSI_Miami.Infrastructure.DataInitializer
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IDataRepository<Movie> movieRepo;
        private readonly IDataSaver dataSaver;

        public DataInitializer(IDataRepository<Movie> movieRepo, IDataSaver dataSaver)
        {
            this.movieRepo = movieRepo ?? throw new ArgumentNullException(nameof(movieRepo));
            this.dataSaver = dataSaver ?? throw new ArgumentNullException(nameof(dataSaver));
        }

        public void Initialize()
        {
            this.InitializeMovies();
        }

        private void InitializeMovies()
        {
            if (!this.movieRepo.All.Any())
            {
                for (int i = 0; i < 100; i++)
                {
                    var movie = new Movie()
                    {
                        Title = "TestMovie" + i,
                        DirectorName = "TestDirector" + i,
                        ReleaseDate = DateTime.Now.Date,
                        IsDeleted = false
                    };

                    this.movieRepo.Add(movie);
                }

                this.dataSaver.SaveChanges();
            }
        }
    }
}
