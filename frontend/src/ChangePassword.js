import React, {useState} from "react";
import "./FormStyles.css";
import Nav from "./Nav";



const ChangePassword = () => {
  const [newPassword, setNewPassword] = useState('');
  const [existingPassword, setExistingPassword] = useState('');
  const [newPasswordRetype, setNewPasswordRetype] = useState('');
  const [errorMessage, setErrorMessage] = useState('');

  //Set the validation errors if they exist, otherwise change the password
  const changePassword = () =>  {

    if(!newPassword || !newPasswordRetype || !existingPassword) {
      setErrorMessage('All fields must be filled out')
    }
  
    if(newPassword !== newPasswordRetype) {
      setErrorMessage('Passwords do not match');
    }
  
    if(existingPassword){//todo if existing password is not associated with current account
      setErrorMessage('The existing password is not associated with this account');
    }

    //todo call api to update password
  
  };
  return (
    <div >
      <Nav/>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Change Your Password </div>
            <form >
              <label className="labelChangePassword" for="existingPassword">
                    <span>
                      Existing Password
                      <span class="required">*</span>
                    </span>
                    <input 
                      type="text" 
                      class="input-field" 
                      onChange={(e)=> setExistingPassword(e.target.value)} 
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
                      type="text" 
                      class="input-field" 
                      onChange={(e)=> setNewPassword(e.target.value)} 
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
                      type="text" 
                      class="input-field" 
                      onChange={(e)=> setNewPasswordRetype(e.target.value)} 
                      name="newPasswordRetype" 
                      value={newPasswordRetype}
                    />
                  </label>
                  <span style={{color: 'red'}}>{errorMessage}</span>
                <div class="changePasswordSubmitButtonContainer"><input onClick={() => changePassword()} type="button" value="Create" /></div>
              </form>
            </div>
        </div>
      </div>
    );
  };
export default ChangePassword;