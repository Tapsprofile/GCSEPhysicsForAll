<template>
  <div class="container">
    <h1>Login</h1>
    <div v-if="error" class="alert alert-error">{{ error }}</div>
    <form @submit.prevent="handleLogin">
      <div class="form-group">
        <label>Email</label>
        <input type="email" v-model="form.email" required />
      </div>
      <div class="form-group">
        <label>Password</label>
        <input type="password" v-model="form.password" required />
      </div>
      <button type="submit" class="btn btn-primary" :disabled="loading">
        {{ loading ? 'Logging in...' : 'Login' }}
      </button>
    </form>
    <p style="margin-top: 1rem;">
      Don't have an account? <router-link to="/register">Register here</router-link>
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
  email: '',
  password: ''
})

const error = ref('')
const loading = ref(false)

const handleLogin = async () => {
  error.value = ''
  loading.value = true
  
  const result = await authStore.login(form.value.email, form.value.password)
  
  loading.value = false
  
  if (result.success) {
    router.push('/dashboard')
  } else {
    error.value = result.message
  }
}
</script>
