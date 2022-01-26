import Vue from "vue";
import Vuex from "vuex";
import PhoneBookService from "../service/phoneBookService";

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        phoneBookService: new PhoneBookService(),

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

            return context.state.phoneBookService.getContacts(params)
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
            return context.state.phoneBookService.deleteContact(contactId)
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
            return context.state.phoneBookService.deleteContacts(contactsId)
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

        createContact(context, newContact) {
            return context.state.phoneBookService.createContact(newContact)
                .then(response => {
                    if (!response.data.success) {
                        context.commit("setContactExistStatus", true);
                        context.commit("setIsSuccess", false);
                    } else {
                        context.dispatch("loadContacts");
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