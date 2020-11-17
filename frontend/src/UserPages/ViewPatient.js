import React from "react";
import "../TableStyles.css";
import Nav from "../Nav";

class ViewPatient extends React.Component {
  constructor (props){
    super(props);

  }
    
  render() {
    var patientList = [{"fname":"Paul", "lname":"Henderson", "admissionDate":"1/5/20"},{"fname":"Larry", "lname":"Mason", "admissionDate":"2/15/20"}];
    var rows = [];
    
    patientList.forEach(function(patient) {
      rows.push(
        <tr>
          <td>{patient.fname}</td>
          <td>{patient.lname}</td>
          <td>{patient.admissionDate}</td>
          <td> 
            <button class = "iconButton" >
              <img src={require("../Icons/icons8-delete-64.png")} alt="delete" className="icon" />
            </button>
          </td>
        </tr>
      );
  }.bind(this));

    return (
      <div >
        <Nav/>
        <h2>Patients</h2>
        <table class="fl-table">
            <thead>
            <tr>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Admission Date</th>
                <th>Delete</th>
            </tr>
            </thead>
            <tbody>
            {rows}
            </tbody>
        </table>
      </div>
    );
  }
}
export default ViewPatient;