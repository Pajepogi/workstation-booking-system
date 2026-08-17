import axios from "axios";
import apiClient from "../api/apiClient";
import type { Workstation } from "../models/Workstation";

const throwRequestError = (error: unknown, fallbackMessage: string): never => {
  if (axios.isAxiosError(error)) {
    const message =
      error.response?.data?.message ?? error.message ?? fallbackMessage;

    throw new Error(message);
  }

  if (error instanceof Error) {
    throw error;
  }

  throw new Error(fallbackMessage);
};

export const getWorkstations = async (
  date: string,
  wingName: string,
): Promise<Workstation[]> => {
  try {
    const response = await apiClient.get<Workstation[]>(
      `/Workstation/status?date=${date}&name=${wingName}`,
    );
    console.log(response.data);
    return response.data;
  } catch (error) {
    throw throwRequestError(error, "Unable to load workstations.");
  }
};
