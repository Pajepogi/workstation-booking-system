interface BookingFiltersProps {
  bookingDate: string;
  wingName: string;
  onDateChange: (date: string) => void;
  onWingChange: (wing: string) => void;
}

export default function BookingFilters({
  bookingDate,
  wingName,
  onDateChange,
  onWingChange,
}: BookingFiltersProps) {
  return (
    <div className="mb-6 flex flex-wrap gap-4 rounded-lg bg-white p-4 shadow">
      <div>
        <label className="mb-1 block text-sm font-medium">Booking Date</label>

        <input
          type="date"
          value={bookingDate}
          onChange={(e) => onDateChange(e.target.value)}
          min={new Date().toISOString().split("T")[0]}
          className="rounded-md border px-3 py-2"
        />
      </div>

      <div>
        <label className="mb-1 block text-sm font-medium">Wing</label>

        <select
          value={wingName}
          onChange={(e) => onWingChange(e.target.value)}
          className="rounded-md border px-3 py-2"
        >
          <option value="SOUTH">South</option>
          <option value="NORTH">North</option>
        </select>
      </div>
    </div>
  );
}
