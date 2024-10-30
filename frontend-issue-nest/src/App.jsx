import * as React from "react";
import './App.css';

import { RouterProvider } from "react-router-dom"

import routers from './routes';


function App() {

  return (
    <>
      <RouterProvider 
          router={routers}
          // fallbackElement={<LoadingPage loading={true}/>}
      />
    </>
  )
}

export default App
