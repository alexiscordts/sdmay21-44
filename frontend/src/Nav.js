import React from "react";
import { Redirect } from "react-router-dom";
import "./Nav.css";

class Dashboard extends React.Component {
    
  constructor(props) {
    super(props);
    this.username = {value : 'Username123'};
    role = "Therapist";
    this.role = {value : role};
    this.menuItems = {values: loadMenuItems()};
  }

  render() {
    return (
    <div ref={this.myRef}>
        <div id="sideNav">
            <div id="topLeft">
            </div>
            {this.menuItems.values}
        </div>

        <div id="topNav">
            <div id="collapseMenuToggle" onClick={closeNav}>☰</div>
            <img src="https://www.unitypoint.org/images/unitypoint/UnityPointHealthLogo.svg" />
            <div id="signout">
                <img src={require("./Icons/icons8-exit-48.png")} />
            </div>
            <div id="userinfo">
                <div id="username">{this.username.value}</div>
                <div id="role">{this.role.value}</div>
            </div>           
        </div>

    </div>
    );
  }
}

var role;
var closed = true;
function closeNav() {
    if (closed == false)
    {
        document.getElementById('sideNav').style.minWidth = '50px';
        document.getElementById('sideNav').style.width = '50px';
    }
    else
    {
        document.getElementById('sideNav').style.minWidth = '230px';
        document.getElementById('sideNav').style.width = '18%';    
    }
    closed = (closed == false) ? true : false;
  }

  function loadMenuItems()
  {
    
    const items = [];
    items.push(
        <button class="menuButton">
            <img class="menuButtonImg" src={require("./Icons/icons8-calendar-48.png")} />
            <div class="menuLabel">View Schedule</div>
        </button>,
        <button class="menuButton">
            <img class="menuButtonImg" src={require("./Icons/icons8-clock-48.png")} />
            <div class="menuLabel">Appointments</div>
        </button>,
        <button class="menuButton">
            <img class="menuButtonImg" src={require("./Icons/icons8-female-user-48.png")} />
            <div class="menuLabel">Manage Patients</div>
        </button>,
        <button class="menuButton">
            <img class="menuButtonImg" src={require("./Icons/icons8-info-48.png")} />
            <div class="menuLabel">View Metrics</div>
        </button>
    );
    return items;
    
  }

export default Dashboard;
