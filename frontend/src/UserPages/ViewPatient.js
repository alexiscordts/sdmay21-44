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
    const url = "http://10.29.163.20:8081/api/patient";

    axios.get(url).then((response) => {
      const patientList = response.data;
      this.setState({ patientList });
    });
  }

  render() {
    var rows = [];
    //Iterate throught the list of patients and render each one on it's own row in the table
    this.state.patientList.forEach(
      function (patient) {
        if (patient.startDate) {
          rows.push(
            <tr>
              <td>{patient.firstName}</td>
              <td>{patient.lastName}</td>
              <td>{patient.startDate.substring(0, 10)}</td>

              <td>
                <button
                  class="iconButton"
                  onClick={() => {
                    // sessionStorage.setItem("email", admin.email);
                    sessionStorage.setItem("patientId", patient.patientId);

                    window.location.href = "/edit_patient";
                  }}
                >
                  <img
                    src={require("../Icons/icons8-edit-64.png")}
                    alt="delete"
                    className="icon"
                  />
                </button>
              </td>
            </tr>
          );
        } else {
          rows.push(
            <tr>
              <td>{patient.firstName}</td>
              <td>{patient.lastName}</td>
              <td>Unavailable</td>

              <td>
                <button
                  class="iconButton"
                  onClick={() => {
                    window.location.href = "/edit_patient";
                  }}
                >
                  <img
                    src={require("../Icons/icons8-delete-64.png")}
                    alt="delete"
                    className="icon"
                  />
                </button>
              </td>
            </tr>
          );
        }
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
              <th>Edit</th>
            </tr>
          </thead>
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewPatient;
