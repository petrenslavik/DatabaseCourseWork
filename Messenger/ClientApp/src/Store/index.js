import Vue from 'vue';
import Vuex from 'vuex';
import account from '@/Store/account'
import conversations from "@/Store/conversations";
import * as signalR from '@microsoft/signalr'
import createSignalRPlugin from "@/Scripts/signalRPlugin";

Vue.use(Vuex);
const plugin = createSignalRPlugin();


export const store = new Vuex.Store({
  state: {
  },
  getters: {},
  mutations: {

  },
  actions: {},
  modules: {
    account,
    conversations,
  },
  plugins: [plugin]
});
