import axios from 'axios'

const API_BASE = '/api'

export const authService = {
  async login(email, password) {
    try {
      const response = await axios.post(`${API_BASE}/auth/auth/login`, {
        email,
        password
      })
      return response.data
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Login failed'
      }
    }
  },

  async register(formData) {
    try {
      const response = await axios.post(`${API_BASE}/auth/auth/register`, formData)
      return response.data
    } catch (error) {
      return {
        success: false,
        message: error.response?.data?.message || 'Registration failed'
      }
    }
  }
}

export const contentService = {
  async getAll(filter = {}) {
    const response = await axios.get(`${API_BASE}/content/content`, { params: filter })
    return response.data
  },

  async getById(id) {
    const response = await axios.get(`${API_BASE}/content/content/${id}`)
    return response.data
  },

  async create(data) {
    const response = await axios.post(`${API_BASE}/content/content`, data)
    return response.data
  },

  async update(id, data) {
    const response = await axios.put(`${API_BASE}/content/content/${id}`, data)
    return response.data
  },

  async delete(id) {
    await axios.delete(`${API_BASE}/content/content/${id}`)
  }
}
