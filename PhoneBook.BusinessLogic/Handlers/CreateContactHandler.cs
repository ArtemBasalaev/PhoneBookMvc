using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<HandlerResult> Handle(ContactDto contactDto)
        {
            if (contactDto == null)
            {
                return new HandlerResult {Message = "Contact is empty"};
            }

            if (string.IsNullOrWhiteSpace(contactDto.FirstName))
            {
                return new HandlerResult {Message = "First name is required"};
            }

            if (string.IsNullOrWhiteSpace(contactDto.LastName))
            {
                return new HandlerResult {Message = "Last name is required"};
            }

            var mobilePhone = contactDto.PhoneNumbers.FirstOrDefault(p => p.Type == PhoneNumberType.Mobile);

            if (mobilePhone == null || string.IsNullOrWhiteSpace(mobilePhone.Phone))
            {
                return new HandlerResult {Message = "Phone number is required"};
            }

            if (await _dbContext.PhoneNumbers.AnyAsync(p => string.Equals(p.Phone, mobilePhone.Phone)))
            {
                return new HandlerResult {Message = "Phone number is exist"};
            }

            Contact contact = new()
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                MiddleName = contactDto.MiddleName,
                PhoneNumbers = new List<PhoneNumber>()
            };

            foreach (var phone in contactDto.PhoneNumbers)
            {
                contact.PhoneNumbers.Add(new PhoneNumber
                {
                    Phone = phone.Phone,
                    Type = phone.Type
                });
            }

            _dbContext.Add(contact);
            await _dbContext.SaveChangesAsync();

            return new HandlerResult {Success = true, Message = "Successfully add"};
        }
    }
}