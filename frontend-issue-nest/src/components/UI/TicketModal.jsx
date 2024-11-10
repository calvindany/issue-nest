import * as React from "react";
import { Box, Typography, Modal, TextField, MenuItem } from "@mui/material";
import DefaultButton from "./DefaultButton";
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

export default function TicketModal({
  open,
  setOpen,
  modalType,
  ticket,
  setTicket,
  isAdmin,
  isClient,
  handleSubmitEditModal,
  handleSubmitCreateModal,
}) {
  const handleClose = () => setOpen(false);

  const optionStatus = [
    {
      value: constants.TICKET_OPEN_STATUS.id,
      label: constants.TICKET_OPEN_STATUS.label,
    },
    {
      value: constants.TICKET_IN_PROGRES_STATUS.id,
      label: constants.TICKET_IN_PROGRES_STATUS.label,
    },
    {
      value: constants.TICKET_RESOLVED_STATUS.id,
      label: constants.TICKET_RESOLVED_STATUS.label,
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
              value={ticket.title}
              onChange={(e) => setTicket({...ticket, title: e.target.value})}
            />
            <TextField
              required
              id="description"
              multiline
              disabled={isAdmin || modalType == "details"}
              label="Ticket Description"
              value={ticket.description}
              onChange={(e) => setTicket({ ...ticket, description: e.target.value })}
            />
            <TextField
              id="outlined-select-status"
              select
              disabled={
                modalType == "details" ? true : !isAdmin
              }
              label="Status"
              value={ticket.status}
              onChange={(event) => setTicket({ ...ticket, status: event.target.value })}
            >
              {optionStatus.map((option) => (
                <MenuItem
                  key={option.value}
                  value={option.value}
                  defaultValue={ticket.status}
                >
                  {option.label}
                </MenuItem>
              ))}
            </TextField>
            {isAdmin ? (
              <TextField
                required
                id="admin-response"
                multiline
                label="Admin Response"
                disabled={modalType == "details"}
                value={ticket.admin_response ? ticket.admin_response : modalType == "details" ? "No Response Added Yet" : ""}
                onChange={(e) => setTicket({ ...ticket, admin_response: e.target.value })}
              />
            ) : (
              <></>
            )}
            {modalType != "details" ? (
              <div className="flex justify-end">
                <DefaultButton
                  variant="contained"
                  type="primary"
                  onclick={() => {
                    if (modalType == "edit") {
                      handleSubmitEditModal(ticket.id);
                    } else {
                      handleSubmitCreateModal(ticket.id);
                    }
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
