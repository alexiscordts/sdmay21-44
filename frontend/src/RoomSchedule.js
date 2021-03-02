import { getQueriesForElement } from "@testing-library/react";
import React from "react";
import "./RoomSchedule.css";

class RoomSchedule extends React.Component {
  constructor(props) {
    super(props);
    this.lines = {values: loadLines()};
    this.hours = {values: loadHours()};
    var d = new Date();
    this.time = {value: loadTimeLine()};
    this.rooms = {values: getRooms()};
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
            const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
             days.forEach(day => {
                document.getElementById(day + "Toggle").style.backgroundColor = null;
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
    this.interval = setInterval(() => this.setState({ time: Date.now() }), 60000); //Render every minute
  }

  componentWillUnmount() {
    window.removeEventListener('resize', this.updateDimensions);
    window.removeEventListener('size', this.load);
    clearInterval(this.interval);
  }

  render() {
    this.time = {value: loadTimeLine()} //Update timeline
    var roomSchedules = loadRooms();
    var roomNumbers = loadRoomNumbers();
    return (
        <div>
        <div id="roomSchedule">
            <div id="scheduleContainer">
                
                {this.time.value}
                <div id="hourColumn">
                    {this.hours.values}
                </div>
                <div id="rooms">
                    
                    {roomSchedules}
                </div>
            </div>
            <div id="toggle">
                <div id="SundayToggle" class="daytoggle" onClick={() => toggleDay("Sunday")}>
                    Su
                </div>
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
                <div id="SaturdayToggle" class="daytoggle" onClick={() => toggleDay("Saturday")}>
                    Sa
                </div>
                <div id="LeftScroll" class="scroll daytoggle" onMouseDown={() => leftScroll()}>
                    &#60;
                </div>
                <div id="RightScroll" class="scroll daytoggle" onMouseDown={() => rightScroll()}>
                    &#62;
                </div>
            </div>
        </div>
        <label class="scrollLabel" for="scrollCheck">
        scroll
        <input type = "checkbox" id="scrollCheck" onChange={() => showScroll()}/>
        </label>
        </div>
    );
  }

  


}

function getRooms() {
    return ["237", "123", "283", '111', '083', '162', '298', '293', '222', '105', '102', '112', '101', '103', '104', '105', '106', "237", "123", "283", '111', '083', '162', '298', '293', '222', '105', '102', '112', '101', '103', '104', '105', '106', '106', "237", "123", "283", '111', '083', '162', '298', '293', '222', '105', '102', '112', '101', '103', '104', '105', '106'];
}

function loadRooms()
{
    var lines = loadLines();
    var hours = loadHours();
    const rooms = getRooms();
    const roomElements = [];
    var appointments = getAppointments(new Date());
    const tuesday = getAppointmentElements(appointments);
    for (let i = 0; i < rooms.length; i++)
    {
        if (i % 10 == 0)
            roomElements.push(<div class="printHours">{hours}</div>);
        if (rooms[i] == "123")
            roomElements.push(
                <div class="room">
                    <div class="roomLabel">{rooms[i]}</div>
                    {lines}
                    {tuesday}
                </div>
            );
            else
            roomElements.push(
                <div class="room">
                    <div class="roomLabel">{rooms[i]}</div>
                    {lines}
                </div>
            );            
    }
    return roomElements;
}

function loadRoomNumbers()
{
    const rooms = getRooms();
    const roomNumberElements = [];
    for (let i = 0; i < rooms.length; i++)
    {
            roomNumberElements.push(
                    <div class="roomLabel2">{rooms[i]}</div>
            );            
    }
    return roomNumberElements;
}

function loadLines()
{
    const items = [];
    for (var i = 0; i < 15; i++)
    {
        if (i % 2)
        {
            items.push(
                <div onClick={() => showAddAppointment()} class="halfHour"><div class="hide">+</div></div>
            );
            items.push(
                <div onClick={() => showAddAppointment()} class="halfHour"><div class="hide">+</div></div>
            );
        }
        else
        {
            items.push(
                <div onClick={() => showAddAppointment()} class="halfHour printGrey"><div class="hide">+</div></div>
            );
             items.push(
                <div onClick={() => showAddAppointment()} class="halfHour printGrey"><div class="hide">+</div></div>
            );

        }
    }
    return items;
}

function loadHours()
{
    const hours = [];
    hours.push(<div id="topSpace"></div>);
    var AMPM = "AM"
    for (var i = 5; i < 20; i++)
    {
        var time = i;
        if (i == 12)
            AMPM = "PM"
        else if (i > 12)
            time = i - 12;
        
        if(i % 2 == 0)
            hours.push(
                <div class="hour">{time} {AMPM}</div>
            );
        else 
            hours.push(
                <div class="hour printGrey">{time} {AMPM}</div>
            );
    }
    return hours;
}

function loadTimeLine()  
{
    var position = getPositionForTimeLine();
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
    return hour * 52 + minute * 52/60 + 36;
}


function getAppointments(date) {
    var d = new Date();
    while (d.getDay() != 1) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    //placeholder appointments
    d.setHours(13,0,0,0);
    var appointment1 = { title: "Patient 1", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin porta sem ut ipsum dictum bibendum. Curabitur sodales interdum lorem, ac.", date: new Date(d), length: 1, type: "1" };
    d.setHours(8,0,0,0);
    var appointment2 = { title: "Patient 2", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque egestas, lectus in congue scelerisque.", date: new Date(d), length: 1, type: "2" };
    d.setHours(9,0,0,0);
    var appointment3 = { title: "Patient 3", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed mauris at nisi consequat eleifend. Sed nulla quam, vehicula at turpis a, cursus aliquet justo. Donec et erat sed mauris semper.", date: new Date(d), length: 2, type: "2" };
    d.setHours(19,0,0,0);
    var appointment4 = { title: "Patient 3", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed mauris at nisi consequat eleifend. Sed nulla quam, vehicula at turpis a, cursus aliquet justo. Donec et erat sed mauris semper.", date: new Date(d), length: 1, type: "1" };
    var appointments = [];
    appointments.push(appointment1, appointment2, appointment3, appointment4);
    return appointments;
}

var numAppointments = 0;
function getAppointmentElements(appointments)   {
    var appointmentElements = []; 
    appointments.forEach(appointment => {
        var start = appointment.date.getHours();
        var end = appointment.date.getHours() + appointment.length;
        var position = (start - 5) * 52 + appointment.date.getMinutes() * 52/60 + 36;
        var style;
        if (appointment.type == "1")
            style = {
                top: position,
                height: appointment.length * 52, 
                minHeight: appointment.length * 52,
                backgroundColor: "#EA7600"
            };
        else
        style = {
            top: position,
            height: appointment.length * 52, 
            minHeight: appointment.length * 52
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
                <div class="hidden" id={id + "Height"}>{appointment.length * 52}px</div>
                <div class="name">{appointment.title}</div>
                <div class="time">Type {appointment.type}</div>
                <div class="time">Room {appointment.room}</div>
                <div class="notes" id={"notes" + num}>Notes: {appointment.notes}</div>
                <button class="editAppointmentButton" id={"editAppointmentButton" + num}>Edit</button>
                <button class="editAppointmentButton" id={"copyAppointmentButton" + num}>Copy</button>
                <button class="editAppointmentButton" id={"deleteAppointmentButton" + num}>Delete</button>             
            </div>
        );
        numAppointments++;
    });
    return appointmentElements;
}

var idExpanded = null;
function seeNotes(id)   {
    let notes = "notes" + id;
    let edit = "editAppointmentButton" + id;
    let copy = "copyAppointmentButton" + id;
    let deleteApp = "deleteAppointmentButton" + id;
    id = "appointment" + id;
    if (document.getElementById(id).style.height != "auto" && idExpanded == null)
    {
        document.getElementById(id).style.height = "auto";
        document.getElementById(id).style.width = "200%";
        document.getElementById(id).style.zIndex = 4;
        document.getElementById(notes).style.display = "block";
        document.getElementById(edit).style.display = "block";
        document.getElementById(copy).style.display = "block";
        document.getElementById(deleteApp).style.display = "block";
        idExpanded = id;
    }
    else if (idExpanded == id)
    {
        document.getElementById(id).style.height = document.getElementById(id + "Height").innerHTML;
        document.getElementById(id).style.width = "100%";
        document.getElementById(id).style.zIndex = 2;
        document.getElementById(notes).style.display = "none";
        document.getElementById(edit).style.display = "none";
        document.getElementById(copy).style.display = "none";
        document.getElementById(deleteApp).style.display = "none";
        idExpanded = null;
    }
}

function toggleDay(toggleDay)
{
    console.log(toggleDay);
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    days.forEach(day => {
        if (day != toggleDay)
        {
            document.getElementById(day + "Toggle").style.backgroundColor = "#A8A9AD";
        }
        else
        {
            document.getElementById(day + "Toggle").style.backgroundColor = "#EA7600";
        }
    });
}

function leftScroll()   
{
    var w = document.getElementById("scheduleContainer").offsetWidth - 75;
    w = w - (w % 101);
    document.getElementById('rooms').scrollBy({
        left: -w,
        behavior: 'smooth'
      });
}

function rightScroll()   
{
    var w = document.getElementById("scheduleContainer").offsetWidth - 75;
    w = w - (w % 101);
    document.getElementById('rooms').scrollBy({
        left: w,
        behavior: 'smooth'
      });
}

function showAddAppointment()   {
    document.getElementById("addAppointment").style.display = "block";
}

function showScroll()   {
    let checked = document.getElementById("scrollCheck").checked;
    if (checked == true)
    {
        document.getElementById("rooms").style.overflowX = "initial";
        checked = false;
    }
    else
    {
        document.getElementById("rooms").style.overflowX = "scroll";
        checked = true;
    }
}

export default RoomSchedule;