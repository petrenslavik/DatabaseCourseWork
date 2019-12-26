<template>
    <div class="conversation" @click="clicked" v-bind:class="{active : isActive}">
        <img :src="data.imageData" height="40px">
        <div class="info">
            <div class="name">
                {{data.title}}
            </div>
            <div class="lastMessage">
                @{{data.username}}
            </div>
        </div>
        <div class="date">
            {{data.sendDate}}
        </div>
    </div>
</template>

<script>
  import {mapGetters} from "vuex";

  export default {
        name: "PossibleChat",
        props:{
            data: Object,
        },
      computed: {
        ...mapGetters({
          currentConversation:'conversations/currentConversation'
        }),
        isActive:function(){
          let name = "";
          if(this.currentConversation)
            name = this.currentConversation.username;
          return this.data.username == name;
        }
      },
    methods: {
      clicked: function (event) {
        this.$store.commit("conversations/SetCurrentConversation", this.data, {root: true});
        if (this.data.isUser) {
          this.$store.dispatch("conversations/LoadUserInfo", this.data.username);
        }
      },
    }
    }
</script>

<style scoped>
    .conversation{
        display: flex;
        flex-direction: row;
        padding: 5px;
        font-size: .85em;
        cursor: pointer;
    }
    .info{
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        flex-grow: 1;
        margin: 0 5px 0 5px;
    }
    .date{
        text-align: right;
    }
    .name{
        font-weight:700;
        word-wrap:break-word;
        white-space: nowrap;
    }
    .lastMessage{
        color: #698192;
        white-space: nowrap;
    }
    .conversation:hover{
        background-color: #fae2e5;
    }
    .active{
        background-color:#dc3545 !important;
        color:white;
    }

    .active .lastMessage{
        color: #fae2e5;
    }
</style>
