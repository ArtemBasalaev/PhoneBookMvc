<template>
  <div class="container" :class="{ 'opacity-25': $store.state.isLoading }" id="phone-book-form" v-cloak>
    <div class="row px-0 d-flex align-items-start">
      <div class="col-md-4">
        <h2 class="display-8 my-3">Add contact</h2>
        <form novalidate :class="{ 'was-validated': isInvalid }">
          <label for="first-name" class="form-label mt-2">First name*:</label>
          <input v-model="firstNameInputText" type="text" class="form-control" id="first-name" required>
          <div class="invalid-feedback">*field is empty</div>

          <label for="middle-name" class="form-label mt-2">Middle name:</label>
          <input v-model="middleNameInputText" type="text" class="form-control" id="middle-name">

          <label for="last-name" class="form-label mt-2">Last name*:</label>
          <input v-model="lastNameInputText" type="text" class="form-control" id="last-name" required>
          <div class="invalid-feedback">*field is empty</div>

          <label for="mobile-phone" class="form-label mt-2">Mobile phone*:</label>
          <input v-model="mobilePhoneInputText" type="text" class="form-control" id="mobile-phone" required>
          <div class="invalid-feedback">*field is empty</div>

          <label for="home-phone" class="form-label mt-2">Home phone:</label>
          <input v-model="homePhoneInputText" type="text" class="form-control" id="home-phone">

          <div class="text-danger mt-2">*required</div>

          <button type="button" class="btn btn-primary my-3 me-1" @click="createContact">Add contact</button>
          <button type="button" class="btn btn-secondary my-3 me-1" @click="clearForm">Clear</button>
          <div class="text-danger fw-bold" v-show="$store.state.hasContact">Phone number already exist!</div>
        </form>
      </div>

      <div class="col-md-4">
        <h2 class="display-8 my-3">Search contact</h2>
        <form id="search-form" novalidate>
          <label for="search-contact" class="form-label mt-2">Type search request:</label>
          <input v-model="searchInputText" type="text" class="form-control" id="search-contact">
          <button @click="searchContacts" type="button" class="btn btn-primary my-3 me-1">Search</button>
          <button @click="clearSearchResult" type="button" class="btn btn-secondary my-3 me-1">Clear</button>
        </form>
      </div>
    </div>

    <div class="row px-0">
      <div class="d-flex justify-content-center">
        <div class="spinner-border" role="status" :class="{ 'invisible': !$store.state.isLoading }"></div>
      </div>
    </div>

    <div class="row px-0">
      <h2 class="display-8 mb-3">Contacts list</h2>
      <div class="table-responsive">
        <table class="table align-middle" id="phone-book">
          <thead class="table-light">
          <tr>
            <th class="col-1" scope="col">
              <input type="checkbox" :value="true" v-model="isCheckedAllContacts">
            </th>
            <th class="col-1" scope="col">№</th>
            <th scope="col">First Name</th>
            <th scope="col">Last Name</th>
            <th scope="col">Phone</th>
            <th class="col-1" scope="col">Delete</th>
          </tr>
          </thead>
          <tbody>
          <table-row @set-contact-checked-to-delete="setIsCheckedToDelete"
                     @set-contact-to-delete="setContactToDelete"
                     v-for="(contact, index) in contacts" :key="contact.id"
                     :is-checked-all="isCheckedAllContacts"
                     :index="index"
                     :contact="contact"></table-row>
          </tbody>
        </table>
      </div>

      <div class="d-flex flex-row-reverse pe-3">
        <button type="button" class="btn btn-danger my-3" data-bs-toggle="modal" data-bs-target="#delete-confirmation"
                @click="setModalDialogDeleteContactsMode" :disabled="!hasContactsToDelete">
          Delete checked
        </button>
      </div>

      <modal-dialog :dialog-message="dialogMessage" @delete-confirm="deleteWithConfirmation"></modal-dialog>
    </div>
  </div>
</template>

<script>
import TableRow from "../components/PhoneBookTableRow.vue";
import ModalDialog from "../components/ModalDialog.vue";

export default {
  components: {
    TableRow,
    ModalDialog
  },

  props: {
    isSuccess: {
      type: Boolean,
      required: true
    }
  },

  created() {
    this.$store.dispatch("loadContacts");
  },

  computed: {
    contacts() {
      return this.$store.state.contacts;
    },

    hasContactsToDelete() {
      return this.contactsIdToDelete.length !== 0;
    },

    contactsCountToDelete() {
      return this.contactsIdToDelete.length;
    }
  },

  watch: {
    isSuccess(newValue) {
      if (newValue) {
        this.clearForm();
        this.clearSearchField();

        this.isCheckedAllContacts = false;
        this.contactsIdToDelete = [];
        this.$store.state.isSuccess = false;
      }
    },

    contactsCountToDelete(newValue) {
      if (newValue === 0) {
        this.isCheckedAllContacts = false;
      }
    }
  },

  data() {
    return {
      contactToDelete: {},
      contactsIdToDelete: [],

      firstNameInputText: "",
      middleNameInputText: "",
      lastNameInputText: "",
      mobilePhoneInputText: "",
      homePhoneInputText: "",

      searchInputText: "",
      term: "",

      isInvalid: false,
      isCheckedAllContacts: false,

      isModalDialogDeleteContactMode: false,
      dialogMessage: ""
    };
  },

  methods: {
    createContact() {
      this.$store.commit("setContactExistStatus", false);

      let firstNameText = this.firstNameInputText.trim();
      let middleNameText = this.middleNameInputText.trim();
      let lastNameText = this.lastNameInputText.trim();
      let mobilePhoneText = this.mobilePhoneInputText.trim();
      let homePhoneText = this.homePhoneInputText.trim();

      if (firstNameText.length === 0 || lastNameText.length === 0 || mobilePhoneText.length === 0) {
        this.isInvalid = true;
        return;
      }

      let newContact = {
        firstName: firstNameText,
        middleName: middleNameText,
        lastName: lastNameText,
        phoneNumbers: [
          {
            phone: mobilePhoneText,
            type: 0
          },
          {
            phone: homePhoneText,
            type: 1
          }
        ]
      };

      this.clearSearchField();
      this.$store.dispatch("createContact", newContact);
    },

    clearForm() {
      this.firstNameInputText = "";
      this.middleNameInputText = "";
      this.lastNameInputText = "";
      this.mobilePhoneInputText = "";
      this.homePhoneInputText = "";

      this.isInvalid = false;
      this.$store.commit("setContactExistStatus", false);
    },

    searchContacts() {
      this.contactsIdToDelete = [];
      this.term = this.searchInputText.trim();

      let term = {
        params: {term: this.term}
      };

      this.$store.dispatch("loadContacts", term);
    },

    clearSearchField(){
      this.searchInputText = "";
      this.term = "";
    },

    clearSearchResult() {
      this.clearSearchField();
      this.$store.dispatch("loadContacts");
    },

    deleteWithConfirmation: function () {
      if (this.isModalDialogDeleteContactMode) {
        this.deleteContact();
        return;
      }

      this.deleteCheckedContacts();
    },

    deleteContact() {
      this.clearSearchField();
      this.$store.dispatch("deleteContact", [this.contactToDelete.id]);
    },

    deleteCheckedContacts() {
      this.clearSearchField();
      this.$store.dispatch("deleteCheckedContacts", this.contactsIdToDelete);
    },

    setContactToDelete(contact) {
      this.contactToDelete = contact;

      this.isModalDialogDeleteContactMode = true;
      this.dialogMessage = "Are you sure you want to delete contact?";
    },

    setIsCheckedToDelete(contactId, isChecked) {
      if (isChecked) {
        this.contactsIdToDelete.push(contactId);
        return;
      }

      this.contactsIdToDelete = this.contactsIdToDelete.filter(id => id !== contactId);
    },

    setModalDialogDeleteContactsMode: function () {
      this.isModalDialogDeleteContactMode = false;
      this.dialogMessage = "Are you sure you want to delete checked contacts?";
    },

    checkContactsDeleteStatus() {
      return this.contactsIdToDelete;
    }
  }
}
</script>