import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

const AddAdmin = () => {
  const [firstName, setFName] = useState("");
  const [lastName, setLName] = useState("");
  const [middleName, setMName] = useState("");

  const [password, setPassword] = useState("");
  const [username, setUsername] = useState("");

  function submitAdmin(e) {
    //need to add the therapist to the user list
    e.preventDefault();
    const active = 1;
    const admin = { firstName, lastName, username, password, active };
    console.log(admin);
    const getUserUrl =
      process.env.REACT_APP_SERVER_URL + "user/getUserByUsername/" + username;
    const url = process.env.REACT_APP_SERVER_URL + "user";
    axios.post(url, admin).catch(function (error) {
      if (error.response) {
        console.log(error.response.data);
        console.log(error.response.status);
        console.log(error.response.headers);
      } else if (error.request) {
        // The request was made but no response was received
        console.log(error.request);
      } else {
        // Something happened in setting up the request that triggered an Error
        console.log("Error", error.message);
      }
      alert("There was a problem creating the user");
    });

    setTimeout(function () {
      axios.get(getUserUrl).then((response) => {
        var userId = parseInt(response.data.userId);
        console.log(userId);
        setTimeout(postPermission(userId), 2000);
      });
    }, 2000);
    setTimeout(function () {
      window.location.href = "/view_admin";
    }, 4000);
  }

  function postPermission(userId) {
    const permUrl = process.env.REACT_APP_SERVER_URL + "permission/";
    const role = "admin";
    const permission = { userId, role };

    axios.post(permUrl, permission);
  }
  return (
    <div>
      <Nav />
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add an Admin </div>
          <form action="" method="post">
            <label for="firstName">
              <span>
                First Name
                <span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                onChange={(e) => setFName(e.target.value)}
                name="firstName"
                value={firstName}
              />
            </label>
            <label for="middleName">
              <span>Middle Name</span>
              <input
                type="text"
                class="input-field"
                onChange={(e) => setMName(e.target.value)}
                name="middleName"
                value={middleName}
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
                onChange={(e) => setLName(e.target.value)}
                name="lastName"
                value={lastName}
              />
            </label>
            <label>
              <span>
                Username
                <span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                onChange={(e) => setUsername(e.target.value)}
                name="username"
                value={username}
              ></input>
            </label>
            <label for="password">
              <span>
                Password
                <span class="required">*</span>
              </span>
              <input
                type="password"
                class="input-field"
                onChange={(e) => setPassword(e.target.value)}
                name="password"
                value={password}
              />
            </label>
            <p class="submitLabel">
              By clicking the button, an email will be sent to the Admin with
              their login information.
            </p>
            <div class="submitLabel">
              <input type="submit" value="Create" onClick={submitAdmin} />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};
export default AddAdmin;
