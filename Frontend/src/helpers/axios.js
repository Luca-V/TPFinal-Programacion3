import axios from 'axios'

const Api = axios.create({
  baseURL: 'http://localhost:5180/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

const CryptoApi = axios.create({
  baseURL: 'https://criptoya.com/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

export { Api, CryptoApi }