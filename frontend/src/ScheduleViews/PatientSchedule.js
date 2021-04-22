import { getQueriesForElement } from "@testing-library/react";
import React from "react";
import "./RoomSchedule.css";

class AllTherapistSchedule extends React.Component {
  constructor(props) {
    super(props);
    this.lines = {values: this.loadLines()};
    var d = new Date();
    this.time = {value: loadTimeLine()};
    this.rooms = {values: getPatients()};
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

  componentDidMount() {
    window.addEventListener('resize', this.updateDimensions);
    this.interval = setInterval(() => this.setState({ time: Date.now() }), 60000); //Render every minute
    toggleDay(new Intl.DateTimeFormat('en-US', {weekday: 'long'}).format(this.props.date));
  }

  componentWillUnmount() {
    window.removeEventListener('resize', this.updateDimensions);
    window.removeEventListener('size', this.load);
    clearInterval(this.interval);
  }

  loadPatients()
    {
        var hours = this.loadHours(this.props.date);
        const rooms = getPatients();
        const roomElements = [];
        var appointments = getAppointments(new Date());
        const tuesday = getAppointmentElements(appointments);
        for (let i = 0; i < rooms.length; i++)
        {
            let lines = this.loadLines(i + 1);
            let percent = Math.floor(Math.random() * 100);
            var color = getColor(percent);
            if (i % 10 == 0)
                roomElements.push(<div class="printHours">{hours}</div>);
            if (i == 3)
                roomElements.push(
                    <div class="therapist">
                        <div class="roomLabel">{rooms[i]}</div>
                        <div class="therapistMetrics" style={color}>{percent} %</div>
                        {lines}
                        {tuesday}
                    </div>
                );
                else
                roomElements.push(
                    <div class="therapist">
                        <div class="roomLabel">{rooms[i]}</div>
                        <div class="therapistMetrics" style={color}>{percent} %</div>
                        {lines}
                    </div>
                );            
        }
        return roomElements;
    }

  loadLines(patientIndex)
    {
        const items = [];
        for (var i = 0; i < 15; i++)
        {
            let time = (i + 5);
            if (this.props.role == "admin")
            {
                if (i % 2)
                {
                    items.push(
                        <div onClick={() => showAddAppointment(time, "00", this.props.date, patientIndex)} class="halfHour"><div class="hide">+</div></div>
                    );
                    items.push(
                        <div onClick={() => showAddAppointment(time, "30", this.props.date, patientIndex)} class="halfHour"><div class="hide">+</div></div>
                    );
                }
                else
                {
                    items.push(
                        <div onClick={() => showAddAppointment(time, "00", this.props.date, patientIndex)} class="halfHour printGrey"><div class="hide">+</div></div>
                    );
                    items.push(
                        <div onClick={() => showAddAppointment(time, "30", this.props.date, patientIndex)} class="halfHour printGrey"><div class="hide">+</div></div>
                    );

                }
            }
            else
            {
                if (i % 2)
                items.push(
                    <div class="halfHour"></div>,
                    <div class="halfHour"></div>
                );
                else
                items.push(
                    <div class="halfHour printGrey"></div>,
                    <div class="halfHour printGrey"></div>
                );
            }
        }
        return items;
    }

  setDay(day)    
    {
        console.log(this.props.date);
        while(this.props.date.getDay() != day)
        {
            if (this.props.date.getDay() > day)
                this.props.date.setDate(this.props.date.getDate() - 1);
            else
                this.props.date.setDate(this.props.date.getDate() + 1);
        }
        this.forceUpdate();
    }

    loadHours(date)
    {
        const hours = [];
        hours.push(<div id="topSpace">{date.toLocaleDateString('en-US')}</div>);
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

  render() {
    this.time = {value: loadTimeLine()} //Update timeline
    var roomSchedules = this.loadPatients();
    var roomNumbers = loadRoomNumbers();
    return (
        <div>
        <div id="roomSchedule">
            <div id="scheduleContainer">
                
                {this.time.value}
                <div id="hourColumn">
                    {this.loadHours(this.props.date)}
                </div>
                <div id="rooms">
                    
                    {roomSchedules}
                </div>
            </div>
            <div id="toggle">
            <div id="SundayToggle" class="daytoggle" onClick={() => {this.setDay(0); toggleDay("Sunday")}}>
                    Su
                </div>
                <div id="MondayToggle" class="daytoggle" onClick={() => {this.setDay(1); toggleDay("Monday");}}>
                    M
                </div>
                <div id="TuesdayToggle" class="daytoggle" onClick={() => {this.setDay(2); toggleDay("Tuesday")}}>
                    T
                </div>
                <div id="WednesdayToggle" class="daytoggle" onClick={() => {this.setDay(3);  toggleDay("Wednesday")}}>
                    W
                </div>
                <div id="ThursdayToggle" class="daytoggle" onClick={() => {this.setDay(4);  toggleDay("Thursday")}}>
                    Th
                </div>
                <div id="FridayToggle" class="daytoggle" onClick={() => {this.setDay(5); toggleDay("Friday")}}>
                    F
                </div>
                <div id="SaturdayToggle" class="daytoggle" onClick={() => {this.setDay(6); toggleDay("Saturday")}}>
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
        <div class="scheduleOptions">
            <button class="adjustColWidth" onClick={() => incColWidth()}>
                +
            </button>

            <button style={{marginRight: "10px"}} class="adjustColWidth" onClick={() => decColWidth()}>
                -
            </button>

            <label class="scrollLabel" for="scrollCheck">
            scroll
            <input type = "checkbox" id="scrollCheck" onChange={() => showScroll()}/>
            </label>

            <label class="metricLabel" for="metricCheck">
            show metrics
            <input type = "checkbox" id="metricCheck" onChange={() => showMetrics()}/>
            </label>
            </div>
        </div>
    );
  }
}

function getPatients() {
    return ['Beatrice Coleman', 'Vivian Allison', 'Marsha Morgan', 'Al Carr', 'Jesus Sutton', 'Pearl Robertson', 'Marion Hammond', 'Jim Chandler', 'Kimberly Rodriquez', 'Joel Ramsey'];
}

function getColor(percent) {
    if (percent <= 10)
        return {backgroundColor: "#ED5314"};
    else if (percent <= 35)
        return {backgroundColor: "#FFB92A"};
    else if (percent < 65)
        return {backgroundColor: "#FEEB51"};
    else
        return {backgroundColor: "#9BCA3E"};
}

function loadRoomNumbers()
{
    const rooms = getPatients();
    const roomNumberElements = [];
    for (let i = 0; i < rooms.length; i++)
    {
            roomNumberElements.push(
                    <div class="roomLabel2">{rooms[i]}</div>
            );            
    }
    return roomNumberElements;
}

function loadTimeLine()  
{
    var position = getPositionForTimeLine();
    const timeStyle = {
        top: position,
      };
    if (position > 0 && position < 780)
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
    var appointment1 = { title: "Beatrice Coleman", therapist: "Lyndon Macdonald", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin porta sem ut ipsum dictum bibendum. Curabitur sodales interdum lorem, ac.", date: new Date(d), length: 1, type: "Pt", subtype: "U" };
    d.setHours(8,0,0,0);
    var appointment2 = { title: "Vivian Allison", therapist: "Lacy Silva", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque egestas, lectus in congue scelerisque.", date: new Date(d), length: 1, type: "Ot", subtype: "ULG" };
    d.setHours(9,0,0,0);
    var appointment3 = { title: "Marsha Morgan", therapist: "Delores Daniels", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed mauris at nisi consequat eleifend. Sed nulla quam, vehicula at turpis a, cursus aliquet justo. Donec et erat sed mauris semper.", date: new Date(d), length: 2, type: "Sp", subtype: "Kitchen" };
    d.setHours(19,0,0,0);
    var appointment4 = { title: "Vivian Allison", therapist: "Omar Needham", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed mauris at nisi consequat eleifend. Sed nulla quam, vehicula at turpis a, cursus aliquet justo. Donec et erat sed mauris semper.", date: new Date(d), length: 1, type: "Rt", subtype: "W/CA" };
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
                <div class="name">{appointment.therapist}</div>
                <div class="time">{appointment.type}: {appointment.subtype}</div>
                <div class="time">Room {appointment.room}</div>
                <div class="notes" id={"notes" + num}>Notes: {appointment.notes}</div>
                <button class="editAppointmentButton" id={"editAppointmentButton" + num} onClick={() => showEditAppointment()}>Edit</button>
                <button class="editAppointmentButton" id={"copyAppointmentButton" + num} onClick={() => showAddAppointment()}>Copy</button>
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
        document.getElementById(id).style.width = "150%";
        document.getElementById(id).style.backgroundColor = "#003e74";
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
        document.getElementById(id).style.backgroundColor = "#00529b";
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
    var w = window.innerWidth - 175;
    w = w - (w % 202);
    document.getElementById('rooms').scrollBy({
        left: -w,
        behavior: 'smooth'
      });
}

function rightScroll()   
{
    var w = window.innerWidth - 175;
    w = w - (w % 202);
    document.getElementById('rooms').scrollBy({
        left: w,
        behavior: 'smooth'
      });
}

function showAddAppointment(hour, minute, date, patientIndex)   {
    document.getElementById("addAppointment").style.display = "block";
    document.getElementById("editAppointment").style.display = "none";
    let time = "";
    if (hour < 10)
        time = "0" + hour + ":" + minute;
    else
        time = hour + ":" + minute;
    console.log(time);
    var selectElements = document.getElementsByClassName("select-field");
    for (var i = 0; i < selectElements.length; i++)
    {
        selectElements[i].selectedIndex = 0;
    }
    var startElements = document.getElementsByClassName("startTime");
    for (var i = 0; i < startElements.length; i++)
    {
        startElements[i].value = time;
    }
    let endTime = "";
    if (hour + 1 < 10)
        endTime = "0" + (hour + 1) + ":" + minute;
    else
        endTime = (hour + 1) + ":" + minute;
    var endElements = document.getElementsByClassName("endTime");
    for (var i = 0; i < startElements.length; i++)
    {
        endElements[i].value = endTime;
    }
    var dateElements = document.getElementsByClassName("date");
    for (let i = 0; i < dateElements.length; i++)
    {
        let year = date.getFullYear();
        let month = date.getMonth() + 1;
        if (month < 10)
            month = '0' + month;
        let day = date.getDate();
        if (day < 10)
            day = '0' + day;
        dateElements[i].value = year + "-" + month + "-" + day;
    }
    document.getElementById("room").selectedIndex = patientIndex;
    document.getElementById("patient").selectedIndex = patientIndex;
}

function showEditAppointment()   {
    document.getElementById("editAppointment").style.display = "block";
    document.getElementById("addAppointment").style.display = "none";
}

function showScroll()   {
    let checked = document.getElementById("scrollCheck").checked;
    if (checked == false)
    {
        document.getElementById("rooms").style.overflowX = "hidden";
    }
    else
    {
        document.getElementById("rooms").style.overflowX = "scroll";
    }
}

function showMetrics()   {
    let checked = document.getElementById("metricCheck").checked;
    var cols = document.getElementsByClassName("therapistMetrics");
    if (checked == true)
    {
        for (var i = 0; i < cols.length; i++)
        {
            cols[i].style.display = "block";
        }
    }
    else
    {
        for (var i = 0; i < cols.length; i++)
        {
            cols[i].style.display = "none";
        }
    }
}

function incColWidth()  {
    var cols = document.getElementsByClassName("therapist");
    for (var i = 0; i < cols.length; i++)
    {
        if (cols[i].getBoundingClientRect().width < 500)
        {
            cols[i].style.minWidth = (cols[i].getBoundingClientRect().width + 10) + "px";
            cols[i].style.width = (cols[i].getBoundingClientRect().width + 10) + "px";
        }
    }
}

function decColWidth()  {
    var cols = document.getElementsByClassName("therapist");
    for (var i = 0; i < cols.length; i++)
    {
        if (cols[i].getBoundingClientRect().width > 150)
        {
            cols[i].style.minWidth = (cols[i].getBoundingClientRect().width - 10) + "px";
            cols[i].style.width = (cols[i].getBoundingClientRect().width + 10) + "px";
        }
    }
}

export default AllTherapistSchedule;