import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import Schedule from "./Schedule";

class Dashboard extends React.Component {
  
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
                    <a href="#">View by room</a>
                    <a href="#">View by therapist</a>
                </div>
                </div>
            <div class="scheduleTitle">{'Inpatient Therapy Scheduler'}</div>
            <Schedule/>
        </div>         
    </div>
    );
  }
}

export default Dashboard;