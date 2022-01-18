<template>
  <div class="modal fade" id="delete-contacts-confirmation" data-bs-backdrop="static" data-bs-keyboard="false"
       tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Confirm delete</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <p>Are you sure you want to delete checked contacts?</p>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
          <button type="button" class="btn btn-danger" data-bs-dismiss="modal" :disabled="!hasContactsToDelete"
                  @click="confirmToDeleteCheckedContacts">
            Delete
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    hasContactsToDelete: {
      type: Boolean,
      required: true
    }
  },

  methods: {
    confirmToDeleteCheckedContacts: function () {
      this.$emit("delete-checked-contacts-confirm");
    }
  }
}
</script>