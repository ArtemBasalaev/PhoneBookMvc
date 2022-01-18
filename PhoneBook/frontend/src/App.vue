<template>
  <div id="app" v-cloak>
    <div class="container">
      <router-view :isSuccess="$store.state.isSuccess"></router-view>
    </div>
  </div>
</template>

<script>
export default {
  name: 'App',
}
</script>

<style lang="scss">
[v-cloak] {
  display: none;
}
</style>
