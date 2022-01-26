import axios from "axios";

export default class PhoneBookService {
    getContacts(term) {
        return axios.get("/api/PhoneBook/getContacts", term);
    }

    createContact(contact) {
        return axios.post("/api/PhoneBook/createContact", contact);
    }

    deleteContact(contactId) {
        return axios.post("/api/PhoneBook/deleteContact", contactId);
    }

    deleteContacts(contactsIds) {
        return axios.post("/api/PhoneBook/deleteContacts", contactsIds);
    }
}