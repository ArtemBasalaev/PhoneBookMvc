using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.BusinessLogic.Handlers;
using PhoneBook.Contracts.Dto;


namespace PhoneBook.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PhoneBookController : ControllerBase
    {
        private readonly GetContactsHandler _getContactsHandler;
        private readonly CreateContactHandler _createContactHandler;
        private readonly DeleteContactHandler _deleteContactHandler;
        private readonly DeleteContactsHandler _deleteContactsHandler;

        public PhoneBookController(GetContactsHandler getContactsHandler, CreateContactHandler createContactHandler, DeleteContactHandler deleteContactHandler, DeleteContactsHandler deleteContactsHandler)
        {
            _getContactsHandler = getContactsHandler ?? throw new System.ArgumentNullException(nameof(getContactsHandler));
            _createContactHandler = createContactHandler ?? throw new System.ArgumentNullException(nameof(createContactHandler));
            _deleteContactHandler = deleteContactHandler ?? throw new System.ArgumentNullException(nameof(deleteContactHandler));
            _deleteContactsHandler = deleteContactsHandler ?? throw new System.ArgumentNullException(nameof(deleteContactsHandler));

        }

        [HttpGet]
        public List<ContactDto> GetContacts(string term)
        {
            return _getContactsHandler.Handle(term);
        }

        [HttpPost]
        public ActionResult CreateContact(ContactDto contact)
        {
            var handleResult = _createContactHandler.Handle(contact);

            return new JsonResult(handleResult);
        }

        [HttpPost]
        public ActionResult DeleteContact(ContactToDeleteDto contactToDelete)
        {
            var handleResult = _deleteContactHandler.Handle(contactToDelete);

            return new JsonResult(handleResult);
        }

        [HttpPost]
        public ActionResult DeleteContacts(int[] contactsId)
        {
            var handleResult = _deleteContactsHandler.Handle(contactsId);

            return new JsonResult(handleResult);
        }
    }
}