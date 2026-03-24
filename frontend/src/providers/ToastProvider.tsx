import { createContext, useCallback, useContext, useMemo, useState, type PropsWithChildren } from "react";
import { AnimatePresence, motion } from "framer-motion";
import { CheckCircle2, Sparkles, TriangleAlert } from "lucide-react";

type ToastTone = "success" | "error" | "info";

type Toast = {
  id: string;
  title: string;
  description?: string;
  tone: ToastTone;
};

type ToastContextValue = {
  pushToast: (toast: Omit<Toast, "id">) => void;
};

const ToastContext = createContext<ToastContextValue | null>(null);

export function ToastProvider({ children }: PropsWithChildren) {
  const [toasts, setToasts] = useState<Toast[]>([]);

  const pushToast = useCallback((toast: Omit<Toast, "id">) => {
    const id = crypto.randomUUID();
    setToasts((current) => [...current, { ...toast, id }]);

    window.setTimeout(() => {
      setToasts((current) => current.filter((item) => item.id !== id));
    }, 4200);
  }, []);

  const value = useMemo(() => ({ pushToast }), [pushToast]);

  return (
    <ToastContext.Provider value={value}>
      {children}
      <div className="pointer-events-none fixed right-4 top-4 z-[60] flex w-[min(92vw,24rem)] flex-col gap-3">
        <AnimatePresence>
          {toasts.map((toast) => (
            <motion.div
              key={toast.id}
              initial={{ opacity: 0, y: -12, scale: 0.96 }}
              animate={{ opacity: 1, y: 0, scale: 1 }}
              exit={{ opacity: 0, y: -8, scale: 0.98 }}
              className="rounded-2xl border border-white/10 bg-cosmic/90 p-4 shadow-oracle backdrop-blur-xl"
            >
              <div className="flex items-start gap-3 text-sm text-moon">
                <div className="mt-0.5 rounded-full border border-white/10 bg-white/5 p-2">
                  {toast.tone === "success" && <CheckCircle2 className="size-4 text-gold" />}
                  {toast.tone === "error" && <TriangleAlert className="size-4 text-rose-300" />}
                  {toast.tone === "info" && <Sparkles className="size-4 text-starlight" />}
                </div>
                <div>
                  <p className="font-semibold">{toast.title}</p>
                  {toast.description ? <p className="mt-1 text-mist/90">{toast.description}</p> : null}
                </div>
              </div>
            </motion.div>
          ))}
        </AnimatePresence>
      </div>
    </ToastContext.Provider>
  );
}

export function useToast() {
  const context = useContext(ToastContext);
  if (!context) {
    throw new Error("useToast must be used within ToastProvider");
  }

  return context;
}
