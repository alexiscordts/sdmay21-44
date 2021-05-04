import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";
import axios from "axios";

const AddRoom = () => {
  const [number, setNumber] = useState("");
  const locationId = sessionStorage.getItem("locationId");
  console.log(locationId);
  return (
    <div>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add a Room </div>
          <form>
            <label for="name">
              <span>
                Number
                <span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                name="number"
                value={number}
                onChange={(e) => setNumber(e.target.value)}
              />
            </label>
            <div class="submitLabel">
            </div>
          </form>
          <button onClick={() => {
                console.log(number + " " + locationId);
                  const r = {
                  number: number,
                  locationId: locationId,
                  active: true
                  }
                  const url =
                    process.env.REACT_APP_SERVER_URL + "room/";
                  axios.post(url, r)
                  .then(function (response) {
                    console.log(response);
                    window.location.href = "manage_rooms";
                  })
                  .catch(function (error) {
                    console.log(error);
                    window.location.href = "manage_rooms";
                  });
                }}>Add</button>
        </div>
      </div>
    </div>
  );
};
export default AddRoom;
