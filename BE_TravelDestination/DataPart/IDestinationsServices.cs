using BE_TravelDestinations.DataPart.Repositories;
using SharedData.Models;

namespace BE_TravelDestinations.DataPart
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
        private readonly IDestinationsRepository _repository;

        public DestinationsService(IDestinationsRepository repository)
        {
            _repository = repository;
        }

        public List<Destinations> GetAllDestinations() => _repository.GetAll().ToList();

        public Destinations GetDestinationsById(int id) => _repository.GetById(id);

        public void AddDestinations(Destinations destination)
        {
            _repository.Add(destination);
        }

        public void UpdateDestinations(Destinations destination)
        {
            _repository.Update(destination.Id, destination);
        }

        public void DeleteDestinations(int id)
        {
            _repository.Delete(id);
        }
    }
}
