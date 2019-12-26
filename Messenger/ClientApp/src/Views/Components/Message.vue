<template>
  <div class="message rounded" :class="[isOwner? 'owner': 'speaker']" >
    <div class="content" v-if="messageType==='text'">
      {{data.text}}
    </div>
    <a :href="'/api/messages/file/' + data.id" class="file" target="_blank" v-else>
      <div class="icon">
        <i class="fas fa-file"/>
      </div>
      <div class="description">
        <div>
          {{data.fileName}}
        </div>
        <div class="sizeInfo">
          {{readableSize(data.size)}}
        </div>
      </div>
    </a>
    <div class="date">
      {{messageDate}}
    </div>
  </div>
</template>

<script>
  import {mapGetters} from "vuex";

  export default {
    name: "Message",
    props: {
      data: Object,
    },

    computed: {
      ...mapGetters({
        currentConversation: 'conversations/currentConversation',
        currentUser: 'account/currentUser',
      }),
      isOwner: function () {
        return this.data.authorId === this.currentUser.Id;
      },
      messageDate: function () {
        let date = this.data.sendDateTime;
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
        let diffDay = diffHour / 24;
        if (diffDay < 1) {
          return `${hour}:${minutes}`;
        } else {
          return `${dayOfMonth}.${month}.${year} ${hour}:${minutes}`;
        }
      },
      messageType: function () {
        if (this.data.text) {
          return "text";
        }
        if (this.data.fileName) {
          return "file";
        }
        return "Not implemented message type";
      },
    },
    methods: {
      readableSize: function (bytes, decimals = 2) {
        if (bytes === 0) return '0 Bytes';

        const k = 1024;
        const dm = decimals < 0 ? 0 : decimals;
        const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

        const i = Math.floor(Math.log(bytes) / Math.log(k));

        return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
      }
    }
  }
</script>

<style scoped>
  .message {
    padding: 5px;
    margin-top: 5px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
  }

  .content {
    align-self: flex-start;
  }

  .date {
    padding-left: 25px;
    font-size: 0.8rem;
    text-align: right;
    color: #698192;
  }

  .owner {
    background-color: #32CD32;
    text-align: right;
    align-self: flex-end;
    padding-right: 10px;
    color: white;
  }

  .speaker {
    background-color: darkblue;
    text-align: left;
    color: white;
    width: fit-content !important;
    width: -moz-fit-content;
  }


  .file {
    padding: 5px;
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
    align-items: start;
  }
</style>
