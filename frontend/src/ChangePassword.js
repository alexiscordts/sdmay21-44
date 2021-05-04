import React, { useState } from "react";
import "./FormStyles.css";
import Nav from "./Nav";
import axios from "axios";

const ChangePassword = () => {
  const [newPassword, setNewPassword] = useState("");
  const [existingPassword, setExistingPassword] = useState("");
  const [newPasswordRetype, setNewPasswordRetype] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const changePassword = () => {
    if (!newPassword || !newPasswordRetype || !existingPassword) {
      setErrorMessage("All fields must be filled out");
      return;
    }

    if (newPassword !== newPasswordRetype) {
      setErrorMessage("Passwords do not match");
      return;
    }

    const url = process.env.REACT_APP_SERVER_URL + "user/getUserByUsername/";
    axios
      .get(url + sessionStorage.getItem('username'))
      .then((response) => {
          response.data.password = existingPassword;
          axios //see if password matches
            .post(process.env.REACT_APP_SERVER_URL + "user/login/", response.data)
            .then((response2) => {
              
              if (CheckPassword(newPassword))
              {
                  response.data.password = newPassword;
                  axios.put(process.env.REACT_APP_SERVER_URL + "user/" + response.data.userId, response.data)
                  .then((response) => {
                    setErrorMessage( "Success! Password changed");
                  })
                  .catch((error) => {
                    setErrorMessage( "Error: something went wrong");
                    console.log("Error caught");
                    console.log(error);
                  });
              }
              else
              return;

              
            })
            .catch((error) => {
              console.log("Error caught");
              console.log(error);
              setErrorMessage(
                "The existing password is not associated with this account"
              );
              return;
            });
        })
      .catch((error) => {
        console.log("Error caught");
        console.log(error);
      });

    //todo call api to update password
  };

  function CheckPassword(pw) 
  { 
    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if(!pw.match(lowerCaseLetters)) {
      setErrorMessage("New password must contain lower case letter");
      return false;
    }

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if(!pw.match(upperCaseLetters)) {
      setErrorMessage("New password must contain upper case letter");
      return false;
    }

    // Validate numbers
    var numbers = /[0-9]/g;
    if(!pw.match(numbers)) {
      setErrorMessage("New password must contain number");
      return false;
    }

    // Validate length
    if(!(pw.length >= 8)) {
      setErrorMessage("New password must have length of at least 8");
      return false;
    }
    return true;
  } 

  return (
    <div>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Change Your Password </div>
          <form>
            <label className="labelChangePassword" for="existingPassword">
              <span>
                Existing Password
                <span class="required">*</span>
              </span>
              <input
                type="password"
                class="input-field"
                onChange={(e) => setExistingPassword(e.target.value)}
                name="existingPassword"
                value={existingPassword}
              />
            </label>
            <label className="labelChangePassword" for="newPassword">
              <span>
                New Password
                <span class="required">*</span>
              </span>
              <input
                type="password"
                class="input-field"
                onChange={(e) => setNewPassword(e.target.value)}
                name="newPassword"
                value={newPassword}
              />
            </label>
            <label className="labelChangePassword" for="newPasswordRetype">
              <span>
                Retype New Password
                <span class="required">*</span>
              </span>
              <input
                type="password"
                class="input-field"
                onChange={(e) => setNewPasswordRetype(e.target.value)}
                name="newPasswordRetype"
                value={newPasswordRetype}
              />
            </label>
            <span style={{ color: "red" }}>{errorMessage}</span>
            <div class="changePasswordSubmitButtonContainer">
              <input
                onClick={() => changePassword()}
                type="button"
                value="Create"
              />
            </div>
          </form>
          {/* </div>
          </form> */}
        </div>
      </div>
    </div>
  );
};
export default ChangePassword;
