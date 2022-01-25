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
        public List<ContactDto> GetContacts(string term,
            [FromServices] GetContactsHandler getContactsHandler)
        {
            return getContactsHandler.Handle(term);
        }

        [HttpPost]
        public async Task<HandlerResult> CreateContactAsync(ContactDto contact, [FromServices] CreateContactHandler createContactHandler)
        {
            return await createContactHandler.HandleAsync(contact);
        }

        [HttpPost]
        public async Task<HandlerResult> DeleteContactAsync(int[] contactsId, [FromServices] DeleteContactsHandler deleteContactsHandler)
        {
            return await deleteContactsHandler.HandleAsync(contactsId);
        }

        [HttpPost]
        public async Task<HandlerResult> DeleteContactsAsync(int[] contactsId, [FromServices] DeleteContactsHandler deleteContactsHandler)
        {
            return await deleteContactsHandler.HandleAsync(contactsId);
        }
    }
}