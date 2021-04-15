import React from "react";
import Nav from "./Nav";
import "./Settings.css";
import { Link } from "react-router-dom";

const Settings = () => {
  return (
    <div>
      <Nav />
      <div class="settingsPageContainer">
      <Link to="/manage_locations"><button
          className="linkButton"
        >
          Manage Locations
        </button></Link>
        <Link to="/manage_rooms"><button
          className="linkButton"
        >
          Manage Rooms
        </button></Link>
        <Link to="/view_therapy_types"><button
          className="linkButton"
        >
          Manage Therapy Types
        </button></Link>
        <Link to="/change_password"><button
          className="linkButton"
        >
          Change Password
        </button></Link>
      </div>
    </div>
  );
};

export default Settings;
