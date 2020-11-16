import React from "react";
import ReactDOM from "react-dom";
import "./App.css";
import Login from "./Login";
import Dashboard from "./Dashboard";
import AddAdmin from "./AddAdmin"
import AddPatient from "./AddPatient";
import AddTherapist from "./AddTherapist";
import EditAdmin from "./EditAdmin"
import EditTherapist from "./EditTherapist";
import ViewAdmin from "./ViewAdmin"
import ViewPatient from "./ViewPatient";
import ViewTherapist from "./ViewTherapist";
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
               <Route path="/add_patient" component={AddPatient} />
               <Route path="/add_therapist" component={AddTherapist} />
               <Route path="/edit_admin" component={EditAdmin} />
               <Route path="/edit_therapist" component={EditTherapist} />
               <Route path="/view_admin" component={ViewAdmin} />
               <Route path="/view_patient" component={ViewPatient} />
               <Route path="/view_therapist" component={ViewTherapist} />
               <Route path="/" component={Login} handleLogin={this.handleLogin} />
            </Switch>
          </Router>
    );
  }
}

ReactDOM.render(<App />, document.getElementById("root"));

export default App;
