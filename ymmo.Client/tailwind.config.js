/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        navy: {
          50: '#eef1f8',
          100: '#d4dceb',
          200: '#a9b9d7',
          300: '#7e96c3',
          400: '#4d6aa0',
          500: '#2c4a80',
          600: '#1f3661',
          700: '#172a4d',
          800: '#101d38',
          900: '#0a1326',
          950: '#050a16',
        },
        brand: {
          50: '#eefcf6',
          100: '#d6f7e8',
          200: '#aeefd2',
          300: '#7be3b8',
          400: '#43cf99',
          500: '#1fb482',
          600: '#15936b',
          700: '#137559',
          800: '#125c48',
          900: '#104b3c',
        },
      },
    },
  },
  plugins: [],
}
