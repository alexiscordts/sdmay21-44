import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

const AddTherapyTypes = () => {
  
  const [name, setName] = useState('');
  const [subtypes, setSubtypes] = useState('');

  const handleAdd = ()  => {

    const url = process.env.REACT_APP_SERVER_URL + "therapymain/"; 
      const abbreviation = name.split(/\s/).reduce((response,word)=> response+=word.slice(0,1),'');
      const type = {
          type: name,
          abbreviation: name,
          active: true
      }
      console.log(type);
      axios
        .post(url, type)
        .then((response) => {
          console.log(response);
          addADLs(subtypes);
        })
        .catch((error) => {
          console.log(error.response)
        });
  }

  function addADLs(subtypes)  {
    const adls = subtypes.split(',');
    console.log(adls.length);
    adls.forEach((adl) => {
      const url = process.env.REACT_APP_SERVER_URL + "therapy/"; 
      const abbreviation = adl.split(/\s/).reduce((response,word)=> response+=word.slice(0,1),'');
      const type = {
          adl,
          type: name,
          abbreviation: adl
      }
      console.log(type);
      axios
        .post(url, type)
        .then((response) => {
          console.log(response);
        })
        .catch((error) => {
          console.log(error.response)
        });
    });
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