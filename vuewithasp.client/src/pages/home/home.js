import '../../assets/main.css'

import { createApp } from 'vue'
//import { createApp } from 'vue/dist/vue.esm-bundler';
import Home from './Home.vue'
import Users from '../../components/Users.vue'
import Menu from './Menu.vue'

import { aliases, mdi } from 'vuetify/iconsets/mdi'
import '@mdi/font/css/materialdesignicons.css' // Ensure you are using css-loader

// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
// VueRouter
import * as VueRouter from 'vue-router'

const routes = [
    {
        path: '/',
        component: Menu,
        children: [
            { path: '', name: 'users', component: Users }
        ]
    },
]

const router = VueRouter.createRouter({    
    history: VueRouter.createWebHashHistory(),
    routes, 
})

const vuetify = createVuetify({
    components,
    directives,
    icons: {
        defaultSet: 'mdi',
        aliases,
        sets: {
            mdi,
        },
    },
})

const app = createApp(Home)

app.use(router)
app.use(vuetify)
app.mount('#home')


