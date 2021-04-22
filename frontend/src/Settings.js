import React from "react";
import Nav from "./Nav";
import "./Settings.css";
import { Link } from "react-router-dom";

class Settings extends React.Component {
  constructor(props) {
    super(props);
  }

getLinks()
{
  const items = [];
  if(this.props.role == "admin")
    items.push(
      <Link to="/manage_locations"><button
            className="linkButton"
          >
            Manage Locations
          </button></Link>,
          <Link to="/manage_rooms"><button
            className="linkButton"
          >
            Manage Rooms
          </button></Link>,
          <Link to="/view_therapy_types"><button
            className="linkButton"
          >
            Manage Therapy Types
          </button></Link>
    )
    return items;
}

render() {
    return (
      <div>
        <div class="settingsPageContainer">
          {this.getLinks()}
          <Link to="/change_password"><button
            className="linkButton"
          >
            Change Password
          </button></Link>
        </div>
      </div>
    );
  };
}

export default Settings;
