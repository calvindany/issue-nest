import React, { lazy } from "react";
import { createBrowserRouter } from "react-router-dom";

const routers = createBrowserRouter([
  {
    path: "/",
    children: [
      {
        async lazy() {
          const { DefaultLayout } = await import("../components");
          return { Component: DefaultLayout };
        },
        children: [
          {
            path: "",
            async lazy() {
              const { Login } = await import("../pages/login");
              return { Component: Login };
            },
          },
        ],
      },
    ],
  },
  {
    path: "/admin",
    children: [
      {
        async lazy() {
          const { AdminLayout } = await import("../components");
          return { Component: AdminLayout };
        },
        children: [
          {
            path: "tickets",
            async lazy() {
              const { Tickets } = await import("../pages/admin");
              return { Component: Tickets };
            },
          },
        ],
      },
    ],
  },
  {
    path: "/client",
    children: [
      {
        async lazy() {
          const { ClientLayout } = await import("../components");
          return { Component: ClientLayout };
        },
        children: [
          {
            path: "tickets",
            async lazy() {
              const { Tickets } = await import("../pages/client");
              return { Component: Tickets };
            },
          },
        ],
      },
    ],
  },
]);

export default routers;
