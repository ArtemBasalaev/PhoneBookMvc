using System;
using System.Linq;
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

        public HandleResult Handle(int[] contactsId)
        {
            if (contactsId != null)
            {
                foreach (var id in contactsId)
                {
                    var contact = _dbContext.Contacts.FirstOrDefault(c => c.Id == id);

                    if (contact != null)
                    {
                        _dbContext.Contacts.Remove(contact);
                        _dbContext.SaveChanges();
                    }
                }

                return new HandleResult { Success = true, Message = "Successfully delete" };
            }

            return new HandleResult { Success = false, Message = "Error" };
        }
    }
}