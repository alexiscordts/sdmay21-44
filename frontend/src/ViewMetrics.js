import React from "react";
import "./Dashboard.css";
import "./ViewMetrics.css";
import Nav from "./Nav";
import MyMetrics from './Metrics/MyMetrics';
import TherapistMetrics from './Metrics/TherapistMetrics';
import PatientMetrics from './Metrics/PatientMetrics';
import RoomMetrics from './Metrics/RoomMetrics';

class ViewMetrics extends React.Component {
    constructor(props) {
        super(props);
        this.metrics = (<MyMetrics />);
        this.date = new Date();
        while (this.date.getDay() != 0) //get Sunday
            {
                this.date.setDate(this.date.getDate() - 1);
            }
        this.metricHeader = "My Metrics - Week of " + this.date.toLocaleDateString();
    }
    //

  render() {

    const data = ([
        {
            "day": "Sunday",
            "percent": 25
        },
        {
            "day": "Monday",
            "percent": 67
        },
        {
            "day": "Tuesday",
            "percent": 80
        },
        {
            "day": "Wednesday",
            "percent": 73
        },
        {
          "day": "Thursday",
          "percent": 42
        },
        {
            "day": "Friday",
            "percent": 21
        },
        {
            "day": "Saturday",
            "percent": 5
        }
      ]);

    return (

    <div id="screen">
        <Nav />
        <div class="pageContainer">
            <div class="dropdown">
                <button class="dropbtn">
                    View Metrics
                    <i class="arrow down"></i>
                </button>
                <div class="dropdown-content">
                <div class="link" onClick={() => { this.metrics = (<MyMetrics />); this.metricHeader = "My Metrics - Week of " + this.date.toLocaleDateString(); this.forceUpdate(); }}>My metrics</div>
                <div class="link" onClick={() => { this.metrics = (<TherapistMetrics />); this.metricHeader = "Therapist Metrics - Week of " + this.date.toLocaleDateString(); this.forceUpdate(); }}>By therapist</div>
                <div class="byRoom link">By room
                    <div class="locations">
                        <div class="link" onClick={() => { this.metrics = (<RoomMetrics />); this.metricHeader = "Room Metrics - Location 1 - Week of " + this.date.toLocaleDateString(); this.forceUpdate(); }}> Location 1</div>
                        <div class="link" onClick={() => { this.metrics = (<RoomMetrics />); this.metricHeader = "Room Metrics - Location 2 - Week of " + this.date.toLocaleDateString(); this.forceUpdate(); }}> Location 2</div>
                    </div>
                </div>
                    
                <div class="byPatient link">By patient
                <div class="locations">
                        <div class="link" onClick={() => { this.metrics = (<PatientMetrics />); this.metricHeader = "Patient Metrics - Location 1 - Week of " + this.date.toLocaleDateString(); this.forceUpdate(); }}> Location 1</div>
                        <div class="link" onClick={() => { this.metrics = (<PatientMetrics />); this.metricHeader = "Patient Metrics - Location 2 - Week of " + this.date.toLocaleDateString(); this.forceUpdate(); }}> Location 2</div>
                    </div>
                </div>
                    
                </div>
            </div>
            <div id="scheduleTitle">{this.metricHeader}</div>       
            {this.metrics}
        </div>
    </div>
    );
  }
}

export default ViewMetrics;