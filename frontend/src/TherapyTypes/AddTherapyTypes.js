import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

class AddTherapyTypes extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      name: "",
      subtypes: [],
    };
    this.handleChange = this.handleChange.bind(this);
    this.handlePost = this.handlePost.bind(this);
  }

  handlePost(event) {
    event.preventDefault();
    const url = "http://10.29.163.20:8081/api/therapy/"; 
    axios
      .post(url, this.state)
      .then(function (response) {
        console.log(response);
      })
      .catch(function (error) {
        console.log(error);
      });
    setTimeout(function () {
      window.location.href = "/view_therapy_types";
    }, 2000);
  }

  handleChange(event) {
    this.setState({
    [event.target.name]: event.target.value,
    });
  }

  render() {

  return (
    <div >
      <Nav/>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add a Therapy Type </div>
            <form onSubmit={this.handlePost}>
              <label for="name">
                <span>
                  Name
                  <span class="required">*</span>
                </span>
                <input type="text" class="input-field" name="name" onChange={this.handleChange}/>
              </label>
              <label for="name">
                <span>
                  Subtypes
                  <span class="required">*</span>
                </span>
                <input type="text" class="input-field" name="subtypes" onChange={this.handleChange}/>
                <p class="submitLabel">(Comma seperated values)</p>
              </label>
              <div class="submitLabel"><input type="submit" value="Create" /></div>
            </form>
          </div>
      </div>
    </div>
  );
};
};
export default AddTherapyTypes;