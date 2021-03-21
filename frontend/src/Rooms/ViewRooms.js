import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "../UserPages/UserStyles.css";
import "../Settings.css";

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
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewRooms;
