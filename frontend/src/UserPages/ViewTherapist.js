import React from "react";
import Nav from "../Nav";
import "./UserStyles.css";
import "../TableStyles.css";
import axios from "axios";
import { Link } from "react-router-dom";

class ViewTherapist extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      userList: [],
      therapistList: [],
    };
  }

  componentDidMount() {
    const url = "http://10.29.163.20:8081/api/";
    axios.get(url + "user").then((response) => {
      const userList = response.data;
      this.setState({ userList });
    });

    axios.get("http://10.29.163.20:8081/api/permission").then((response) => {
      this.setState({
        therapistList: this.state.therapistList.concat(response.data),
      });
    });
  }
  render() {
    var rows = [];
    this.state.therapistList.forEach(
      function (permission) {
        if (permission.role === "therapist") {
          this.state.userList.forEach(function (therapist) {
            if (therapist.userId === permission.userId) {
              rows.push(
                <tr>
                  <td>{therapist.firstName}</td>
                  <td>{therapist.lastName}</td>
                  <td>{therapist.username}</td>
                  <td>
                    <Link to="/edit_admin"><button
                      class="iconButton"
                      onClick={() => {
                        sessionStorage.setItem("fname", therapist.firstName);
                        sessionStorage.setItem("lname", therapist.lastName);
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
        <div class="userHeaderRow">
          <h2>Therapists</h2>
          <Link to="/add_therapist"><button
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
export default ViewTherapist;
