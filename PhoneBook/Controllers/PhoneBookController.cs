using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.BusinessLogic.Handlers;
using PhoneBook.Contracts.Dto;
using System;
using System.Threading.Tasks;

namespace PhoneBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PhoneBookController : ControllerBase
    {
        [HttpGet]
        public async Task<List<ContactDto>> GetContacts(string term, [FromServices] GetContactsHandler getContactsHandler)
        {
            if (getContactsHandler == null)
            {
                throw new ArgumentNullException(nameof(getContactsHandler));
            }

            return await getContactsHandler.Handle(term);
        }

        [HttpPost]
        public async Task<HandlerResult> CreateContact(ContactDto contact, [FromServices] CreateContactHandler createContactHandler)
        {
            if (createContactHandler == null)
            {
                throw new ArgumentNullException(nameof(createContactHandler));
            }

            return await createContactHandler.Handle(contact);
        }

        [HttpPost]
        public async Task<HandlerResult> DeleteContact(int[] contactsId, [FromServices] DeleteContactsHandler deleteContactsHandler)
        {
            if (deleteContactsHandler == null)
            {
                throw new ArgumentNullException(nameof(deleteContactsHandler));
            }

            return await deleteContactsHandler.Handle(contactsId);
        }

        [HttpPost]
        public async Task<HandlerResult> DeleteContacts(int[] contactsId,
            [FromServices] DeleteContactsHandler deleteContactsHandler)
        {
            if (deleteContactsHandler == null)
            {
                throw new ArgumentNullException(nameof(deleteContactsHandler));
            }

            return await deleteContactsHandler.Handle(contactsId);
        }
    }
}