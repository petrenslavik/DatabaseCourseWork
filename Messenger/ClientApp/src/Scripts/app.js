import Vue from "vue";
import router from './router';
import App from '@/Views/App.vue';
import 'bootstrap'
import 'bootstrap/dist/css/bootstrap.min.css'
import '@fortawesome/fontawesome-free/js/all'
import {store} from '@/Store/index'

new Vue({
		store,
		router:router,
		render: h => h(App)
}).$mount('#app');
