import React from "react";
import "../TableStyles.css";
import "./UserStyles.css";
import Nav from "../Nav";
import axios from "axios";

class ViewAdmin extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      userList: [],
      adminList: [],
    };
  }

  componentDidMount() {
    const url = "http://10.29.163.20:8081/api/";

    // var adminList = [];

    // axios.get(url + "user").then((response) => {
    //   const users = response.data;
    //   this.setState({ users });
    // });

    axios.get(url + "user").then((response) => {
      const userList = response.data;
      this.setState({ userList });
    });

    axios.get("http://10.29.163.20:8081/api/permission").then((response) => {
      this.setState({ adminList: this.state.adminList.concat(response.data) });
    });

    // console.log(this.state);
    // axios.get(url + "permission").then((response) => {
    //   // console.log(response.data);
    //   response.data.forEach(function (user) {
    //     if (user.role === "admin") {
    //       axios
    //         .get(url + "user/getUserByUserId/" + user.userId)
    //         .then((response) => {
    //           // console.log(response.data);
    //           adminList = adminList.concat(response.data);
    //           // console.log(adminList);
    //         })
    //         .catch(function (error) {
    //           console.log(error);
    //         });
    //     }
    //   });
    // });

    // console.log("Got to end");
    // this.setState({ adminList });
    // console.log(this.state.adminNumList);
    // console.log("in function");
    // console.log(adminList);
  }

  render() {
    var rows = [];

    this.state.adminList.forEach(
      function (permission) {
        if (permission.role === "admin") {
          this.state.userList.forEach(function (admin) {
            if (admin.userId === permission.userId) {
              rows.push(
                <tr>
                  <td>{admin.firstName}</td>
                  <td>{admin.lastName}</td>
                  <td>{admin.username}</td>
                  <td>
                    <button
                      class="iconButton"
                      onClick={() => {
                        sessionStorage.setItem("fname", admin.fname);
                        sessionStorage.setItem("lname", admin.lname);
                        // sessionStorage.setItem("email", admin.email);
                        window.location.href = "/edit_admin";
                      }}
                    >
                      <img
                        src={require("../Icons/icons8-edit-64.png")}
                        alt="edit"
                        className="icon"
                      />
                    </button>
                  </td>
                </tr>
              );
            }
          });
        }
      }.bind(this)
    );

    // this.state.userList.forEach(
    //   function (admin) {
    //     rows.push(
    //       <tr>
    //         <td>{admin.firstName}</td>
    //         <td>{admin.lastName}</td>
    //         <td>{admin.username}</td>
    //         <td>
    //           <button
    //             class="iconButton"
    //             onClick={() => {
    //               sessionStorage.setItem("fname", admin.fname);
    //               sessionStorage.setItem("lname", admin.lname);
    //               // sessionStorage.setItem("email", admin.email);
    //               window.location.href = "/edit_admin";
    //             }}
    //           >
    //             <img
    //               src={require("../Icons/icons8-edit-64.png")}
    //               alt="edit"
    //               className="icon"
    //             />
    //           </button>
    //         </td>
    //       </tr>
    //     );
    //   }.bind(this)
    // );

    return (
      <div>
        <Nav />
        <div class="userHeaderRow">
          <h2>Admins</h2>
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
export default ViewAdmin;
