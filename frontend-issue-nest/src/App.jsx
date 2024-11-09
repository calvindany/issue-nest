import * as React from "react";
import "./App.css";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { Toaster } from 'react-hot-toast';

import { RouterProvider } from "react-router-dom";

import routers from "./routes";

function App() {
  const theme = createTheme({
    typography: {
      fontFamily: "Poppins, sans-serif", // Use your chosen font here
    },
  });
  return (
    <>
      <ThemeProvider theme={theme}>
        <Toaster />
        <RouterProvider
          router={routers}
          // fallbackElement={<LoadingPage loading={true}/>}
        />
      </ThemeProvider>
    </>
  );
}

export default App;
