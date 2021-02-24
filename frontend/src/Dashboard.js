import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import TherapistSchedule from "./TherapistSchedule";
import RoomSchedule from "./RoomSchedule";

class Dashboard extends React.Component {
    constructor(props) {
        super(props);
        this.schedule = (<TherapistSchedule />);
    }
    //

  render() {

    return (
    <div id="screen" onResize>
        <Nav />
        <div class="pageContainer">
            <div class="dropdown">
                <button class="dropbtn">
                    View Schedule
                    <i class="arrow down"></i>
                </button>
                <div class="dropdown-content">
                    <a href="#">View all </a>
                    <a href="#" onClick={() => {
            this.schedule = (<RoomSchedule />);
            } 
        }>View by room</a>
                    <a href="#" onClick={() => {
            this.schedule = (<TherapistSchedule />);
            } 
        }>View by therapist</a>
                </div>
                </div>
            <div class="scheduleTitle">{'Inpatient Therapy Scheduler'}</div>
            {this.schedule}
        </div>         
    </div>
    );
  }
}

export default Dashboard;