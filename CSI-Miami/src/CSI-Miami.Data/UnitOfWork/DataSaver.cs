namespace CSI_Miami.Data.UnitOfWork
{
    public class DataSaver : IDataSaver
    {
        private readonly ApplicationDbContext context;

        public DataSaver(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public async void SaveChangesAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }

}
