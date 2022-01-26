<template>
  <div id="app">
      <router-view :isSuccess="$store.state.isSuccess"></router-view>
  </div>
</template>

<script>
export default {
  name: "App"
};
</script>