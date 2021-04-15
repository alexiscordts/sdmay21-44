import React from "react";
import { Link } from "react-router-dom";
import "./Nav.css";

const userRole = {
  therapist: 0,
  nurse: 1,
  admin: 2
};

class Nav extends React.Component {
  constructor(props) {
    super(props);
    this.username = { value: "Username123" };
    console.log("role: " + this.props.role);
    switch(this.props.role) {
      case 0:
        this.role = "Therapist";
        break;
        case 1:
          this.role = "Nurse";
        break;
        case 2:
          this.role = "Admin";
          break;
          default:
          this.role = "none";
    }
    this.menuItems = { values: loadMenuItems() };
  }

  render() {
    return (
      <div ref={this.myRef}>
        <div id="sideNav">
          <div id="topLeft"></div>
          {this.menuItems.values}
        </div>

        <div id="topNav">
          <div id="collapseMenuToggle" onClick={closeNav}>
            ☰
          </div>
          <img src="https://www.unitypoint.org/images/unitypoint/UnityPointHealthLogo.svg" />
          <div id="appName"> - &nbsp;Therapy Scheduler</div>
          <div id="signout">
            <img src={require("./Icons/icons8-exit-48.png")} />
          </div>
          <div id="userinfo">
            <div id="username">{this.username.value}</div>
            <div id="role">{this.role}</div>
          </div>
        </div>
      </div>
    );
  }
}

var role;
var closed = true;
function closeNav() {
  if (closed == false) {
    document.getElementById("sideNav").style.minWidth = "50px";
    document.getElementById("sideNav").style.width = "50px";
  } else {
    document.getElementById("sideNav").style.minWidth = "230px";
    document.getElementById("sideNav").style.width = "18%";
  }
  closed = closed == false ? true : false;
}

function loadMenuItems() {
  const items = [];
  items.push(
    <Link to="/dashboard"><button
      class="menuButton"
    >
      <img
        class="menuButtonImg"
        src={require("./Icons/icons8-calendar-48.png")}
      />
      <div class="menuLabel">View Schedule</div>
    </button></Link>,

<Link to="/view_metrics"><button class="menuButton"
    >
      <img
        class="menuButtonImg"
        src={require("./Icons/icons8-combo-chart-50.png")}
      />
      <div class="menuLabel">View Metrics</div>
    </button></Link>,

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
    </button></Link>,

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

export default Nav;
