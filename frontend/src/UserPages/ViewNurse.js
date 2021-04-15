import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "./UserStyles.css";
import axios from "axios";
import { Link } from "react-router-dom";

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
                    <Link to="/edit_admin"><button
                      class="iconButton"
                      onClick={() => {
                        sessionStorage.setItem("fname", nurse.fname);
                        sessionStorage.setItem("lname", nurse.lname);
                        // sessionStorage.setItem("email", admin.email);
                      }}
                    >
                      <img
                        src={require("../Icons/icons8-edit-64.png")}
                        alt="edit"
                        className="icon"
                      />
                    </button></Link>
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
          <Link to="/add_nurse"><button
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
export default ViewNurse;
