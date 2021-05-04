import React from "react";
import Nav from "../Nav";
import "../TableStyles.css";
import "../UserPages/UserStyles.css";
import "../Settings.css";
import { Link } from "react-router-dom";
import axios from "axios";

class ViewTherapyTypes extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      therapyList: [],
    };
  }

  componentDidMount() {
    this.updateTherapistList();
  }

  updateTherapistList() {
    const url = "http://10.29.163.20:8081/api/therapy";
    axios.get(url).then((response) => {
      axios.get("http://10.29.163.20:8081/api/therapymain").then((response2) => {
        const adlList = response.data;
        const therapyList = response2.data;
        const therapies = [];
        adlList.forEach(adl => {
          therapyList.forEach(therapy => {
            if (adl.type == therapy.type)
              therapies.push(adl);
          })
        })
        this.setState({ therapyList: therapies });
      });
    });
  }

  deleteTherapy(key) {
    console.log(key);
    const url = "http://10.29.163.20:8081/api/therapymain/" + key;
        axios.delete(url)
        .then((response) => {
          console.log(response.data);
          this.updateTherapistList();
        })
        .catch((error) => {
            console.log("Error caught");
            console.log(error);
        });

      axios.get("http://10.29.163.20:8081/api/therapy")
      .then((response) => {
        const therapies = response.data;
        therapies.forEach(therapy => {
          if (therapy.type == key)
          {
            axios.delete("http://10.29.163.20:8081/api/therapy/" + therapy.adl)
            .then((response2) => {
              console.log(response2.data);
              this.updateTherapistList();
            })
            .catch((error) => {
                console.log("Error caught");
                console.log(error);
            });
          }
        });
      })
      .catch((error) => {
          console.log("Error caught");
          console.log(error);
      });
  }

  render() {
    var rows = [];

    var typesWithSubtypes = new Map();

    this.state.therapyList.forEach((object) => {
      if (typesWithSubtypes.has(object.type)) {
        var subs = typesWithSubtypes.get(object.type);
        subs.push(object.adl);
        typesWithSubtypes.set(object.type, subs);
      } else {
        typesWithSubtypes.set(object.type, [object.adl]);
      }
    });

    typesWithSubtypes.forEach(
      function (value, key) {
        const valuesToDisplay = [];
        value.forEach((val) => {
          valuesToDisplay.push(val + ", ");
        });
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
            <td>
              <button
                class="iconButton"
                onClick={() => {this.deleteTherapy(key)}}
              >
                <img
                  src={require("../Icons/icons8-delete-64.png")}
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
              <th>Subtype</th>
              <th>Edit</th>
              <th>Delete</th>
            </tr>
          </thead>
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
export default ViewTherapyTypes;
