import * as React from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { Container, Typography, TextField } from "@mui/material";

import { userLocalStorage } from "../../helpers";
import { PasswordField, DefaultButton } from "../../components";

export default function Login() {
  const [email, setEmail] = React.useState("");
  const [password, setPassword] = React.useState("");
  const navigate = useNavigate();

  const handleSubmit = () => {
    const requestBody = {
      email: email,
      password: password,
    };
    axios
      .post(`${import.meta.env.VITE_API_BASE_URL}/auth`, requestBody)
      .then((res) => {
        userLocalStorage.save(res.data.result);
        navigate("/admin/tickets");
      })
      .catch((err) => {
        console.log(err);
      });
  };
  return (
    <>
      <Container className="h-[100vh] flex flex-col items-center justify-center">
        <div className="flex flex-col gap-12 min-w-[400px] max-w-[600px] border border-1 shadow-md rounded-md p-12">
          <Typography variant="p" className="font-bold text-primary-i text-2xl">
            Login Page
          </Typography>

          <div className="flex flex-col gap-4">
            <TextField
              required
              id="outlined-required"
              label="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <PasswordField value={password} onChange={setPassword} />

            <div className="flex justify-end">
              <DefaultButton
                variant="contained"
                type="primary"
                onclick={handleSubmit}
              >
                Login
              </DefaultButton>
            </div>
          </div>
        </div>
      </Container>
    </>
  );
}
