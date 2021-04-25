import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";
class EditAdmin extends React.Component {
  constructor(props) {
    super(props);
    this.state = { admin: [] };
    this.deleteAdmin = this.deleteAdmin.bind(this);
    this.deletePermission = this.deletePermission.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  componentDidMount() {
    var admin = [];
    var id = sessionStorage.getItem("userId");
    axios
      .get("http://10.29.163.20:8081/api/user/getUserByUserId/" + id)
      .then((response) => {
        admin = response.data;
        this.setState({ admin });
      });
  }

  handleChange(event) {
    this.setState({
      admin: {
        ...this.state.admin,
        [event.target.name]: event.target.value,
      },
    });
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
                  defaultValue={this.state.admin.firstName}
                  onChange={this.handleChange}
                />
              </label>
              <label for="middleName">
                <span>Middle Name</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="middleName"
                  defaultValue={this.state.admin.middleName}
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
                  onChange={this.handleChange}
                  defaultValue={this.state.admin.lastName}
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
                <input
                  type="button"
                  value="Save"
                  onClick={() => {
                    console.log(this.state.admin);
                    const url =
                      "http://10.29.163.20:8081/api/user/" +
                      this.state.admin.userId;
                    axios.put(url, this.state.admin);
                    setTimeout(() => {
                      window.location.href = "/view_admin";
                    }, 2000);
                  }}
                />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  }
}
export default EditAdmin;
