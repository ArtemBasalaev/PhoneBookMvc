using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Contracts.Dto;
using PhoneBook.DataAccess;

namespace PhoneBook.BusinessLogic.Handlers
{
    public class DeleteContactsHandler
    {
        private readonly PhoneBookDbContext _dbContext;

        public DeleteContactsHandler(PhoneBookDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<HandlerResult> HandleAsync(int[] contactsIds)
        {
            if (contactsIds == null)
            {
                return new HandlerResult
                {
                    Message = "Contacts ids is undefined"
                };
            }

            var contactsToDelete = await _dbContext.Contacts
                .Where(c => contactsIds.Contains(c.Id))
                .ToListAsync();

            if (!contactsToDelete.Any())
            {
                return new HandlerResult
                {
                    Message = "Contacts ids is not exist"
                };
            }

            _dbContext.Contacts.RemoveRange(contactsToDelete);
            await _dbContext.SaveChangesAsync();

            return new HandlerResult
            {
                Success = true,
                Message = "Successfully delete"
            };
        }
    }
}