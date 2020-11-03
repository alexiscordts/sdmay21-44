import React from "react";
import "./TableStyles.css";
import Nav from "./Nav";

class ViewTherapist extends React.Component {
  constructor (props){
    super(props);

  }
    
  render() {
    var therapistList = [{"fname":"Amy", "lname":"Adams", "email":"amy@up.org"},{"fname":"John", "lname":"Smith","email":"john@up.org"}];
    var rows = [];
    
    therapistList.forEach(function(therapist) {
      rows.push(
        <tr>
          <td>{therapist.fname}</td>
          <td>{therapist.lname}</td>
          <td>{therapist.email}</td>
          <td> 
            <a href="edit_therapist" >
              <img src={require("./Icons/icons8-edit-64.png")} alt="edit" className="icon" />
            </a>
          </td>
        </tr>
      );
  }.bind(this));

    return (
      <div >
        <Nav/>
        <h2>Therapists</h2>
        <table class="fl-table">
            <thead>
            <tr>
                <th>First Name</th>
                <th>Last Name</th>
                <th>E-mail</th>
                <th>Edit</th>
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
export default ViewTherapist;