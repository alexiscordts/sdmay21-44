
import React, {useState} from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

const EditTherapyTypes = () => {
  const subtypes = sessionStorage.getItem("subtypes").split(',');
  const [newAdl, setNewAdl] = useState('');

  const deleteAdl = async (adl) => {
    const url = process.env.REACT_APP_SERVER_URL + "therapy/"+adl; 
  
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

  const updateAdl = async(subtypeName, adl) => {
    const url = process.env.REACT_APP_SERVER_URL + "therapy/"+subtypeName; 
  
     await axios
      .delete(url)
      .then(function (response) {
        console.log(response);
      })
      .catch(function (error) {
        console.log(error);
      });

      addAdl(adl);
  }
  
  const SubtypeInputs = ({subtypeName}) => {
    const [adl, setAdl] = useState(subtypeName);
    return (
    <div style={{
      display: "flex",
      flexDirection: "row",
      marginBottom: "5px"}}>
      <input
        type="text"
        className="inputFieldSubtype"
        name={`subtype${subtypeName}`}
        defaultValue={adl}
        style={{width: "350px"}}
        onChange={(e)=> {
          setAdl(e.target.value)
        }}
      />
      <input type="button" value="Delete" style={{marginLeft: "10px", marginRight: "5px"}} onClick={() => deleteAdl(subtypeName)}/>
      <input type="button" value="Save" onClick={()=> updateAdl(subtypeName, adl)}/>
    </div>
  )};

  const addAdl = async(adl) => {
    const url = process.env.REACT_APP_SERVER_URL + "therapy/"; 
    await axios
    .post(url, {
      adl,
      type: sessionStorage.getItem("name"),
      abbreviation: adl.split(/\s/).reduce((response,word)=> response+=word.slice(0,1),'')
    })
    .then(function (response) {
      console.log(response);
    })
    .catch(function (error) {
      console.log(error);
    });
  window.location.href = "/view_therapy_types";
  }
  
  return (
    <div >
      <Nav/>
      <div className="formScreen">
        <div className="form-style">
          <div className="form-style-heading"> Edit Therapy Types</div>
            <form >
              <label for="name"><span>Name</span>{sessionStorage.getItem("name")}</label>
              <label for="subtypes">
                <span>Subtypes
                  <span className="required">*</span>
                </span>
                <div className="subtypeInputContainer">
                  {subtypes.map((subtype) => <SubtypeInputs subtypeName={subtype} />)}
                </div>
                <div style={{
                  display: "flex",
                  flexDirection: "row",
                  marginBottom: "5px"}}>
                  <input
                    type="text"
                    className="inputFieldSubtype"
                    name={`subtypeNewAdl`}
                    defaultValue={newAdl}
                    style={{width: "225px", marginLeft: "100px", marginRight: "20px"}}
                    onChange={(e)=> {
                      setNewAdl(e.target.value)
                    }}
                  />
                  <input type="button" value="Add" onClick={()=> addAdl(newAdl)}/>
                </div>
              </label>
            </form>
          </div>
        </div>
      </div>
    );
  };
export default EditTherapyTypes;