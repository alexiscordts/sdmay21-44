import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import TherapistSchedule from "./ScheduleViews/TherapistSchedule";
import RoomSchedule from "./ScheduleViews/RoomSchedule";
import AllTherapistSchedule from "./ScheduleViews/AllTherapistSchedule";
import PatientSchedule from "./ScheduleViews/PatientSchedule";
import AddAppointment from "./AddAppointment";
import EditAppointment from "./EditAppointment";
import ReactToPrint from "react-to-print";
import axios from "axios";

class Dashboard extends React.Component {
  constructor(props) {
    super(props);
    this.date = new Date();
    this.schedule = <TherapistSchedule date={this.date} />;
    this.scheduleHeader = "My Schedule";
    this.weeks = this.getDropdownDates();
    this.state = {
      weekChanged: false,
      schedule: 1,
    };
  }
  //

  getLocations() {
    //return <div class="link" onClick={() => {this.schedule = (<RoomSchedule date={this.date} />); this.scheduleHeader = "Room Schedule - Location 3 - "; this.forceUpdate();}}> Location 3</div>;
  }

  getDropdownDates() {
    let d = new Date();
    while (d.getDay() != 0) {
      //get Sunday
      d.setDate(d.getDate() - 1);
    }
    this.week = d.toLocaleDateString("en-US");
    var elements = [];
    d.setDate(d.getDate() - 7);
    d.setDate(d.getDate() - 7);
    for (let i = 0; i < 5; i++) {
      let newDate = new Date(d.getTime());
      elements.push(
        <div
          class="link"
          onClick={() => {
            this.date = newDate;
            this.schedule = this.setSchedule(newDate);
            this.week = newDate.toLocaleDateString("en-US");
            this.setState({ weekChanged: true });
            console.log(newDate.toString());
          }}
        >
          {newDate.toLocaleDateString("en-US")}
        </div>
      );
      d.setDate(d.getDate() + 7);
    }
    return elements;
  }

  setSchedule(newDate) {
    this.schedule = <div></div>;
    switch (this.state.schedule) {
      case 1:
        return <TherapistSchedule date={newDate} />;
      case 2:
        return <AllTherapistSchedule date={this.date} />;
      case 3:
        return <RoomSchedule date={this.date} />;
      case 4:
        return <PatientSchedule date={this.date} />;
    }
  }

  render() {
    return (
      <div id="screen">
        <Nav />
        <div class="pageContainer">
          <div class="dropdown">
            <button class="dropbtn">
              {this.scheduleHeader}
              <i class="arrow down"></i>
            </button>
            <div class="dropdown-content">
              <div
                class="link"
                onClick={() => {
                  this.schedule = <TherapistSchedule date={this.date} />;
                  this.scheduleHeader = "My Schedule";
                  this.locationHeader = "";
                  this.setState({ schedule: 1 });
                }}
              >
                My schedule
              </div>
              <div
                class="link"
                onClick={() => {
                  this.schedule = <AllTherapistSchedule date={this.date} />;
                  this.scheduleHeader = "Therapist Schedule";
                  this.locationHeader = "";
                  console.log("hi");
                  this.setState({ schedule: 2 });
                }}
              >
                By therapist
              </div>
              <div class="byRoom link">
                By room
                <div class="locations">
                  <div
                    class="link"
                    onClick={() => {
                      this.schedule = <RoomSchedule date={this.date} />;
                      this.scheduleHeader = "Room Schedule";
                      this.locationHeader = "Location 1 - ";
                      this.setState({ schedule: 3 });
                    }}
                  >
                    {" "}
                    Location 1
                  </div>
                  <div
                    class="link"
                    onClick={() => {
                      this.schedule = <RoomSchedule date={this.date} />;
                      this.scheduleHeader = "Room Schedule";
                      this.locationHeader = "Location 2 - ";
                      this.setState({ schedule: 3 });
                    }}
                  >
                    {" "}
                    Location 2
                  </div>
                </div>
              </div>

              <div class="byPatient link">
                By patient
                <div class="locations">
                  <div
                    class="link"
                    onClick={() => {
                      this.schedule = <PatientSchedule date={this.date} />;
                      this.scheduleHeader = "Patient Schedule";
                      this.locationHeader = "Location 1 - ";
                      this.setState({ schedule: 4 });
                    }}
                  >
                    {" "}
                    Location 1
                  </div>
                  <div
                    class="link"
                    onClick={() => {
                      this.schedule = <PatientSchedule date={this.date} />;
                      this.scheduleHeader = "Patient Schedule";
                      this.locationHeader = "Location 2 - ";
                      this.setState({ schedule: 4 });
                    }}
                  >
                    {" "}
                    Location 2
                  </div>
                  {this.getLocations()}
                </div>
              </div>
            </div>
          </div>
          <div id="scheduleTitle">
            {this.locationHeader} Week of
            <div class="datedropdown">
              <button class="datedropbtn">
                {this.week}
                <i class="datearrow down"></i>
              </button>
              <div class="datedropdown-content">{this.weeks}</div>
            </div>
          </div>

          <button
            style={{ marginRight: "30px" }}
            class="topbtn"
            onClick={() => hideCopyDayForm()}
          >
            {/* temporarily changed duplicate to add property */}
            <img
              src={require("./Icons/icons8-add-property-48.png")}
              alt="edit"
              height="30"
            />
          </button>

          <button class="topbtn" onClick={() => showAddAppointment()}>
            <img
              src={require("./Icons/icons8-add-property-48.png")}
              alt="edit"
              height="30"
            />
          </button>

          <ReactToPrint
            trigger={() => (
              <button class="topbtn">
                <img
                  src={require("./Icons/icons8-print-48.png")}
                  alt="edit"
                  height="30"
                />
              </button>
            )}
            onBeforeGetContent={() => showTimes()}
            onAfterPrint={() => {
              hideTimes();
              this.schedule = this.setSchedule(this.date);
              this.forceUpdate();
            }}
            documentTitle={
              this.scheduleHeader +
              " for " +
              this.date.toLocaleDateString("en-US")
            }
            content={() => this.schedule}
          />

          <div id="copyDayForm">
            <div class="form-style">
              <form>
                <span>
                  Copy from: <span class="required">* </span>
                </span>
                <input
                  style={{ width: "160px", marginBottom: "10px" }}
                  class="input-field"
                  type="date"
                  name="date"
                ></input>

                <span>
                  Copy to: <span class="required">* </span>
                </span>
                <input
                  style={{ width: "160px", marginBottom: "10px" }}
                  class="input-field"
                  type="date"
                  name="date"
                ></input>

                <input
                  style={{ width: "100%" }}
                  type="submit"
                  value="Copy Day"
                />
              </form>
            </div>
          </div>

          <div ref={(el) => (this.schedule = el)}>{this.schedule}</div>
        </div>
        <AddAppointment />
        <EditAppointment />
      </div>
    );
  }
}

function showAddAppointment() {
  document.getElementById("addAppointment").style.display = "block";
  document.getElementById("editAppointment").style.display = "none";
}

function showTimes() {
  setSizes();
  hideMetrics();
  var x = document.getElementsByClassName("printHours");
  for (var i = 0; i < x.length; i++) {
    x[i].style.width = "75px";
  }
}

function hideMetrics() {
  if (document.getElementById("metricCheck") != null)
    document.getElementById("metricCheck").checked = false;
  var cols = document.getElementsByClassName("therapistMetrics");
  for (var i = 0; i < cols.length; i++) {
    cols[i].style.display = "none";
  }
}

function setSizes() {
  var cols = document.getElementsByClassName("room");
  for (var i = 0; i < cols.length; i++) {
    cols[i].style.minWidth = "100px";
    cols[i].style.width = "100px";
  }
  var cols = document.getElementsByClassName("therapist");
  for (var i = 0; i < cols.length; i++) {
    cols[i].style.minWidth = "100px";
    cols[i].style.width = "100px";
  }
}

function hideTimes() {
  var x = document.getElementsByClassName("printHours");
  for (var i = 0; i < x.length; i++) {
    x[i].style.width = "0px";
  }
}

var copyDayFormIsHidden = true;
function hideCopyDayForm() {
  if (copyDayFormIsHidden) {
    copyDayFormIsHidden = false;
    document.getElementById("copyDayForm").style.display = "block";
  } else {
    copyDayFormIsHidden = true;
    document.getElementById("copyDayForm").style.display = "none";
  }
}

export default Dashboard;
