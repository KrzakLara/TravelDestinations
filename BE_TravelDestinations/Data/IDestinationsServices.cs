using SharedData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_TravelDestinations.Data
{
    public interface IDestinationsServices
    {
        List<Destinations> GetAllDestinations();
        Destinations GetDestinationsById(int id);
        void AddDestinations(Destinations destinations);
        void UpdateDestinations(Destinations destinations);
        void DeleteDestinations(int id);
    }

    public class DestinationsService : IDestinationsServices
    {
        private readonly List<Destinations> _destinations = new List<Destinations>();

        public DestinationsService()
        {
            _destinations.Add(new Destinations { Id = 1, Name = "Bora Bora, French Polynesia", Type = DestinationsType.island_paradises, Code = "BBFP2023", Price = 5000.00 });
            _destinations.Add(new Destinations { Id = 2, Name = "Mount Everest Base Camp, Nepal", Type = DestinationsType.mountains, Code = "MEBC2023", Price = 2500.00 });
            _destinations.Add(new Destinations { Id = 3, Name = "New York City, USA", Type = DestinationsType.urban_metropolises, Code = "NYC2023", Price = 3000.00 });
            _destinations.Add(new Destinations { Id = 4, Name = "The Colosseum, Italy", Type = DestinationsType.historical_sites, Code = "TCI2023", Price = 1500.00 });
            _destinations.Add(new Destinations { Id = 5, Name = "Serengeti National Park, Tanzania", Type = DestinationsType.wildlife_safaris, Code = "SNPT2023", Price = 4500.00 });
        }


        public List<Destinations> GetAllDestinations() => _destinations;

        public Destinations GetDestinationsById(int id) => _destinations.SingleOrDefault(c => c.Id == id);

        public void AddDestinations(Destinations hardware)
        {
            var lastHardware = _destinations.Count() == 0 ? 0 : _destinations.Last().Id;
            lastHardware += 1;
            hardware.Id = lastHardware;
            _destinations.Add(hardware);
        }

        public void UpdateDestinations(Destinations hardware)
        {
            var index = _destinations.FindIndex(c => c.Id == hardware.Id);
            if (index != -1)
            {
                _destinations[index] = hardware;
            }
        }

        public void DeleteDestinations(int id)
        {
            _destinations.RemoveAll(c => c.Id == id);
        }


    }
}
