import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { authService } from '../services/auth'

export const useAuthStore = defineStore('auth', () => {
  const user = ref(null)
  const token = ref(localStorage.getItem('token'))

  const isAuthenticated = computed(() => !!token.value)

  const login = async (email, password) => {
    const response = await authService.login(email, password)
    if (response.success) {
      user.value = response.user
      token.value = response.token
      localStorage.setItem('token', response.token)
    }
    return response
  }

  const register = async (formData) => {
    const response = await authService.register(formData)
    if (response.success) {
      user.value = response.user
      token.value = response.token
      localStorage.setItem('token', response.token)
    }
    return response
  }

  const logout = () => {
    user.value = null
    token.value = null
    localStorage.removeItem('token')
  }

  return { user, token, isAuthenticated, login, register, logout }
})
