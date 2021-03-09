import React from "react";
import Nav from "../Nav";
import "./UserStyles.css";
import "../TableStyles.css";
import axios from "axios";

class ViewPatient extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      patientList: [],
    };
  }

  componentDidMount() {
    const url = "http://localhost:52723/api/Patient";

    axios
      .get(url)
      // .then((json = {}) => json.data)
      .then((response) => {
        // console.log(response.data);
        const patientList = response.data;
        this.setState({ patientList });
      });
  }

  render() {
    var rows = [];

    this.state.patientList.forEach(
      function (patient) {
        rows.push(
          <tr>
            <td>{patient.firstName}</td>
            <td>{patient.lastName}</td>
            <td>{patient.startDate}</td>
            <td>
              <button class="iconButton">
                <img
                  src={require("../Icons/icons8-delete-64.png")}
                  alt="delete"
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
          <h2>Patients</h2>
          <button
            class="iconAddUserButton"
            onClick={() => {
              window.location.href = "/add_patient";
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
              <th>Admission Date</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewPatient;
