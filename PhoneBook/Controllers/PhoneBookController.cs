using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.BusinessLogic.Handlers;
using PhoneBook.Contracts.Dto;
using System.Threading.Tasks;

namespace PhoneBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PhoneBookController : ControllerBase
    {
        [HttpGet]
        public Task<List<ContactDto>> GetContactsAsync(string term, [FromServices] GetContactsHandler getContactsHandler)
        {
            return getContactsHandler.HandleAsync(term);
        }

        [HttpPost]
        public Task<HandlerResult> CreateContactAsync(ContactDto contact, [FromServices] CreateContactHandler createContactHandler)
        {
            return createContactHandler.HandleAsync(contact);
        }

        [HttpPost]
        public Task<HandlerResult> DeleteContactsAsync(int[] contactsIds, [FromServices] DeleteContactsHandler deleteContactsHandler)
        {
            return deleteContactsHandler.HandleAsync(contactsIds);
        }
    }
}