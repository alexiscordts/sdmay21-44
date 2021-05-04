import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "./UserStyles.css";
import axios from "axios";
import { Link } from "react-router-dom";

class ViewPhysician extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      userList: [],
      physicianList: [],
    };
  }

  componentDidMount() {
    const url = process.env.REACT_APP_SERVER_URL + "";
    axios.get(url + "user").then((response) => {
      const userList = response.data;
      this.setState({ userList });
    });

    axios.get(process.env.REACT_APP_SERVER_URL + "permission").then((response) => {
      this.setState({ physicianList: this.state.physicianList.concat(response.data) });
    });
  }
  render() {
    var rows = [];
    this.state.physicianList.forEach(
      function (permission) {
        if (permission.role === "physician") {
          this.state.userList.forEach(function (physician) {
            if (physician.userId === permission.userId) {
              rows.push(
                <tr>
                  <td>{physician.firstName}</td>
                  <td>{physician.lastName}</td>
                  <td>{physician.username}</td>
                  <td>
                    <button
                      class="iconButton"
                      onClick={() => {
                        sessionStorage.setItem("userId", physician.userId);
                        window.location.href = "/edit_physician";
                      }}
                    >
                      <img
                        src={require("../Icons/icons8-edit-64.png")}
                        alt="edit"
                        className="icon"
                      />
                    </button>
                  </td>
                </tr>
              );
            }
          });
        }
      }.bind(this)
    );

    return (
      <div>
        <div class="userHeaderRow">
          <h2>Physicians</h2>
          <Link to="/add_physician"><button
            class="iconAddUserButton"
          >
            <img
              src={require("../Icons/icons8-add-user-male-48.png")}
              alt="edit"
              className="iconAddUser"
            />
          </button></Link>
        </div>
        <table class="user-table">
          <thead>
            <tr>
              <th>First Name</th>
              <th>Last Name</th>
              <th>Username</th>
              <th>Edit</th>
            </tr>
          </thead>
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewPhysician;
