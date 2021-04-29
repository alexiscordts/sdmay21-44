import React from "react";
import Nav from "./Nav";
import "./Settings.css";

//This is a page with buttons linking to different settings pages
const Settings = () => {
  return (
    <div>
      <Nav />
      <div class="settingsPageContainer">
        <button
          className="linkButton"
          onClick={() => {
            window.location.href = "/manage_locations";
          }}
        >
          Manage Locations
        </button>
        <button
          className="linkButton"
          onClick={() => {
            window.location.href = "/manage_rooms";
          }}
        >
          Manage Rooms
        </button>
        <button
          className="linkButton"
          onClick={() => {
            window.location.href = "/view_therapy_types";
          }}
        >
          Manage Therapy Types
        </button>
        <button
          className="linkButton"
          onClick={() => {
            console.log('here');
            window.location.href = "/change_password";
          }}
        >
          Change Password
        </button>
      </div>
    </div>
  );
};

export default Settings;
