<template>
  <tr>
    <td>
      <input type="checkbox" :value="true" v-model="isChecked" @change="setContactCheckedToDelete">
    </td>
    <td v-text="index + 1"></td>
    <td v-text="contact.firstName"></td>
    <td v-text="contact.lastName"></td>
    <td>
      <div v-for="phoneNumber in contact.phoneNumbers" :key="phoneNumber.id" v-text="phoneNumber.phone"></div>
    </td>
    <td>
      <button @click="setContactToDelete" type="button" class="btn btn-danger" data-bs-toggle="modal"
              data-bs-target="#modal-dialog">
        Delete
      </button>
    </td>
  </tr>
</template>

<script>
export default {
  props: {
    contact: {
      type: Object,
      required: true
    },

    index: {
      type: Number,
      required: true
    },

    isCheckedAll: {
      type: Boolean,
      required: true
    }
  },

  data() {
    return {
      isChecked: false
    };
  },

  methods: {
    setContactToDelete() {
      this.$emit("set-contact-to-delete", this.contact);
    },

    setContactCheckedToDelete() {
      this.$emit("set-contact-checked-to-delete", this.contact.id, this.isChecked);
    }
  },

  watch: {
    isCheckedAll(newValue) {
      this.isChecked = newValue;
      this.setContactCheckedToDelete();
    }
  }
};
</script>