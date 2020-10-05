import React from "react";
import ReactDOM from "react-dom";
import "./App.css";
import Login from "./Login";

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
      <div className="App">
        <header className="App-header">
          <Login handleLogin={this.handleLogin} />
        </header>
      </div>
    );
  }
}

ReactDOM.render(<App />, document.getElementById("root"));

export default App;
