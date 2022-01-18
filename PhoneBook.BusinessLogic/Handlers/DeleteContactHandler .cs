using System;
using System.Linq;
using PhoneBook.Contracts.Dto;
using PhoneBook.DataAccess;

namespace PhoneBook.BusinessLogic.Handlers
{
    public class DeleteContactHandler
    {
        private readonly PhoneBookDbContext _dbContext;

        public DeleteContactHandler(PhoneBookDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public HandleResult Handle(ContactToDeleteDto contactToDelete)
        {
            if (contactToDelete != null)
            {
                var contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == contactToDelete.Id);

                if (contact != null)
                {
                    _dbContext.Contacts.Remove(contact);
                    _dbContext.SaveChanges();

                    return new HandleResult { Success = true, Message = "Successfully delete" };
                }
            }

            return new HandleResult { Success = false, Message = "Error" };
        }
    }
}