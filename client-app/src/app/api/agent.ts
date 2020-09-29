import axios, { AxiosResponse } from 'axios';
import {IEnvelope} from '../models/envelope';
import { IUser, IUserFormValues } from '../models/user';
import { history } from '../..';


axios.defaults.baseURL = "https://localhost:44398/api";

axios.interceptors.request.use((config) => {
  const token = window.localStorage.getItem('jwt');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
}, error => {
  return Promise.reject(error);
})

axios.interceptors.response.use(undefined, error => {
  if (error.message === 'Network Error' && !error.response) {
    history.push('/notfound')
  }

  const {status, data, config} = error.response;

  if (status === 404) {
      history.push('/notfound')
  }
  if (status === 400 && config.method === 'get' && data.errors.hasOwnProperty('id')) {
      history.push('/notfound')
  }
  if (status === 500) {
    console.log('Server error - check the terminal for more info!')
  }
  throw error.response;
})
const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    get: (url: string) =>
      axios
        .get(url)
        .then(responseBody),
    post: (url: string, body: {}) =>
      axios
        .post(url, body)
        .then(responseBody),
    put: (url: string, body: {}) =>
      axios
        .put(url, body)
        .then(responseBody),
    del: (url: string) =>
      axios
        .delete(url)
        .then(responseBody),
    postForm: (url: string, file: Blob) => {
      let formData = new FormData();
      formData.append('File', file);
      return axios
        .post(url, formData, {
          headers: { 'Content-type': 'multipart/form-data' }
        })
        .then(responseBody);
    }
  };

  const Shows = {
    list: (params: URLSearchParams): Promise<IEnvelope> =>
      axios.get('/shows',{params: params}).then(sleep(500)).then(responseBody),
    details: (id: string) => requests.get(`/shows/${id}`)
  };

  const User = {
    current: (): Promise<IUser> => requests.get('/user'),
    login: (user: IUserFormValues): Promise<IUser> => requests.post(`/user/login`, user),
    register: (user: IUserFormValues): Promise<IUser> => requests.post(`/user/register`, user),
}

const sleep = (ms: number) => (response: AxiosResponse) =>
  new Promise<AxiosResponse>(resolve =>
    setTimeout(() => resolve(response), ms)
  );
  export default {
    Shows,User
  };
  