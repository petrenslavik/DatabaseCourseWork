import axios from 'axios'

let api = null;

const state = {
  conversations: [],
  currentConversation: {},
  searchedChats: [],
  currentUser: {},
  filter: "",
  conversationUsernames: new Set(),
};

const getters = {
  currentChatName: (state, getters, rootState, rootGetters) => {
    let authenticatedUser = rootGetters["account/currentUser"];
    let conversation = state.currentConversation;
    if (conversation.users && conversation.users.length === 2) {
      let user = conversation.users.find(user => user.id !== authenticatedUser.Id);
      return user.firstName + " " + user.secondName;
    }
    if (conversation.title)
      return conversation.title;

    return "";
  },
  conversations: (state, getters, rootState, rootGetters) => {
    let authenticatedUser = rootGetters["account/currentUser"];
    return state.conversations.filter(conversation => {
      if (conversation.users.length === 2) {
        let user = conversation.users.find(user => user.id !== authenticatedUser.Id);
        return user.username.includes(state.filter);
      }
      return true;
    });
  },
  currentConversation: state => {
    return state.currentConversation;
  },
  searchedChats: state => {
    return state.searchedChats;
  },
  currentMessages: state => {
    if (state.currentConversation.messages) {
      return state.currentConversation.messages;
    }
    return [];
  },
  currentUser: state => {
    return state.currentUser;
  },
};

const mutations = {
  ReceiveConversations: (state, data) => {
    state.conversations = data;
    for (let i = 0; i < data.length; i++) {
      let conversation = data[i];
      conversation.users.forEach((user) => {
        state.conversationUsernames.add(user.username);
      });
      if (conversation.messages) {
        conversation.messages.forEach((element) => element.sendDateTime = new Date(Date.parse(element.sendDateTime)));
        conversation.messages.sort((first, second) => first.sendDateTime - second.sendDateTime);
      }
    }
  },
  ReceiveSearchedChats: (state, data) => {
    if (data) {
      state.searchedChats = data.filter((chat) => {
        console.log(!state.conversationUsernames.has(chat.username));
        console.log(state.conversationUsernames);
        return !state.conversationUsernames.has(chat.username);
      });
    }
  },
  ViewCreated: (state, data) => {
    api = axios.create();
    // api.defaults.headers.post['Content-Type'] = 'application/json'
  },
  SetCurrentConversation: (state, data) => {
    state.currentConversation = data;
    state.currentConversation.amountOfNewMessages = 0;
  },
  SetCurrentUser: (state, data) => {
    state.currentUser = data;
  },
  AddNewMessage: (state, message) => {
    message.sendDateTime = new Date(Date.parse(message.sendDateTime));
    let conversation = state.conversations.find(m => m.conversationId === message.conversationId);
    conversation.messages.push(message);
    if (conversation !== state.currentConversation) {
      if (conversation.amountOfNewMessages)
        conversation.amountOfNewMessages++;
      else
        conversation.amountOfNewMessages = 1;
    }
  },
  AddNewConversation: (state, data) => {
    data.users.forEach((user) => {
      state.conversationUsernames.add(user.username);
    });
    state.conversations.unshift(data);
  },
  ApplyFilter: (state, filter) => {
    state.filter = filter;
  }
};

const actions = {
  Search: async (context, payload) => {
    if (payload) {
      api.get("/api/conversations/find/" + payload)
        .then((response) => context.commit('conversations/ReceiveSearchedChats', response.data, {root: true}))
    } else {
      context.commit('conversations/ReceiveSearchedChats', [], {root: true})
    }
    context.commit('ApplyFilter', payload);
  },
  LoadAll: async (context, payload) => {
    api.get("/api/conversations")
      .then((response) => context.commit('conversations/ReceiveConversations', response.data, {root: true}))
  },
  LoadUserInfo: async (context, username) => {
    api.get("/api/users/find/" + username)
      .then((response) => context.commit('SetCurrentUser', response.data));
  },
  LoadMessage: async (context, payload) => {
    api.get("/api/messages/" + payload)
      .then((response) => context.commit('AddNewMessage', response.data));
  },
  LoadConversation: async (context, payload) => {
    api.get("/api/conversations/" + payload)
      .then((response) => context.commit('AddNewConversation', response.data));
  },
  SendTextMessage: async (context, payload) => {
    let data = new FormData();
    data.append("text", payload);
    return context.dispatch("SendMessage", data);
  },
  SendFileMessages: async (context, files) => {
    for (let i = 0; i < files.length; i++) {
      let data = new FormData();
      data.append("file", files[i]);
      context.dispatch("SendMessage", data);
    }
  },
  SendMessage: async (context, form) => {
    let conversation = context.getters.currentConversation;
    if (conversation.conversationId) {
      form.append("conversationId", conversation.conversationId);
    } else {
      form.append("userId", context.getters.currentUser.id);
    }
    return api.post("/api/messages", form).then((response) => context.commit('AddNewMessage', response.data));
  }
};
export default {
  namespaced: true,
  state,
  getters,
  mutations,
  actions,
};
