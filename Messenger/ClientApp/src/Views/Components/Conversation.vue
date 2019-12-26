<template>
  <div class="conversation" @click="clicked" v-bind:class="{active : isActive}">
    <img :src="conversation.imageData" height="40px" alt="image">
    <div class="info">
      <div class="name">
        {{conversation.firstName}} {{conversation.secondName}}
      </div>
      <div class="lastMessage">
        {{messageContent}}
      </div>
    </div>
    <div class="rigthInfo">
      <div class="date">
        {{lastMessageDate}}
      </div>
      <div class="UnreadMessages" v-if="data.amountOfNewMessages>0">
        {{data.amountOfNewMessages}}
      </div>
    </div>
  </div>
</template>

<script>
  import {mapGetters} from "vuex";

  export default {
    name: "Conversation",
    props: {
      data: Object,
    },
    computed: {
      ...mapGetters({
        currentConversation: 'conversations/currentConversation',
        currentUser: 'account/currentUser',
      }),
      conversation: function () {
        if (this.data.users.length === 2) {
          let user = this.data.users.find(user => user.id !== this.currentUser.Id);
          return user;
        }
        return {};
      },
      lastMessage: function () {
        if (this.data.messages) {
          return this.data.messages[this.data.messages.length-1];
        }
        return {
          date: "",
          content: "",
        };
      },
      messageContent:function(){
        let message = this.lastMessage;
        if(message.text){
          return message.text;
        }
        if(message.fileName){
          return message.fileName;
        }
        return "Not implemented message type";
      },
      lastMessageDate: function(){
        let date = this.lastMessage.sendDateTime;
        let hour = date.getHours();
        let minutes = date.getMinutes();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();
        let dayOfMonth = date.getDate();

        year = year.toString().slice(-2);
        month = month < 10 ? '0' + month : month;
        dayOfMonth = dayOfMonth < 10 ? '0' + dayOfMonth : dayOfMonth;
        hour = hour < 10 ? '0' + hour : hour;
        minutes = minutes < 10 ? '0' + minutes : minutes;

        let diffMs = new Date() - date;
        let diffSec = Math.round(diffMs / 1000);
        let diffMin = diffSec / 60;
        let diffHour = diffMin / 60;
        let diffDay = diffHour/ 24;
        if(diffDay<1){
          return `${hour}:${minutes}`;
        }else{
          return `${dayOfMonth}.${month}.${year} ${hour}:${minutes}`;
        }
      },
      isActive:function(){
        let id = "";
        if(this.currentConversation)
          id= this.currentConversation.conversationId;
        return this.data.conversationId === id;
      }
    },
    methods: {
      clicked: function (event) {
        this.$store.commit("conversations/SetCurrentConversation", this.data, {root: true});
      }
    }
  }
</script>

<style scoped>
  .conversation {
    display: flex;
    flex-direction: row;
    padding: 5px;
    font-size: .85em;
    cursor: pointer;
  }

  .info {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    flex-grow: 1;
    margin: 0 5px 0 5px;
  }

  .date {
    text-align: right;
  }

  .name {
    font-weight: 700;
    word-wrap: break-word;
    white-space: nowrap;
  }

  .lastMessage {
    color: #698192;
    white-space: nowrap;
    overflow: hidden;
  }

  .conversation:hover {
    background-color: #fae2e5;
  }

  .active {
    background-color: #dc3545 !important;
    color: white;
  }

  .active .lastMessage {
    color: #fae2e5;
  }

  .rigthInfo {
    display: flex;
    flex-direction: column;
  }

  .UnreadMessages {
    background-color: orange;
    color: black;
    border-radius: 50%;
    width: 20px;
    align-self: end;
    text-align: center;
  }
</style>
