import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";
class EditAdmin extends React.Component {
  constructor(props) {
    super(props);
    this.deleteAdmin = this.deleteAdmin.bind(this);
    this.deletePermission = this.deletePermission.bind(this);
  }

  deleteAdmin() {
    const url =
      "http://10.29.163.20:8081/api/user/" + sessionStorage.getItem("userId");
    axios.delete(url);
    setTimeout(this.deletePermission, 2000);
  }

  deletePermission() {
    const url =
      "http://10.29.163.20:8081/api/permission/" +
      sessionStorage.getItem("userId");
    axios.delete(url);
    window.location.href = "/view_admin";
  }
  render() {
    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Edit Admin Information</div>
            <form action="" method="post">
              <label for="fname">
                <span>
                  First Name <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="fname"
                  defaultValue={sessionStorage.getItem("firstName")}
                />
              </label>
              <label for="lname">
                <span>
                  Last Name <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="lname"
                  defaultValue={sessionStorage.getItem("lastName")}
                />
              </label>
              {/* <label for="email">
                <span>
                  E-mail <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="email"
                  defaultValue={sessionStorage.getItem("email")}
                />
              </label> */}
              <div class="buttonContainer">
                <input
                  type="button"
                  value="Delete"
                  onClick={() => {
                    if (
                      window.confirm(
                        "Are you sure you want to delete this user?"
                      )
                    ) {
                      this.deleteAdmin();
                    }
                  }}
                />
                <input type="submit" value="Save" />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  }
}
export default EditAdmin;
