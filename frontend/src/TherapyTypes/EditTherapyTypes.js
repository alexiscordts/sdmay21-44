
import React from "react";
import "../FormStyles.css";
import Nav from "../Nav";

const SubtypeInputs = ({subtypeName}) => {
  console.log('here wiht ', subtypeName)
  return (
  <input
    type="text"
    className="inputFieldSubtype"
    name={`subtype${subtypeName}`}
    defaultValue={subtypeName}
  />
);};

const EditTherapyTypes = () => {
  const subtypes = sessionStorage.getItem("subtypes").split(',');

    console.log('subtypes', subtypes);
  
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