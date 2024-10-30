/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          i: '#091540',
        },
        secondary: {
          i: '#F8F7F9',
        },
      },
    },
  },
  plugins: [],
}