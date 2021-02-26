import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import TherapistSchedule from "./TherapistSchedule";
import RoomSchedule from "./RoomSchedule";
import AllTherapistSchedule from "./AllTherapistSchedule";
import AddAppointment from "./UserPages/AddAppointment";

class Dashboard extends React.Component {
    constructor(props) {
        super(props);
        this.schedule = (<TherapistSchedule />);
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
            <button class="topbtn">Print Schedule</button>
            {this.schedule}
        </div>
        <AddAppointment />
    </div>
    );
  }
}

function showAddAppointment()   {
    document.getElementById("addAppointment").style.display = "block";
}

export default Dashboard;