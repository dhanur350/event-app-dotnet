<template>
  <div class="min-h-screen">
    <!-- Header -->
    <header class="bg-white/80 backdrop-blur-sm border-b border-white/20 sticky top-0 z-40">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="flex items-center justify-between h-16">
          <div class="flex items-center">
            <h1 class="text-2xl font-bold text-slate-900">EventFlow</h1>
          </div>
          
          <div class="flex items-center space-x-4">
            <nav class="flex space-x-4">
              <router-link
                to="/events"
                class="text-slate-600 hover:text-slate-900 px-3 py-2 rounded-md text-sm font-medium"
                :class="{ 'bg-slate-100 text-slate-900': $route.name === 'PublicEvents' }"
              >
                All Events
              </router-link>
              <router-link
                v-if="authStore.isAuthenticated"
                to="/my-events"
                class="text-slate-600 hover:text-slate-900 px-3 py-2 rounded-md text-sm font-medium"
                :class="{ 'bg-slate-100 text-slate-900': $route.name === 'MyEvents' }"
              >
                My Events
              </router-link>
            </nav>
            
            <div v-if="authStore.isAuthenticated" class="flex items-center space-x-4">
              <span class="text-slate-600">Welcome, {{ authStore.username }}</span>
              <button
                @click="handleLogout"
                class="btn-secondary text-sm"
              >
                Logout
              </button>
            </div>
            <div v-else>
              <router-link to="/login" class="btn-primary text-sm">
                Login
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="mb-8">
        <h2 class="text-3xl font-bold text-slate-900 mb-2">Upcoming Events</h2>
        <p class="text-slate-600">Discover and RSVP to exciting events in your area</p>
      </div>

      <!-- Events Grid -->
      <div v-if="eventsStore.loading" class="flex items-center justify-center py-12">
        <div class="flex items-center space-x-3">
          <div class="spinner w-6 h-6"></div>
          <span class="text-slate-600">Loading events...</span>
        </div>
      </div>
      
      <div v-else-if="eventsStore.events.length === 0" class="text-center py-12">
        <svg class="mx-auto w-16 h-16 text-slate-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3a2 2 0 012-2h4a2 2 0 012 2v4m-6 4v10m6-10v10m-6-4h6" />
        </svg>
        <h3 class="text-lg font-medium text-slate-900 mb-2">No upcoming events</h3>
        <p class="text-slate-600 mb-6">Check back later for new events!</p>
        <router-link
          v-if="authStore.isAuthenticated"
          to="/my-events"
          class="btn-primary"
        >
          Create an Event
        </router-link>
      </div>
      
      <div v-else class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <EventCard
          v-for="event in eventsStore.events"
          :key="event.id"
          :event="event"
          :is-authenticated="authStore.isAuthenticated"
          :has-rsvped="eventsStore.hasUserRsvped(event.id)"
          :is-full="eventsStore.isEventFull(event)"
          @rsvp="handleRsvp"
        />
      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useEventsStore } from '../stores/events'
import { useNotificationStore } from '../stores/notification'
import EventCard from '../components/EventCard.vue'
import type { Event } from '../types'

const router = useRouter()
const authStore = useAuthStore()
const eventsStore = useEventsStore()
const notificationStore = useNotificationStore()

const handleLogout = () => {
  authStore.logout()
  notificationStore.showNotification('Logged out successfully', 'success')
  router.push('/events')
}

const handleRsvp = async (event: Event) => {
  if (!authStore.isAuthenticated) {
    notificationStore.showNotification('Please log in to RSVP', 'error')
    return
  }

  try {
    await eventsStore.rsvpToEvent(event.id)
    notificationStore.showNotification(`Successfully RSVP'd to ${event.name}!`, 'success')
  } catch (error: any) {
    notificationStore.showNotification(error.message || 'Failed to RSVP', 'error')
  }
}

onMounted(() => {
  eventsStore.fetchAllEvents()
})
</script>