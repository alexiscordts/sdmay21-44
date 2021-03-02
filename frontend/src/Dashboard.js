import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import TherapistSchedule from "./TherapistSchedule";
import RoomSchedule from "./RoomSchedule";
import AllTherapistSchedule from "./AllTherapistSchedule";
import AddAppointment from "./UserPages/AddAppointment";
import ReactToPrint from "react-to-print";

class Dashboard extends React.Component {
    constructor(props) {
        super(props);
        this.schedule = (<TherapistSchedule />);
        this.date = new Date();
        this.scheduleHeader = "My Schedule - Week of " + this.date.toLocaleDateString();
    }
    //

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
                <a href="#" onClick={() => {this.schedule = (<TherapistSchedule />); this.scheduleHeader = "My Schedule - Week of " + this.date.toLocaleDateString();}}>My schedule</a>
                <a href="#" onClick={() => {this.schedule = (<AllTherapistSchedule />); this.scheduleHeader = "Therapist Schedule - " + this.date.toLocaleDateString();}}>By therapist</a>
                <a href="#" onClick={() => {this.schedule = (<RoomSchedule />); this.scheduleHeader = "Room Schedule - " + this.date.toLocaleDateString();}}>By room</a>
                    
                </div>
            </div>
            <div id="scheduleTitle">{this.scheduleHeader}</div>
            <button class="topbtn" onClick={() => showAddAppointment()}>Add Appointment</button>
            <ReactToPrint trigger={() => <button class="topbtn">Print Schedule</button>} 
            onBeforeGetContent={() => showTimes()}
            onAfterPrint={() => hideTimes()}
            documentTitle={this.scheduleHeader}
            content={() => this.schedule} />
            <div ref={(el) => (this.schedule = el)}>{this.schedule}</div>
        </div>
        <AddAppointment />
    </div>
    );
  }
}

function showAddAppointment()   {
    document.getElementById("addAppointment").style.display = "block";
}

function showTimes()   {
    var x = document.getElementsByClassName("printHours");
    for(var i=0; i< x.length;i++){
        x[i].style.width = "75px";
     }
}

function hideTimes()   {
    var x = document.getElementsByClassName("printHours");
    for(var i=0; i< x.length;i++){
        x[i].style.width = "0px";
     }
}

export default Dashboard;