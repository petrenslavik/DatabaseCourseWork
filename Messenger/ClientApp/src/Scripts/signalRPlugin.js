import * as signalR from "@microsoft/signalr";

export default function createSignalRPlugin() {
  return store => {
    let token;
    let messengerHub = new signalR.HubConnectionBuilder().withUrl("/api/messenger", {accessTokenFactory: () => token})
      .build();

    store.subscribe(mutation => {
      if (mutation.type === 'account/SET_AUTHENTICATION_TOKEN') {
        token = mutation.payload.access_token;
        messengerHub.start();
      }
    });

    messengerHub.on("ReceivedNewMessage", (id) => {
      store.dispatch("conversations/LoadMessage", id, {root: true});
    });

    messengerHub.on("ReceivedNewConversation", (id)=>{
      store.dispatch("conversations/LoadConversation", id, {root: true});
    })
  }
}
