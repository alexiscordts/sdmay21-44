import React from "react";
import "./FormStyles.css";
import Nav from "./Nav";

class AddPatient extends React.Component {
    
      render() {
        return (
          <div >
            <Nav/>
            <div class="formScreen">
              <div class="form-style">
                <div class="form-style-heading"> Add a Patient </div>
                  <form action="" method="post">
                    <label for="fname"><span>First Name <span class="required">*</span></span><input type="text" class="input-field" name="fname" value="" /></label>
                    <label for="lname"><span>Last Name <span class="required">*</span></span><input type="text" class="input-field" name="lname" value="" /></label>
                    <label for="admissionDate"><span>Admission Date <span class="required">*</span></span><input type="date" class="input-field" name="admissionDate"  /></label>
                    <div class="submitLabel"><input type="submit" value="Create" /></div>
                  </form>
                </div>
            </div>
          </div>
        );
      }
    }
    
export default AddPatient;