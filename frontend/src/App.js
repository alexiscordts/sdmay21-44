import React from "react";
import ReactDOM from "react-dom";
import "./App.css";
import Login from "./Login";
import Dashboard from "./Dashboard";
import AddAdmin from "./UserPages/AddAdmin";
import AddNurse from "./UserPages/AddNurse";
import AddPatient from "./UserPages/AddPatient";
import AddTherapist from "./UserPages/AddTherapist";
import EditAdmin from "./UserPages/EditAdmin";
import EditNurse from "./UserPages/EditNurse";
import EditTherapist from "./UserPages/EditTherapist";
import ViewAdmin from "./UserPages/ViewAdmin";
import ViewNurse from "./UserPages/ViewNurse";
import ViewPatient from "./UserPages/ViewPatient";
import ViewTherapist from "./UserPages/ViewTherapist";
import Settings from "./Settings";
import AddLocation from "./AddLocation";
import ViewLocations from "./ViewLocations";
import EditLocation from "./EditLocation";
import AddRoom from "./Rooms/AddRoom";
import ViewRoom from "./Rooms/ViewRooms";
import ViewMetrics from "./ViewMetrics";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import AddTherapyTypes from "./TherapyTypes/AddTherapyTypes";
import EditTherapyTypes from "./TherapyTypes/EditTherapyTypes";
import ViewTherapyTypes from "./TherapyTypes/ViewTherapyTypes";
import ChangePassword from "./ChangePassword";
import Nav from './Nav.js'
import axios from "axios";
import EditPatient from "./UserPages/EditPatient";
import ViewPhysician from "./UserPages/ViewPhysician";
import AddPhysician from './UserPages/AddPhysician';
import EditPhysician from './UserPages/EditPhysician';

export default class App extends React.Component {
  constructor() {
    super();
    console.log("local storage: " + sessionStorage.getItem('loggedIn'));
      if (sessionStorage.getItem('loggedIn') && sessionStorage.getItem('role') != null && sessionStorage.getItem('username') != null)
      {
        this.state = {
          loggedIn: true,
        }
        this.role = sessionStorage.getItem('role');
        this.username = sessionStorage.getItem('username');
      }
     else
     {
        this.state = {
          loggedIn: false,
        }
      }
     //sessionStorage.setItem('loggedIn', true);
     //console.log("local storage: " + sessionStorage.getItem('loggedIn'));

    this.handleLogin = this.handleLogin.bind(this);
  }

  handleLogin(data, password) {
    this.username = data.username;
    data.password = password;
    axios //see if password matches
      .post(process.env.REACT_APP_SERVER_URL + "user/login/", data)
      .then((response) => {
        console.log("log in success!");
        this.setState({
          loggedIn: true,
        });
        sessionStorage.setItem('loggedIn', true);
        sessionStorage.setItem('username', this.username);
        sessionStorage.setItem('id', data.userId);
        sessionStorage.setItem('firstname', data.firstName);
        sessionStorage.setItem('lastname', data.lastName);
      })
      .catch((error) => {
        console.log("Error caught");
        console.log(error);
        this.setState({
          errors: "Error: username / password incorrect",
        });
      });
    console.log("Parent handled login");

    axios //get permission of user
      .get(process.env.REACT_APP_SERVER_URL + "permission/" + data.userId)
      .then((response) => {
        this.role = response.data.role;
        sessionStorage.setItem('role', response.data.role);
      })
      .catch((error) => {
        console.log(error);
      });

    console.log(this.state.loggedIn);
    return true;
  }

  render() {
    if (this.state.loggedIn)
    {
      if (this.role == "admin")
      return (
        <div>
        <div id="topNav">
          <div id="collapseMenuToggle" onClick={closeNav}>
            ☰
          </div>
          <img src="https://www.unitypoint.org/images/unitypoint/UnityPointHealthLogo.svg" />
          <div id="appName"> - &nbsp;Therapy Scheduler</div>
          <div id="signout" onClick={() => {this.setState({
          loggedIn: false,
           });}}>
            <img src={require("./Icons/icons8-exit-48.png")} />
          </div>
          <div id="userinfo">
            <div id="username">{this.username}</div>
            <div id="role">{this.role}</div>
          </div>
        </div>
        <Router>
          <Nav role={this.role}/>
          <Switch>
            <Route path="/dashboard" component={() => <Dashboard role={this.role} />} />
            <Route path="/add_admin" component={AddAdmin} />
            <Route path="/add_nurse" component={AddNurse} />
            <Route path="/add_patient" component={AddPatient} />
            <Route path="/add_therapist" component={AddTherapist} />
            <Route path="/add_therapy_types" component={AddTherapyTypes} />
            <Route path="/edit_admin" component={EditAdmin} />
            <Route path="/edit_nurse" component={EditNurse} />
            <Route path="/edit_therapist" component={EditTherapist} />
            <Route path="/edit_therapy_types" component={EditTherapyTypes} />
            <Route path="/view_admin" component={ViewAdmin} />
            <Route path="/view_nurse" component={ViewNurse} />
            <Route path="/view_patient" component={ViewPatient} />
            <Route path="/view_therapist" component={ViewTherapist} />
            <Route path="/view_therapy_types" component={ViewTherapyTypes} />
            <Route path="/settings" component={() => <Settings role={this.role} />} />
            <Route path="/manage_locations" component={ViewLocations} />
            <Route path="/add_location" component={AddLocation} />
            <Route path="/edit_location" component={EditLocation} />
            <Route path="/manage_rooms" component={ViewRoom} />
            <Route path="/add_room" component={AddRoom} />
            <Route path="/view_Metrics" component={ViewMetrics} />
            <Route path="/change_password" component={ChangePassword} />
            <Route path="/edit_patient" component={EditPatient} />
            <Route path="/view_physician" component={ViewPhysician} />
            <Route path="/add_physician" component={AddPhysician} />
            <Route path="/edit_physician" component={EditPhysician} />
            <Route exact path="/" component={() => <Dashboard role={this.role} />} />
          </Switch>
        </Router>
        </div>
      );
      else
      return (
        <div>
        <div id="topNav">
          <div id="collapseMenuToggle" onClick={closeNav}>
            ☰
          </div>
          <img src="https://www.unitypoint.org/images/unitypoint/UnityPointHealthLogo.svg" />
          <div id="appName"> - &nbsp;Therapy Scheduler</div>
          <div id="signout" onClick={() => {this.setState({
          loggedIn: false,
           });}}>
            <img src={require("./Icons/icons8-exit-48.png")} />
          </div>
          <div id="userinfo">
            <div id="username">{this.username}</div>
            <div id="role">{this.role}</div>
          </div>
        </div>
        <Router>
          <Nav role={this.role}/>
          <Switch>
            <Route path="/dashboard" component={() => <Dashboard role={this.role} />} />
            <Route path="/view_Metrics" component={ViewMetrics} />
            <Route path="/change_password" component={ChangePassword} />
            <Route exact path="/" component={() => <Dashboard role={this.role} />} />
            <Route path="/settings" component={() => <Settings role={this.role} />} />
          </Switch>
        </Router>
        </div>
      );
      }
      else
      return (
        <Login handleLogin={this.handleLogin} />
      );
      
  }
}

var closed = true;
function closeNav() {
  if (closed == false) {
    document.getElementById("sideNav").style.minWidth = "50px";
    document.getElementById("sideNav").style.width = "50px";
    const elements = document.getElementsByClassName("menuLabel");
    for (let i = 0; i < elements.length; i++)
    {
      elements[i].style.display = "none";
    }
  } else {
    document.getElementById("sideNav").style.minWidth = "230px";
    document.getElementById("sideNav").style.width = "18%";
    const elements = document.getElementsByClassName("menuLabel");
    for (let i = 0; i < elements.length; i++)
    {
      elements[i].style.display = "block";
    }
  }
  closed = closed == false ? true : false;
}

ReactDOM.render(<App />, document.getElementById("root"));