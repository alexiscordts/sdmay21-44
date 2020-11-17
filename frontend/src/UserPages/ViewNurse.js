import React from "react";
import "../TableStyles.css";
import Nav from "../Nav";

class ViewNurse extends React.Component {
  constructor (props){
    super(props);

  }
    
  render() {
    var nurseList = [{"fname":"Elaine", "lname":"Green", "email":"elaine@up.org"},{"fname":"Richard", "lname":"Markson","email":"rich@up.org"}];
    var rows = [];
    
    nurseList.forEach(function(nurse) {
      rows.push(
        <tr>
          <td>{nurse.fname}</td>
          <td>{nurse.lname}</td>
          <td>{nurse.email}</td>
          <td> 
            <button class = "iconButton" onClick={() => {
                sessionStorage.setItem("fname", nurse.fname);
                sessionStorage.setItem("lname", nurse.lname);
                sessionStorage.setItem("email", nurse.email);
                window.location.href = "/edit_nurse";
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
        <h2>Nurses</h2>
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
export default ViewNurse;