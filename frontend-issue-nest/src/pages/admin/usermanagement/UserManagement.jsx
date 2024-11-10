import * as React from "react";
import axios from "axios";

import { Typography } from "@mui/material";
import { DataTable, DefaultButton, UserModal } from "../../../components";
import { userLocalStorage } from "../../../helpers";

export default function UserManagement() {
  const [open, setOpen] = React.useState();
  const [ user, setUser ] = React.useState({
    name: null,
    email: null,
    role: null,
    password: null,
    is_active: false,
  });
  React.useEffect(() => {
    console.log(user)
  }, [user])
  const [ rows, setRows ] = React.useState([]);
  
  const getUserManagement = () => {
    const token = userLocalStorage.getItem("token");

    axios.get(`${import.meta.env.VITE_API_BASE_URL}/user`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      }
    )
    .then((res) => {
      const data = res.data.result.map((user) => ({
        ...user,
        action: <RenderActionItem data={user} />
      }));

      setRows(data);
    })
    .catch((err) => {
      console.log(err);
    })
  }

  const submitCreateUser = () => {
    console.log('aa')
    const token = userLocalStorage.getItem("token");
    const requestData = {
      name: user.name,
      email: user.email,
      role: user.role,
      password: user.password
    }

    axios.post(`${import.meta.env.VITE_API_BASE_URL}/user`, requestData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      }
    )
    .then((res) => {
      console.log(res);
      setOpen(false);
    })
    .catch((err) => {
      console.log(err);
    })
  }

  const handleUpdateUserStatus = (id, new_status) => {
    const token = userLocalStorage.getItem("token");
    const requestData = {
      is_active: !new_status,
    }
    axios.put(`${import.meta.env.VITE_API_BASE_URL}/user/${id}/status`, requestData,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        }
      }
    )
    .then((res) => {
      console.log(res);
      getUserManagement();
    })
    .catch((err) => {
      console.log(err);
    })
  }

  React.useEffect(() => {
    getUserManagement()
  }, [])

  const columns = [
    { id: "id", label: "User Id", minWidth: 100, align: "center"},
    { id: "name", label: "Fullname", minWidth: 170},
    { id: "email", label: "Email", minWidth: 170},
    { id: "role_name", label: "Role", minWidth: 100},
    { id: "is_active", label: "Active Status", minWidth: 100, align: "center", format: "is_active"},
    { id: "action", label: "Action"}
  ]

  const RenderActionItem = ({ data }) => {
    return (
      <>
        <div className="flex gap-2">
          <DefaultButton
            variant="contained"
            type="primary"
            bgColor= { data.is_active ? "#FF0000" : "#4CAF50" }
            custom={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: "10px"
            }}
            onclick={() => { handleUpdateUserStatus(data.id, data.is_active) }}
          >
            { data.is_active ? "Disable" : "Enable" } User
          </DefaultButton>
        </div>
      </>
    )
  }

  return (
    <>
      <UserModal
        open={open}
        setOpen={setOpen}
        user={user}
        setUser={setUser}
        handleCreateSubmision={submitCreateUser}
      />
      <div className="h-[100%]">
        <div className="flex justify-between mt-7 mb-4">
          <Typography variant="h5">User Management</Typography>
          <DefaultButton
            variant="contained"
            onclick={() => setOpen(true)}
            custom={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: "10px"
            }}
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6">
              <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
            </svg>
            Create
          </DefaultButton>
        </div>
        <DataTable columns={columns} rows={rows} />
      </div>
    </>
  );
}
