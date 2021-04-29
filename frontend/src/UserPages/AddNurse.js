import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

const AddNurse = () => {
  const [firstName, setFName] = useState("");
  const [lastName, setLName] = useState("");
  const [password, setPassword] = useState("");
  const [username, setUsername] = useState("");
  // const [userId, setUserId] = useState("");

  function submitNurse(e) {
    //need to add the nurse to the user list
    e.preventDefault();
    const active = 1;
    const nurse = { firstName, lastName, username, password, active };
    console.log(nurse);
    const getUserUrl =
      "http://10.29.163.20:8081/api/user/getUserByUsername/" + username;
    const url = "http://10.29.163.20:8081/api/user";
    axios.post(url, nurse).catch(function (error) {
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
    });

    setTimeout(function () {
      axios.get(getUserUrl).then((response) => {
        // console.log(response.data.userId);

        // setUserId(response.data.userId);
        //Now add nurse permission to backend...
        // console.log(userId);
        var userId = parseInt(response.data.userId);
        console.log(userId);
        setTimeout(postPermission(userId), 2000);
      });
    }, 2000);

    setTimeout(function () {
      window.location.href = "/view_nurse";
    }, 3000);
  }

  function postPermission(userId) {
    const permUrl = "http://10.29.163.20:8081/api/permission/";
    const role = "nurse";
    const permission = { userId, role };
    axios.post(permUrl, permission);
  }

  //Form for adding a nurse
  return (
    <div>
      <Nav />
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add a Nurse </div>
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
              By clicking the button, an email will be sent to the Nurse with
              their login information.
            </p>
            <div class="submitLabel">
              <input type="submit" value="Create" onClick={submitNurse} />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default AddNurse;
