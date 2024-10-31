import * as React from "react";
import axios from "axios";
import { Container, Typography } from "@mui/material";

import { DataTable, DefaultButton, TicketModal } from "../../../components";

import { userLocalStorage } from "../../../helpers";

export default function Tickets() {
  const [rows, setRows] = React.useState([]);
  const [open, setOpen] = React.useState(false);
  const [modalType, setModalType] = React.useState("add");

  const [isClient, _] = React.useState(
    userLocalStorage.getItem("role") == "Client"
  );
  const [id, setId] = React.useState("");
  const [ticketName, setTicketName] = React.useState("");
  const [ticketDescription, setTicketDescription] = React.useState("");
  const [ticketStatus, setTicketStatus] = React.useState("");
  const [adminResponse, setAdminResponse] = React.useState("");

  const getTicketsData = () => {
    const token = userLocalStorage.getItem("token");
    axios
      .get(`${import.meta.env.VITE_API_BASE_URL}/ticket`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => {
        console.log(res.data.result);

        const data = res.data.result.map((ticket) => ({
          ...ticket,
          action: <RenderActionButton data={ticket} />,
        }));

        setRows(data);
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const RenderActionButton = ({ data }) => {
    return (
      <>
        <div className="flex gap-2">
          <DefaultButton
            variant="contained"
            bgColor=""
            onclick={() => handleOpenModal("details", data)}
          >
            Details
          </DefaultButton>
          <DefaultButton
            variant="contained"
            bgColor=""
            onclick={() => handleOpenModal("edit", data)}
          >
            Edit
          </DefaultButton>
        </div>
      </>
    );
  };

  const handleOpenModal = (type, data) => {
    setId(data.id);
    setTicketName(data.title);
    setTicketDescription(data.description);
    setTicketStatus(data.status);
    setAdminResponse(data.admin_response);

    setModalType(type);

    setOpen(true);
  };

  const handleSubmitEditModal = (id) => {
    const token = userLocalStorage.getItem("token");

    const requestData = {
      title: ticketName,
      description: ticketDescription,
      status: ticketStatus,
    };

    axios
      .put(`${import.meta.env.VITE_API_BASE_URL}/ticket/${id}`, requestData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => {
        console.log(res.data.result);
        getTicketsData();
        setOpen(false);
      })
      .catch((err) => {
        console.log(err);
      });
  };

  const handleSubmitDeleteModal = (id) => {
    const token = userLocalStorage.getItem("token");

    axios
      .delete(`${import.meta.env.VITE_API_BASE_URL}/ticket/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => {
        console.log(res.data);
        getTicketsData();
      })
      .catch((err) => {
        console.log(err);
      });
  };

  React.useEffect(() => {
    getTicketsData();
  }, []);

  const columns = [
    { id: "id", label: "Ticket Id", minWidth: 100 },
    { id: "title", label: "Title", minWidth: 170 },
    // {
    //   id: "description",
    //   label: "Description",
    //   minWidth: 170,
    // },
    {
      id: "status",
      label: "Status",
      minWidth: 170,
    },
    {
      id: "client_name",
      label: "Client Name",
      minWidth: 170,
    },
    {
      id: "created_at",
      label: "Issue Date",
      minWidth: 170,
    },
    {
      id: "action",
      label: "Action",
      minWidth: 170,
    },
  ];

  return (
    <>
      <TicketModal
        open={open}
        setOpen={setOpen}
        modalType={modalType}
        id={id}
        ticketName={ticketName}
        setTicketName={setTicketName}
        ticketDescription={ticketDescription}
        setTicketDescription={setTicketDescription}
        ticketStatus={ticketStatus}
        setTicketStatus={setTicketStatus}
        adminResponse={adminResponse}
        setAdminResponse={setAdminResponse}
        isAdmin={!isClient}
        handleSubmitEditModal={handleSubmitEditModal}
      />
      <Container className="h-[100vh]">
        <Typography variant="h5">Tickets</Typography>
        <DataTable columns={columns} rows={rows} />
      </Container>
    </>
  );
}
