import * as React from "react";
import { Box, Typography, Modal, TextField, MenuItem } from "@mui/material";
import DefaultButton from "./DefaultButton";
import PasswordField from "./PasswordField";
import * as constants from "../../constants/global";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  borderRadius: "10px",
  boxShadow: 24,
  p: 4,
};

export default function UserModal({
  open,
  setOpen,
  user,
  setUser,
  handleCreateSubmision
}) {
  const handleClose = () => setOpen(false);

  const optionRole = [
    {
      value: constants.USER_ADMIN_ROLE.id,
      label: constants.USER_ADMIN_ROLE.label
    },
    {
      value: constants.USER_CLIENT_ROLE.id,
      label: constants.USER_CLIENT_ROLE.label,
    }
  ]

  return (
    <div>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style} className="flex flex-col gap-8">
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Create User
          </Typography>

          <div className="flex flex-col gap-5">
            <TextField
              required
              id="name"
              label="User Fullname"
              value={ user.name }
              onChange={(e) => { setUser({ ...user, name: e.target.value })}}
            />
            <TextField
              required
              id="email"
              label="Email"
              value={ user.email }
              onChange={(e) => { setUser({ ...user, email: e.target.value })}}
            />
            <PasswordField 
              value={ user.password } 
              onChange={ (value) => { setUser({ ...user, password: value }) }} 
            />
            {/* <TextField
              required
              type="password"
              id="password"
              label="Password"
              value={ user.email }
              onChange={(e) => { setUser({ ...user, password: e.target.value })}}
            /> */}
            <TextField
              select
              required
              id="role"
              label="Role"
              value={ user.role }
              onChange={(e) => { setUser({ ...user, role: e.target.value })}}
            >
              {optionRole.map((option) => (
                <MenuItem
                  key={option.value}
                  value={option.value}
                  defaultValue={user.role}
                >
                  {option.label}
                </MenuItem>
              ))}
            </TextField>
            <div className="flex justify-end">
              <DefaultButton
                variant="contained"
                type="primary"
                onclick={handleCreateSubmision}
              >
                Submit
              </DefaultButton>
            </div>
          </div>
        </Box>
      </Modal>
    </div>
  );
}
