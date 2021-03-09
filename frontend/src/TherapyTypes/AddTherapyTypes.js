import React, {useState} from "react";
import "../FormStyles.css";
import Nav from "../Nav";

const AddTherapyTypes = () => {
  const [name, setName] = useState('');
  const [address, setSubtypes] = useState('');

  return (
    <div >
      <Nav/>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add a Therapy Type </div>
            <form action="" method="post">
              <label for="name">
                <span>
                  Name
                  <span class="required">*</span>
                </span>
                <input type="text" class="input-field" name="name" value={name} onClick={(e)=>setName(e.target.value)}/>
              </label>
              <label for="name">
                <span>
                  Subtypes
                  <span class="required">*</span>
                </span>
                <input type="text" class="input-field" name="subtype" value={address} onClick={(e)=>setSubtypes(e.target.value)}/>
              </label>
              <div class="submitLabel"><input type="submit" value="Create" /></div>
            </form>
          </div>
      </div>
    </div>
  );
};
export default AddTherapyTypes;