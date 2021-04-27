import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

const AddTherapyTypes = () => {
  
  const [name, setName] = useState('');
  const [subtypes, setSubtypes] = useState('');

  const handleAdd = async ()  => {
    subtypes.split(',').forEach(async (adl) => {
      const url = "http://10.29.163.20:8081/api/therapy/"; 
      const abbreviation = adl.split(/\s/).reduce((response,word)=> response+=word.slice(0,1),'');
      await  axios
        .post(url, {
          adl,
          type: name,
          abbreviation
        })
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
         
        console.log('data',error.response.data);
        console.log(error.response.status);
        console.log(error.response.headers);
        });
    })
    //   window.location.href = "/view_therapy_types";
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
                  Subtypes
                  <span class="required">*</span>
                </span>
                <input type="text" class="input-field" name="subtypes" onChange={(e) => setSubtypes(e.target.value)}/>
                <p class="submitLabel">(Comma seperated values)</p>
              </label>
              <div class="submitLabel"><input type="button" value="Create" onClick={() => handleAdd()} /></div>
            </form>
          </div>
      </div>
    </div>
  );
};

export default AddTherapyTypes;