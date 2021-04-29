import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

const AddTherapyTypes = () => {
  
  const [name, setName] = useState('');
  const [subtypes, setSubtypes] = useState('');

  const handleAdd = ()  => {
    subtypes.split(',').forEach((adl) => {
      const url = "http://10.29.163.20:8081/api/therapy/"; 
      const abbreviation = adl.split(/\s/).reduce((response,word)=> response+=word.slice(0,1),'');
      axios
        .post(url, {
          adl,
          type: name,
          abbreviation
        })
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error.response)
        });
    })
    setTimeout(function () {
     window.location.href = "/view_therapy_types";
    }, 2000);
  }

  return (
    <div >
      <Nav/>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add a Therapy Type </div>
            <form>
              <label for="name">
                <span>
                  Name
                  <span class="required">*</span>
                </span>
                <input type="text" class="input-field" name="name" onChange={(e) => setName(e.target.value)}/>
              </label>
              <label for="name">
                <span>
                  Subtype
                  <span class="required">*</span>
                </span>
                <input type="text" class="input-field" name="subtypes" onChange={(e) => setSubtypes(e.target.value)}/>
              </label>
              <div class="submitLabel"><input type="button" value="Create" onClick={() => handleAdd()} /></div>
            </form>
          </div>
      </div>
    </div>
  );
};

export default AddTherapyTypes;