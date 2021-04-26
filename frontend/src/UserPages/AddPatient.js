import React, { useState } from "react";
import { Component } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";
import { render } from "@testing-library/react";
class AddPatient extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      roomList: [],
      userList: [],
      therapistList: [],
      locationList: [],
      firstName: null,
      middleName: null,
      lastName: null,
      address: null,
      phoneNumber: null,
      locationId: null,
      roomNumber: null,
      pmrPhysicianId: null,
      therapistId: null,
      rooms: [],
    };
    this.handleChange = this.handleChange.bind(this);
    this.submitPatient = this.submitPatient.bind(this);
    this.updateRoom = this.updateRoom.bind(this);
  }

  componentDidMount() {
    const url = "http://10.29.163.20:8081/api/";
    axios.get(url + "user").then((response) => {
      const userList = response.data;
      this.setState({ userList });
    });

    axios.get("http://10.29.163.20:8081/api/permission").then((response) => {
      this.setState({
        therapistList: this.state.therapistList.concat(response.data),
      });
    });

    axios.get("http://10.29.163.20:8081/api/location").then((response) => {
      this.setState({
        locationList: this.state.locationList.concat(response.data),
      });
    });

    axios.get("http://10.29.163.20:8081/api/room").then((response) => {
      this.setState({ roomList: this.state.roomList.concat(response.data) });
    });
  }

  submitPatient(e) {
    const url = "http://10.29.163.20:8081/api/patient";
    e.preventDefault();
    const active = 1;
    const roomNumber = 401;
    const {
      firstName,
      middleName,
      lastName,
      address,
      phoneNumber,
      locationId,
      pmrPhysicianId,
      therapistId,
    } = this.state;
    const startDate = new Date();
    const patient = {
      firstName,
      middleName,
      lastName,
      address,
      phoneNumber,
      locationId,
      startDate,
      pmrPhysicianId,
      therapistId,
      active,
      roomNumber,
    };
    console.log(patient);
    axios.post(url, patient).catch((error) => {
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
      alert("There was a problem creating the patient");
    });
    setTimeout(() => {
      window.location.href = "/view_patient";
    }, 2000);
  }

  handleChange(event) {
    this.setState({
      [event.target.name]: event.target.value,
    });
  }

  updateRoom(event) {
    this.setState({ [event.target.name]: event.target.value });
    var id = event.target.value;
    var rooms = [];
    rooms.push(
      <option hidden disabled selected value>
        Select a Room
      </option>
    );
    this.state.roomList.forEach(function (room) {
      if (id == room.locationId) {
        rooms.push(<option value={room.number}>{room.number}</option>);
      }
    });
    this.setState({ rooms });
  }

  render() {
    var therapists = [];
    therapists.push(
      <option hidden disabled selected value>
        Select a Therapist
      </option>
    );
    this.state.therapistList.forEach(
      function (permission) {
        if (permission.role === "therapist") {
          this.state.userList.forEach(function (therapist) {
            if (therapist.userId === permission.userId) {
              therapists.push(
                <option value={therapist.userId}>
                  {therapist.firstName} {therapist.lastName}
                </option>
              );
            }
          });
        }
      }.bind(this)
    );
    var locations = [];
    locations.push(
      <option hidden disabled selected value>
        Select a Location
      </option>
    );
    this.state.locationList.forEach(
      function (location) {
        locations.push(
          <option value={location.locationId}>{location.name}</option>
        );
      }.bind(this)
    );

    var users = [];
    this.state.userList.forEach(function (user) {
      users.push(
        <option value={user.userId}>
          {user.firstName} {user.lastName}
        </option>
      );
    });

    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Add a Patient </div>
            <form action="" method="post">
              <label for="firstName">
                <span>
                  First Name
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="firstName"
                  value={this.state.firstName}
                />
              </label>
              <label for="middleName">
                <span>Middle Name</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="middleName"
                  value={this.state.middleName}
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
                  value={this.state.lastName}
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
                  value={this.state.address}
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
                  value={this.state.phoneNumber}
                />
              </label>
              <label for="location">
                <span>
                  Location <span class="required">*</span>
                </span>
                <select
                  type="number"
                  class="input-field"
                  onChange={this.updateRoom}
                  name="locationId"
                  value={this.state.locationId}
                >
                  {locations}
                </select>
              </label>
              <label for="roomNumber">
                <span>
                  Room<span class="required">*</span>
                </span>
                <select
                  type="number"
                  class="input-field"
                  onChange={this.handleChange}
                  name="roomNumber"
                  value={this.state.roomNumber}
                >
                  {this.state.rooms}
                </select>
              </label>
              <label for="pmrPhysician">
                <span>
                  Primary Physician
                  <span class="required">*</span>
                </span>
                <select
                  type="number"
                  class="input-field"
                  onChange={this.handleChange}
                  name="pmrPhysicianId"
                  value={this.state.pmrPhysicianId}
                >
                  {users}
                </select>
              </label>
              <br></br>
              <label for="therapist">
                <span>
                  Therapist <span class="required">*</span>
                </span>
                <select
                  type="number"
                  class="input-field"
                  id="therapistId"
                  name="therapistId"
                  value={this.state.therapistId}
                  onChange={this.handleChange}
                >
                  {therapists}
                </select>
              </label>
              <div class="submitLabel">
                <input
                  type="submit"
                  value="Create"
                  onClick={this.submitPatient}
                />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  }
}

export default AddPatient;
