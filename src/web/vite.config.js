import { defineConfig } from 'vite'

export default defineConfig({
    server: {
        proxy: {
            '/api': {
                target: 'http://localhost:5244',
                changeOrigin: true,
            },
        },
    },
    build: {
        terserOptions: {
            mangle: {
                reserved: ['grecaptcha']
            }
        }
    }
})
