import type { Workstation } from "../../models/Workstation";
import type { UserOption } from "../../services/userService";

interface BookingModalProps {
  desk: Workstation | null;
  bookingDate: string;
  isOpen: boolean;
  loading?: boolean;
  userRole: string;
  isPermanent: boolean;
  selectedUser: string;
  users: UserOption[];
  onUserChange: (employeeNumber: string) => void;
  onPermanentChange: (value: boolean) => void;
  onClose: () => void;
  onConfirm: () => void;
}

export default function BookingModal({
  desk,
  bookingDate,
  isOpen,
  loading = false,
  userRole,
  isPermanent,
  selectedUser,
  users,
  onUserChange,
  onPermanentChange,
  onClose,
  onConfirm,
}: BookingModalProps) {
  if (!isOpen || !desk) {
    return null;
  }

  const isReserved = desk.status === "Reserved" || desk.status === "Booked";

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
      <div className="w-full max-w-md rounded-xl bg-white shadow-xl">
        {/* Header */}
        <div className="border-b p-5">
          <h2 className="text-xl font-bold text-gray-900">
            {isReserved ? "Cancel Booking" : "Confirm Booking"}
          </h2>

          <p className="mt-1 text-sm text-gray-500">
            {isReserved
              ? "Review the details before cancelling."
              : "Review your booking details before proceeding."}
          </p>
        </div>

        {/* Content */}
        <div className="space-y-4 p-5">
          <div>
            <p className="text-sm text-gray-500">Desk</p>
            <p className="font-semibold">{desk.code}</p>
          </div>

          <div>
            <p className="text-sm text-gray-500">Desk ID</p>
            <p className="font-semibold">{desk.id}</p>
          </div>

          <div>
            <p className="text-sm text-gray-500">Status</p>
            <p
              className={`font-semibold ${
                isReserved ? "text-red-600" : "text-green-600"
              }`}
            >
              {desk.status}
            </p>
          </div>

          {!isReserved && userRole.toLowerCase() === "admin" && (
            <div>
              <p className="mb-2 text-sm text-gray-500">Reserve For</p>

              <select
                value={selectedUser}
                onChange={(e) => onUserChange(e.target.value)}
                className="w-full rounded-lg border border-gray-300 p-2"
              >
                <option value="">Select Employee</option>

                {users.map((user) => (
                  <option key={user.employeeNumber} value={user.employeeNumber}>
                    {user.fullName} ({user.employeeNumber})
                  </option>
                ))}
              </select>
            </div>
          )}

          {!isReserved && (
            <div>
              <p className="text-sm text-gray-500">Booking Date</p>
              <p className="font-semibold">{bookingDate}</p>
            </div>
          )}

          {!isReserved && userRole.toLowerCase() === "admin" && (
            <div className="rounded-lg border border-amber-200 bg-amber-50 p-4">
              <label className="flex items-start gap-3 cursor-pointer">
                <input
                  type="checkbox"
                  checked={isPermanent}
                  onChange={(e) => onPermanentChange(e.target.checked)}
                  className="mt-1 h-4 w-4 rounded border-gray-300"
                />

                <div>
                  <p className="font-medium text-amber-900">
                    Permanent Workstation Assignment
                  </p>

                  <p className="text-sm text-amber-700">
                    Assign this workstation permanently to the employee. Booking
                    date will be ignored.
                  </p>
                </div>
              </label>
            </div>
          )}

          <div
            className={`rounded-lg p-3 text-sm ${
              isReserved
                ? "border border-red-200 bg-red-50 text-red-800"
                : "border border-blue-200 bg-blue-50 text-blue-800"
            }`}
          >
            {isReserved ? (
              <>
                You are about to cancel the reservation for workstation{" "}
                <strong>{desk.code}</strong>.
              </>
            ) : (
              <>
                {isPermanent ? (
                  <>
                    You are about to permanently assign workstation{" "}
                    <strong>{desk.code}</strong>.
                  </>
                ) : (
                  <>
                    You are about to reserve workstation{" "}
                    <strong>{desk.code}</strong> for{" "}
                    <strong>{bookingDate}</strong>.
                  </>
                )}
              </>
            )}
          </div>
        </div>

        {/* Footer */}
        <div className="flex justify-end gap-3 border-t p-5">
          <button
            type="button"
            onClick={onClose}
            disabled={loading}
            className="rounded-lg border border-gray-300 px-4 py-2 text-gray-700 hover:bg-gray-100 disabled:opacity-50"
          >
            Close
          </button>

          <button
            type="button"
            onClick={onConfirm}
            disabled={loading}
            className={`rounded-lg px-4 py-2 text-white disabled:opacity-50 ${
              isReserved
                ? "bg-red-600 hover:bg-red-700"
                : "bg-blue-600 hover:bg-blue-700"
            }`}
          >
            {loading
              ? isReserved
                ? "Cancelling..."
                : "Booking..."
              : isReserved
                ? "Cancel Booking"
                : "Confirm Booking"}
          </button>
        </div>
      </div>
    </div>
  );
}
