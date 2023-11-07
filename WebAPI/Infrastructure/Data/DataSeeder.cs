using Domain.Entities;
namespace Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly DataContext dataContext;

        public DataSeeder(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public void Seed()
        {
            var client = new Client(new Guid("c9d98a47-cb0b-4150-9ca7-2d8bbf979656"), "John", "Smith", "john@gmail.com", "+18202820232");
            var client1 = new Client(Guid.NewGuid(), "Nhat", "Phan", "phanminhnhat130895@gmail.com", "+84366016101");

            dataContext.Add(client);
            dataContext.Add(client1);
            dataContext.SaveChanges();
        }
    }
}
