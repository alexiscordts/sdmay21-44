import React from "react";
import "./TableStyles.css";
import Nav from "./Nav";

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
              <img src={require("./Icons/icons8-edit-64.png")} alt="edit" className="icon" />
            </button>
          </td>
        </tr>
      );
  }.bind(this));

    return (
      <div >
        <Nav/>
        <h2>Admins</h2>
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
export default ViewAdmin;