import Vue from 'vue';
import VueRouter from 'vue-router';
import {store} from '@/Store/index';

Vue.use(VueRouter);

let router = new VueRouter({
  mode: "history",
  routes: [
    {
      name: "Home",
      path: "/",
      component: () => import ("@/Views/Layout/Home"),
      props: true
    },
    {
      name: "Register",
      path: "/register",
      component: () => import ("@/Views/Layout/Register"),
      props: true,
    },
    {
      name: "RegisterConfirmation",
      path: "/confirmRegister",
      component: () => import ("@/Views/Layout/RegisterConfirmation"),
      props: true,
    },
    {
      name: "EmailConfirm",
      path: "/confirmEmail",
      component: () => import ("@/Views/Layout/ConfirmEmail"),
      props: true,
    },
    {
      name: "Login",
      path: "/login",
      component: () => import ("@/Views/Layout/Login"),
      props: true,
    },
    {
      name: "Conversations",
      path: "/conversations",
      component: () => import ("@/Views/Layout/Conversations"),
      props: true,
    },
    {
      name: "MessengerView",
      path: "/messenger",
      component: () => import ("@/Views/Layout/MessengerView"),
      props: true,
      meta:{
        requiresAuth: true,
      }
    },
    {
      name: "Error404",
      path: "*",
      component: () => import ("@/Views/Layout/Error404"),
      props: true
    }
  ]
});

router.beforeEach((to, from, next) => {
  if(to.matched.some(record => record.meta.requiresAuth)) {
    if (!store.getters['account/isAuthenticated']) {
      console.log("heere");
      console.log(store);
      next({
        path: '/login',
        query: { redirect: to.fullPath }
      })
    }
    else{
      next();
    }
  }else {
    next();
  }
});

export default router;
