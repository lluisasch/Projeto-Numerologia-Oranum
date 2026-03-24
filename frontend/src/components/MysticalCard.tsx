import type { PropsWithChildren } from "react";
import { cn } from "@/lib/utils";

type MysticalCardProps = PropsWithChildren<{
  className?: string;
}>;

export function MysticalCard({ className, children }: MysticalCardProps) {
  return <div className={cn("glass-panel relative overflow-hidden p-6 sm:p-8", className)}>{children}</div>;
}
