// import axios from "axios";

// const apiClient = axios.create({
//   baseURL: import.meta.env.VITE_API_URL,
//   withCredentials: true,
//   timeout: 10000,
//   headers: {
//     Accept: "application/json",
//   },
// });

// apiClient.interceptors.response.use(
//   (response) => response,
//   (error) => {
//     const status = error.response?.status;

//     const message =
//       error.response?.data?.message ??
//       error.response?.data?.title ??
//       error.response?.statusText ??
//       error.message ??
//       "Request failed.";

//     const customError = new Error(message) as Error & {
//       status?: number;
//     };

//     customError.status = status;

//     return Promise.reject(customError);
//   },
// );

import axios from "axios";

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  withCredentials: true,
  timeout: 10000,
  headers: {
    Accept: "application/json",
  },
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    // Pass the original Axios error through so axios.isAxiosError() remains true
    return Promise.reject(error);
  },
);

export default apiClient;
