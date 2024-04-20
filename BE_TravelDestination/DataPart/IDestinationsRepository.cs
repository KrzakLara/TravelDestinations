using SharedData.Models;
using System.Collections.Concurrent;

namespace BE_TravelDestinations.DataPart.Repositories
{
    public interface IDestinationsRepository
    {
        IEnumerable<Destinations> GetAll();
        Destinations GetById(int id);
        void Add(Destinations destination);
        void Update(int id, Destinations destination);
        void Delete(int id);
    }

    public class DestinationsRepository : IDestinationsRepository
    {
        private readonly ConcurrentDictionary<int, Destinations> _destinations = new ConcurrentDictionary<int, Destinations>();

        public DestinationsRepository()
        {
            // Initialize with some data
            Add(new Destinations { Id = 1, Name = "Bora Bora, French Polynesia", Type = DestinationsType.island_paradises, Code = "BBFP2023", Price = 5000.00 });
            Add(new Destinations { Id = 2, Name = "Aspen, Colorado", Type = DestinationsType.winter_sports_resorts, Code = "ASP2023", Price = 3200.00 });
            Add(new Destinations { Id = 3, Name = "Tokyo, Japan", Type = DestinationsType.urban_metropolises, Code = "TKY2023", Price = 2700.00 });
            Add(new Destinations { Id = 4, Name = "Machu Picchu, Peru", Type = DestinationsType.historical_sites, Code = "MPRU2023", Price = 1800.00 });
            Add(new Destinations { Id = 5, Name = "Banff National Park, Canada", Type = DestinationsType.nature_reserves, Code = "BNP2023", Price = 2200.00 });
            Add(new Destinations { Id = 6, Name = "Maasai Mara, Kenya", Type = DestinationsType.wildlife_safaris, Code = "MMK2023", Price = 4000.00 });
            Add(new Destinations { Id = 7, Name = "Phuket, Thailand", Type = DestinationsType.beach, Code = "PHKT2023", Price = 2500.00 });
            Add(new Destinations { Id = 8, Name = "Swiss Alps", Type = DestinationsType.mountains, Code = "SALP2023", Price = 3100.00 });

        }

        public IEnumerable<Destinations> GetAll() => _destinations.Values;

        public Destinations GetById(int id) => _destinations.GetValueOrDefault(id);

        public void Add(Destinations destination)
        {
            _destinations.TryAdd(destination.Id, destination);
        }

        public void Update(int id, Destinations destination)
        {
            _destinations[id] = destination;
        }

        public void Delete(int id)
        {
            _destinations.TryRemove(id, out var removed);
        }
    }
}
