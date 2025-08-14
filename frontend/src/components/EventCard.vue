<template>
  <div class="task-card">
    <div class="flex items-start justify-between mb-4">
      <div class="flex-1">
        <h3 class="text-lg font-semibold text-slate-900 mb-1">{{ event.name }}</h3>
        <p class="text-sm text-slate-600">Created by {{ event.createdBy }}</p>
      </div>
    </div>

    <div class="space-y-3 mb-4">
      <div class="flex items-center text-sm text-slate-600">
        <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7V3a2 2 0 012-2h4a2 2 0 012 2v4m-6 4v10m6-10v10m-6-4h6" />
        </svg>
        {{ formatDate(event.date) }} at {{ formatTime(event.time) }}
      </div>

      <div class="flex items-center text-sm text-slate-600">
        <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
        </svg>
        {{ event.location }}
      </div>

      <div v-if="event.description" class="text-sm text-slate-600">
        <p class="line-clamp-2">{{ event.description }}</p>
      </div>
    </div>

    <!-- RSVP Status -->
    <div class="flex items-center justify-between mb-4">
      <div class="flex items-center space-x-2">
        <span class="text-sm font-medium text-slate-700">RSVPs:</span>
        <span class="text-sm font-bold" :class="getRsvpStatusColor()">
          {{ event.currentRsvpCount }} / {{ event.maxRsvpCount }}
        </span>
      </div>
      
      <div class="flex items-center">
        <div class="w-24 bg-slate-200 rounded-full h-2">
          <div 
            class="h-2 rounded-full transition-all duration-300"
            :class="getRsvpBarColor()"
            :style="{ width: `${getRsvpPercentage()}%` }"
          ></div>
        </div>
      </div>
    </div>

    <!-- RSVP Button -->
    <div class="flex justify-center">
      <button
        v-if="!isAuthenticated"
        disabled
        class="w-full py-2 px-4 bg-slate-100 text-slate-500 rounded-lg text-sm font-medium cursor-not-allowed"
      >
        Log in to RSVP
      </button>
      
      <button
        v-else-if="hasRsvped"
        disabled
        class="w-full py-2 px-4 bg-success-100 text-success-700 rounded-lg text-sm font-medium cursor-not-allowed flex items-center justify-center"
      >
        <svg class="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 13l4 4L19 7" />
        </svg>
        RSVP'd!
      </button>
      
      <button
        v-else-if="isFull"
        disabled
        class="w-full py-2 px-4 bg-error-100 text-error-700 rounded-lg text-sm font-medium cursor-not-allowed"
      >
        Event Full
      </button>
      
      <button
        v-else
        @click="$emit('rsvp', event)"
        class="w-full btn-primary text-sm"
      >
        RSVP
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Event } from '../types'

const props = defineProps<{
  event: Event
  isAuthenticated: boolean
  hasRsvped: boolean
  isFull: boolean
}>()

const emit = defineEmits<{
  rsvp: [event: Event]
}>()

const formatDate = (dateString: string) => {
  const date = new Date(dateString)
  return date.toLocaleDateString('en-US', {
    weekday: 'long',
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

const formatTime = (timeString: string) => {
  const [hours, minutes] = timeString.split(':')
  const date = new Date()
  date.setHours(parseInt(hours), parseInt(minutes))
  return date.toLocaleTimeString('en-US', {
    hour: 'numeric',
    minute: '2-digit',
    hour12: true
  })
}

const getRsvpPercentage = () => {
  return Math.min((props.event.currentRsvpCount / props.event.maxRsvpCount) * 100, 100)
}

const getRsvpStatusColor = () => {
  const percentage = getRsvpPercentage()
  if (percentage >= 100) return 'text-error-600'
  if (percentage >= 80) return 'text-warning-600'
  return 'text-success-600'
}

const getRsvpBarColor = () => {
  const percentage = getRsvpPercentage()
  if (percentage >= 100) return 'bg-error-500'
  if (percentage >= 80) return 'bg-warning-500'
  return 'bg-success-500'
}
</script>