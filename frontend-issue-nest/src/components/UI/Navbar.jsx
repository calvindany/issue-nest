import { useNavigate } from "react-router-dom";
import { Container, Button } from "@mui/material";

import { userLocalStorage } from "../../helpers";

export default function Navbar() {
  const navigate = useNavigate();

  const isAdmin = userLocalStorage.getItem("role_name") == "Admin";
  const handleLogout = () => {
    userLocalStorage.remove();
    navigate("/");
  };
  return (
    <>
      <nav className=" py-5 px-4 bg-primary-i">
        <Container className="flex justify-end">
          <div className="flex items-center gap-7 text-white">
            <a href={`/${isAdmin ? "admin" : "client"}/tickets`}>Tickets</a>
            {isAdmin ? (
              <a href={`/${isAdmin ? "admin" : "client"}/user-management`}>
                User Management
              </a>
            ) : (
              <></>
            )}
            <Button
              onClick={handleLogout}
              sx={{
                textTransform: "none",
                backgroundColor: "red",
                color: "white",
              }}
            >
              Logout
            </Button>
          </div>
        </Container>
      </nav>
    </>
  );
}
