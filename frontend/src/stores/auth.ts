import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const username = ref<string | null>(localStorage.getItem('username'))
  
  const isAuthenticated = computed(() => !!username.value)
  
  const login = (usernameInput: string) => {
    username.value = usernameInput
    localStorage.setItem('username', usernameInput)
  }
  
  const logout = () => {
    username.value = null
    localStorage.removeItem('username')
    // Clear RSVP data on logout
    localStorage.removeItem('rsvpData')
  }
  
  const initializeAuth = () => {
    const storedUsername = localStorage.getItem('username')
    if (storedUsername) {
      username.value = storedUsername
    }
  }
  
  return {
    username,
    isAuthenticated,
    login,
    logout,
    initializeAuth
  }
})