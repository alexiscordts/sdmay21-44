import { getQueriesForElement } from "@testing-library/react";
import React from "react";
import "./RoomSchedule.css";
import axios from "axios";

class AllTherapistSchedule extends React.Component {
  constructor(props) {
    super(props);
    this.lines = {values: this.loadLines()};
    this.hours = {values: loadHours()};
    this.time = {value: loadTimeLine()};
    this.rooms = {values: this.getRooms()};
    var appointments = getAppointments(d);
    var d = new Date();
    while (d.getDay() != 1) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    this.tuesday = {values: getAppointmentElements(appointments)};
    this.state = {
        userList: [],
        therapistList: [],
      };
  }

    loadRooms(users, therapists)
    {
        var hours = loadHours();
        therapists = this.getRooms(users, therapists);
        const roomElements = [];
        var appointments = getAppointments(new Date());
        const tuesday = getAppointmentElements(appointments);
        for (let i = 0; i < therapists[0].length; i++)
        {
            let lines = this.loadLines(i + 1, therapists[1][i]);
            let percent = Math.floor(Math.random() * 100);
            var color = getColor(percent);
            if (i % 10 == 0)
                roomElements.push(<div class="printHours">{hours}</div>);
            if (i == 3)
                roomElements.push(
                    <div class="therapist">
                        <div class="roomLabel">{therapists[0][i]}</div>
                        <div class="therapistMetrics" style={color}>{percent} %</div>
                        {lines}
                        {tuesday}
                    </div>
                );
                else
                roomElements.push(
                    <div class="therapist">
                        <div class="roomLabel">{therapists[0][i]}</div>
                        <div class="therapistMetrics" style={color}>{percent} %</div>
                        {lines}
                    </div>
                );            
        }
        return roomElements;
    }

    loadLines(therapistIndex, therapistId)
    {
        const items = [];
        for (var i = 0; i < 15; i++)
        {
            let time = (i + 5);
            if (i % 2)
            {
                items.push(
                    <div onClick={() => {console.log(this.props.date.toString()); showAddAppointment(time, "00", this.props.date, therapistIndex); this.props.therapistEvent.therapistId = therapistId; this.setTimes(this.props.date, time, 0)}} class="halfHour"><div class="hide">+</div></div>
                );
                items.push(
                    <div onClick={() => {console.log(this.props.date.toString()); showAddAppointment(time, "30", this.props.date, therapistIndex); this.props.therapistEvent.therapistId = therapistId; this.setTimes(this.props.date, time, 30)}} class="halfHour"><div class="hide">+</div></div>
                );
            }
            else
            {
                items.push(
                    <div onClick={() => {console.log(this.props.date.toString()); showAddAppointment(time, "00", this.props.date, therapistIndex); this.props.therapistEvent.therapistId = therapistId; this.setTimes(this.props.date, time, 0)}} class="halfHour printGrey"><div class="hide">+</div></div>
                );
                items.push(
                    <div onClick={() => {console.log(this.props.date.toString()); showAddAppointment(time, "30", this.props.date, therapistIndex); this.props.therapistEvent.therapistId = therapistId; this.setTimes(this.props.date, time, 30)}} class="halfHour printGrey"><div class="hide">+</div></div>
                );

            }
        }
        return items;
    }

    setTimes(date, hour, minute)
    {
        this.props.therapistEvent.startTime = new Date(date);
        this.props.therapistEvent.endTime = new Date(date);
        this.props.therapistEvent.startTime.setHours(hour);
        this.props.therapistEvent.startTime.setMinutes(minute);
        this.props.therapistEvent.endTime.setHours(hour + 1);
        this.props.therapistEvent.endTime.setMinutes(minute);
    
    }

    setDay(day)    
    {
        console.log(this.props.date.getDay());
        console.log(day);
        while(this.props.date.getDay() != day)
        {
            if (this.props.date.getDay() > day)
                this.props.date.setDate(this.props.date.getDate() - 1);
            else
                this.props.date.setDate(this.props.date.getDate() + 1);
        }
        console.log(this.props.date.getDay());
        console.log(day);
        console.log(this.props.date.toString());
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
  };

  componentDidMount() {
    window.addEventListener('resize', this.updateDimensions);
    window.addEventListener('load', this.load);
    this.interval = setInterval(() => this.setState({ time: Date.now() }), 60000); //Render every minute
    toggleDay(new Intl.DateTimeFormat('en-US', {weekday: 'long'}).format(this.props.date));
    const url = "http://10.29.163.20:8081/api/";
        axios.get(url + "user").then((response) => {
            console.log(response);
          const userList = response.data;
          this.setState({ userList });
        });
    
        axios.get("http://10.29.163.20:8081/api/permission").then((response) => {
          this.setState({
            therapistList: this.state.therapistList.concat(response.data),
          });
        });
  }

  componentWillUnmount() {
    window.removeEventListener('resize', this.updateDimensions);
    window.removeEventListener('size', this.load);
    clearInterval(this.interval);
  }

  getRooms(users, therapists) {
        var names = [];
        var ids = [];
        try {
        therapists.forEach(therapist => {
            users.forEach(user => {
                if (therapist.role == "therapist" && user.userId == therapist.userId)
                    names.push(user.firstName + " " + user.lastName);
                    ids.push(user.userId);
            });
            
        });
        }
        catch (error)
        {
            console.log(error);
        }
        var therapists = [];
        therapists.push(names);
        therapists.push(ids);
        return therapists;
    }

  render() {
    this.time = {value: loadTimeLine()} //Update timeline
    toggleDay(new Intl.DateTimeFormat('en-US', {weekday: 'long'}).format(this.props.date));
    this.therapistSchedules = this.loadRooms(this.state.userList, this.state.therapistList);
    console.log(this.state.userList, );
    return (
        <div>
        <div id="roomSchedule">
            <div id="scheduleContainer">
                
                {this.time.value}
                <div id="hourColumn">
                    {this.hours.values}
                </div>
                <div id="rooms">     
                    {this.therapistSchedules}
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
    var appointment1 = { title: "Beatrice Coleman", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin porta sem ut ipsum dictum bibendum. Curabitur sodales interdum lorem, ac.", date: new Date(d), length: 1, type: "Pt", subtype: "U" };
    d.setHours(8,0,0,0);
    var appointment2 = { title: "Vivian Allison", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque egestas, lectus in congue scelerisque.", date: new Date(d), length: 1, type: "Ot", subtype: "ULG" };
    d.setHours(9,0,0,0);
    var appointment3 = { title: "Marsha Morgan", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed mauris at nisi consequat eleifend. Sed nulla quam, vehicula at turpis a, cursus aliquet justo. Donec et erat sed mauris semper.", date: new Date(d), length: 2, type: "Sp", subtype: "Kitchen" };
    d.setHours(19,0,0,0);
    var appointment4 = { title: "Vivian Allison", room: "123", notes: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed mauris at nisi consequat eleifend. Sed nulla quam, vehicula at turpis a, cursus aliquet justo. Donec et erat sed mauris semper.", date: new Date(d), length: 1, type: "Rt", subtype: "W/CA" };
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
        var style = {
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
                <div class="name">Room {appointment.room}</div>
                <div class="time">{appointment.type}: {appointment.subtype}</div>
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

function showAddAppointment(hour, minute, date, therapistIndex)   {
    document.getElementById("addAppointment").style.display = "block";
    document.getElementById("editAppointment").style.display = "none";
    let time = "";
    if (hour < 10)
        time = "0" + hour + ":" + minute;
    else
        time = hour + ":" + minute;
    var selectElements = document.getElementsByClassName("select-field");
    for (var i = 0; i < selectElements.length; i++)
    {
        selectElements[i].selectedIndex = 0;
    }
    var startElements = document.getElementsByClassName("startTime");
    for (let i = 0; i < startElements.length; i++)
    {
        startElements[i].value = time;
    }
    let endTime = "";
    if (hour + 1 < 10)
        endTime = "0" + (hour + 1) + ":" + minute;
    else
        endTime = (hour + 1) + ":" + minute;
    var endElements = document.getElementsByClassName("endTime");
    for (let i = 0; i < startElements.length; i++)
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
    var therapistElements = document.getElementsByClassName("selectTherapist");
    for (let i = 0; i < therapistElements.length; i++)
    {
        therapistElements[i].selectedIndex = therapistIndex;
    }
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