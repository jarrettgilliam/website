import { defineConfig } from 'vite'
import { viteStaticCopy } from 'vite-plugin-static-copy'

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
    },
    plugins: [
        viteStaticCopy({
            targets: [
                {
                    src: 'src/robots.txt',
                    dest: ''
                },
                {
                    src: 'src/favicon.ico',
                    dest: ''
                }
            ]
        })
    ]
})
