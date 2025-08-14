export interface User {
  id: number
  name: string
  email: string
  createdAt: string
}

export interface LoginCredentials {
  username: string
}

export interface Event {
  id: number
  name: string
  date: string
  time: string
  location: string
  description: string
  maxRsvpCount: number
  currentRsvpCount: number
  createdBy: string
  createdAt: string
  updatedAt: string
}

export interface CreateEventDto {
  name: string
  date: string
  time: string
  location: string
  description: string
  maxRsvpCount: number
}

export interface UpdateEventDto {
  name: string
  date: string
  time: string
  location: string
  description: string
  maxRsvpCount: number
}

export interface RsvpStatus {
  eventId: number
  hasRsvped: boolean
}