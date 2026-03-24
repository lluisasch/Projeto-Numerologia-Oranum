import { createBrowserRouter } from "react-router-dom";
import { RootLayout } from "@/components/RootLayout";
import { HomePage } from "@/pages/HomePage";
import { NameResultPage } from "@/pages/NameResultPage";
import { BirthDatePage } from "@/pages/BirthDatePage";
import { CompatibilityPage } from "@/pages/CompatibilityPage";
import { NotFoundPage } from "@/pages/NotFoundPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <RootLayout />,
    children: [
      {
        index: true,
        element: <HomePage />,
      },
      {
        path: "resultado/nome",
        element: <NameResultPage />,
      },
      {
        path: "resultado/data",
        element: <BirthDatePage />,
      },
      {
        path: "resultado/compatibilidade",
        element: <CompatibilityPage />,
      },
      {
        path: "*",
        element: <NotFoundPage />,
      },
    ],
  },
]);
