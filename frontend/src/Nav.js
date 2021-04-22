import React from "react";
import { Link } from "react-router-dom";
import "./Nav.css";

class Nav extends React.Component {
  constructor(props) {
    super(props);
    this.username = { value: "Username123" };
    console.log("role: " + this.props.role);
    this.menuItems = { values: this.loadMenuItems(this.props.role) };
  }

  loadMenuItems(role) {
    const items = [];
    items.push(
      <button
        class="menuButton"
        onClick={() => {
          window.location.href = "/dashboard";
        }}
      >
        <img
          class="menuButtonImg"
          src={require("./Icons/icons8-calendar-48.png")}
        />
        <div class="menuLabel">View Schedule</div>
      </button>);
  
  if (role == "therapist" || role == "admin")
  items.push(
  <button class="menuButton"
      onClick={() => {
        window.location.href = "/view_metrics";
      }}
      >
        <img
          class="menuButtonImg"
          src={require("./Icons/icons8-combo-chart-50.png")}
        />
        <div class="menuLabel">View Metrics</div>
      </button>);
  
  if (role == "admin")
  items.push(
  <Link to="/view_patient"><button
        class="menuButton"
      >
        <img
          class="menuButtonImg"
          src={require("./Icons/icons8-wheelchair-50.png")}
        />
        <div class="menuLabel">Manage Patients</div>
      </button></Link>,
  
  <Link to="/view_therapist"><button
        class="menuButton"
      >
        <img
          class="menuButtonImg"
          src={require("./Icons/icons8-user-male-50.png")}
        />
        <div class="menuLabel">Manage Therapists</div>
      </button></Link>,
  
  <Link to="/view_nurse"><button
        class="menuButton"
      >
        <img
          class="menuButtonImg"
          src={require("./Icons/icons8-stethoscope-50.png")}
        />
        <div class="menuLabel">Manage Nurses</div>
      </button></Link>,
  
  <Link to="/view_admin"><button
        class="menuButton"
      >
        <img
          class="menuButtonImg"
          src={require("./Icons/icons8-microsoft-admin-50.png")}
        />
        <div class="menuLabel">Manage Admins</div>
      </button></Link>);
  
  items.push(
  <Link to="/settings"><button
        class="menuButton"
      >
        <img
          class="menuButtonImg"
          src={require("./Icons/icons8-settings-48.png")}
        />
        <div class="menuLabel">Settings</div>
      </button></Link>
    );
    return items;
  }

  render() {
    return (
      <div ref={this.myRef}>
        <div id="sideNav">
          <div id="topLeft"></div>
          {this.menuItems.values}
        </div>    
      </div>
    );
  }
}

export default Nav;
