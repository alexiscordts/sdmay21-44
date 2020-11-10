import React from "react";
import "./Login.css";

class Login extends React.Component {
  constructor(props) {
    super(props);

    this.handleLoginAttempt = props.handleLoginAttempt;
    this.state = {
      email: "",
      password: "",
      loggedIn: false,
    };
    this.handleChange = this.handleChange.bind(this);
  }

  handleLoginAttempt(event) {
    event.preventDefault();
    const { email, password } = this.state;
    const user = {
      emai: email,
      password: password,
    };

    //Confirm with db that the user exists

    //For now we let anyone login
    this.props.handleLoginAttempt();
    this.loggedIn = true;
  }

  /**
   * This handles the input to email and password, showing what the user types on screen.
   * @param {*} event
   */
  handleChange(event) {
    this.setState({
      [event.target.name]: event.target.value,
    });
  }

  render() {
    return (
      <div className="Login">
        <header className="Login-UI">
          <img src="https://www.unitypoint.org/images/unitypoint/UnityPointHealthLogo.svg"></img>
          <form type="loginform" onSubmit={this.handleLoginAttempt}>
           <div>
           
            <input
              type="email"
              name="email"
              placeholder="Enter Email"
              value={this.state.email}
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

            <button className="LoginButton" type="submit">
              Login
            </button>

            </div>
          </form>
        </header>
      </div>
    );
  }
}

export default Login;
