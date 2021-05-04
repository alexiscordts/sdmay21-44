import { getQueriesForElement } from "@testing-library/react";
import React from "react";
import "./RoomSchedule.css";
import axios from "axios";

class RoomSchedule extends React.Component {
  constructor(props) {
    super(props);
    this.numAppointments = 0;
    this.lines = {values: this.loadLines()};
    var d = new Date();
    this.time = {value: loadTimeLine()};
    var d = new Date();
    while (d.getDay() != 1) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    console.log(this.props.location);
    this.state = {
        patients: [],
        rooms: [],
        therapistList: [],
        userList: []
    }
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
    const url = process.env.REACT_APP_SERVER_URL + "";
        axios.get(url + "user").then((response) => {
            console.log(response);
          const userList = response.data;
          this.setState({ userList });
          axios
        .get(process.env.REACT_APP_SERVER_URL + "room")
        .then((response) => {
            const rooms = response.data;
            this.setState({ rooms });
            axios.get(process.env.REACT_APP_SERVER_URL + "permission").then((response) => {
            const therapistList = [];
            const permissions = response.data;
            this.state.userList.forEach(user =>{
                permissions.forEach(permission => {
                    if (permission.userId == user.userId && permission.role == "therapist")
                        therapistList.push(user);
                })
            })
            this.setState({therapistList});  
                axios
                .get(process.env.REACT_APP_SERVER_URL + "patient")
                .then((response) => {
                    const patients = response.data;
                    this.setState({ patients });
                    this.props.getAppointments();
                });       
                });
        });
        });
      
  }

  componentWillUnmount() {
    window.removeEventListener('resize', this.updateDimensions);
    window.removeEventListener('size', this.load);
    clearInterval(this.interval);
  }

  getRooms() {
    const items = [];
        this.state.rooms.forEach(
            room => {
                if (room.locationId == this.props.location.locationId)
                    items.push(room);
            })
    return items;
}

    loadRooms()
    {
        var hours = this.loadHours(this.props.date);
        const roomElements = [];
        var rooms = this.getRooms();
        for (let i = 0; i < rooms.length; i++)
        {
            let lines = this.loadLines(rooms[i]);
            const appointments = [];
            for(let j = 0; j < this.props.appointments.length; j++) //get appointments for this therapist
            {
                if (this.props.appointments[j].roomNumber == rooms[i].number && this.props.appointments[j].locationId == rooms[i].locationId)
                    appointments.push(this.props.appointments[j]);
            }
            if (i % 10 == 0)
                roomElements.push(<div class="printHours">{hours}</div>);
                roomElements.push(
                    <div class="room">
                        <div class="roomLabel">{rooms[i].number}</div>
                        {lines}
                        {this.getAppointmentElements(appointments)}
                    </div>
                );            
        }
        return roomElements;
    }

  loadLines(room)
    {
        const items = [];
        for (var i = 0; i < 15; i++)
        {
            let time = (i + 5);
            if (this.props.role == "admin" || this.props.role == "therapist")
            {
                if (i % 2)
                {
                    items.push(
                        <div onClick={() => {this.showAddAppointment(time, "00", this.props.date, room); this.props.setTimes(this.props.date, time, 0)}} class="halfHour"><div class="hide">+</div></div>
                    );
                    items.push(
                        <div onClick={() => {this.showAddAppointment(time, "30", this.props.date, room); this.props.setTimes(this.props.date, time, 30)}} class="halfHour"><div class="hide">+</div></div>
                    );
                }
                else
                {
                    items.push(
                        <div onClick={() => {this.showAddAppointment(time, "00", this.props.date, room); this.props.setTimes(this.props.date, time, 0)}} class="halfHour printGrey"><div class="hide">+</div></div>
                    );
                    items.push(
                        <div onClick={() => {this.showAddAppointment(time, "30", this.props.date, room); this.props.setTimes(this.props.date, time, 30)}} class="halfHour printGrey"><div class="hide">+</div></div>
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
        this.props.getAppointments();
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

    getColor(id)
    {
        for (let i = 0; i < this.state.therapistList.length; i++)
        {
            let therapist = this.state.therapistList[i];
            if (therapist.userId == id && therapist.color != null)
                return therapist.color;
        }
        return "#00529b";
    }

    getAppointmentElements(appointments)   {
        var appointmentElements = []; 
        appointments.forEach(appointment => {
            var start = new Date(appointment.startTime);
            var end = new Date(appointment.endTime);
            var position = (start.getHours() - 5) * 52 + start.getMinutes() * 52/60 + 36;
            var style = {
                    top: position,
                    height: Math.abs(end - start) / 36e5 * 52, 
                    minHeight: Math.abs(end - start) / 36e5 * 52
                };
            var id = "appointment" + this.numAppointments.toString();
            var num = this.numAppointments.toString();
            var therapist = this.getTherapist(appointment.therapistId);
            var patient = this.getPatientById(appointment.patientId);
            if(therapist != null && patient != null)
            {
                var style = {
                    top: position,
                    height: Math.abs(end - start) / 36e5 * 52, 
                    minHeight: Math.abs(end - start) / 36e5 * 52,
                    backgroundColor: this.getColor(patient.therapistId)
                };
                appointmentElements.push(
                    <div class="appointment" style={style} id={id} onClick={() => seeNotes(num)}>
                        <div class="hidden" id={id + "Height"}>{Math.abs(end - start) / 36e5 * 52}px</div>
                        <div class="name">{therapist.firstName + " " + therapist.lastName}</div>
                        <div class="name">{patient.firstName + " " + patient.lastName}</div>
                        <div class="time">{appointment.adl}</div>
                        <div class="notes" id={"notes" + num}>Notes: {appointment.notes}</div>
                        {this.getAppointmentButtons(num, appointment.appointmentId)}
                    </div>
                );
                this.numAppointments++;
            }
        });
        return appointmentElements;
    }

    getTherapist(id) {
        console.log("therapists");
        console.log(this.state.therapistList);
        console.log(this.state.therapistList);
        for(let i = 0; i < this.state.therapistList.length; i++)
            if (this.state.therapistList[i].userId == id)
                return this.state.therapistList[i];
    }

    getPatientById(id)
    {
        console.log("patients");
        console.log(this.state.patients);
        for (let i = 0; i < this.state.patients.length; i++)
        {
            if (this.state.patients[i].patientId == id)
                return this.state.patients[i];
        }
        
    }

    getAppointmentButtons(num, id)
    {
        const items = [];
        if (this.props.role == "admin")
        items.push(
            <button class="editAppointmentButton" id={"copyAppointmentButton" + num} onClick={() => this.props.copyAppointment(id)}>Copy</button>,
            <button class="editAppointmentButton" id={"deleteAppointmentButton" + num} onClick={() => this.deleteAppointment(id)}>Delete</button>
        )
        return items;
    }

    deleteAppointment(id)
    {
        axios.delete(process.env.REACT_APP_SERVER_URL + "appointment/" + id).then((response) => {
            console.log(response);
            this.props.getAppointments();
        }).catch((error) => {
            console.log("Error caught");
            console.log(error);
        });
    }

showAddAppointment(hour, minute, date, room)   {
    document.getElementById("addAppointment").style.display = "block";
    let time = "";
    if (hour < 10)
        time = "0" + hour + ":" + minute;
    else
        time = hour + ":" + minute;
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
    document.getElementById("location").value = this.props.location.locationId;
    document.getElementById("room").value = room.number;
    this.props.appointment.locationId = this.props.location.locationId;
    this.props.appointment.roomNumber = room.number;
    var found = false;
    this.state.patients.forEach(patient => {
        if (patient.roomNumber == room.number && patient.locationId == this.props.location.locationId)
        {
            document.getElementById("patient").value = patient.patientId;
            this.props.appointment.patientId = patient.patientId;
            document.getElementById("appointmentTherapist").value = patient.therapistId;
            this.props.appointment.therapistId = patient.therapistId;
            document.getElementById("physician").value = patient.pmrPhysicianId;
            this.props.appointment.pmrPhysicianId = patient.pmrPhysicianId;
            found = true;
        }
    })
    if (found == false)
    {
        document.getElementById("patient").selectedIndex = 0;
        document.getElementById("appointmentTherapist").selectedIndex = 0;
        document.getElementById("physician").selectedIndex = 0;
        this.props.appointment.pmrPhysicianId = null;
        this.props.appointment.therapistId = null;
        this.props.appointment.patientId = null;
    }
    if(this.props.role == "therapist")
    {
        this.props.appointment.therapistId = sessionStorage.getItem("id");
        document.getElementById("appointmentTherapist").selectedIndex = 1;
    }

}

  render() {
    this.time = {value: loadTimeLine()} //Update timeline
    toggleDay(new Intl.DateTimeFormat('en-US', {weekday: 'long'}).format(this.props.date));
    return (
        <div>
        <div id="roomSchedule">
            <div id="scheduleContainer">
                
                {this.time.value}
                <div id="hourColumn">
                    {this.loadHours(this.props.date)}
                </div>
                    <div id="rooms">      
                         {this.loadRooms()}
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
            </div>
        </div>
    );
  }

  


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
      return;
}

function getPositionForTimeLine()
{
    var d = new Date();
    var hour = d.getHours() - 5;
    var minute = d.getMinutes();
    return hour * 52 + minute * 52/60 + 36;
}

var idExpanded = null;
function seeNotes(id)   {
    let notes = "notes" + id;
    let copy = "copyAppointmentButton" + id;
    let deleteApp = "deleteAppointmentButton" + id;
    id = "appointment" + id;
    if (document.getElementById(id).style.height != "auto" && idExpanded == null)
    {
        document.getElementById(id).style.height = "auto";
        document.getElementById(id).style.width = "150%";
        document.getElementById(id).style.zIndex = 4;
        document.getElementById(notes).style.display = "block";
        if (document.getElementById(copy))
            document.getElementById(copy).style.display = "block";
        if (document.getElementById(deleteApp))
            document.getElementById(deleteApp).style.display = "block";
        idExpanded = id;
    }
    else if (idExpanded == id)
    {
        document.getElementById(id).style.height = document.getElementById(id + "Height").innerHTML;
        document.getElementById(id).style.width = "100%";
        document.getElementById(id).style.zIndex = 2;
        document.getElementById(notes).style.display = "none";
        if (document.getElementById(copy))  
            document.getElementById(copy).style.display = "none";
        if (document.getElementById(deleteApp))
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

function showScroll()   {
    let checked = document.getElementById("scrollCheck").checked;
    if (checked == false)
    {
        document.getElementById("rooms").style.overflowX = "hidden";
        checked = false;
    }
    else
    {
        document.getElementById("rooms").style.overflowX = "scroll";
        checked = true;
    }
}

function incColWidth()  {
    var cols = document.getElementsByClassName("room");
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
    var cols = document.getElementsByClassName("room");
    for (var i = 0; i < cols.length; i++)
    {
        if (cols[i].getBoundingClientRect().width > 100)
        {
             cols[i].style.minWidth = (cols[i].getBoundingClientRect().width - 10) + "px";
             cols[i].style.width = (cols[i].getBoundingClientRect().width - 10) + "px";
        }
    }
}

export default RoomSchedule;