import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

class EditPhysician extends React.Component {
  constructor(props) {
    super(props);
    this.state = { physician: [], password: null };
    this.deletePhysician = this.deletePhysician.bind(this);
    this.deletePermission = this.deletePermission.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }
  handleChange(event) {
    this.setState({
      physician: {
        ...this.state.physician,
        [event.target.name]: event.target.value,
      },
    });
  }

  componentDidMount() {
    var physician = [];
    var id = sessionStorage.getItem("userId");
    axios
      .get(process.env.REACT_APP_SERVER_URL + "user/getUserByUserId/" + id)
      .then((response) => {
        physician = response.data;
        const password = physician.password;
        this.setState({ physician });
        this.setState({ password });
      });
  }

  deletePhysician() {
    const url =
      process.env.REACT_APP_SERVER_URL + "user/" + sessionStorage.getItem("userId");
    console.log(url);
    axios.delete(url);
    setTimeout(this.deletePermission, 2000);
  }

  deletePermission() {
    const url =
      process.env.REACT_APP_SERVER_URL + "permission/" +
      sessionStorage.getItem("userId");
    axios.delete(url);
    window.location.href = "/view_physician";
  }
  render() {
    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Edit Physician Information</div>
            <form action="" method="post">
              <label for="firstName">
                <span>
                  First Name <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="firstName"
                  onChange={this.handleChange}
                  defaultValue={this.state.physician.firstName}
                />
              </label>
              <label for="middleName">
                <span>Middle Name</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="middleName"
                  defaultValue={this.state.physician.middleName}
                />
              </label>
              <label for="lastName">
                <span>
                  Last Name <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="lastName"
                  onChange={this.handleChange}
                  defaultValue={this.state.physician.lastName}
                />
              </label>
              <label for="address">
                <span>Address</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="address"
                  value={this.state.physician.address}
                />
              </label>
              <label for="phoneNumber">
                <span>Phone Number</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="phoneNumber"
                  value={this.state.physician.phoneNumber}
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
                  value={this.state.physician.username}
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
                  value={this.state.physician.password}
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
                      this.deletePhysician();
                    }
                  }}
                />
                <input
                  type="button"
                  value="Save"
                  onClick={() => {
                    const url =
                      process.env.REACT_APP_SERVER_URL + "user/" +
                      this.state.physician.userId;
                    if (this.state.password != this.state.physician.password) {
                      axios.put(url, this.state.physician);
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
                      } = this.state.physician;
                      const active = 1;
                      const physician = {
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
                      axios.put(url, physician);
                    }
                    setTimeout(() => {
                      window.location.href = "/view_physician";
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
export default EditPhysician;
