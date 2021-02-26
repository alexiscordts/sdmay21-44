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
        this.printSchedule = (<RoomSchedule />);
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
                <a href="#" onClick={() => {
                    this.schedule = (<TherapistSchedule />);}}>My schedule</a>
                    <a href="#" onClick={() => {this.schedule = (<AllTherapistSchedule />);}}>By therapist</a>
                    <a href="#" onClick={() => {this.schedule = (<RoomSchedule />);}}>By room</a>
                    
                </div>
            </div>
            <button class="topbtn" onClick={() => showAddAppointment()}>Add Appointment</button>
            <ReactToPrint trigger={() => <button class="topbtn">Print Schedule</button>} 
            onBeforeGetContent={() => showTimes()}
            onAfterPrint={() => hideTimes()}
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