import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "../UserPages/UserStyles.css";
import "../Settings.css";
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
    var id = event.target.value;
    var rooms = [];
    this.state.roomList.forEach(function (room) {
      if (id == room.locationId) {
        rooms.push(
          <tr>
            <td>{room.number}</td>
            <td>
              <button
                class="iconButton"
                onClick={() => {
                  const url = "http://10.29.163.20:8081/api/room";
                  console.log(room);
                  axios.delete(url, { data: room }).catch((error) => {
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
    this.setState({ rooms });
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
        <div class="userHeaderRow">
          <h2>Rooms</h2>
          <div class="dropdown">
            <select
              class="dropbtn"
              value={this.state.locationId}
              onChange={this.updateRoom}
            >
              {locations}
            </select>
          </div>

          <button
            class="iconAddUserButton"
            onClick={() => {
              window.location.href = "/add_room";
            }}
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
          <tbody>{this.state.rooms}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewRooms;
