import * as React from "react";

import { Container } from "@mui/material";
import { Outlet, useNavigate } from "react-router-dom";

import { Navbar } from "../";
import { userLocalStorage } from "../../helpers";

export default function AdninLayout() {
  const navigate = useNavigate();
  React.useEffect(() => {
    const isAdmin = userLocalStorage.getItem("role") == "Admin";

    if (!isAdmin) {
      userLocalStorage.remove();
      navigate("/");
    }
  }, []);

  return (
    <>
      <Navbar />

      <Container
        maxWidth="xl"
        className="flex flex-col min-h-[100vh] bg-secondary-i justify-between pt-10"
      >
        <Outlet />
      </Container>
    </>
  );
}
