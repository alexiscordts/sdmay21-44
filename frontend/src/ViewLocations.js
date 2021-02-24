import React from "react";
import Nav from "./Nav";
import "./TableStyles.css";
import "./UserPages/UserStyles.css";
import "./Settings.css";

class ViewLocations extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    var locationsList = [
      { name: "Des Moines", address: "123 4th Street West Des Moines" },
      { name: "Downtown", address: "123 Court Ave Des Moines" },
    ];
    var rows = [];

    locationsList.forEach(
      function (location) {
        rows.push(
          <tr>
            <td>{location.name}</td>
            <td>{location.address}</td>
            <td>
              <button
                class="iconButton"
                onClick={() => {
                  sessionStorage.setItem("name", location.name);
                  sessionStorage.setItem("address", location.address);
                  window.location.href = "/edit_location";
                }}
              >
                <img
                  src={require("./Icons/icons8-edit-64.png")}
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
          <h2>Addresses</h2>
          <button
            class="iconAddUserButton"
            onClick={() => {
              window.location.href = "/add_location";
            }}
          >
            <img
              src={require("./Icons/icons8-plus-50.png")}
              alt="add"
              className="iconAddLocation"
            />
          </button>
        </div>
        <table class="user-table">
          <thead>
            <tr>
              <th>Name</th>
              <th>Address</th>
              <th>Edit</th>
            </tr>
          </thead>
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewLocations;
