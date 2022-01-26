using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Contracts.Dto;
using PhoneBook.DataAccess;

namespace PhoneBook.BusinessLogic.Handlers
{
    public class GetContactsHandler
    {
        private readonly PhoneBookDbContext _dbContext;

        public GetContactsHandler(PhoneBookDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Task<List<ContactDto>> HandleAsync(string term)
        {
            var queryString = term ?? "";

            return _dbContext.Contacts
                .AsNoTracking()
                .Where(c => c.FirstName.Contains(queryString)
                            || c.LastName.Contains(queryString)
                            || c.PhoneNumbers.Any(p => p.Phone.Contains(queryString)))
                .Select(c => new ContactDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MiddleName = c.MiddleName,
                    PhoneNumbers = c.PhoneNumbers
                        .Select(p => new PhoneNumberDto
                        {
                            Id = p.Id,
                            Phone = p.Phone,
                            Type = p.Type
                        })
                        .OrderBy(p => p.Phone)
                        .ToList()
                })
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ThenBy(p => p.MiddleName)
                .ToListAsync();
        }
    }
}