import React from "react";
import "./FormStyles.css";
import Nav from "./Nav";
import axios from "axios";

function deleteLocation() {
  const url =
    "http://10.29.163.20:8081/api/location/" +
    sessionStorage.getItem("locationId");
  axios.delete(url);
  setTimeout(function () {
    window.location.href = "/manage_locations";
  }, 2000);
}

const EditLocation = () => {
  return (
    <div>
      <Nav />
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Edit Location</div>
          <form action="" method="post">
            <label for="fname">
              <span>
                Name<span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                name="name"
                defaultValue={sessionStorage.getItem("name")}
              />
            </label>
            <label for="lname">
              <span>
                Address<span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                name="address"
                defaultValue={sessionStorage.getItem("address")}
              />
            </label>
            <div class="buttonContainer">
              <input
                type="button"
                value="Delete"
                onClick={() => {
                  if (
                    window.confirm(
                      "Are you sure you want to delete this location?"
                    )
                  ) {
                    deleteLocation();
                  }
                }}
              />
              <input type="submit" value="Save" />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};
export default EditLocation;
