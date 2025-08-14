import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Event, CreateEventDto, UpdateEventDto, RsvpStatus } from '../types'

export const useEventsStore = defineStore('events', () => {
  const events = ref<Event[]>([])
  const userEvents = ref<Event[]>([])
  const loading = ref(false)
  const rsvpStatuses = ref<RsvpStatus[]>([])
  
  // Load initial data from localStorage
  const loadEvents = () => {
    const storedEvents = localStorage.getItem('events')
    if (storedEvents) {
      events.value = JSON.parse(storedEvents)
    }
    
    const storedRsvpData = localStorage.getItem('rsvpData')
    if (storedRsvpData) {
      rsvpStatuses.value = JSON.parse(storedRsvpData)
    }
  }
  
  // Save events to localStorage
  const saveEvents = () => {
    localStorage.setItem('events', JSON.stringify(events.value))
  }
  
  // Save RSVP data to localStorage
  const saveRsvpData = () => {
    localStorage.setItem('rsvpData', JSON.stringify(rsvpStatuses.value))
  }
  
  const fetchAllEvents = async () => {
    loading.value = true
    try {
      // Filter for upcoming events only
      const now = new Date()
      events.value = events.value.filter(event => {
        const eventDateTime = new Date(`${event.date}T${event.time}`)
        return eventDateTime >= now
      })
      saveEvents()
    } finally {
      loading.value = false
    }
  }
  
  const fetchUserEvents = async (username: string) => {
    loading.value = true
    try {
      userEvents.value = events.value.filter(event => event.createdBy === username)
    } finally {
      loading.value = false
    }
  }
  
  const createEvent = async (eventData: CreateEventDto, username: string) => {
    const newEvent: Event = {
      id: Date.now(), // Simple ID generation
      ...eventData,
      currentRsvpCount: 0,
      createdBy: username,
      createdAt: new Date().toISOString(),
      updatedAt: new Date().toISOString()
    }
    
    events.value.unshift(newEvent)
    saveEvents()
    return newEvent
  }
  
  const updateEvent = async (id: number, eventData: UpdateEventDto, username: string) => {
    const index = events.value.findIndex(e => e.id === id && e.createdBy === username)
    if (index !== -1) {
      events.value[index] = {
        ...events.value[index],
        ...eventData,
        updatedAt: new Date().toISOString()
      }
      saveEvents()
      return events.value[index]
    }
    throw new Error('Event not found or unauthorized')
  }
  
  const deleteEvent = async (id: number, username: string) => {
    const index = events.value.findIndex(e => e.id === id && e.createdBy === username)
    if (index !== -1) {
      events.value.splice(index, 1)
      saveEvents()
      // Remove RSVP statuses for this event
      rsvpStatuses.value = rsvpStatuses.value.filter(rsvp => rsvp.eventId !== id)
      saveRsvpData()
    } else {
      throw new Error('Event not found or unauthorized')
    }
  }
  
  const rsvpToEvent = async (eventId: number) => {
    const eventIndex = events.value.findIndex(e => e.id === eventId)
    if (eventIndex === -1) {
      throw new Error('Event not found')
    }
    
    const event = events.value[eventIndex]
    if (event.currentRsvpCount >= event.maxRsvpCount) {
      throw new Error('Event is full')
    }
    
    // Check if user already RSVP'd
    const existingRsvp = rsvpStatuses.value.find(rsvp => rsvp.eventId === eventId)
    if (existingRsvp?.hasRsvped) {
      throw new Error('Already RSVP\'d to this event')
    }
    
    // Increment RSVP count
    events.value[eventIndex].currentRsvpCount++
    
    // Update RSVP status
    if (existingRsvp) {
      existingRsvp.hasRsvped = true
    } else {
      rsvpStatuses.value.push({ eventId, hasRsvped: true })
    }
    
    saveEvents()
    saveRsvpData()
    return events.value[eventIndex]
  }
  
  const hasUserRsvped = (eventId: number): boolean => {
    const rsvpStatus = rsvpStatuses.value.find(rsvp => rsvp.eventId === eventId)
    return rsvpStatus?.hasRsvped || false
  }
  
  const isEventFull = (event: Event): boolean => {
    return event.currentRsvpCount >= event.maxRsvpCount
  }
  
  // Initialize data on store creation
  loadEvents()
  
  return {
    events,
    userEvents,
    loading,
    fetchAllEvents,
    fetchUserEvents,
    createEvent,
    updateEvent,
    deleteEvent,
    rsvpToEvent,
    hasUserRsvped,
    isEventFull
  }
})