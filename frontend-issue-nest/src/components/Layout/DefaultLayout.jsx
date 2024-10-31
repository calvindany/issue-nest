import { Container } from "@mui/material";
import { Outlet } from "react-router-dom";

export default function DefaultLayout() {
  return (
    <>
      <Container
        maxWidth="xl"
        className="flex flex-col min-w-[100vw] min-h-[100vh] bg-secondary-i justify-between"
      >
        <Outlet />
      </Container>
    </>
  );
}
