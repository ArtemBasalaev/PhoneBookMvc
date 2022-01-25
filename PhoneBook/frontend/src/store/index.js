import Vue from "vue";
import Vuex from "vuex";
import ApiUrls from "../api/ApiUrls";
import RequestMethods from "../api/RequestMethods";

let apiUrls = new ApiUrls("/api/PhoneBook/GetContacts", "/api/PhoneBook/CreateContact", "/api/PhoneBook/DeleteContact", "/api/PhoneBook/DeleteContacts");
let requestMethods = new RequestMethods(apiUrls);

Vue.use(Vuex);

export default new Vuex.Store({
    state: {
        methods: requestMethods,
        apiUrls: apiUrls,

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

            return context.state.methods.getContacts(params)
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
            return context.state.methods.deleteContact(contactId)
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
            return context.state.methods.deleteContacts(contactsId)
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
            return context.state.methods.createContact(newContact)
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