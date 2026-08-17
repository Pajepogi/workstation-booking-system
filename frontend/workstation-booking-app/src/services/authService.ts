import apiClient from "../api/apiClient";
import type { AuthUser } from "../models/AuthUser";

export const getCurrentUser = async (): Promise<AuthUser> => {
  const response = await apiClient.get<AuthUser>("/Auth/current-user");

  return response.data;
};
