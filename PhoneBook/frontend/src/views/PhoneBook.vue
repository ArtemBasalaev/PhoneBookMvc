<template>
  <div id="phone-book-form">
    <div class="row mb-3 d-flex align-items-start">
      <div class="col-md-4">
        <h2 class="display-8 my-3">Add contact</h2>
        <form novalidate :class="{ 'was-validated': isInvalid }">
          <label for="first-name" class="form-label mt-2">First name:</label>
          <input v-model="firstNameInputText" type="text" class="form-control" id="first-name" required>
          <div class="invalid-feedback">*field is empty</div>

          <label for="middle-name" class="form-label mt-2">Middle name:</label>
          <input v-model="middleNameInputText" type="text" class="form-control" id="middle-name">

          <label for="last-name" class="form-label mt-2">Last name:</label>
          <input v-model="lastNameInputText" type="text" class="form-control" id="last-name" required>
          <div class="invalid-feedback">*field is empty</div>

          <label for="mobile-phone" class="form-label mt-2">Mobile phone:</label>
          <input v-model="mobilePhoneInputText" type="text" class="form-control" id="mobile-phone" required>
          <div class="invalid-feedback">*field is empty</div>

          <label for="home-phone" class="form-label mt-2">Home phone:</label>
          <input v-model="homePhoneInputText" type="text" class="form-control" id="home-phone">

          <button type="button" class="btn btn-primary my-3 me-1" @click="createContact">Add contact</button>
          <button type="button" class="btn btn-secondary my-3 me-1" @click="clearForm">Clear</button>
          <div class="text-danger fw-bold" v-show="$store.state.hasContact">Phone number already exist!</div>
        </form>
      </div>

      <div class="col-md-4">
        <h2 class="display-8 my-3">Search contact</h2>
        <form id="search-form" novalidate>
          <label for="search-contact" class="form-label mt-2">Type search request</label>
          <input v-model="searchInputText" type="text" class="form-control" id="search-contact">
          <button @click="searchContacts" type="button" class="btn btn-primary my-3 me-1">Search</button>
          <button @click="clearSearch" type="button" class="btn btn-secondary my-3 me-1">Clear</button>
        </form>
      </div>
    </div>

    <div class="container px-0">
      <h2 class="display-8 mt-5 mb-3">Contacts list</h2>
      <div class="table-responsive px-0">
        <table class="table align-middle ps-0" id="phone-book">
          <thead class="table-light">
          <tr>
            <th class="col-1" scope="col">
              <input type="checkbox" :value="true" v-model="isCheckedAllContacts">
            </th>
            <th class="col-1" scope="col">№</th>
            <th scope="col">Last Name</th>
            <th scope="col">First Name</th>
            <th scope="col">Middle Name</th>
            <th scope="col">Phone</th>
            <th class="col-1" scope="col">Delete</th>
          </tr>
          </thead>
          <tbody>
          <table-row @set-contact-delete-status="setContactDeleteStatus"
                     @set-contact-to-delete="setContactToDelete"
                     v-for="(contact, index) in contacts" :key="contact.id"
                     :is-checked-all="isCheckedAllContacts"
                     :index="index"
                     :contact="contact"></table-row>
          </tbody>
        </table>
      </div>

      <div class="d-flex flex-row-reverse">
        <button type="button" class="btn btn-danger my-3" data-bs-toggle="modal"
                data-bs-target="#delete-contacts-confirmation" @click="checkContactsDeleteStatus">
          Delete checked
        </button>
      </div>

      <contact-delete-modal-dialog @delete-contact-confirm="deleteContact"></contact-delete-modal-dialog>
      <contacts-delete-modal-dialog @delete-checked-contacts-confirm="deleteCheckedContacts"
                                    :has-contacts-to-delete="hasContactsToDelete"></contacts-delete-modal-dialog>
    </div>
  </div>
</template>

<script>
import TableRow from "../components/PhoneBookTableRow.vue";
import ContactDeleteModalDialog from "../components/ContactDeleteModalDialog.vue";
import ContactsDeleteModalDialog from "../components/ContactsDeleteModalDialog.vue";

export default {
  components: {
    TableRow,
    ContactDeleteModalDialog,
    ContactsDeleteModalDialog
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
        this.searchInputText = "";
        this.term = "";
        this.isCheckedAllContacts = false;

        this.clearForm();

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
      therm: "",

      isInvalid: false,
      isCheckedAllContacts: false,
    };
  },

  methods: {
    searchContacts() {
      this.contactsIdToDelete = [];
      this.term = this.searchInputText.trim();

      let contactId = {
        params: {term: this.term}
      };

      this.$store.dispatch("loadContacts", contactId);
    },

    deleteContact() {
      this.$store.dispatch("deleteContact", {id: this.contactToDelete.id});

      this.searchInputText = "";
    },

    deleteCheckedContacts() {
      this.$store.dispatch("deleteCheckedContacts", this.contactsIdToDelete);

      this.contactsIdToDelete = [];

      this.searchInputText = "";
      this.term = "";
    },

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

      this.$store.dispatch("createContact", newContact);

      this.searchInputText = "";
      this.term = "";
    },

    setContactToDelete(contact) {
      this.contactToDelete = contact;
    },

    setContactDeleteStatus(contactId, deleteStatus) {
      if (deleteStatus) {
        this.contactsIdToDelete.push(contactId);
        return;
      }

      this.contactsIdToDelete = this.contactsIdToDelete.filter(id => id !== contactId);
    },

    checkContactsDeleteStatus() {
      return this.contactsIdToDelete;
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

    clearSearch() {
      this.searchInputText = "";
      this.term = "";
      this.$store.dispatch("loadContacts");
    }
  }
}
</script>