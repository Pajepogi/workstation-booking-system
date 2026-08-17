export type Wing = "NORTH" | "SOUTH";

export interface Workstation {
  id: number;
  bookingId?: number;
  code: string;
  wing: Wing;
  xPosition: number;
  yPosition: number;
  width: number;
  height: number;
  isActive: boolean;
  userId?: string;
  userName?: string;
  status: string;
}
