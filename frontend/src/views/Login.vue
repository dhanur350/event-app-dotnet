<template>
  <div class="min-h-screen flex items-center justify-center px-4">
    <div class="max-w-md w-full">
      <div class="glass-card rounded-2xl p-8 animate-fade-in">
        <div class="text-center mb-8">
          <h1 class="text-3xl font-bold text-slate-900 mb-2">Welcome to EventFlow</h1>
          <p class="text-slate-600">Enter your username to get started</p>
        </div>
        
        <form @submit.prevent="handleLogin" class="space-y-6">
          <div>
            <label for="username" class="block text-sm font-medium text-slate-700 mb-2">
              Username
            </label>
            <input
              id="username"
              v-model="form.username"
              type="text"
              required
              class="input-field"
              placeholder="Enter your username"
            />
          </div>
          
          <button
            type="submit"
            :disabled="loading || !form.username.trim()"
            class="w-full btn-primary py-3 text-base"
          >
            <span v-if="loading" class="flex items-center justify-center">
              <div class="spinner mr-2"></div>
              Logging in...
            </span>
            <span v-else>Login</span>
          </button>
        </form>
        
        <div class="mt-6 text-center">
          <p class="text-slate-600 text-sm">
            No password required - just enter any username to continue
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useNotificationStore } from '../stores/notification'

const router = useRouter()
const authStore = useAuthStore()
const notificationStore = useNotificationStore()

const loading = ref(false)
const form = ref({
  username: ''
})

const handleLogin = async () => {
  loading.value = true
  try {
    authStore.login(form.value.username.trim())
    notificationStore.showNotification(`Welcome, ${form.value.username}!`, 'success')
    router.push('/events')
  } catch (error: any) {
    notificationStore.showNotification('Login failed', 'error')
  } finally {
    loading.value = false
  }
}
</script>