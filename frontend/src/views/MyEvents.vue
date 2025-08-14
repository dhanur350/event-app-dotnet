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
                to="/my-events"
                class="text-slate-600 hover:text-slate-900 px-3 py-2 rounded-md text-sm font-medium"
                :class="{ 'bg-slate-100 text-slate-900': $route.name === 'MyEvents' }"
              >
                My Events
              </router-link>
            </nav>
            
            <div class="flex items-center space-x-4">
              <span class="text-slate-600">Welcome, {{ authStore.username }}</span>
              <button
                @click="handleLogout"
                class="btn-secondary text-sm"
              >
                Logout
              </button>
            </div>
          </div>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <div class="flex items-center justify-between mb-8">
        <div>
          <h2 class="text-3xl font-bold text-slate-900 mb-2">My Events</h2>
          <p class="text-slate-600">Manage the events you've created</p>
        </div>
        
        <button
          @click="showCreateModal = true"
          class="btn-primary flex items-center space-x-2"
        >
          <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
          </svg>
          <span>Create Event</span>
        </button>
      </div>

      <!-- Events Grid -->
      <div v-if="eventsStore.loading" class="flex items-center justify-center py-12">
        <div class="flex items-center space-x-3">
          <div class="spinner w-6 h-6"></div>
          <span class="text-slate-600">Loading your events...</span>
        </div>
      </div>
      
      <div v-else-if="eventsStore.userEvents.length === 0" class="text-center py-12">
        <svg class="mx-auto w-16 h-16 text-slate-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3a2 2 0 012-2h4a2 2 0 012 2v4m-6 4v10m6-10v10m-6-4h6" />
        </svg>
        <h3 class="text-lg font-medium text-slate-900 mb-2">No events created yet</h3>
        <p class="text-slate-600 mb-6">Create your first event to get started!</p>
        <button
          @click="showCreateModal = true"
          class="btn-primary"
        >
          Create Event
        </button>
      </div>
      
      <div v-else class="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <MyEventCard
          v-for="event in eventsStore.userEvents"
          :key="event.id"
          :event="event"
          @edit="editEvent"
          @delete="deleteEvent"
        />
      </div>
    </main>

    <!-- Create/Edit Event Modal -->
    <EventModal
      v-if="showCreateModal || showEditModal"
      :event="editingEvent"
      @close="closeModals"
      @save="saveEvent"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import { useEventsStore } from '../stores/events'
import { useNotificationStore } from '../stores/notification'
import MyEventCard from '../components/MyEventCard.vue'
import EventModal from '../components/EventModal.vue'
import type { Event } from '../types'

const router = useRouter()
const authStore = useAuthStore()
const eventsStore = useEventsStore()
const notificationStore = useNotificationStore()

const showCreateModal = ref(false)
const showEditModal = ref(false)
const editingEvent = ref<Event | null>(null)

const handleLogout = () => {
  authStore.logout()
  notificationStore.showNotification('Logged out successfully', 'success')
  router.push('/events')
}

const editEvent = (event: Event) => {
  editingEvent.value = event
  showEditModal.value = true
}

const deleteEvent = async (event: Event) => {
  if (confirm('Are you sure you want to delete this event?')) {
    try {
      await eventsStore.deleteEvent(event.id, authStore.username!)
      notificationStore.showNotification('Event deleted successfully', 'success')
      // Refresh user events
      await eventsStore.fetchUserEvents(authStore.username!)
    } catch (error: any) {
      notificationStore.showNotification(error.message || 'Failed to delete event', 'error')
    }
  }
}

const closeModals = () => {
  showCreateModal.value = false
  showEditModal.value = false
  editingEvent.value = null
}

const saveEvent = async (eventData: any) => {
  try {
    if (editingEvent.value) {
      await eventsStore.updateEvent(editingEvent.value.id, eventData, authStore.username!)
      notificationStore.showNotification('Event updated successfully', 'success')
    } else {
      await eventsStore.createEvent(eventData, authStore.username!)
      notificationStore.showNotification('Event created successfully', 'success')
    }
    closeModals()
    // Refresh user events
    await eventsStore.fetchUserEvents(authStore.username!)
  } catch (error: any) {
    notificationStore.showNotification(error.message || 'Failed to save event', 'error')
  }
}

onMounted(() => {
  if (authStore.username) {
    eventsStore.fetchUserEvents(authStore.username)
  }
})
</script>