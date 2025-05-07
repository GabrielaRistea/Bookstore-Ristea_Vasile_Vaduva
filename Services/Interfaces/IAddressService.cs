using Bookstore.Models;

namespace Bookstore.Services.Interfaces
{
    public interface IAddressService
    {
        Address? GetAddressById(int id);
        void AddAddress(Address address);
        void DeleteAddress(int id);
        void UpdateAddress(Address address);
        Address? GetAddressByUserId(int userId);

    }
}
