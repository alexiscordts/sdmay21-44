import React from "react";
import Nav from "./Nav";
import "./Settings.css";

const Settings = () => {
  return (
    <div>
      <Nav />
      <div class="pageContainer">
        <button
          class="linkButton"
          onClick={() => {
            window.location.href = "/manage_locations";
          }}
        >
          Manage Locations
        </button>
        <button
          class="linkButton"
          onClick={() => {
            window.location.href = "/manage_rooms";
          }}
        >
          Manage Rooms
        </button>
        <button
          class="linkButton"
          onClick={() => {
            window.location.href = "/view_therapy_types";
          }}
        >
          Manage Therapy Types
        </button>
      </div>
    </div>
  );
};

export default Settings;
