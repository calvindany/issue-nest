import { Container } from '@mui/material';
import { Outlet } from 'react-router-dom';

export default function Layout() {
  return (
    <>
      <Container
        maxWidth="xl"
        className="mt-[70px] px-[0px] flex flex-col min-h-[calc(100vh-70px)] justify-between"
      >
        <Outlet />
      </Container>
    </>
  );
}
