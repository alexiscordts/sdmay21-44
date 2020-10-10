import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";

class Dashboard extends React.Component {
  constructor(props) {
    super(props);
    this.lines = {values: loadLines()};
    this.hours = {values: loadHours()};
    var d = new Date();
    this.time = {value: loadTimeLine()};
    this.days = {values: getDays()};
  } 

  render() {
    return (
    <div id="screen">
        <Nav />
        <div id="schedule">
            <div id="days">
                <div id="topRow"></div>
                {this.days.values}
                
            </div>
            <div id="scheduleContainer">
                <div id="hourColumn">
                    {this.hours.values}
                </div>
                <div class="day">                   
                    {this.lines.values}
                    <div class="appointment" id="appt1">
                        <div class="name">Patient Name / Appt name</div>
                        <div class="time">6 AM - 7 AM</div>
                        <div class="time">Room 123</div>
                        <div class="notes">Notes: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec vehicula volutpat mauris, eu luctus turpis viverra a. Class aptent taciti.</div>
                    </div>
                    {this.time.value}
                </div>
                <div class="day">
                    {this.lines.values}
                </div>
                <div class="day">
                    {this.lines.values}
                </div>
                <div class="day">
                    {this.lines.values}
                </div>
                <div class="day">
                    {this.lines.values}
                </div>
            </div>
        </div>           
    </div>
    );
  }
}

function loadLines()
{
    const items = [];
    for (var i = 0; i < 30; i++)
    {
        items.push(
            <div class="halfHour"></div>
        );
    }
    return items;
}

function loadHours()
{
    const hours = [];
    var AMPM = "AM"
    for (var i = 5; i < 20; i++)
    {
        var time = i;
        if (i == 12)
            AMPM = "PM"
        else if (i > 12)
            time = i - 12;
        hours.push(
            <div class="hour">{time} {AMPM}</div>
        );
    }
    return hours;
}

function loadTimeLine()  
{
    //console.log("Clicked");
    var d = new Date();
    var hour = d.getHours() - 5;
    var minute = d.getMinutes();
    var position = hour * 82 + minute * 82/60;
    console.log(position);
    const timeStyle = {
        top: position,
      };
    if (position > 0 && position < 1230)
        return <div id="timeLine" style={timeStyle}></div>;
    else
      return
}

function getDays()
{
    var d = new Date();
    while (d.getDay() != 1) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
    const dayElements = [];
    for (var i = 0; i < 5; i++)
    {
        dayElements.push(
            <div class="dayLabel"> 
                <div class="label">
                    {days[i]}<br />
                    {d.getMonth() + 1}/{d.getDate()}
                </div>
            </div>
            );
        d.setDate(d.getDate() + 1);
    }
    return dayElements;
}

export default Dashboard;