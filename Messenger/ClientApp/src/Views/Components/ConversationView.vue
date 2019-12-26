<template>
  <div class="wrapper" ref="ddropzone">
    <div style="visibility:hidden;opacity: 0;" ref="dropzone" class="dropzone">
      <div ref="textnode" class="textnode">Drop files to send.</div>
    </div>
    <div v-if="files.length!==0" class="popup-container">
      <div class="popup">
        <div class="file" v-for="file in files">
          <div class="icon">
            <i class="fas fa-file"/>
          </div>
          <div class="description">
            <div>
              {{file.name}}
            </div>
            <div class="sizeInfo">
              {{readableSize(file.size)}}
            </div>
          </div>
        </div>
        <div class="buttons">
          <button @click="deactivate">Cancel</button>
          <button @click="send">Send</button>
        </div>
      </div>
    </div>
    <div class="messages">
      <message v-for="message in currentMessages" :key="message.id" :data="message"/>
    </div>
    <div class="sendMessage">
      <div class="input-group mb-3">
        <input type="text" class="form-control" v-model="currentText" placeholder="Message ..."
               aria-label="Enter your message:" v-on:keyup.enter="sendMessage">
        <div class="input-group-append">
          <button v-on:click="sendMessage" class="btn btn-outline-secondary" type="button">
            <i class="fas fa-paper-plane"/>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import Message from "@/Views/Components/Message";
  import {mapGetters} from "vuex";

  export default {
    name: "ConversationView",
    components: {Message},
    data: function () {
      return {
        currentText: "",
        files: [],
      };
    },
    computed: {
      ...mapGetters({
        currentMessages: "conversations/currentMessages",
        currentConversation: "conversations/currentConversation",
      }),
    },
    methods: {
      sendMessage: function () {
        if (this.currentText) {
          this.$store.dispatch("conversations/SendTextMessage", this.currentText)
            .then(() => this.currentText = "");
        }
      },
      readableSize: function (bytes, decimals = 2) {
        if (bytes === 0) return '0 Bytes';

        const k = 1024;
        const dm = decimals < 0 ? 0 : decimals;
        const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

        const i = Math.floor(Math.log(bytes) / Math.log(k));

        return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
      },
      deactivate: function () {
        this.files = [];
      },
      send: function () {
        this.$store.dispatch("conversations/SendFileMessages", this.files, {root: true})
          .then(() => this.files = []);
      }
    },
    mounted() {
      let dropElement = this.$refs.dropzone;
      let wrapper = this.$refs.ddropzone;
      let dropElementText = this.$refs.textnode;
      ['dragover', 'drop'].forEach(type => {
        document.addEventListener(type, (e) => {
          e.preventDefault();
        });
      });

      wrapper.addEventListener("dragenter", function (e) {
        dropElement.style.visibility = "";
        dropElement.style.opacity = 1;
        dropElementText.style.fontSize = "48px";
      });

      wrapper.addEventListener("dragleave", function (e) {
        e.preventDefault();

        dropElement.style.visibility = "hidden";
        dropElement.style.opacity = 0;
        dropElementText.style.fontSize = "42px";
      });

      wrapper.addEventListener("dragover", function (e) {
        e.preventDefault();
        dropElement.style.visibility = "";
        dropElement.style.opacity = 1;
        dropElementText.style.fontSize = "48px";
      });


      wrapper.addEventListener("drop", (e) => {
        e.preventDefault();
        dropElement.style.visibility = "hidden";
        dropElement.style.opacity = 0;
        dropElementText.style.fontSize = "42px";
        console.log(Object.keys(this.currentConversation).length === 0 && this.currentConversation.constructor === Object);
        if(!(Object.keys(this.currentConversation).length === 0 && this.currentConversation.constructor === Object )) {
          for (let i = 0; i < e.dataTransfer.files.length; i++) {
            this.files.push(e.dataTransfer.files[i]);
          }
        }
      });

      //document.addEventListener("click", (e) => {
      //  let element = e.target;
      //  let isHandled = false;
      //  let container = wrapper.children[1];
      //  while (element.parentElement !== null) {
      //    if (element.parentElement === container) {
      //      e.stopPropagation();
      //      e.preventDefault();
      //      isHandled = true;
      //      return;
      //    } else
      //      element = element.parentElement;
      //  }
      //  if (!isHandled) {
      //    this.deactivate();
      //  }
      //  ;
      //});
    }
  }
</script>

<style scoped>
  .messages {
    display: flex;
    flex-direction: column;
    flex: 1 0 auto;
    overflow-y: auto;
    height: 100px;
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
    position: relative;
  }

  .sendMessage {
    max-height: 400px;
  }

  div.dropzone {
    position: absolute;
    top: 0;
    left: 0;
    z-index: 9999999999;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    transition: visibility 175ms, opacity 175ms;
    display: table;
    text-shadow: 1px 1px 2px #000;
    color: #fff;
    font: bold 42px Oswald, DejaVu Sans, Tahoma, sans-serif;
  }

  div.textnode {
    display: table-cell;
    text-align: center;
    vertical-align: middle;
    transition: font-size 175ms;
  }

  .popup-container {
    align-items: center;
    justify-content: center;
    display: flex;
    position: absolute;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    z-index: 9999999;
    margin-left: -5px;
  }

  .popup {
    border-radius: 6px;
    box-shadow: 0 10px 20px rgba(0, 0, 0, 0.10), 0 6px 6px rgba(0, 0, 0, 0.16);
    position: absolute;
    padding: 10px;
    background: #6422EB;
    min-width: 300px;
  }

  .file {
    padding: 5px;
    background-color: #4d8dd2;
    border-radius: 6px;
    display: flex;
    flex-direction: row;
    margin-bottom: 10px;
  }

  .icon {
    display: flex;
    align-items: center;
    justify-content: center;
    color: #dc3545;
    background-color: white;
    font-size: 20px;
    min-width: 48px;
    min-height: 48px;
    border-radius: 50%;
  }

  .description {
    display: flex;
    flex-direction: column;
    color: white;
    flex: 1 0 auto;
    padding: 0 5px;
  }

  .buttons {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
    margin-top: 5px;
  }

  .buttons > button {
    color: white;
    border: none;
    padding: 10px;
    margin-left: 5px;
    background: #6422EB;
    border-radius: 6px;
    min-width: 80px;
  }

  .buttons > button:hover {
    background: #501bbc;
  }
</style>
