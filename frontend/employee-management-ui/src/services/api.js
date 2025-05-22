import axios from "axios";

const API = axios.create({
  baseURL: "http://localhost:5180/api", // .NET Core backend URL
  headers: {
    "Content-Type": "application/json",
  },
});

export default API;
