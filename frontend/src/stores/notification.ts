import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useNotificationStore = defineStore('notification', () => {
  const show = ref(false)
  const message = ref('')
  const type = ref<'success' | 'error' | 'info'>('info')
  
  const showNotification = (msg: string, notificationType: 'success' | 'error' | 'info' = 'info') => {
    message.value = msg
    type.value = notificationType
    show.value = true
    
    setTimeout(() => {
      show.value = false
    }, 4000)
  }
  
  const hideNotification = () => {
    show.value = false
  }
  
  return {
    show,
    message,
    type,
    showNotification,
    hideNotification
  }
})