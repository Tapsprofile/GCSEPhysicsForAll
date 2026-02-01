<template>
  <div id="app">
    <nav class="navbar">
      <div class="nav-container">
        <router-link to="/" class="nav-brand">GCSE Physics For All</router-link>
        <div class="nav-links">
          <template v-if="!authStore.isAuthenticated">
            <router-link to="/login">Login</router-link>
            <router-link to="/register">Register</router-link>
          </template>
          <template v-else>
            <span class="welcome">Welcome, {{ authStore.user?.firstName }}!</span>
            <router-link to="/dashboard">Dashboard</router-link>
            <router-link to="/content">Content</router-link>
            <button @click="logout" class="btn-logout">Logout</button>
          </template>
        </div>
      </div>
    </nav>
    <main class="main-content">
      <router-view />
    </main>
  </div>
</template>

<script setup>
import { useAuthStore } from './stores/auth'

const authStore = useAuthStore()

const logout = () => {
  authStore.logout()
}
</script>
