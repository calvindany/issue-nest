import * as React from "react";

import { Container } from "@mui/material";
import { Outlet, useNavigate } from "react-router-dom";

import { Navbar } from "..";
import { userLocalStorage } from "../../helpers";

export default function ClientLayout() {
  const navigate = useNavigate();
  React.useEffect(() => {
    const isClient = userLocalStorage.getItem("role_name") == "Client";

    if (!isClient) {
      userLocalStorage.remove();
      navigate("/");
    }
  }, []);

  return (
    <>
      <Navbar />

      <Container
        maxWidth="xl"
        className="flex flex-col bg-secondary-i justify-between pt-10"
      >
        <Outlet />
      </Container>
    </>
  );
}
