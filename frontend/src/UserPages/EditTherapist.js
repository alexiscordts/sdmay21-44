import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";

class EditTherapist extends React.Component {

    
  render() {
    return (
      <div >
        <Nav/>
        <div class="formScreen">
          <div class="form-style">
            <div class="form-style-heading"> Edit Therapist Information</div>
              <form action="" method="post">
                <label for="fname"><span>First Name <span class="required">*</span></span><input type="text" class="input-field" name="fname" defaultValue={sessionStorage.getItem("fname")} /></label>
                <label for="lname"><span>Last Name <span class="required">*</span></span><input type="text" class="input-field" name="lname" defaultValue={sessionStorage.getItem("lname")} /></label>
                <label for="email"><span>E-mail <span class="required">*</span></span><input type="text" class="input-field" name="email" defaultValue={sessionStorage.getItem("email")} /></label>
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
                  <div class="buttonContainer">
                  <input type="button" value="Delete"/>
                  <input type="submit" value="Save" />
                </div>
              </form>
            </div>
          </div>
        </div>
      );
    }
  }
export default EditTherapist;