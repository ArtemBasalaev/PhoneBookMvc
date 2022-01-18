using System;
using System.Collections.Generic;
using System.Linq;
using PhoneBook.Contracts.Dto;
using PhoneBook.DataAccess;
using PhoneBook.DataAccess.Model;

namespace PhoneBook.BusinessLogic.Handlers
{
    public class CreateContactHandler
    {
        private readonly PhoneBookDbContext _dbContext;

        public CreateContactHandler(PhoneBookDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public HandleResult Handle(ContactDto contactDto)
        {
            if (contactDto == null)
            {
                return new HandleResult { Success = false, Message = "Contact is empty" };
            }

            if (contactDto.FirstName.Length == 0)
            {
                return new HandleResult { Success = false, Message = "First name is required" };
            }

            if (contactDto.LastName.Length == 0)
            {
                return new HandleResult { Success = false, Message = "Last name is required" };
            }

            var mobilePhone = contactDto.PhoneNumbers.FirstOrDefault(p => p.Type == PhoneNumberType.Mobile);

            if (mobilePhone == null || mobilePhone.Phone.Length == 0)
            {
                return new HandleResult { Success = false, Message = "Phone number is required" };
            }

            if (_dbContext.PhoneNumbers.Select(p => p.Phone).Contains(mobilePhone.Phone))
            {
                return new HandleResult { Success = false, Message = "Phone number is exist" };
            }

            var phone1 = new PhoneNumber
            {
                Phone = contactDto.PhoneNumbers[0].Phone,
                Type = contactDto.PhoneNumbers[0].Type == 0 ? PhoneNumberType.Mobile : PhoneNumberType.Home
            };

            var phone2 = new PhoneNumber
            {
                Phone = contactDto.PhoneNumbers[1].Phone,
                Type = contactDto.PhoneNumbers[1].Type == 0 ? PhoneNumberType.Mobile : PhoneNumberType.Home
            };


            var phoneNumbers = new List<PhoneNumber>
            {
                phone1,
                phone2
            };

            Contact contact = new()
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                MiddleName = contactDto.MiddleName,
                PhoneNumbers = phoneNumbers
            };

            _dbContext.Add(contact);
            _dbContext.SaveChanges();

            return new HandleResult { Success = true, Message = "Successfully add" };
        }
    }
}