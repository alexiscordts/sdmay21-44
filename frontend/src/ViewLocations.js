import React from "react";
import Nav from "./Nav";
import "./TableStyles.css";
import "./UserPages/UserStyles.css";
import "./Settings.css";
import axios from "axios";

class ViewLocations extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      locationList: [],
    };
  }

  componentDidMount() {
    const url = "http://10.29.163.20:8081/api/Location";

    axios
      .get(url)
      // .then((json = {}) => json.data)
      .then((response) => {
        console.log(response.data);
        const locationList = response.data;
        this.setState({ locationList });
      });
  }

  render() {
    var rows = [];

    this.state.locationList.forEach(
      function (location) {
        rows.push(
          <tr>
            <td>{location.name}</td>
            <td>Address</td>
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
          <h2>Locations</h2>
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
