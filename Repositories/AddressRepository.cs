using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly BookstoreContext _context;
        public AddressRepository(BookstoreContext context)
        {
            _context = context;
        }
        public IQueryable<Address> FindByCondition(Expression<Func<Address, bool>> expression) =>
           _context.Set<Address>().Where(expression).AsNoTracking();
        public void Create(Address address) =>
            _context.Set<Address>().Add(address);
        public void Update(Address address) =>
            _context.Set<Address>().Update(address);
        public void Delete(Address address) =>
            _context.Set<Address>().Remove(address);
        public void Save()
        {
            _context.SaveChanges();
        }
        public Address GetAddressByUserId(int userId)
        {
            return _context.Addresses
                           .AsNoTracking()
                           .FirstOrDefault(a => a.IdUser == userId);
        }

    }
}
