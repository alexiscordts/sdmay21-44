
import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

const EditTherapyTypes = () => {
  const subtypes = sessionStorage.getItem("subtypes").split(',');

  const deleteAdl = async (adl) => {
    console.log(adl)
    const url = "http://10.29.163.20:8081/api/therapy/"+adl; 
  
     await axios
      .delete(url)
      .then(function (response) {
        console.log(response);
      })
      .catch(function (error) {
        console.log(error);
      });
      window.location.href = "/view_therapy_types";
  }
  
  const SubtypeInputs = ({subtypeName}) => (
    <div style={{
      display: "flex",
      flexDirection: "row",
      marginBottom: "5px"}}>
      <input
        type="text"
        className="inputFieldSubtype"
        name={`subtype${subtypeName}`}
        defaultValue={subtypeName}
        style={{width: "350px"}}
      />
      <input type="button" value="Delete" style={{marginLeft: "10px"}} onClick={() => deleteAdl(subtypeName)}/>
    </div>
  );
  
  return (
    <div >
      <Nav/>
      <div className="formScreen">
        <div className="form-style">
          <div className="form-style-heading"> Edit Therapy Types</div>
            <form action="" method="post">
              <label for="name"><span>Name<span className="required">*</span></span><input type="text" className="input-field" name="name" defaultValue={sessionStorage.getItem("name")} /></label>
              <label for="subtypes">
                <span>Subtypes
                  <span className="required">*</span>
                </span>
                <div className="subtypeInputContainer">
                  {subtypes.map((subtype) => <SubtypeInputs subtypeName={subtype} />)}
                </div>
              </label>
              <div className="buttonContainer">
                <input type="submit" value="Save" />
              </div>
            </form>
          </div>
        </div>
      </div>
    );
  };
export default EditTherapyTypes;