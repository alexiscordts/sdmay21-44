import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "../UserPages/UserStyles.css";
import "../Settings.css";
import { Link } from "react-router-dom";

class ViewRooms extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    var roomsList = [{ number: 401 }, { number: 402 }];
    var rows = [];
    roomsList.forEach(
      function (room) {
        rows.push(
          <tr>
            <td>{room.number}</td>
            <td>
              <button
                class="iconButton"
                onClick={() => {
                  //Delete room
                  console.log("Room delete click" + room.number);
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
      }.bind(this)
    );

    return (
      <div>
        <Nav />
        <div class="userHeaderRow">
          <h2>Rooms</h2>
          <div class="dropdown">
            <button class="dropbtn">
              Location
              <i class="arrow down"></i>
            </button>
            <div class="dropdown-content">
              <a>Location 1 </a>
              <br></br>
              <a>Location 2</a>
              <br></br>
              <a>Location 3</a>
            </div>
          </div>

          <Link to="/add_room"><button
            class="iconAddUserButton"
          >
            <img
              src={require("../Icons/icons8-plus-48.png")}
              alt="add"
              className="iconAddLocation"
            />
          </button></Link>
        </div>
        <table class="user-table">
          <thead>
            <tr>
              <th>Room Number</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewRooms;
