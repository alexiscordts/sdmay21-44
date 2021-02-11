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
import {BrowserRouter as Router, Switch, Route} from "react-router-dom";

class App extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      loggedIn: false,
    };

    this.handleLogin = this.handleLogin.bind(this);
  }

  handleLogin(data) {
    this.setState = {
      loggedIn: true,
    };
    return true;
  }

  render() {
    return (  
          <Router>
            <Switch>
               <Route path="/dashboard" component={Dashboard} />
               <Route path="/add_admin" component={AddAdmin} />
               <Route path="/add_nurse" component={AddNurse} />
               <Route path="/add_patient" component={AddPatient} />
               <Route path="/add_therapist" component={AddTherapist} />
               <Route path="/edit_admin" component={EditAdmin} />
               <Route path="/edit_nurse" component={EditNurse} />
               <Route path="/edit_therapist" component={EditTherapist} />
               <Route path="/view_admin" component={ViewAdmin} />
               <Route path="/view_nurse" component={ViewNurse} />
               <Route path="/view_patient" component={ViewPatient} />
               <Route path="/view_therapist" component={ViewTherapist} />
               <Route path="/settings" component={Settings} />
               <Route path="/manage_locations" component={ViewLocations} />
               <Route path="/add_location" component={AddLocation} />
               <Route path="/edit_location" component={EditLocation} />
               <Route path="/" component={Login} handleLogin={this.handleLogin} />
            </Switch>
          </Router>
    );
  }
}

ReactDOM.render(<App />, document.getElementById("root"));

export default App;
