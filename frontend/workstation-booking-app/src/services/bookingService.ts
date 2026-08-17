import axios from "axios";
import apiClient from "../api/apiClient";
import type { Booking } from "../models/Booking";

export interface CreateBookingRequest {
  userId: string;
  userName: string;
  role: string;
  workstationId: number;
  bookingDate: string;
  isPermanent: boolean;
}

export const throwRequestError = (
  error: unknown,
  fallbackMessage: string,
): Error => {
  if (axios.isAxiosError(error) && error.response?.data) {
    const data = error.response.data;

    // Handles JSON response: { "message": "User already has a booking for this date." }
    if (typeof data === "object" && data !== null) {
      const serverMessage = data.message || data.Message;
      if (serverMessage && typeof serverMessage === "string") {
        return new Error(serverMessage);
      }
    }

    // Handles plain text response: "User already has a booking for this date."
    if (typeof data === "string" && data.trim() !== "") {
      return new Error(data);
    }
  }

  return new Error(fallbackMessage);
};

export const createBooking = async (
  booking: CreateBookingRequest,
): Promise<Booking> => {
  try {
    const response = await apiClient.post<Booking>("/Booking", booking);
    return response.data;
  } catch (error) {
    throw throwRequestError(error, "Unable to create booking.");
  }
};

export const cancelBooking = async (bookingId: number) => {
  try {
    const response = await apiClient.delete(`/Booking/${bookingId}`);
    return response.data;
  } catch (error) {
    throw throwRequestError(error, "Unable to cancel booking.");
  }
};
