import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "../UserPages/UserStyles.css";
import "../Settings.css";
import { Link } from "react-router-dom";

class ViewTherapyTypes extends React.Component {
  constructor(props) {
    super(props);
  }

  render() {
    var therapyList = [
      { name: "Speech", subtypes: ["type1", "type2"] },
      { name: "Physical", subtypes: ["type1", "type2", "type3"] },
    ];
    var rows = [];

    therapyList.forEach(
      function (therapyType) {
        rows.push(
          <tr>
            <td>{therapyType.name}</td>
            <td>{therapyType.subtypes}</td>
            <td>
              <Link to="/edit_therapy_types"><button
                class="iconButton"
                onClick={() => {
                  sessionStorage.setItem("name", therapyType.name);
                  sessionStorage.setItem("subtypes", therapyType.subtypes);
                }}
              >
                <img
                  src={require("../Icons/icons8-edit-64.png")}
                  alt="edit"
                  className="icon"
                />
              </button></Link>
            </td>
          </tr>
        );
      }.bind(this)
    );

    return (
      <div>
        <div class="userHeaderRow">
          <h2>Therapy Types</h2>
          <Link to="/add_therapy_types"><button
            class="iconAddUserButton"
          >
            <img
              src={require("../Icons/icons8-plus-48.png")}
              alt="add"
              className="iconAddLocation"
            />
          </button></Link>
        </div>
        <table class="user-table">
          <thead>
            <tr>
              <th>Name</th>
              <th>Subtypes</th>
              <th>Edit</th>
            </tr>
          </thead>
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewTherapyTypes;
