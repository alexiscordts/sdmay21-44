import axios from "axios";
import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";

class EditTherapist extends React.Component {
  constructor(props) {
    super(props);
    this.state = { therapist: [], password: null };
    this.handleChange = this.handleChange.bind(this);
    this.deletePermission = this.deletePermission.bind(this);
  }
  handleChange(event) {
    this.setState({
      therapist: {
        ...this.state.therapist,
        [event.target.name]: event.target.value,
      },
    });
  }
  componentDidMount() {
    var therapist = [];
    var id = sessionStorage.getItem("userId");
    axios
      .get(process.env.REACT_APP_SERVER_URL + "user/getUserByUserId/" + id)
      .then((response) => {
        therapist = response.data;
        var password = therapist.password;
        this.setState({ password });
        this.setState({ therapist });
      });
  }

  deleteTherapist() {
    const url =
      process.env.REACT_APP_SERVER_URL + "user/" + this.state.therapist.userId;
    console.log(url);
    axios.delete(url);
    setTimeout(this.deletePermission, 2000);
  }
  deletePermission() {
    const url =
      process.env.REACT_APP_SERVER_URL + "permission/" + this.state.therapist.userId;
    window.location.href = "/view_therapist";
  }

  render() {
    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Edit Therapist </div>
            <form action="" method="post">
              <label for="fname">
                <span>
                  First Name
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="firstName"
                  value={this.state.therapist.firstName}
                />
              </label>
              <label for="middleName">
                <span>Middle Name</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="middleName"
                  value={this.state.therapist.middleName}
                />
              </label>
              <label for="lastName">
                <span>
                  Last Name
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="lastName"
                  value={this.state.therapist.lastName}
                />
              </label>
              <label for="address">
                <span>Address</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="address"
                  value={this.state.therapist.address}
                />
              </label>
              <label for="phoneNumber">
                <span>Phone Number</span>
                <input
                  type="text"
                  class="input-field"
                  onChange={this.handleChange}
                  name="phoneNumber"
                  value={this.state.therapist.phoneNumber}
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
                  value={this.state.therapist.username}
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
                  value={this.state.therapist.password}
                />
              </label>
              <label for="color">
                <span>Color</span>
                <span class="required">*</span>
                <input
                  type="color"
                  name="color"
                  onChange={this.handleChange}
                  value={this.state.therapist.color}
                ></input>
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
                      this.deleteTherapist();
                    }
                  }}
                />
                <input
                  type="button"
                  value="Save"
                  onClick={() => {
                    const url =
                      process.env.REACT_APP_SERVER_URL + "user/" +
                      this.state.therapist.userId;
                    if (this.state.password != this.state.therapist.password) {
                      axios.put(url, this.state.therapist);
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
                      } = this.state.therapist;
                      const active = 1;
                      const therapist = {
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
                      console.log(therapist);
                      axios.put(url, therapist);
                    }
                    setTimeout(() => {
                      window.location.href = "/view_therapist";
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
export default EditTherapist;
