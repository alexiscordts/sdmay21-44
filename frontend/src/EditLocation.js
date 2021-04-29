import React from "react";
import "./FormStyles.css";
import Nav from "./Nav";
import axios from "axios";

class EditLocation extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      location: [],
      address: "",
      name: "",
      phoneNumber: "",
      active: 1,
    };
    this.handleChange = this.handleChange.bind(this);
    this.handlePut = this.handlePut.bind(this);
  }

  componentDidMount() {
    var location = [];
    var id = sessionStorage.getItem("locationId");
    axios
      .get(
        "http://10.29.163.20:8081/api/location/getLocationByLocationId/" + id
      )
      .then((response) => {
        location = response.data;
        this.setState({ location });
      });
  }

  handlePut(event) {
    event.preventDefault();

    const url =
      "http://10.29.163.20:8081/api/location/" +
      sessionStorage.getItem("locationId");
    console.log(this.state.location);
    axios
      .put(url, this.state.location)
      .then(function (response) {
        console.log(response);
      })
      .catch(function (error) {
        console.log(error);
      });
    setTimeout(function () {
      window.location.href = "/manage_locations";
    }, 2000);
  }

  deleteLocation() {
    const url =
      "http://10.29.163.20:8081/api/location/" +
      sessionStorage.getItem("locationId");
    axios.delete(url);
    setTimeout(function () {
      window.location.href = "/manage_locations";
    }, 2000);
  }

  handleChange(event) {
    this.setState({
      location: {
        ...this.state.location,
        [event.target.name]: event.target.value,
      },
    });
  }
  render() {
    return (
      <div>
        <Nav />
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Update Location </div>
            <form onSubmit={this.handlePost}>
              <label for="name">
                <span>
                  Name
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="name"
                  onChange={this.handleChange}
                  defaultValue={this.state.location.name}
                />
              </label>
              <label for="name">
                <span>
                  Address
                  <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="address"
                  defaultValue={this.state.location.address}
                  onChange={this.handleChange}
                />
              </label>
              <label for="phoneNumber">
                <span>
                  Phone Number <span class="required">*</span>
                </span>
                <input
                  type="text"
                  class="input-field"
                  name="phoneNumber"
                  defaultValue={this.state.location.phoneNumber}
                  onChange={this.handleChange}
                />
              </label>
              <div class="buttonContainer">
                <input
                  type="button"
                  value="Delete"
                  onClick={() => {
                    if (
                      window.confirm(
                        "Are you sure you want to delete this location?"
                      )
                    ) {
                      this.deleteLocation();
                    }
                  }}
                />
                <input type="button" value="Update" onClick={this.handlePut} />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  }
}

export default EditLocation;
