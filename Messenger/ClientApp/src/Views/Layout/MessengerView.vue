<template>
  <div class="container">
    <StatusRow></StatusRow>
    <div class="page">
      <ConversationsList></ConversationsList>
      <ConversationView></ConversationView>
    </div>
  </div>
</template>

<script>
  import StatusRow from "@/Views/Components/StatusRow";
  import ConversationsList from "@/Views/Components/ConversationsList";
  import ConversationView from "@/Views/Components/ConversationView";

  export default {
    name: "MessengerView",
    components: {ConversationView, ConversationsList, StatusRow},
    created: function () {
      this.$store.commit("conversations/ViewCreated");
      this.$store.dispatch("conversations/LoadAll",null,{root:true});
    }
  }
</script>

<style>

  .container {
    display: flex;
    flex-direction: column;
    height: 100%;
    overflow: hidden;
  }

  .page {
    height: 100%;
    background-color: white;
    margin-bottom: 15px;
    display: flex;
    flex-direction: row;
  }
</style>
