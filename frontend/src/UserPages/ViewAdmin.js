import React from "react";
import "../TableStyles.css";
import "./UserStyles.css";
import Nav from "../Nav";

class ViewAdmin extends React.Component {
  constructor (props){
    super(props);

  }
    
  render() {
    var adminList = [{"fname":"Sarah", "lname":"Geller", "email":"sarah@up.org"},{"fname":"Zach", "lname":"Johnson","email":"zach@up.org"}];
    var rows = [];
    
    adminList.forEach(function(admin) {
      rows.push(
        <tr>
          <td>{admin.fname}</td>
          <td>{admin.lname}</td>
          <td>{admin.email}</td>
          <td> 
            <button class = "iconButton" onClick={() => {
                sessionStorage.setItem("fname", admin.fname);
                sessionStorage.setItem("lname", admin.lname);
                sessionStorage.setItem("email", admin.email);
                window.location.href = "/edit_admin";
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
          <h2>Admins</h2>
          <button class = "iconAddUserButton" onClick={() => {window.location.href = "/add_patient"}}>
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
export default ViewAdmin;