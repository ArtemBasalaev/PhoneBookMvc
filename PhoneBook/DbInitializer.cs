using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneBook.DataAccess;

namespace PhoneBook
{
    public class DbInitializer
    {
        private readonly PhoneBookDbContext _dbContext;

        public DbInitializer(PhoneBookDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task InitializeAsync()
        {
            return _dbContext.Database.MigrateAsync();
        }
    }
}