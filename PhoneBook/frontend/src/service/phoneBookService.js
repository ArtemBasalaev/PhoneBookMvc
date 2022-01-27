import axios from "axios";

export default class PhoneBookService {
    getContacts(term) {
        return axios.get("/api/PhoneBook/getContacts", term);
    }

    createContact(contact) {
        return axios.post("/api/PhoneBook/createContact", contact);
    }

    deleteContacts(contactsIds) {
        return axios.post("/api/PhoneBook/deleteContacts", contactsIds);
    }
}