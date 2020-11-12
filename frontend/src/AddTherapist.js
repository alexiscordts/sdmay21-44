import React from "react";
import "./FormStyles.css";
import Nav from "./Nav";

class AddTherapist extends React.Component {

    
      render() {
        return (
          <div >
            <Nav/>
            <div class="formScreen">
              <div class="form-style">
                <div class="form-style-heading"> Add a Therapist </div>
                  <form action="" method="post">
                    <label for="fname"><span>First Name <span class="required">*</span></span><input type="text" class="input-field" name="fname" value="" /></label>
                    <label for="lname"><span>Last Name <span class="required">*</span></span><input type="text" class="input-field" name="lname" value="" /></label>
                    <label for="email"><span>E-mail <span class="required">*</span></span><input type="text" class="input-field" name="email" value="" /></label>
                    <label><span>Therapy Type <span class="required">*</span></span>
                      <div className="checkBoxArea">
                        <label for = "chkPT">
                          <input type = "checkbox" id = "chkPT" value = "PT"/>
                          Physical Therapy
                        </label>
                        <label for = "chkOT">
                          <input type = "checkbox" id = "chkOT" value = "OT"/>
                          Occupational Therapy
                        </label>
                        <label for = "chkST">
                          <input type = "checkbox" id = "chkST" value = "ST"/>
                          Speech Therapy
                        </label>
                      </div>
                    </label>
                    <p class="submitLabel">By clicking the button, an email will be sent to the Therapist with their login information.</p>
                    <div class="submitLabel"><input type="submit" value="Create" /></div>
                  </form>
                </div>
            </div>
          </div>
        );
      }
    }
export default AddTherapist;