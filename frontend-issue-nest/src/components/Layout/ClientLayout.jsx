import { Container } from "@mui/material";
import { Outlet } from "react-router-dom";

import { Navbar } from "..";

export default function ClientLayout() {
  return (
    <>
      <Navbar />

      <Container
        maxWidth="xl"
        className="flex flex-col min-w-[100vw] min-h-[100vh] bg-secondary-i justify-between pt-10"
      >
        <Outlet />
      </Container>
    </>
  );
}
