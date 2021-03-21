import React from "react";
import axios from "axios";
import "./Login.css";

class Login extends React.Component {
  constructor(props) {
    super(props);

    this.handleLogin = props.handleLogin;

    this.state = {
      username: "",
      password: "",
      loggedIn: false,
    };
    this.handleChange = this.handleChange.bind(this);
    this.handleLoginAttempt = this.handleLoginAttempt.bind(this);
  }

  handleLoginAttempt(event) {
    event.preventDefault();
    const { username, password } = this.state;
    const user = {
      username: username,
      password: password,
    };

    const url = "http://10.29.163.20:8081/api/user/getUserByUsername/";

    // Confirm with db that the user exists
    axios
      .get(url + user.username)
      .then((response) => {
        console.log(response.data);

        this.setState({ errors: "" });
        //For now we let anyone login
        this.props.handleLogin();
        this.loggedIn = true;
      })
      .catch((error) => {
        console.log("Error caught");
        console.log(error);
        this.setState({
          errors: "Error: username / password incorrect",
        });
      });

    // this.props.handleLogin();
    // this.loggedIn = true;
  }

  /**
   * This handles the input to username and password, showing what the user types on screen.
   * @param {*} event
   */
  handleChange(event) {
    this.setState({
      [event.target.name]: event.target.value,
    });
  }

  render() {
    return (
      <div id="parent">
        <img
          id="unityImg"
          src="https://www.unitypoint.org/images/unitypoint/UnityPointHealthLogo.svg"
        ></img>
        <h3>Inpatient Therapy Scheduling System</h3>
        <form
          id="loginform"
          type="loginform"
          onSubmit={this.handleLoginAttempt}
        >
          <input
            type="username"
            name="username"
            placeholder="Enter username"
            value={this.state.username}
            onChange={this.handleChange}
            required
          />
          <br></br>
          <input
            type="password"
            name="password"
            placeholder="Enter Password"
            value={this.state.password}
            onChange={this.handleChange}
            required
          />
          <br></br>
          <button id="LoginButton" type="submit">
            Login
          </button>
        </form>
      </div>
    );
  }
}

export default Login;
