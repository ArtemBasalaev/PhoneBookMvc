import axios from "axios";

export default class RequestMethods {
    constructor(apiUrls) {
        this.apiUrls = apiUrls;
    }

    getContacts(term) {
        return axios.get(this.apiUrls.getContactsUrl, term);
    }

    createContact(contact) {
        return axios.post(this.apiUrls.createContactUrl, contact);
    }

    deleteContact(contactId) {
        return axios.post(this.apiUrls.deleteContactUrl, contactId);
    }

    deleteContacts(contactsId) {
        return axios.post(this.apiUrls.deleteContactsUrl, contactsId);
    }
}