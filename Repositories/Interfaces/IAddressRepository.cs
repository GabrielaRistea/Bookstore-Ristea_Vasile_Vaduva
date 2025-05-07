using Bookstore.Models;
using System.Linq.Expressions;

namespace Bookstore.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        IQueryable<Address> FindByCondition(Expression<Func<Address, bool>> expression);
        void Create (Address address);
        void Update (Address address);
        void Delete(Address address);
        void Save();
        Address GetAddressByUserId(int userId);

    }
}
