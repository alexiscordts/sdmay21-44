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
    var appointments = getAppointments(d);
    var d = new Date();
    while (d.getDay() != 1) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    this.tuesday = {values: getAppointmentElements(appointments)}
  }

  updateDimensions = () => {
    if (window.innerWidth > 550)
        {
            const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
             days.forEach(day => {
                document.getElementById(day + "Toggle").style.backgroundColor = null;
                document.getElementsByClassName(day)[0].style.display = null;
                document.getElementsByClassName(day)[1].style.display = null;
             });
        }
  };
  
  load = () => {
    document.getElementById("scheduleContainer").scrollTop = getPositionForTimeLine() - 200;
    console.log("loaded");
  };

  componentDidMount() {
    window.addEventListener('resize', this.updateDimensions);
    window.addEventListener('load', this.load);
  }

  componentWillUnmount() {
    window.removeEventListener('resize', this.updateDimensions);
    window.removeEventListener('size', this.load);
  }

  render() {
    return (
    <div id="screen" onResize>
        <Nav />
        <div id="schedule">
            <div id="days">
                <div id="topRow"></div>
                {this.days.values}              
            </div>
            <div id="scheduleContainer">
                {this.time.value}
                <div id="hourColumn">
                    {this.hours.values}
                </div>
                <div class="day Monday">                   
                    {this.lines.values}
                </div>
                <div class="day Tuesday">
                    {this.lines.values}
                    {this.tuesday.values}
                </div>
                <div class="day Wednesday">
                    {this.lines.values}
                </div>
                <div class="day Thursday">
                    {this.lines.values}
                </div>
                <div class="day Friday">
                    {this.lines.values}
                </div>
            </div>
            <div id="toggle">
                <div id="MondayToggle" class="daytoggle" onClick={() => toggleDay("Monday")}>
                    M
                </div>
                <div id="TuesdayToggle" class="daytoggle" onClick={() => toggleDay("Tuesday")}>
                    T
                </div>
                <div id="WednesdayToggle" class="daytoggle" onClick={() => toggleDay("Wednesday")}>
                    W
                </div>
                <div id="ThursdayToggle" class="daytoggle" onClick={() => toggleDay("Thursday")}>
                    Th
                </div>
                <div id="FridayToggle" class="daytoggle" onClick={() => toggleDay("Friday")}>
                    F
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
    var position = getPositionForTimeLine();
    console.log(position);
    const timeStyle = {
        top: position,
      };
    if (position > 0 && position < 1230)
        return <div id="timeLine" style={timeStyle}></div>;
    else
      return
}

function getPositionForTimeLine()
{
    var d = new Date();
    var hour = d.getHours() - 5;
    var minute = d.getMinutes();
    return hour * 82 + minute * 82/60;
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
            <div className={'dayLabel ' + days[i]}> 
                    {days[i]}<br />
                    {d.getMonth() + 1}/{d.getDate()}
            </div>
            );
        d.setDate(d.getDate() + 1);
    }
    return dayElements;
}

function getAppointments(date) {
    var d = new Date();
    while (d.getDay() != 1) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    //placeholder appointments
    d.setHours(13,0,0,0);
    var appointment1 = { title: "Patient 1", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin porta sem ut ipsum dictum bibendum. Curabitur sodales interdum lorem, ac.", date: new Date(d), length: 1 };
    //d.setDate(d.getDate() + 1);
    d.setHours(8,0,0,0);
    var appointment2 = { title: "Patient 2", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque egestas, lectus in congue scelerisque.", date: new Date(d), length: 1 };
    //d.setDate(d.getDate() + 1);
    d.setHours(9,0,0,0);
    var appointment3 = { title: "Patient 3", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed mauris at nisi consequat eleifend. Sed nulla quam, vehicula at turpis a, cursus aliquet justo. Donec et erat sed mauris semper.", date: new Date(d), length: 2 };
    var appointments = [];
    appointments.push(appointment1);
    appointments.push(appointment2);
    appointments.push(appointment3);
    return appointments;
}

var numAppointments = 0;
function getAppointmentElements(appointments)   {
    var appointmentElements = []; 
    appointments.forEach(appointment => {
        var start = appointment.date.getHours();
        var end = appointment.date.getHours() + appointment.length;
        var position = (start - 5) * 82 + appointment.date.getMinutes() * 82/60;
        const style = {
            top: position,
            height: appointment.length * 82, 
            minHeight: appointment.length * 82,
        };
        var startAMOrPM = "AM";
        var endAMOrPM = "AM";
        if (start >= 12)
            startAMOrPM = "PM";
        if (start > 12)
            start -= 12;
        if (end >= 12)
            endAMOrPM = "PM";
        if (end > 12)
            end -= 12;
        var time = start + " " + startAMOrPM + " - " + end + " " + endAMOrPM;
        var id = "appointment" + numAppointments.toString();
        var num = numAppointments.toString();
        appointmentElements.push(
            <div class="appointment" style={style} id={id} onClick={() => seeNotes(num)}>
                <div class="hidden" id={id + "Height"}>{appointment.length * 82}px</div>
                <div class="name">{appointment.title}</div>
                <div class="time">{time}</div>
                <div class="time">Room {appointment.room}</div>
                <div class="notes" id={"notes" + num}>Notes: {appointment.notes}</div>
            </div>
        );
        numAppointments++;
    });
    return appointmentElements;
}

var idExpanded = null;
function seeNotes(id)   {
    var notes = "notes" + id;
    id = "appointment" + id;
    if (document.getElementById(id).style.height != "auto" && idExpanded == null)
    {
        document.getElementById(id).style.height = "auto";
        document.getElementById(id).style.backgroundColor = "#003e74";
        document.getElementById(id).style.zIndex = 4;
        document.getElementById(notes).style.display = "block";
        idExpanded = id;
    }
    else if (idExpanded == id)
    {
        document.getElementById(id).style.height = document.getElementById(id + "Height").innerHTML;
        document.getElementById(id).style.backgroundColor = "#00529b";
        document.getElementById(id).style.zIndex = 2;
        document.getElementById(notes).style.display = "none";
        idExpanded = null;
    }
}

function toggleDay(toggleDay)
{
    console.log(toggleDay);
    const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
    days.forEach(day => {
        if (day != toggleDay)
        {
            document.getElementById(day + "Toggle").style.backgroundColor = "#A8A9AD";
            document.getElementsByClassName(day)[0].style.display = "none";
            document.getElementsByClassName(day)[1].style.display = "none";
        }
        else
        {
            document.getElementById(day + "Toggle").style.backgroundColor = "#EA7600";
            document.getElementsByClassName(day)[0].style.display = "block";
            document.getElementsByClassName(day)[1].style.display = "block";
        }
    });
}

export default Dashboard;