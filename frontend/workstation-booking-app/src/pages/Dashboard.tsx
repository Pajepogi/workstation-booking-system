import { useEffect, useState } from "react";
import toast from "react-hot-toast";

import WorkstationGrid from "../components/workstations/WorkstationGrid";
import BookingModal from "../components/bookings/BookingModal";
import BookingFilters from "../components/filters/BookingFilters";

import { getWorkstations } from "../services/workstationService";
import { createBooking } from "../services/bookingService";
import { cancelBooking } from "../services/bookingService";

import type { Workstation } from "../models/Workstation";
import type { AuthenticatedUser } from "../services/userService";

import type { UserOption } from "../services/userService";

import { getUsers } from "../services/userService";

interface DashboardProps {
  currentUser: AuthenticatedUser;
  onLogout: () => void;
}

export default function Dashboard({ currentUser, onLogout }: DashboardProps) {
  const [workstations, setWorkstations] = useState<Workstation[]>([]);
  const [selectedDesk, setSelectedDesk] = useState<Workstation | null>(null);

  const [loading, setLoading] = useState(false);
  const [bookingLoading, setBookingLoading] = useState(false);

  const [bookingDate, setBookingDate] = useState(
    new Date().toISOString().split("T")[0],
  );

  const [selectedUser, setSelectedUser] = useState(currentUser.employeeNumber);

  const [users, setUsers] = useState<UserOption[]>([]);

  const [isPermanent, setIsPermanent] = useState(false);

  const [wingName, setWingName] = useState("SOUTH");

  const loadWorkstations = async () => {
    try {
      setLoading(true);

      const data = await getWorkstations(bookingDate, wingName);

      setWorkstations(data);
    } catch (error) {
      console.error("Failed to fetch workstations:", error);

      toast.error(
        error instanceof Error ? error.message : "Failed to load workstations.",
      );
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const loadUsers = async () => {
      try {
        const data = await getUsers();

        setUsers(data);
      } catch (error) {
        console.error(error);
      }
    };

    if (currentUser.role === "Admin") {
      loadUsers();
    }
  }, [currentUser.role]);

  useEffect(() => {
    loadWorkstations();
  }, [bookingDate, wingName]);

  const handleConfirm = async () => {
    if (!selectedDesk) return;

    const isReserved =
      selectedDesk.status === "Reserved" || selectedDesk.status === "Booked";

    if (isReserved) {
      const canCancel =
        currentUser.role === "Admin" ||
        selectedDesk.userId === currentUser.employeeNumber;

      if (!canCancel) {
        toast.error("You are not allowed to cancel this booking.");
        return;
      }

      await handleCancellation();
      return;
    }

    const toastId = toast.loading("Creating booking...");

    try {
      setBookingLoading(true);

      const userId = isPermanent ? selectedUser : currentUser.employeeNumber;

      const userName = users.find(
        (e) => e.employeeNumber === selectedUser,
      )?.fullName;
      const userFullname = isPermanent ? userName : currentUser.fullName;

      console.log(
        "user: ",
        userFullname + "   " + selectedUser + " " + isPermanent,
      );

      await createBooking({
        userId: userId,
        userName: userFullname!,
        workstationId: selectedDesk.id,
        bookingDate: new Date(bookingDate).toISOString(),
        isPermanent,
        role: currentUser.role,
      });

      await loadWorkstations();

      setSelectedDesk(null);
      setIsPermanent(false);

      toast.success(`Desk ${selectedDesk.code} booked successfully!`, {
        id: toastId,
      });
    } catch (error) {
      console.error("Booking failed:", error);

      await loadWorkstations();

      setSelectedDesk(null);
      setIsPermanent(false);

      toast.error(
        error instanceof Error ? error.message : "Failed to create booking.",
        {
          id: toastId,
        },
      );
    } finally {
      setBookingLoading(false);
    }
  };

  const handleCancellation = async () => {
    if (!selectedDesk?.bookingId) return;

    const toastId = toast.loading("Cancelling booking...");

    try {
      setBookingLoading(true);

      await cancelBooking(selectedDesk.bookingId);

      await loadWorkstations();

      setSelectedDesk(null);
      setIsPermanent(false);

      toast.success(
        `Desk ${selectedDesk.code} booking cancelled successfully!`,
        {
          id: toastId,
        },
      );
    } catch (error) {
      await loadWorkstations();

      setSelectedDesk(null);
      setIsPermanent(false);

      toast.error(
        error instanceof Error ? error.message : "Failed to cancel booking.",
        {
          id: toastId,
        },
      );
    } finally {
      setBookingLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-slate-100">
      <div className="mx-auto max-w-7xl p-6">
        <div className="flex items-center justify-between rounded-xl bg-white px-4 py-3 shadow-sm mb-6">
          <div>
            <p className="text-xs font-medium uppercase tracking-wide text-slate-400">
              Signed in as
            </p>

            <p className="mt-1 text-sm font-semibold text-slate-800">
              {currentUser.fullName}
            </p>

            <p className="text-xs text-slate-500">{currentUser.role}</p>
          </div>

          <button
            onClick={onLogout}
            className="rounded-md bg-red-500 px-4 py-2 text-white hover:bg-red-600"
          >
            Logout
          </button>
        </div>

        <BookingFilters
          bookingDate={bookingDate}
          wingName={wingName}
          onDateChange={setBookingDate}
          onWingChange={setWingName}
        />

        <div className="rounded-xl bg-white p-6 shadow-sm">
          {loading ? (
            <div className="flex flex-col items-center justify-center py-12">
              <div className="h-10 w-10 animate-spin rounded-full border-4 border-blue-600 border-t-transparent" />

              <p className="mt-4 text-sm text-slate-500">
                Loading workstations...
              </p>
            </div>
          ) : workstations.length === 0 ? (
            <div className="py-12 text-center">
              <p className="text-lg font-medium text-slate-700">
                No workstations available
              </p>

              <p className="mt-1 text-sm text-slate-500">
                Try selecting a different date or wing.
              </p>
            </div>
          ) : (
            <>
              <div className="mb-4 flex items-center justify-between">
                <span className="text-sm text-slate-500">
                  {workstations.length} workstation(s) found
                </span>

                {selectedDesk && (
                  <span className="text-sm font-medium text-blue-600">
                    Selected: {selectedDesk.code}
                  </span>
                )}
              </div>

              <WorkstationGrid
                workstations={workstations}
                onDeskSelected={setSelectedDesk}
              />
            </>
          )}
        </div>

        <BookingModal
          desk={selectedDesk}
          bookingDate={bookingDate}
          isOpen={!!selectedDesk}
          loading={bookingLoading}
          userRole={currentUser.role}
          users={users}
          selectedUser={selectedUser}
          isPermanent={isPermanent}
          onUserChange={setSelectedUser}
          onPermanentChange={setIsPermanent}
          onClose={() => {
            setSelectedDesk(null);
            setIsPermanent(false);
          }}
          onConfirm={handleConfirm}
        />
      </div>
    </div>
  );
}
