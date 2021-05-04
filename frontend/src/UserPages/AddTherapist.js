import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

class AddTherapist extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      therapist: {
        firstName: null,
        middleName: null,
        lastName: null,
        address: null,
        phoneNumber: null,
        username: null,
        password: null,
        color: null,
        active: 1,
      },
    };
    this.handleChange = this.handleChange.bind(this);
    // this.postPermission = this.postPermission.bind(this);
    this.submitTherapist = this.submitTherapist.bind(this);
  }
  handleChange(event) {
    this.setState({
      therapist: {
        ...this.state.therapist,
        [event.target.name]: event.target.value,
      },
    });
  }
  // postPermission(userId) {
  //   const permUrl = process.env.REACT_APP_SERVER_URL + "permission/";
  //   const role = "therapist";
  //   const permission = { userId, role };
  //   console.log(permission);

  submitTherapist(e) {
    //need to add the therapist to the user list
    e.preventDefault();
    console.log(this.state.therapist);
    // let therapist = this.state.therapist;
    const username = this.state.therapist.username;
    const getUserUrl =
      process.env.REACT_APP_SERVER_URL + "user/getUserByUsername/" + username;
    const url = process.env.REACT_APP_SERVER_URL + "user";
    console.log(this.state.therapist.username);
    axios.post(url, this.state.therapist).catch(function (error) {
      if (error.response) {
        console.log(error.response.data);
        console.log(error.response.status);
        console.log(error.response.headers);
      } else if (error.request) {
        // The request was made but no response was received
        console.log(error.request);
      } else {
        // Something happened in setting up the request that triggered an Error
        console.log("Error", error.message);
      }
    });

    setTimeout(function () {
      axios.get(getUserUrl).then((response) => {
        var userId = parseInt(response.data.userId);
        setTimeout(
          () => {
            const permUrl = process.env.REACT_APP_SERVER_URL + "permission/";
            const role = "therapist";
            const permission = { userId, role };
            axios.post(permUrl, permission).catch((error) => {
              console.log(error);
            });
          },
          2000,
          userId
        );
      });
    }, 2000);

    setTimeout(function () {
      window.location.href = "/view_therapist";
    }, 5000);
  }

  render() {
    //form for adding a therpaist

    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Add a Therapist </div>
            <form action="" method="post">
              <label for="fname">
                <span>
                  First Name
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="firstName"
                  value={this.state.therapist.firstName}
                />
              </label>
              <label for="middleName">
                <span>Middle Name</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="middleName"
                  value={this.state.therapist.middleName}
                />
              </label>
              <label for="lastName">
                <span>
                  Last Name
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="lastName"
                  value={this.state.therapist.lastName}
                />
              </label>
              <label for="address">
                <span>
                  Address
                  {/* <span class="required">*</span> */}
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="address"
                  value={this.state.therapist.address}
                />
              </label>
              <label for="phoneNumber">
                <span>
                  Phone Number
                  {/* <span class="required">*</span> */}
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="phoneNumber"
                  value={this.state.therapist.phoneNumber}
                />
              </label>
              <label for="email">
                <span>
                  Username
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="username"
                  value={this.state.therapist.username}
                />
              </label>
              <label for="password">
                <span>
                  Password
                  <span class="required">*</span>
                </span>
                <input
                  type="password"
                  class="input-field"
                  onChange={this.handleChange}
                  name="password"
                  value={this.state.therapist.password}
                />
              </label>
              <label for="color">
                <span>Color</span>
                <span class="required">*</span>
                <input
                  type="color"
                  name="color"
                  onChange={this.handleChange}
                ></input>
              </label>
              <p class="submitLabel">
                By clicking the button, an email will be sent to the Therapist
                with their login information.
              </p>
              <div class="submitLabel">
                <input
                  type="button"
                  value="Create"
                  onClick={this.submitTherapist}
                />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  }
}
export default AddTherapist;
