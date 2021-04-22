import React, { useState } from "react";
import "../FormStyles.css";
import Nav from "../Nav";

const AddRoom = () => {
  const [number, setNumber] = useState("");

  return (
    <div>
      <div class="formScreen">
        <div class="form-style">
          <div class="form-style-heading"> Add a Room </div>
          <form action="" method="post">
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
                onClick={(e) => setNumber(e.target.value)}
              />
            </label>
            <div class="submitLabel">
              <input type="submit" value="Add" />
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};
export default AddRoom;
