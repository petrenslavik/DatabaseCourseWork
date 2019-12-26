import axios from 'axios'

const api = axios.create();
api.defaults.headers.post['Content-Type'] = 'multipart/form-data';

const state = {
  currentUser: null,
  token: null,
};

const getters = {
  currentUser: state => {
    return state.currentUser;
  },
  isAuthenticated : state =>{
    return state.currentUser != null;
  }
};

const mutations = {
  SET_AUTHENTICATION_TOKEN: (state,data)=>{
    state.token = data.access_token;
    state.currentUser = data.user;
    axios.defaults.headers.common['Authorization'] = data.access_token;
    api.defaults.headers.common['Authorization'] = data.access_token;
  }
};

const actions = {
  Login: async (context,payload)=>{
    return api.post("/api/account/token",payload).then((response)=>context.commit('account/SET_AUTHENTICATION_TOKEN',response.data,{root:true}));
  },
  Register: async(context,payload)=>{
    return api.post("/api/account/register",payload);
  },
  ConfirmRegistration: async(context,payload)=>{
    return api.get("/api/account/confirmRegistration?token="+payload);
  }
};

export default {
  namespaced:true,
  state,
  getters,
  mutations,
  actions,
};
