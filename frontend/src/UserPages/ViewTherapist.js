import React from "react";
import Nav from "../Nav";
import "./UserStyles.css";
import "../TableStyles.css";

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
            <button class = "iconButton" onClick={() => {
                sessionStorage.setItem("fname", therapist.fname);
                sessionStorage.setItem("lname", therapist.lname);
                sessionStorage.setItem("email", therapist.email);
                window.location.href = "/edit_therapist";
                }
            }>
              <img src={require("../Icons/icons8-edit-64.png")} alt="edit" className="icon" />
            </button>
          </td>
        </tr>
      );
  }.bind(this));

    return (
      <div >
        <Nav/>
        <div class = "userHeaderRow">
          <h2>Therapists</h2>
          <button class = "iconAddUserButton" onClick={() => {window.location.href = "/add_therapist"}}>
            <img src={require("../Icons/icons8-add-user-male-48.png")} alt="edit" className="iconAddUser" />
          </button>
        </div>
        <table class="user-table">
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