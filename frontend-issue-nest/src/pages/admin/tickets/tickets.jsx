import * as React from "react";
import axios from "axios";
import moment from "moment";
import { Typography } from "@mui/material";

import { DataTable, DefaultButton, TicketModal } from "../../../components";

import { userLocalStorage } from "../../../helpers";
import toast from "react-hot-toast";

export default function Tickets() {
  const [rows, setRows] = React.useState([]);
  const [open, setOpen] = React.useState(false);
  
  const isAdmin = userLocalStorage.getItem("role") == "Admin";
  
  const [modalType, setModalType] = React.useState("add");
  const [ticket, setTicket] = React.useState({
    id: null,
    title: null,
    description: null,
    status: null,
    response: null,
  });
  // const [id, setId] = React.useState("");
  // const [ticketName, setTicketName] = React.useState("");
  // const [ticketDescription, setTicketDescription] = React.useState("");
  // const [ticketStatus, setTicketStatus] = React.useState("");
  // const [adminResponse, setAdminResponse] = React.useState("");

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
          created_at: moment(ticket.created_at).format("HH-mm-YYYY"),
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
            custom={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: "10px"
            }}
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6">
              <path strokeLinecap="round" strokeLinejoin="round" d="M19.5 14.25v-2.625a3.375 3.375 0 0 0-3.375-3.375h-1.5A1.125 1.125 0 0 1 13.5 7.125v-1.5a3.375 3.375 0 0 0-3.375-3.375H8.25m5.231 13.481L15 17.25m-4.5-15H5.625c-.621 0-1.125.504-1.125 1.125v16.5c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125V11.25a9 9 0 0 0-9-9Zm3.75 11.625a2.625 2.625 0 1 1-5.25 0 2.625 2.625 0 0 1 5.25 0Z" />
            </svg>
            Details
          </DefaultButton>
          <DefaultButton
            variant="contained"
            bgColor=""
            onclick={() => handleOpenModal("edit", data)}
            custom={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: "10px"
            }}
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6">
              <path strokeLinecap="round" strokeLinejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10" />
            </svg>
            Edit
          </DefaultButton>
          <DefaultButton
            variant="contained"
            type="primary"
            bgColor="#FF0000"
            onclick={() => handleSubmitDeleteModal(data.id)}
            custom={{
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              gap: "10px"
            }}
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6">
              <path strokeLinecap="round" strokeLinejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
            </svg>
            Delete
          </DefaultButton>
        </div>
      </>
    );
  };

  const handleOpenModal = (type, data) => {
    const ticket = data;

    // setId(data.id);
    // setTicketName(data.title);
    // setTicketDescription(data.description);
    // setTicketStatus(data.status);
    // setAdminResponse(data.admin_response);

    setTicket(ticket);
    setModalType(type);
    setOpen(!open);
  };

  const handleSubmitEditModal = (id) => {
    const loadingToast = toast.loading("Submiting data...");
    const token = userLocalStorage.getItem("token");

    const requestData = {
      status: parseInt(ticket.status),
      admin_response: ticket.response,
    };
    axios
      .put(
        `${import.meta.env.VITE_API_BASE_URL}/ticket/${id}/response`,
        requestData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      )
      .then((res) => {
        console.log(res.data.result);
        getTicketsData();
        setOpen(false);

        toast.success(`Success update ticket with id: ${id}`, {
          id: loadingToast
        })
      })
      .catch((err) => {
        toast.error(`Error white updating ticket`, {
          id: loadingToast
        })
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
    { id: "id", label: "Ticket Id", minWidth: 100, align: "center" },
    { id: "title", label: "Title", minWidth: 170 },
    // {
    //   id: "description",
    //   label: "Description",
    //   minWidth: 170,
    // },
    {
      id: "status_name",
      label: "Status",
      minWidth: 120,
      align: "center"
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
        ticket={ticket}
        setTicket={setTicket}
        // id={id}
        // ticketName={ticketName}
        // setTicketName={setTicketName}
        // ticketDescription={ticketDescription}
        // setTicketDescription={setTicketDescription}
        // ticketStatus={ticketStatus}
        // setTicketStatus={setTicketStatus}
        // adminResponse={adminResponse}
        // setAdminResponse={setAdminResponse}
        isAdmin={isAdmin}
        handleSubmitEditModal={handleSubmitEditModal}
      />
      <div className="h-[100%]">
        <div className="mt-7 mb-4">
          <Typography variant="h5">Tickets</Typography>
        </div>
        <DataTable columns={columns} rows={rows} />
      </div>
    </>
  );
}
