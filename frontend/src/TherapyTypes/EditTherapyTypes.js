
import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";

const EditTherapyTypes = () => {

  return (
    <div >
      <Nav/>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Edit Therapy Types</div>
            <form action="" method="post">
              <label for="name"><span>Name<span class="required">*</span></span><input type="text" class="input-field" name="name" defaultValue={sessionStorage.getItem("name")} /></label>
              <label for="subtypes"><span>Subtypes<span class="required">*</span></span><input type="text" class="input-field" name="subtypes" defaultValue={sessionStorage.getItem("su types")} /></label>
              <div class="buttonContainer">
                <input type="button" value="Delete"/>
                <input type="submit" value="Save" />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  };
export default EditTherapyTypes;