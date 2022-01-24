import Vue from "vue";
import Vuex from "vuex";
import axios from "axios";

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        isLoading: false,
        isSuccess: false,
        contacts: [],
        hasContact: false
    },

    mutations: {
        setIsLoading(state, value) {
            state.isLoading = value;
        },

        setIsSuccess(state, value) {
            state.isSuccess = value;
        },

        setContacts(state, contacts) {
            state.contacts = contacts;
        },

        setContactExistStatus(state, status) {
            state.hasContact = status;
        }
    },

    actions: {
        loadContacts(context, params) {
            context.commit("setIsLoading", true);
            context.commit("setContacts", []);

            return axios.get("/api/PhoneBook/GetContacts", params)
                .then(response => {
                    context.commit("setContacts", response.data);
                })
                .catch(() => {
                    alert("Load contacts fail");
                })
                .then(() => {
                context.commit("setIsLoading", false);
            });
        },

        deleteContact(context, contactId) {
            return axios.post("/api/PhoneBook/DeleteContact", contactId)
                .then(response => {
                    if (!response.data.success) {
                        context.commit("setIsSuccess", false);
                        alert(response.data.message);
                    } else {
                        context.commit("setIsSuccess", true);
                        context.dispatch("loadContacts");
                    }
                })
                .catch(() => {
                    alert("Delete fail");
                });
        },

        deleteCheckedContacts(context, contactsId) {
            return axios.post("/api/PhoneBook/DeleteContacts", contactsId)
                .then(response => {
                    if (!response.data.success) {
                        context.commit("setIsSuccess", false);
                        alert(response.data.message);
                    } else {
                        context.commit("setIsSuccess", true);
                        context.dispatch("loadContacts");
                    }
                })
                .catch(() => {
                    alert("Delete contact fail");
                });
        },

        createContact(context, newContact, term) {
            return axios.post("/api/PhoneBook/CreateContact", newContact)
                .then(response => {
                    if (!response.data.success) {
                        context.commit("setContactExistStatus", true);
                        context.commit("setIsSuccess", false);
                    } else {
                        context.dispatch("loadContacts", term);
                        context.commit("setContactExistStatus", false);
                        context.commit("setIsSuccess", true);
                    }
                })
                .catch(() => {
                    alert("Create fail");
                });
        }
    }
});