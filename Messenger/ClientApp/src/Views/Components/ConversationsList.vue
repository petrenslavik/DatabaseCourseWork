<template>
  <div class="page col-3 border-danger">
    <div class="searchBox">
      <div class="input-group mb-3">
        <div class="input-group-prepend">
          <span class="input-group-text" id="addon"><i class="fas fa-search"/></span>
        </div>
        <input type="text" placeholder="Search" class="form-control" aria-label="search"
               aria-describedby="addon" @input="searchChats" v-model="chatName">
      </div>
    </div>
    <div class="conversations">
      <conversation v-for="conversation in conversations" :key="conversation.id" :data="conversation"/>
      <div v-if="haveResults" class="group">Global search results</div>
      <possible-chat v-for="chat in searchedChats" :key="chat.username" :data="chat"/>
    </div>
  </div>
</template>

<script>
  import PossibleChat from "@/Views/Components/PossibleChat";
  import Conversation from "@/Views/Components/Conversation";
  import {mapActions, mapGetters} from 'vuex';

  export default {
    name: "ConversationsList",
    components: {
      PossibleChat,
      Conversation,
    },
    data: function () {
      return {
        chatName: null,
      }
    },
    computed: {
      ...mapGetters({
        conversations: 'conversations/conversations',
        currentConversation: 'conversations/currentConversation',
        searchedChats: 'conversations/searchedChats',
      }),
      haveResults:function(){
        return this.searchedChats.length!==0;
      }
    },
    methods: {
      searchChats: function () {
          this.$store.dispatch("conversations/Search", this.chatName, {root: true});
      },
    }
  }
</script>

<style scoped>
  .searchBox {
    padding-left: 5px;
    padding-right: 5px;
  }

  .page {
    border-right: 2px solid black;
    padding-right: 0;
    padding-left: 0;
    padding-top: 10px;
    display: flex;
    flex-direction: column;
  }

  .conversations {
    overflow-x: hidden;
    overflow-y: auto;
    display: flex;
    flex-direction: column;
    flex: 1 0 auto;
    height: 50px;
  }

  .group{
    padding:5px;
    background-color: lightgrey;
  }
</style>
