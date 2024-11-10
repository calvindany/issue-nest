import * as React from "react";
import axios from "axios";
import moment from "moment";
import toast from "react-hot-toast";

import { Container, Typography } from "@mui/material";

import { DataTable, DefaultButton, TicketModal } from "../../../components";

import { userLocalStorage } from "../../../helpers";
import * as constants from "../../../constants/global";

export default function Tickets() {
  const [rows, setRows] = React.useState([]);
  const [open, setOpen] = React.useState(false);
  const [modalType, setModalType] = React.useState("add");

  const [isClient, _] = React.useState(
    userLocalStorage.getItem("role") == "Client"
  );
  const [ticket, setTicket] = React.useState({
    id: null,
    title: null,
    description: null,
    status: null,
    response: null,
  })

  React.useEffect(() => {
    console.log(ticket)
  }, [ticket])

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
        </div>
      </>
    );
  };

  const handleOpenModal = (type, data) => {
    if (type != "create") {
      setTicket(data)
    } else {
      data = {
        id: null,
        title: null,
        description: null,
        status: constants.TICKET_OPEN_STATUS.id,
        response: null,
      }

      setTicket(data)
    }

    setModalType(type);

    setOpen(true);
  };

  const handleSubmitCreateModal = () => {
    const loadingToast = toast.loading("Please wait...")
    const token = userLocalStorage.getItem("token");

    const requestData = {
      title: ticket.title,
      description: ticket.description,
      status: ticket.id ? parseInt(ticket.id) : 1,
    };

    axios
      .post(`${import.meta.env.VITE_API_BASE_URL}/ticket`, requestData, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((res) => {
        console.log(res.data.result);
        getTicketsData();
        setOpen(false);

        toast.success(`Success create new ticket with id: ${res.data.result.id}`, {
          id: loadingToast
        })
      })
      .catch((err) => {
        console.log(err);
        toast.error(`Error when create ticket`, {
          id: loadingToast
        })
      });
  };

  const handleSubmitEditModal = (id) => {
    const token = userLocalStorage.getItem("token");

    const requestData = {
      title: ticket.title,
      description: ticket.description,
      status: ticket.status,
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

  React.useEffect(() => {
    getTicketsData();
  }, []);

  const columns = [
    { id: "id", label: "Ticket Id", minWidth: 100, align: "center" },
    { id: "title", label: "Title", minWidth: 170 },
    {
      id: "status_name",
      label: "Status",
      minWidth: 120,
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
        isAdmin={!isClient}
        isClient={isClient}
        handleSubmitEditModal={handleSubmitEditModal}
        handleSubmitCreateModal={handleSubmitCreateModal}
      />
      <div className="h-[100%]">
        <Typography variant="h5">Tickets</Typography>

        <div className="flex justify-end mt-7 mb-4">
          <DefaultButton
            variant="contained"
            onclick={() => handleOpenModal("create", 0)}
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
