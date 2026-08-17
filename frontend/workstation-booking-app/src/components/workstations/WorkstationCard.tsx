import type { Workstation } from "../../models/Workstation";

interface Props {
  workstation: Workstation;
  onClick: (workstation: Workstation) => void;
}

// Config map for light/minimalist styles
const STATUS_STYLES: Record<string, string> = {
  Available:
    "bg-white border-gray-300 text-gray-900 hover:bg-gray-50 hover:border-gray-400",
  Reserved: "bg-gray-200 border-gray-400 text-gray-700 hover:bg-gray-300",
  Booked: "bg-gray-400 border-gray-500 text-gray-900 cursor-not-allowed",
};

const DEFAULT_STYLE = "bg-gray-100 border-gray-300 text-gray-700";

export default function WorkstationCard({ workstation, onClick }: Props) {
  const statusClasses = STATUS_STYLES[workstation.status] ?? DEFAULT_STYLE;

  return (
    <button
      type="button"
      onClick={() => onClick(workstation)}
      aria-label={`Workstation ${workstation.code}, Status: ${workstation.status}`}
      className={`absolute flex h-10 w-17.5 flex-col items-center justify-center rounded-md border-2 p-1 text-xs font-semibold shadow-xs transition-colors focus:outline-hidden focus:ring-2 focus:ring-blue-500 ${statusClasses}`}
      style={{
        left: `${workstation.xPosition}px`,
        top: `${workstation.yPosition}px`,
      }}
    >
      <span className="truncate w-full text-center leading-tight">
        {workstation.code}
      </span>

      {workstation.userName && (
        <span className="truncate w-full text-[10px] font-normal leading-tight opacity-75">
          {workstation.userName}
        </span>
      )}
    </button>
  );
}
