import { useMemo, useRef, useState, useEffect } from "react";
import type { Workstation } from "../../models/Workstation";
import WorkstationCard from "../workstations/WorkstationCard";

interface Props {
  workstations: Workstation[];
  onDeskSelected: (workstation: Workstation) => void;
  onAddWorkstation?: (position: {
    xPosition: number;
    yPosition: number;
  }) => void;
  isAddMode?: boolean;
  padding?: number;
}

const DESK_WIDTH = 70;
const DESK_HEIGHT = 40;

export default function WorkstationGrid({
  workstations,
  onDeskSelected,
  onAddWorkstation,
  isAddMode = false,
  padding = 80,
}: Props) {
  const containerRef = useRef<HTMLDivElement>(null);
  const [scale, setScale] = useState(1);

  const { baseWidth, baseHeight } = useMemo(() => {
    if (workstations.length === 0) {
      return {
        baseWidth: 300,
        baseHeight: 200,
      };
    }

    //const maxX = Math.max(...workstations.map((w) => w.xPosition + DESK_WIDTH));
    const maxX = Math.max(...workstations.map((w) => w.xPosition + DESK_WIDTH));
    const maxY = Math.max(
      ...workstations.map((w) => w.yPosition + DESK_HEIGHT),
    );

    return {
      baseWidth: maxX + padding,
      baseHeight: maxY + padding,
    };
  }, [workstations, padding]);

  useEffect(() => {
    const updateScale = () => {
      if (!containerRef.current) return;

      const parentWidth =
        containerRef.current.parentElement?.clientWidth ?? baseWidth;

      const nextScale = Math.min(1, parentWidth / baseWidth);

      setScale(nextScale);
    };

    updateScale();

    window.addEventListener("resize", updateScale);

    return () => window.removeEventListener("resize", updateScale);
  }, [baseWidth]);

  const handleCanvasClick = (e: React.MouseEvent<HTMLDivElement>) => {
    if (!onAddWorkstation || !containerRef.current) return;

    const rect = containerRef.current.getBoundingClientRect();

    const xPosition = Math.round((e.clientX - rect.left) / scale);

    const yPosition = Math.round((e.clientY - rect.top) / scale);

    onAddWorkstation({
      xPosition,
      yPosition,
    });
  };

  return (
    <div className="w-full overflow-x-auto p-2">
      <div className="flex justify-center min-w-max">
        <div
          ref={containerRef}
          onClick={handleCanvasClick}
          style={{
            width: `${baseWidth * scale}px`,
            height: `${baseHeight * scale}px`,
          }}
          className={`relative rounded-xl border border-gray-300 bg-gray-50/60 shadow-sm transition-all duration-200 select-none ${
            isAddMode || onAddWorkstation
              ? "cursor-crosshair"
              : "cursor-default"
          }`}
        >
          <div
            style={{
              width: `${baseWidth}px`,
              height: `${baseHeight}px`,
              transform: `scale(${scale})`,
              transformOrigin: "top left",
            }}
            className="absolute inset-0"
          >
            {/* Blueprint Grid */}
            <div
              className="absolute inset-0 rounded-xl"
              style={{
                backgroundImage: `
                  linear-gradient(to right, #e5e7eb 1px, transparent 1px),
                  linear-gradient(to bottom, #e5e7eb 1px, transparent 1px)
                `,
                backgroundSize: "20px 20px",
              }}
            />

            {/* Workstations */}
            {workstations.map((ws) => (
              <div key={ws.id} onClick={(e) => e.stopPropagation()}>
                <WorkstationCard workstation={ws} onClick={onDeskSelected} />
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
