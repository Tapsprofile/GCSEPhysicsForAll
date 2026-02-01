const API_BASE = import.meta.env.VITE_API_BASE || "http://localhost:5000";

export async function fetchJson(path) {
  const response = await fetch(`${API_BASE}${path}`);
  if (!response.ok) {
    const message = `Request failed (${response.status}) for ${path}`;
    throw new Error(message);
  }
  return response.json();
}
