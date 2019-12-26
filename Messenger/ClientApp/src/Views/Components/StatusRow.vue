<template>
  <div class="statusRow  card rounded-top">
    <div class="col-3">Messenger</div>
    <div class="speaker">
      <div v-if="currentChatName">
        {{currentChatName}}
      </div>
      <div v-else>
      </div>
    </div>
    <div class="dropdown show dropleft">
      <a class="btn btn-primary dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown"
         aria-haspopup="true" aria-expanded="false">
        ...
      </a>

      <div class="dropdown-menu" aria-labelledby="dropdownMenuLink">
        <a class="dropdown-item" href="#" @click="clearConversation">Delete conversation</a>
      </div>
    </div>
  </div>
</template>

<script>
  import {mapGetters} from 'vuex';

  export default {
    name: "StatusRow",
    computed: {
      ...mapGetters({
        currentChatName:'conversations/currentChatName'
      })
    },
    methods:{
      clearConversation: function(event){
        this.$store.dispatch("conversations/DeleteConversation");
      }
    }
  }
</script>

<style scoped>
  .speaker {
    flex: 1 0 auto;
    display: flex;
    justify-content: center;
    align-items: center;
  }

  .statusRow {
    flex-direction: row;
    width: 100%;
    text-align: center;
    background-color: #dc3545;
    min-height: 40px;
    color: white;
  }

  .statusRow .col-3 {
    display: flex;
    justify-content: center;
    align-items: center
  }

  .card {
    border-width: 1px;
    border-bottom-width: 0;
    padding-right: 0;
    padding-left: 0;
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }
</style>
