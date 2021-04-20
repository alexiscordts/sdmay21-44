import React, { useState } from "react";
import "./FormStyles.css";
import Nav from "./Nav";
import axios from "axios";

class AddLocation extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      address: "",
      name: "",
      phoneNumber: "",
      active: 1,
    };
    this.handleChange = this.handleChange.bind(this);
    this.handlePost = this.handlePost.bind(this);
  }

  handlePost(event) {
    event.preventDefault();
    const url = "http://10.29.163.20:8081/api/location/";
    axios
      .post(url, this.state)
      .then(function (response) {
        console.log(response);
      })
      .catch(function (error) {
        console.log(error);
      });
    setTimeout(function () {
      window.location.href = "/manage_locations";
    }, 2000);
  }

  handleChange(event) {
    this.setState({
      [event.target.name]: event.target.value,
    });
  }
  render() {
    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Add a Location </div>
            <form onSubmit={this.handlePost}>
              <label for="name">
                <span>
                  Name
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="name"
                  onChange={this.handleChange}
                />
              </label>
              <label for="name">
                <span>
                  Address
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="address"
                  onChange={this.handleChange}
                />
              </label>
              <label for="phoneNumber">
                <span>
                  Phone Number <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="phoneNumber"
                  onChange={this.handleChange}
                />
              </label>
              <div class="submitLabel">
                <input type="submit" value="Create" />
              </div>
            </form>
          </div>
        </div>
        {/* <button
        onClick={() => {
          console.log("submitted");
          const url = "http://10.29.163.20:8081/api/location/";
          axios
            .post(url, {
              locationID: "7",
              name: "test",
              appointment: [],
              patient: [],
              room: [],
            })
            .then(function (response) {
              console.log(response);
            })
            .catch(function (error) {
              console.log(error);
            });
        }}
      >
        Click
      </button> */}
      </div>
    );
  }
}

export default AddLocation;
