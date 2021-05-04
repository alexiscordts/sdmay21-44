import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

class EditPatient extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      roomList: [],
      userList: [],
      therapistList: [],
      locationList: [],
      rooms: [],
      patient: [],
      physicians: []
    };
    this.deletePatient = this.deletePatient.bind(this);
    this.updateRoom = this.updateRoom.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(event) {
    this.setState({
      patient: {
        ...this.state.patient,
        [event.target.name]: event.target.value,
      },
    });
  }
  componentDidMount() {
    const url = process.env.REACT_APP_SERVER_URL + "";
    axios.get(url + "user").then((response) => {
      const userList = response.data;
      this.setState({ userList });
      axios.get(process.env.REACT_APP_SERVER_URL + "permission").then((response) => {
      this.setState({
        therapistList: this.state.therapistList.concat(response.data),
      });
      const physicians = [];
      this.state.therapistList.forEach(permission => {
          this.state.userList.forEach(user => {
            if (user.userId == permission.userId && permission.role == 'physician')
              physicians.push(user);
          });
      });
      this.setState({physicians});
    });
    });

    axios.get(process.env.REACT_APP_SERVER_URL + "location").then((response) => {
      this.setState({
        locationList: this.state.locationList.concat(response.data),
      });
    });

    var patient = [];
    var id = sessionStorage.getItem("patientId");
    axios.get(process.env.REACT_APP_SERVER_URL + "patient/" + id).then((response) => {
      patient = response.data;
      this.setState({ patient });
      axios.get(process.env.REACT_APP_SERVER_URL + "room").then((response) => {
      this.setState({ roomList: this.state.roomList.concat(response.data) });
      this.getRooms(patient.locationId);
    });
    });
  }
  updateRoom(event) {
    this.setState({
      patient: {
        ...this.state.patient,
        [event.target.name]: event.target.value,
      },
    });
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

  getRooms(id)  {
    var rooms = [];
    this.state.roomList.forEach(function (room) {
      if (id == room.locationId) {
        rooms.push(<option value={room.number}>{room.number}</option>);
      }
    });
    this.setState({rooms});
  }

  deletePatient() {
    if (window.confirm("Are you sure you want to delete this patient?")) {
      const url =
        process.env.REACT_APP_SERVER_URL + "patient/" + this.state.patient.patientId;
      axios.delete(url);
      setTimeout(() => {
        window.location.href = "/view_patient";
      }, 2000);
    }
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
    users.push(
      <option hidden disabled selected value>
        Select a Physician
      </option>
    );
    this.state.physicians.forEach(function (user) {
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
            <div class="form-style-heading"> Edit Patient </div>
            <form>
              <label for="firstName">
                <span>First Name</span>
                <input
                  target="patient"
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="firstName"
                  defaultValue={this.state.patient.firstName}
                />
              </label>
              <label for="middleName">
                <span>Middle Name</span>
                <input
                  target="patient"
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="middleName"
                  defaultValue={this.state.patient.middleName}
                />
              </label>
              <label for="lastName">
                <span>Last Name</span>
                <input
                  target="patient"
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="lastName"
                  defaultValue={this.state.patient.lastName}
                />
              </label>
              <label for="address">
                <span>Address</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="address"
                  defaultValue={this.state.patient.address}
                />
              </label>
              <label for="phoneNumber">
                <span>Phone Number</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="phoneNumber"
                  defaultValue={this.state.patient.phoneNumber}
                />
              </label>
              <label for="location">
                <span>Location</span>
                <select
                  type="number"
                  class="input-field"
                  onChange={this.updateRoom}
                  name="locationId"
                  value={this.state.patient.locationId}
                >
                  {locations}
                </select>
              </label>
              <label for="roomNumber">
                <span>Room</span>
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
                <span>Primary Physician</span>
                <select
                  type="number"
                  class="input-field"
                  onChange={this.handleChange}
                  name="pmrPhysicianId"
                  value={this.state.patient.pmrPhysicianId}
                >
                  {users}
                </select>
              </label>
              <br></br>
              <label for="therapist">
                <span>Therapist</span>
                <select
                  type="number"
                  class="input-field"
                  name="therapistId"
                  value={this.state.patient.therapistId}
                  onChange={this.handleChange}
                >
                  {therapists}
                </select>
              </label>
              <div class="buttonContainer">
                <input
                  type="button"
                  value="Delete"
                  onClick={this.deletePatient}
                />
                <input
                  type="button"
                  value="Save"
                  onClick={() => {
                    console.log(this.state.patient);
                    const url =
                      process.env.REACT_APP_SERVER_URL + "patient/" +
                      this.state.patient.patientId;
                    axios.put(url, this.state.patient);
                  }}
                />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  }
}
export default EditPatient;
