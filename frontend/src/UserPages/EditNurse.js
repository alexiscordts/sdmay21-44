import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

class EditNurse extends React.Component {
  constructor(props) {
    super(props);
    this.state = { nurse: [], password: null };
    this.deleteNurse = this.deleteNurse.bind(this);
    this.deletePermission = this.deletePermission.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }
  handleChange(event) {
    this.setState({
      nurse: {
        ...this.state.nurse,
        [event.target.name]: event.target.value,
      },
    });
  }

  componentDidMount() {
    var nurse = [];
    var id = sessionStorage.getItem("userId");
    axios
      .get("http://10.29.163.20:8081/api/user/getUserByUserId/" + id)
      .then((response) => {
        nurse = response.data;
        const password = nurse.password;
        this.setState({ nurse });
        this.setState({ password });
      });
  }

  deleteNurse() {
    const url =
      "http://10.29.163.20:8081/api/user/" + sessionStorage.getItem("userId");
    console.log(url);
    axios.delete(url);
    setTimeout(this.deletePermission, 2000);
  }

  deletePermission() {
    const url =
      "http://10.29.163.20:8081/api/permission/" +
      sessionStorage.getItem("userId");
    axios.delete(url);
    window.location.href = "/view_nurse";
  }
  render() {
    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Edit Nurse Information</div>
            <form action="" method="post">
              <label for="fname">
                <span>
                  First Name <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="fname"
                  onChange={this.handleChange}
                  defaultValue={this.state.nurse.firstName}
                />
              </label>
              <label for="middleName">
                <span>Middle Name</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="middleName"
                  defaultValue={this.state.nurse.middleName}
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
                  defaultValue={this.state.nurse.lastName}
                />
              </label>
              <label for="address">
                <span>Address</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="address"
                  value={this.state.nurse.address}
                />
              </label>
              <label for="phoneNumber">
                <span>Phone Number</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="phoneNumber"
                  value={this.state.nurse.phoneNumber}
                />
              </label>
              <label for="email">
                <span>
                  Username
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="username"
                  value={this.state.nurse.username}
                />
              </label>
              <label for="password">
                <span>
                  Password
                  <span class="required">*</span>
                </span>
                <input
                  type="password"
                  class="input-field"
                  onChange={this.handleChange}
                  name="password"
                  value={this.state.nurse.password}
                />
              </label>
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
                      this.deleteNurse();
                    }
                  }}
                />
                <input
                  type="button"
                  value="Save"
                  onClick={() => {
                    const url =
                      "http://10.29.163.20:8081/api/user/" +
                      this.state.nurse.userId;
                    if (this.state.password != this.state.nurse.password) {
                      axios.put(url, this.state.nurse);
                    } else {
                      const {
                        userId,
                        username,
                        firstName,
                        lastName,
                        middleName,
                        address,
                        phoneNumber,
                        color,
                      } = this.state.nurse;
                      const active = 1;
                      const nurse = {
                        userId,
                        username,
                        firstName,
                        lastName,
                        middleName,
                        address,
                        phoneNumber,
                        color,
                        active,
                      };
                      axios.put(url, nurse);
                    }
                    setTimeout(() => {
                      window.location.href = "/view_nurse";
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
export default EditNurse;
