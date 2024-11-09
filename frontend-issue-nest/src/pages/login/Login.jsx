import * as React from "react";
import axios from "axios";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";
import { Container, Typography, TextField } from "@mui/material";

import { userLocalStorage } from "../../helpers";
import { PasswordField, DefaultButton } from "../../components";

export default function Login() {
  const [email, setEmail] = React.useState("");
  const [password, setPassword] = React.useState("");
  const navigate = useNavigate();

  React.useEffect(() => {
    const handleKeyDown = (event) => {
      if(event.key == "Enter") {
        if(email.trim() && password.trim()){
          handleSubmit();
        }
      }
    }

    window.addEventListener("keydown", handleKeyDown)

    return () => {
      window.removeEventListener("keydown", handleKeyDown)
    }
  }, [email, password])

  const handleSubmit = () => {
    const loadingToast = toast.loading("Please Wait...")
    const requestBody = {
      email: email,
      password: password,
    };
    axios
      .post(`${import.meta.env.VITE_API_BASE_URL}/auth`, requestBody)
      .then((res) => {
        const data = res.data.result;
        userLocalStorage.save(data);

        setTimeout(() => {
          if (data.role == "Admin") {
            navigate("/admin/tickets");
          } else if (data.role == "Client") {
            navigate("/client/tickets");
          }
        }, 2000)

        toast.success("Login Success", { id: loadingToast });
      })
      .catch((err) => {
        console.log(err);
        toast.error(`${err.response.data.message}`, { id: loadingToast });
      });
  };
  return (
    <>
      <Container className="h-[100vh] flex flex-col items-center justify-center">
        <div className="flex flex-col gap-12 w-[300px] sm:w-[300px] md:w-[500px] border border-1 shadow-xl rounded-2xl px-4 md:px-12 py-14 md:py-20">
          <Typography variant="p" className="font-500 text-primary-i text-2xl">
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

            <div className="flex justify-end mt-4">
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
