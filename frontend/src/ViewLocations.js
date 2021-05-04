import React from "react";
import Nav from "./Nav";
import "./TableStyles.css";
import "./UserPages/UserStyles.css";
import "./Settings.css";
import { Link } from "react-router-dom";
import axios from "axios";

class ViewLocations extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      locationList: [],
    };
  }

  componentDidMount() {
    const url = process.env.REACT_APP_SERVER_URL + "Location";

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
            <td>{location.address}</td>
            <td>
              <button
                class="iconButton"
                onClick={() => {
                  sessionStorage.setItem("name", location.name);
                  sessionStorage.setItem("address", location.address);
                  sessionStorage.setItem("locationId", location.locationId);
                  sessionStorage.setItem("phoneNumber", location.phoneNumber);
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
        <div class="userHeaderRow">
          <h2>Locations</h2>
          <Link to="/add_location"><button
            class="iconAddUserButton"
          >
            <img
              src={require("./Icons/icons8-plus-48.png")}
              alt="add"
              className="iconAddLocation"
            />
          </button></Link>
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
