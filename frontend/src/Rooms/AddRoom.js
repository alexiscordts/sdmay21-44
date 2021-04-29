import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

class AddRoom extends React.Component {
  constructor(props) {
    super(props);
    this.state = { locationList: [], number: null, locationId: null };
    this.handleChange = this.handleChange.bind(this);
    this.handlePost = this.handlePost.bind(this);
  }
  componentDidMount() {
    axios.get("http://10.29.163.20:8081/api/location/").then((response) => {
      this.setState({
        locationList: this.state.locationList.concat(response.data),
      });
    });
  }
  handleChange(event) {
    this.setState({
      [event.target.name]: event.target.value,
    });
  }

  handlePost() {
    const url = "http://10.29.163.20:8081/api/room/";

    const { number, locationId } = this.state;
    const active = 1;
    const location = null;
    const appointment = [];
    const patient = [];

    const room = { number, locationId, active, location, appointment, patient };

    console.log(room);
    axios
      .post(url, { data: room })
      .then(function (response) {
        console.log(response);
      })
      .catch(function (error) {
        console.log(error);
      });
    // setTimeout(function () {
    //   window.location.href = "/manage_locations";
    // }, 2000);
  }

  render() {
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
    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Add a Room </div>
            <form action="" method="post">
              <div class="dropdown">
                <select
                  class="dropbtn"
                  name="locationId"
                  value={this.state.locationId}
                  onChange={this.handleChange}
                >
                  {locations}
                </select>
              </div>
              <br></br>
              <br></br>

              <label>
                <span>
                  Number
                  <span class="required">*</span>
                </span>
                <input
                  type="number"
                  class="input-field"
                  name="number"
                  value={this.state.number}
                  onChange={this.handleChange}
                />
              </label>

              <div class="submitLabel">
                <input
                  type="button"
                  value="Add Room"
                  onClick={this.handlePost}
                />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  }
}

export default AddRoom;
