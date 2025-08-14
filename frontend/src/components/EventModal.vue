<template>
  <div class="fixed inset-0 z-50 overflow-y-auto">
    <div class="flex items-center justify-center min-h-screen px-4 pt-4 pb-20 text-center sm:block sm:p-0">
      <!-- Background overlay -->
      <div
        class="fixed inset-0 transition-opacity bg-slate-500 bg-opacity-75 backdrop-blur-sm"
        @click="$emit('close')"
      ></div>

      <!-- Modal panel -->
      <div class="inline-block w-full max-w-md p-6 my-8 overflow-hidden text-left align-middle transition-all transform glass-card rounded-2xl animate-slide-up">
        <div class="flex items-center justify-between mb-6">
          <h3 class="text-lg font-semibold text-slate-900">
            {{ event ? 'Edit Event' : 'Create New Event' }}
          </h3>
          <button
            @click="$emit('close')"
            class="p-2 text-slate-400 hover:text-slate-600 transition-colors duration-200"
          >
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>

        <form @submit.prevent="handleSubmit" class="space-y-6">
          <div>
            <label for="name" class="block text-sm font-medium text-slate-700 mb-2">
              Event Name
            </label>
            <input
              id="name"
              v-model="form.name"
              type="text"
              required
              class="input-field"
              placeholder="Enter event name"
            />
          </div>

          <div class="grid grid-cols-2 gap-4">
            <div>
              <label for="date" class="block text-sm font-medium text-slate-700 mb-2">
                Date
              </label>
              <input
                id="date"
                v-model="form.date"
                type="date"
                required
                class="input-field"
                :min="today"
              />
            </div>

            <div>
              <label for="time" class="block text-sm font-medium text-slate-700 mb-2">
                Time
              </label>
              <input
                id="time"
                v-model="form.time"
                type="time"
                required
                class="input-field"
              />
            </div>
          </div>

          <div>
            <label for="location" class="block text-sm font-medium text-slate-700 mb-2">
              Location
            </label>
            <input
              id="location"
              v-model="form.location"
              type="text"
              required
              class="input-field"
              placeholder="Enter event location"
            />
          </div>

          <div>
            <label for="description" class="block text-sm font-medium text-slate-700 mb-2">
              Description
            </label>
            <textarea
              id="description"
              v-model="form.description"
              rows="3"
              class="input-field resize-none"
              placeholder="Enter event description"
            ></textarea>
          </div>

          <div>
            <label for="maxRsvpCount" class="block text-sm font-medium text-slate-700 mb-2">
              Maximum RSVP Count
            </label>
            <input
              id="maxRsvpCount"
              v-model.number="form.maxRsvpCount"
              type="number"
              required
              min="1"
              max="1000"
              class="input-field"
              placeholder="Enter maximum number of attendees"
            />
          </div>

          <div class="flex space-x-3 pt-4">
            <button
              type="button"
              @click="$emit('close')"
              class="flex-1 btn-secondary"
            >
              Cancel
            </button>
            <button
              type="submit"
              :disabled="loading"
              class="flex-1 btn-primary"
            >
              <span v-if="loading" class="flex items-center justify-center">
                <div class="spinner mr-2"></div>
                Saving...
              </span>
              <span v-else>{{ event ? 'Update' : 'Create' }} Event</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue'
import type { Event } from '../types'

const props = defineProps<{
  event?: Event | null
}>()

const emit = defineEmits<{
  close: []
  save: [eventData: any]
}>()

const loading = ref(false)
const form = ref({
  name: '',
  date: '',
  time: '',
  location: '',
  description: '',
  maxRsvpCount: 50
})

const today = computed(() => {
  return new Date().toISOString().split('T')[0]
})

watch(() => props.event, (newEvent) => {
  if (newEvent) {
    form.value = {
      name: newEvent.name,
      date: newEvent.date,
      time: newEvent.time,
      location: newEvent.location,
      description: newEvent.description,
      maxRsvpCount: newEvent.maxRsvpCount
    }
  } else {
    form.value = {
      name: '',
      date: '',
      time: '',
      location: '',
      description: '',
      maxRsvpCount: 50
    }
  }
}, { immediate: true })

const handleSubmit = async () => {
  loading.value = true
  try {
    emit('save', form.value)
  } finally {
    loading.value = false
  }
}
</script>