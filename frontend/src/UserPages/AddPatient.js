import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";

const AddPatient = () => {
  const [fName, setFName] = useState("");
  const [lName, setLName] = useState("");
  const [date, setDate] = useState(undefined);
  return (
    <div>
      <Nav />
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add a Patient </div>
          <form action="" method="post">
            <label for="fname">
              <span>
                First Name
                <span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                onChange={(e) => setFName(e.target.value)}
                name="fname"
                value={fName}
              />
            </label>
            <label for="lname">
              <span>
                Last Name
                <span class="required">*</span>
              </span>
              <input
                type="text"
                class="input-field"
                onChange={(e) => setLName(e.target.value)}
                name="lname"
                value={lName}
              />
            </label>
            <label for="admissionDate">
              <span>
                Admission Date
                <span class="required">*</span>
              </span>
              <input
                type="date"
                class="input-field"
                name="admissionDate"
                value={date}
                onChange={(e) => setDate(e.target.value)}
              />
            </label>
            <div class="submitLabel">
              <input type="submit" value="Create" />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default AddPatient;
