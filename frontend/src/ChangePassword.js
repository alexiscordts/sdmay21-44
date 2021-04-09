import React, { useState } from "react";
import "./FormStyles.css";
import Nav from "./Nav";

const ChangePassword = () => {
  const [newPassword, setNewPassword] = useState("");
  const [existingPassword, setExistingPassword] = useState("");
  const [newPasswordRetype, setNewPasswordRetype] = useState("");

  return (
    <div>
      <Nav />
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Change Your Password </div>
          <form action="" method="post">
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
            <div class="changePasswordSubmitButtonContainer">
              <input type="submit" value="Create" />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};
export default ChangePassword;
