using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;
using Bookstore.Models;

namespace Bookstore.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repository;
        public AddressService(IAddressRepository repository)
        {
            _repository = repository;
        }
        public Address? GetAddressById(int id)
        {
            return _repository.FindByCondition(a => a.Id == id).FirstOrDefault();
        }
        public void AddAddress(Address address)
        {
            _repository.Create(address);
            _repository.Save();
        }
        public void UpdateAddress(Address address)
        {
            _repository.Update(address);
            _repository.Save();
        }
        public void DeleteAddress(int id)
        {
            var address = GetAddressById(id);
            if (address != null)
            {
                _repository.Delete(address);
                _repository.Save();
            }
        }
        public Address? GetAddressByUserId(int userId)
        {
            return _repository.GetAddressByUserId(userId);
        }

    }
}
