using System;
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

        public async Task<HandlerResult> HandleAsync(int[] contactsId)
        {
            if (contactsId == null)
            {
                return new HandlerResult
                {
                    Message = "Error"
                };
            }

            foreach (var id in contactsId)
            {
                var contact = _dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);

                if (contact != null)
                {
                    _dbContext.Contacts.Remove(await contact);
                }
            }

            await _dbContext.SaveChangesAsync();

            return new HandlerResult
            {
                Success = true, Message = "Successfully delete"
            };
        }
    }
}