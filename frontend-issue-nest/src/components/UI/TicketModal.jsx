import * as React from "react";
import { Box, Typography, Modal, TextField, MenuItem } from "@mui/material";
import DefaultButton from "./DefaultButton";

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

export default function TicketModal({
  open,
  setOpen,
  modalType,
  id,
  ticketName,
  setTicketName,
  ticketDescription,
  setTicketDescription,
  ticketStatus,
  setTicketStatus,
  adminResponse,
  setAdminResponse,
  isAdmin,
  handleSubmitEditModal,
}) {
  const handleClose = () => setOpen(false);

  const status = [
    {
      value: "Open",
      label: "Open",
    },
    {
      value: "In Progress",
      label: "In Progress",
    },
    {
      value: "Resolved",
      label: "Resolved",
    },
  ];

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
            {modalType == "details" ? "Details" : "Edit"} Ticket
          </Typography>

          <div className="flex flex-col gap-5">
            <TextField
              required
              id="Ticket Name"
              label="Ticket Name"
              disabled={isAdmin || modalType == "details"}
              value={ticketName}
              onChange={(e) => setTicketName(e.target.value)}
            />
            <TextField
              required
              id="description"
              multiline
              disabled={isAdmin || modalType == "details"}
              label="Ticket Description"
              value={ticketDescription}
              onChange={(e) => setTicketDescription(e.target.value)}
            />
            <TextField
              id="outlined-select-status"
              select
              disabled={isAdmin || modalType == "details"}
              label="Status"
              defaultValue={ticketStatus}
            >
              {status.map((option) => (
                <MenuItem
                  key={option.value}
                  value={option.value}
                  defaultValue={status}
                  onChange={() => setTicketStatus(option.value)}
                >
                  {option.label}
                </MenuItem>
              ))}
            </TextField>
            <TextField
              required
              id="admin-response"
              multiline
              label="Admin Response"
              disabled={modalType == "details"}
              value={adminResponse}
              onChange={(e) => setAdminResponse(e.target.value)}
            />
            {modalType != "details" ? (
              <div className="flex justify-end">
                <DefaultButton
                  variant="contained"
                  type="primary"
                  onclick={() => {
                    handleSubmitEditModal(id);
                    handleClose();
                  }}
                >
                  Submit
                </DefaultButton>
              </div>
            ) : (
              <></>
            )}
          </div>
        </Box>
      </Modal>
    </div>
  );
}
