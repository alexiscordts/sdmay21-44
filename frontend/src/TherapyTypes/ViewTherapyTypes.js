import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "../UserPages/UserStyles.css";
import "../Settings.css";
import axios from "axios";

class ViewTherapyTypes extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      therapyList: [],
    };
  }

  componentDidMount() {

    const url = "http://10.29.163.20:8081/api/therapy";
    axios.get(url)
      // .then((json = {}) => json.data)
      .then((response) => {
         console.log('response', response);
        const therapyList = response.data;
        this.setState({ therapyList });
      });
  }

  render() {
    var rows = [];

    var typesWithSubtypes = new Map();
    
    this.state.therapyList.forEach((object) => {
      if(typesWithSubtypes.has(object.type)){
        var subs = typesWithSubtypes.get(object.type);
        subs.push(object.adl)
        typesWithSubtypes.set(object.type, subs)
      } else {
        typesWithSubtypes.set(object.type, [object.adl]);
      }
    })

    typesWithSubtypes.forEach(
      function (value, key) {
        const valuesToDisplay = [];
        value.forEach((val) => {
          valuesToDisplay.push(val + ', ');
        })
        rows.push(
          <tr>
            <td>{key}</td>
            <td>{valuesToDisplay}</td>
            <td>
              <button
                class="iconButton"
                onClick={() => {
                  sessionStorage.setItem("name", key);
                  sessionStorage.setItem("subtypes", value.toString());
                  window.location.href = "/edit_therapy_types";
                }}
              >
                <img
                  src={require("../Icons/icons8-edit-64.png")}
                  alt="edit"
                  className="icon"
                />
              </button>
            </td>
          </tr>
        );
      }.bind(this)
    );

    return (
      <div>
        <Nav />
        <div class="userHeaderRow">
          <h2>Therapy Types</h2>
          <button
            class="iconAddUserButton"
            onClick={() => {
              window.location.href = "/add_therapy_types";
            }}
          >
            <img
              src={require("../Icons/icons8-plus-48.png")}
              alt="add"
              className="iconAddLocation"
            />
          </button>
        </div>
        <table class="user-table">
          <thead>
            <tr>
              <th>Name</th>
              <th>Subtype</th>
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
