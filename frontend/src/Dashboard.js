import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import TherapistSchedule from "./ScheduleViews/TherapistSchedule";
import RoomSchedule from "./ScheduleViews/RoomSchedule";
import AllTherapistSchedule from "./ScheduleViews/AllTherapistSchedule";
import PatientSchedule from "./ScheduleViews/PatientSchedule";
import AddAppointment from "./AddAppointment";
import EditAppointment from "./EditAppointment"
import ReactToPrint from "react-to-print";

class Dashboard extends React.Component {
    constructor(props) {
        super(props);
        this.schedule = (<TherapistSchedule />);
        this.date = new Date();
        this.scheduleHeader = "My Schedule - Week of ";
    }
    

  render() {

    return (
    <div id="screen">
        <Nav />
        <div class="pageContainer">
            <div class="dropdown">
                <button class="dropbtn">
                    View Schedule
                    <i class="arrow down"></i>
                </button>
                <div class="dropdown-content">
                    <div class="link" onClick={() => {this.schedule = (<TherapistSchedule />); this.scheduleHeader = "My Schedule - Week of "; this.forceUpdate();}}>My schedule</div>
                    <div class="link" onClick={() => {this.schedule = (<AllTherapistSchedule />); this.scheduleHeader = "Therapist Schedule - "; console.log("hi"); this.forceUpdate();}}>By therapist</div>
                    <div class="byRoom link">By room
                        <div class="locations">
                            <div class="link" onClick={() => {this.schedule = (<RoomSchedule />); this.scheduleHeader = "Room Schedule - Location 1 - "; this.forceUpdate();}}> Location 1</div>
                            <div class="link" onClick={() => {this.schedule = (<RoomSchedule />); this.scheduleHeader = "Room Schedule - Location 2 - "; this.forceUpdate();}}> Location 2</div>
                        </div>
                    </div>
                        
                    <div class="byPatient link">By patient
                    <div class="locations">
                            <div class="link" onClick={() => {this.schedule = (<AllTherapistSchedule />); this.scheduleHeader = "Patient Schedule - Location 1 - "; this.forceUpdate();}}> Location 1</div>
                            <div class="link" onClick={() => {this.schedule = (<AllTherapistSchedule />); this.scheduleHeader = "Patient Schedule - Location 2 - "; this.forceUpdate();}}> Location 2</div>
                        </div>
                    </div>
                    
                </div>
            </div>
            <div id="scheduleTitle">
                {this.scheduleHeader}
                <select class="selectWeek" name="therapist">             
                  <option value="sponge" selected>3/14/2021</option>
                  <option value="squid">3/21/2021</option>
                  <option value="pat">3/28/2021</option>
              </select>
            </div>
            <button style={{marginRight: "30px"}} class="topbtn" onClick={() => showAddAppointment()}>
                <img src={require("./Icons/icons8-add-property-48.png")} alt="edit" height="30" />
            </button>

            <ReactToPrint trigger={() => <button class="topbtn"><img src={require("./Icons/icons8-print-48.png")} alt="edit" height="30" /></button>} 
            onBeforeGetContent={() => showTimes()}
            onAfterPrint={() => hideTimes()}
            documentTitle={this.scheduleHeader}
            content={() => this.schedule} />
            <div ref={(el) => (this.schedule = el)}>{this.schedule}</div>
        </div>
        <AddAppointment />
        <EditAppointment />
    </div>
    );
  }
}

function showAddAppointment()   {
    document.getElementById("addAppointment").style.display = "block";
    document.getElementById("editAppointment").style.display = "none";
}

function showTimes()   {
    setSizes();
    hideMetrics();
    var x = document.getElementsByClassName("printHours");
    for(var i=0; i< x.length;i++){
        x[i].style.width = "75px";
     }
}

function hideMetrics()  {
    if (document.getElementById("metricCheck") != null)
        document.getElementById("metricCheck").checked = false;
    var cols = document.getElementsByClassName("therapistMetrics");
    for (var i = 0; i < cols.length; i++)
    {
        cols[i].style.display = "none";
    }
}

function setSizes() {
    var cols = document.getElementsByClassName("room");
    for (var i = 0; i < cols.length; i++)
    {
            cols[i].style.minWidth = "100px";
            cols[i].style.width = "100px";
    }
    var cols = document.getElementsByClassName("therapist");
    for (var i = 0; i < cols.length; i++)
    {
            cols[i].style.minWidth = "100px";
            cols[i].style.width = "100px";
    }
}

function hideTimes()   {
    var x = document.getElementsByClassName("printHours");
    for(var i=0; i< x.length;i++){
        x[i].style.width = "0px";
     }
}

export default Dashboard;