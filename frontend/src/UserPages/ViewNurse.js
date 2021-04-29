import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "./UserStyles.css";
import axios from "axios";

class ViewNurse extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      userList: [],
      nurseList: [],
    };
  }

  componentDidMount() {
    const url = "http://10.29.163.20:8081/api/";
    axios.get(url + "user").then((response) => {
      const userList = response.data;
      this.setState({ userList });
    });

    axios.get("http://10.29.163.20:8081/api/permission").then((response) => {
      this.setState({ nurseList: this.state.nurseList.concat(response.data) });
    });
  }
  render() {
    var rows = [];
    //Iterate throught the list of nurses and render each one on it's own row in the table
    this.state.nurseList.forEach(
      function (permission) {
        if (permission.role === "nurse") {
          this.state.userList.forEach(function (nurse) {
            if (nurse.userId === permission.userId) {
              rows.push(
                <tr>
                  <td>{nurse.firstName}</td>
                  <td>{nurse.lastName}</td>
                  <td>{nurse.username}</td>
                  <td>
                    <button
                      class="iconButton"
                      onClick={() => {
                        sessionStorage.setItem("userId", nurse.userId);
                        window.location.href = "/edit_nurse";
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
        <Nav />
        <div class="userHeaderRow">
          <h2>Nurses</h2>
          <button
            class="iconAddUserButton"
            onClick={() => {
              window.location.href = "/add_nurse";
            }}
          >
            <img
              src={require("../Icons/icons8-add-user-male-48.png")}
              alt="edit"
              className="iconAddUser"
            />
          </button>
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
export default ViewNurse;
