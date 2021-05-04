import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "../UserPages/UserStyles.css";
import "../Settings.css";
import { Link } from "react-router-dom";
import axios from "axios";

class ViewRooms extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      locationList: [],
      roomList: [],
      rooms: [],
      locationId: null,
    };
    this.updateRoom = this.updateRoom.bind(this);
  }

  componentDidMount() {
    axios.get("http://10.29.163.20:8081/api/location/").then((response) => {
      this.setState({
        locationList: this.state.locationList.concat(response.data),
      });
    });

    axios.get("http://10.29.163.20:8081/api/room").then((response) => {
      this.setState({ roomList: this.state.roomList.concat(response.data) });
    });
  }

  updateRoom(event) {
    this.setState({ [event.target.name]: event.target.value });
  }

  getRoomList(event)  {
    var id = this.state.locationId;
    var rooms = [];
    if (id != null)
    this.state.roomList.forEach(function (room) {
      if (id == room.locationId) {
        rooms.push(
          <tr>
            <td>{room.number}</td>
            <td>
              <button
                class="iconButton"
                onClick={() => {
                  console.log()
                  const r = {
                  number: room.number,
                  locationId: id
                  }
                  const url =
                    "http://10.29.163.20:8081/api/room/deleteRoom/";
                  axios.post(url, r)
                  .then((response) => {
                    console.log(response.data);
                    axios.get("http://10.29.163.20:8081/api/room").then((response2) => {
                      const rooms = response2.data;
                      event.setState({ roomList: rooms });
                    });
                  })
                  .catch((error) => {
                      console.log("Error caught");
                      console.log(error);
                  });
                }}
              >
                <img
                  src={require("../Icons/icons8-delete-64.png")}
                  alt="edit"
                  className="icon"
                />
              </button>
            </td>
          </tr>
        );
      }
    });
    return rooms;
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
        <div class="userHeaderRow">
          <h2>Rooms</h2>
          <div class="dropdown">
            <select
              class="dropbtn"
              name="locationId"
              value={this.state.locationId}
              onChange={this.updateRoom}
            >
              {locations}
            </select>
          </div>

          <button onClick={() => {if (this.state.locationId != null) { sessionStorage.setItem("locationId", this.state.locationId); window.location.href = "add_room"}}}
            class="iconAddUserButton"
          >
            <img
              src={require("../Icons/icons8-plus-48.png")}
              alt="add"
              className="iconAddLocation"
            />
          </button>
        </div>
        <table class="user-table">
          <thead>
            <tr>
              <th>Room Number</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>{this.getRoomList(this)}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewRooms;
