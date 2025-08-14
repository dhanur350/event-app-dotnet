<template>
  <div class="task-card">
    <div class="flex items-start justify-between mb-4">
      <div class="flex-1">
        <h3 class="text-lg font-semibold text-slate-900 mb-1">{{ event.name }}</h3>
        <p class="text-sm text-slate-600">Created by you</p>
      </div>
      
      <div class="flex items-center space-x-2">
        <button
          @click="$emit('edit', event)"
          class="p-2 text-slate-400 hover:text-primary-600 transition-colors duration-200"
          title="Edit event"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" />
          </svg>
        </button>
        
        <button
          @click="$emit('delete', event)"
          class="p-2 text-slate-400 hover:text-error-600 transition-colors duration-200"
          title="Delete event"
        >
          <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
          </svg>
        </button>
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

    <!-- Status Badge -->
    <div class="flex justify-center">
      <span 
        class="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium"
        :class="getStatusBadgeClass()"
      >
        {{ getStatusText() }}
      </span>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Event } from '../types'

const props = defineProps<{
  event: Event
}>()

const emit = defineEmits<{
  edit: [event: Event]
  delete: [event: Event]
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

const getStatusText = () => {
  const percentage = getRsvpPercentage()
  if (percentage >= 100) return 'Full'
  if (percentage >= 80) return 'Almost Full'
  if (percentage > 0) return 'Active'
  return 'No RSVPs Yet'
}

const getStatusBadgeClass = () => {
  const percentage = getRsvpPercentage()
  if (percentage >= 100) return 'bg-error-100 text-error-700'
  if (percentage >= 80) return 'bg-warning-100 text-warning-700'
  if (percentage > 0) return 'bg-success-100 text-success-700'
  return 'bg-slate-100 text-slate-600'
}
</script>