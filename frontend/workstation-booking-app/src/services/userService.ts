import apiClient from "../api/apiClient";

export interface AuthenticatedUser {
  employeeNumber: string;
  fullName: string;
  role: string;
  email: string;
  departmentName: string;
  isAuthenticated: boolean;
}

export interface LoginRequest {
  employeeNumber: string;
  password: string;
}

export interface UserOption {
  id: number;
  employeeNumber: string;
  fullName: string;
  email: string;
  departmentName: string;
  role: string;
}

export const login = async (
  employeeNumber: string,
  password: string,
): Promise<AuthenticatedUser> => {
  const response = await apiClient.post<AuthenticatedUser>("/Users/login", {
    employeeNumber,
    password,
  });

  localStorage.setItem("currentUser", JSON.stringify(response.data));

  return response.data;
};

export const getCurrentUser = (): AuthenticatedUser | null => {
  const user = localStorage.getItem("currentUser");

  if (!user || user === "undefined") {
    return null;
  }

  try {
    return JSON.parse(user);
  } catch {
    localStorage.removeItem("currentUser");
    return null;
  }
};

export const logout = (): void => {
  localStorage.removeItem("currentUser");
};

export const getUsers = async () => {
  const response = await apiClient.get("/Users");
  return response.data;
};

export interface UpdateProfileRequest {
  fullName: string;
  email: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export const updateProfile = async (payload: UpdateProfileRequest) => {
  const response = await apiClient.put("/Users/profile", payload);

  const currentUser = getCurrentUser();

  if (currentUser) {
    localStorage.setItem(
      "currentUser",
      JSON.stringify({
        ...currentUser,
        fullName: payload.fullName,
        email: payload.email,
      }),
    );
  }

  return response.data;
};

export const changePassword = async (payload: ChangePasswordRequest) => {
  const response = await apiClient.put("/Users/change-password", payload);

  return response.data;
};
