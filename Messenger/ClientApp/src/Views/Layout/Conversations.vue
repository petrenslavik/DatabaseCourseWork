<template>
  <div class="container">
    <div class="statusRow  card rounded-top">
      <div class="col-3">Messenger</div>
      <div class="speaker">
        <div v-if="currentUser">
          {{currentUser.FirstName}} {{currentUser.SecondName}}
        </div>
        <div v-else-if="currentConversation">
          {{currentConversation.Speaker.FirstName}} {{currentConversation.Speaker.SecondName}}
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
    <div class="page">
      <div class="col-3 border-danger">
        <div class="searchBox">
          <div class="input-group mb-3">
            <div class="input-group-prepend">
              <span class="input-group-text" id="addon"><i class="fas fa-search"></i></span>
            </div>
            <input type="text" placeholder="Search" class="form-control" aria-label="search"
                   aria-describedby="addon" @input="searchUsers" v-model="searchStr">
          </div>
        </div>
        <div class="conversations">
          <conversation :class="[currentConversation == conversation? 'active': '']"
                        v-on:setConversation="setCurrentConversation" v-for="conversation in conversations"
                        v-if="!isSearching" :key="conversation.id"
                        :data="conversation"></conversation>
          <possible-user v-on:setConversation="setCurrentUser" v-for="user in users" v-if="isSearching"
                         :key="user.id" :data="user"></possible-user>
        </div>
      </div>
      <div class="wrapper">
        <div class="messages">
          <message v-for="message in currentMessages" :key="message.id" :data="message"
                   :curUser="authorizedUser"></message>
        </div>
        <div class="sendMessage">
          <div class="input-group mb-3">
            <input type="text" class="form-control" v-model="curMessage" placeholder="Message ..."
                   aria-label="Enter your message:" v-on:keyup.enter="sendMessage">
            <div class="input-group-append">
              <button v-on:click="sendMessage" class="btn btn-outline-secondary" type="button"><i
                class="fas fa-paper-plane"></i></button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import PossibleUser from "@/Views/Components/PossibleChat";
  import Conversation from "@/Views/Components/Conversation";
  import Message from "@/Views/Components/Message";

  export default {
    name: "Conversations",
    components: {
      Conversation,
      PossibleUser,
      Message
    },
    data: function () {
      return {
        searchStr: null,
        conversations: null,
        users: null,
        isSearching: false,
        currentConversation: null,
        currentUser: null,
        currentElement: null,
        curMessage: null,
        authorizedUser: null,
        currentMessages: null,
        lastUpdate: null,
        messagesElement: null,
      }
    },
    watch: {
      currentConversation: function () {
        let data = new FormData();
        data.append("conversationId", this.currentConversation.Id);
        // lib.fetch("message/GetAll", data, this.messagesLoaded);
      }
    },
    created: function () {
      // lib.fetch("conversation/getAll", null, this.receivedConversations);
      //  this.lastUpdate = new Date();
      // lib.fetch("user/getCurrent", null, this.curUserReceived);
      // setInterval(this.loadMessages, 5000);
    },
    mounted: function () {
      this.messagesElement = document.querySelector(".messages");
    },
    methods: {
      loadMessages: function () {
        let data = new FormData();
        data.append("date", this.lastUpdate.toISOString());
        this.lastUpdate = new Date();
        // lib.fetch("message/GetUpdate", data, this.loadedMessages);
      },
      loadedMessages: function (data) {
        for (let mes of data) {
          let message = {};
          message.ConversationId = mes.ConversationId;
          if (this.currentConversation != null && this.currentConversation.Id == message.ConversationId) {
            message.Id = mes.MessageId;
            message.IsRead = mes.IsRead;
            message.SendDateTime = mes.SendDateTime;
            message.SenderId = mes.SenderId;
            message.TypeId = mes.TypeId;
            message.Content = {};
            message.Content.Id = mes.Id;
            message.Content.Text = mes.Text;
            message.Content.MessageId = mes.MessageId;
            this.currentMessages.push(message);
            this.currentConversation.LastMessage.Content.Text = message.Content.Text;
            this.currentConversation.LastMessage.SendDateTime = mes.SendDateTime;
          } else {
            let flag = false;
            for (let conversation of this.conversations) {
              if (conversation.Id == message.ConversationId) {
                flag = true;
                conversation.NewMessages++;
                conversation.LastMessage.Content.Text = mes.Text;
                conversation.LastMessage.SendDateTime = mes.SendDateTime;
              }
            }
            if (flag == false) {
              let requestData = new FormData();
              requestData.append("conversationId", mes.ConversationId);
              //  lib.fetch("conversation/GetConversation", requestData, this.receivedConversation);
            }
          }
        }
        this.messagesElement.scrollTop = this.messagesElement.scrollHeight;
      },
      receivedConversation: function (data) {
        data.NewMessages = 1;
        this.conversations.push(data);
      },
      messagesLoaded: function (data) {
        this.currentMessages = data;
        this.messagesElement.scrollTop = this.messagesElement.scrollHeight;
      },
      curUserReceived: function (data) {
        if (!data.Id)
          this.$router.push("/login");
        this.authorizedUser = data;
      },
      receivedConversations: function (data) {
        if (data.code == 0) {
        } else {
          this.successfulLoad(data);
        }
      },
      successfulLoad: function (data) {
        for (let conversation of data) {
          conversation.NewMessages = 0;
        }
        this.conversations = data;
      },
      searchUsers: function (event) {
        let value = event.target.value;
        if (value) {
          this.isSearching = true;
          let data = new FormData();
          data.append("searchString", value);
          // lib.fetch("user/FindUsers", data, this.receivedUsers);
        } else {
          this.isSearching = false;
          this.users = null;
        }
      },
      receivedUsers: function (data) {
        if (data.code == 0 && this.isSearching) {
        } else {
          var arr = data[0];
          this.users = arr;
        }
      },
      setCurrentUser: function (data, element) {
        this.currentMessages = null;
        this.currentUser = data;
        if (this.currentElement) {
          this.currentElement.classList.toggle("active");
        }

        this.currentElement = element;
        element.classList.toggle("active");
      },
      setCurrentConversation: function (data, element) {
        if (this.currentElement) {
          this.currentElement.classList.toggle("active");
        }
        this.currentMessages = null;
        this.searchStr = null;
        this.users = null;
        this.isSearching = null;
        this.currentUser = null;
        this.currentConversation = data;
        data.NewMessages = 0;
        this.currentElement = element;
        element.classList.toggle("active");
      },
      sendMessage: function (event) {
        let val = this.curMessage;
        if (!val)
          return;
        if (this.currentUser) {
          let data = new FormData();
          data.append("userId", this.currentUser.Id);
          data.append("messageType", "1");
          data.append("messageContent", val);
          // lib.fetch("conversation/CreateConversation", data, this.conversationCreated);
        } else if (this.currentConversation) {
          let data = new FormData();
          data.append("conversationId", this.currentConversation.Id);
          data.append("messageType", "1");
          data.append("messageContent", val);
          //  lib.fetch("message/AddMessage", data, this.messageSent);
        }
        this.curMessage = null;
      },
      messageSent: function (data) {
        this.currentMessages.push(data);
        this.currentConversation.LastMessage.Content.Text = data.Content.Text;
        this.currentConversation.LastMessage.SendDateTime = data.SendDateTime;
        this.messagesElement.scrollTop = this.messagesElement.scrollHeight;
      },
      conversationCreated: function (data) {
        data.Speaker = this.currentUser;
        this.conversations.unshift(data);
        if (this.currentElement) {
          this.currentElement.classList.toggle("active");
        }
        this.currentMessages = null;
        this.searchStr = null;
        this.users = null;
        this.isSearching = null;
        this.currentUser = null;
        this.currentConversation = data;
        this.currentElement = document.querySelector(".conversations").firstElementChild;
        console.log(this.currentElement);
        this.currentElement.classList.toggle("active");
      },
      clearConversation: function () {
        if (this.currentConversation) {
          let data = new FormData();
          data.append("conversationId", this.currentConversation.Id);
          // lib.fetch("conversation/Delete",data,this.clearedConversation);
        }
      },
      clearedConversation: function () {
        for (let i = 0; i < this.conversations.length; i++) {
          if (this.conversations[i] === this.currentConversation) {
            this.conversations.splice(i, 1);
            break;
          }
        }
        this.currentConversation = null;
        this.currentMessages = null;
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

  .messages {
    overflow-y: auto;
    display: flex;
    flex-direction: column;
    flex: 1 0 auto;
    height: 50px;
  }

  .sendMessage {
    padding-top: 10px;
  }

  .wrapper {
    padding-left: 5px;
    padding-right: 5px;
    flex-grow: 1;
    display: flex;
    flex-direction: column;
  }

  .container {
    display: flex;
    flex-direction: column;
    height: 100%;
    overflow: hidden;
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
    border-bottom-width: 0px;
    padding-right: 0;
    padding-left: 0;
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }

  .page {
    height: 100%;
    background-color: white;
    margin-bottom: 15px;
    display: flex;
    flex-direction: row;
  }

  .searchBox {
    padding-left: 5px;
    padding-right: 5px;
  }

  .page .col-3 {
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

  .sendMessage {
    max-height: 400px;
  }
</style>
