import React from "react";
import ReactDOM from "react-dom";
import "./App.css";
import Login from "./Login";
import Dashboard from "./Dashboard";
import AddTherapist from "./AddTherapist"
import EditTherapist from "./EditTherapist"
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
               <Route path="/add_therapist" component={AddTherapist} />
               <Route path="/edit_therapist" component={EditTherapist} />
               <Route path="/" component={Login} handleLogin={this.handleLogin} />
            </Switch>
          </Router>
    );
  }
}

ReactDOM.render(<App />, document.getElementById("root"));

export default App;
