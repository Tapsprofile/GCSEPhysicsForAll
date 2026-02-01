<template>
  <div class="container">
    <h1>Register</h1>
    <div v-if="error" class="alert alert-error">{{ error }}</div>
    <form @submit.prevent="handleRegister">
      <div class="form-group">
        <label>First Name</label>
        <input type="text" v-model="form.firstName" required />
      </div>
      <div class="form-group">
        <label>Last Name</label>
        <input type="text" v-model="form.lastName" required />
      </div>
      <div class="form-group">
        <label>Email</label>
        <input type="email" v-model="form.email" required />
      </div>
      <div class="form-group">
        <label>Password</label>
        <input type="password" v-model="form.password" required />
      </div>
      <div class="form-group">
        <label>Role</label>
        <select v-model="form.role" required>
          <option value="">Select a role</option>
          <option value="Student">Student</option>
          <option value="Teacher">Teacher</option>
          <option value="Parent">Parent</option>
        </select>
      </div>
      <button type="submit" class="btn btn-primary" :disabled="loading">
        {{ loading ? 'Registering...' : 'Register' }}
      </button>
    </form>
    <p style="margin-top: 1rem;">
      Already have an account? <router-link to="/login">Login here</router-link>
    </p>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const form = ref({
  firstName: '',
  lastName: '',
  email: '',
  password: '',
  role: ''
})

const error = ref('')
const loading = ref(false)

const handleRegister = async () => {
  error.value = ''
  loading.value = true
  
  const result = await authStore.register(form.value)
  
  loading.value = false
  
  if (result.success) {
    router.push('/dashboard')
  } else {
    error.value = result.message
  }
}
</script>
