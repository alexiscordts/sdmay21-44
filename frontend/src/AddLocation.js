import React, { useState } from "react";
import "./FormStyles.css";
import Nav from "./Nav";
import axios from "axios";

const AddLocation = () => {
  const [name, setName] = useState("");
  const [address, setAddress] = useState("");

  return (
    <div>
      {/* <Nav /> */}
      {/* <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add a Location </div>
          <form>
            <label for="name">
              <span>
                Name
                <span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                name="name"
                value={name}
                onClick={(e) => setName(e.target.value)}
              />
            </label>
            <label for="name">
              <span>
                Address
                <span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                name="address"
                value={address}
                onClick={(e) => setAddress(e.target.value)}
              />
            </label>
            <div class="submitLabel">
              <input
                type="submit"
                value="Create"
                onClick={() => {
                  console.log("submitted");
                  const url = " https://localhost:44348/api/location/";
                  axios.post(" https://localhost:44348/api/location/", {
                    locationID: 4,
                    name: "Other",
                    appointment: [],
                    patient: [],
                    room: [],
                  });
                }}
              />
            </div>
          </form> */}
      {/* </div> */}
      {/* </div> */}
      <button
        onClick={() => {
          console.log("submitted");
          const url = "https://localhost:44348/api/location/";
          axios
            .post(url, {
              locationID: "5",
              name: "blah",
              appointment: "[]",
              patient: "[]",
              room: "[]",
            })
            .then(function (response) {
              console.log(response);
            });
        }}
      >
        Click
      </button>
    </div>
  );
};
export default AddLocation;
