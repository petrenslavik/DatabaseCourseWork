<template>
    <div id="app">
      <transition name="slide" mode="out-in">
        <router-view></router-view>
      </transition>
    </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style>
    #app{
        height: 100%;
        background-color: forestgreen;
    }
    .slide-leave-active,
    .slide-enter-active {
      transition: 0.5s;
    }
    .slide-enter {
      transform: translate(100%, 0);
    }
    .slide-leave-to {
      transform: translate(-100%, 0);
    }

</style>
