import React, { lazy } from "react";
import { createBrowserRouter } from "react-router-dom";

const routers = createBrowserRouter([
    {
        path: "/",
        children: [
            {
                async lazy() {
                    const { Layout } = await import(
                        "../components"
                    );
                    return { Component: Layout };
                },
                children: [
                    {
                        path: "",
                        async lazy() {
                            const { Login } = await import(
                                "../pages/login"
                            );
                            return { Component: Login };
                        }
                    }
                ]
            }
        ]
    }
]);

export default routers