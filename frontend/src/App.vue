<template>
  <div id="app">
    <router-view />
    
    <!-- Global Notification -->
    <div
      v-if="notificationStore.show"
      class="fixed top-4 right-4 z-50 max-w-sm w-full animate-slide-up"
    >
      <div
        class="glass-card rounded-lg p-4 shadow-lg border-l-4"
        :class="getNotificationClass()"
      >
        <div class="flex items-center justify-between">
          <div class="flex items-center">
            <svg
              v-if="notificationStore.type === 'success'"
              class="w-5 h-5 mr-3 text-success-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
            </svg>
            <svg
              v-else-if="notificationStore.type === 'error'"
              class="w-5 h-5 mr-3 text-error-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
            <svg
              v-else
              class="w-5 h-5 mr-3 text-primary-600"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            <p class="text-sm font-medium text-slate-900">
              {{ notificationStore.message }}
            </p>
          </div>
          <button
            @click="notificationStore.hideNotification()"
            class="ml-4 text-slate-400 hover:text-slate-600"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useAuthStore } from './stores/auth'
import { useNotificationStore } from './stores/notification'

const authStore = useAuthStore()
const notificationStore = useNotificationStore()

const getNotificationClass = () => {
  switch (notificationStore.type) {
    case 'success':
      return 'border-success-500'
    case 'error':
      return 'border-error-500'
    default:
      return 'border-primary-500'
  }
}

onMounted(() => {
  authStore.initializeAuth()
})
</script>