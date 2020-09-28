import axios, { AxiosResponse } from 'axios';
import {IEnvelope} from '../models/envelope';
import { IUser, IUserFormValues } from '../models/user';


axios.defaults.baseURL = "https://localhost:44398/api";

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
    list: (): Promise<IEnvelope> =>
      axios.get('/shows').then(responseBody),
    details: (id: string) => requests.get(`/shows/${id}`)
  };

  const User = {
    current: (): Promise<IUser> => requests.get('/user'),
    login: (user: IUserFormValues): Promise<IUser> => requests.post(`/user/login`, user),
    register: (user: IUserFormValues): Promise<IUser> => requests.post(`/user/register`, user),
}
  export default {
    Shows,User
  };
  