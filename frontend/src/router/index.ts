import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import Login from '../views/Login.vue'
import PublicEvents from '../views/PublicEvents.vue'
import MyEvents from '../views/MyEvents.vue'

const routes = [
  {
    path: '/',
    redirect: '/events'
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/events',
    name: 'PublicEvents',
    component: PublicEvents
  },
  {
    path: '/my-events',
    name: 'MyEvents',
    component: MyEvents,
    meta: { requiresAuth: true }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore()
  
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login')
  } else {
    next()
  }
})

export default router