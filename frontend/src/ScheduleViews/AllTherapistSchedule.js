import { getQueriesForElement } from "@testing-library/react";
import React from "react";
import "./RoomSchedule.css";
import axios from "axios";

class AllTherapistSchedule extends React.Component {
  constructor(props) {
    super(props);
    this.numAppointments = 0;
    this.lines = {values: this.loadLines()};
    this.hours = {values: this.loadHours(this.props.date)};
    this.time = {value: loadTimeLine()};
    var d = new Date();
    while (d.getDay() != 1) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    this.state = {
        userList: [],
        therapistList: [],
        patients: [],
        locations: []
      };
      console.log(this.props.role);
  }

    loadRooms(date, users, therapists)
    {
        therapists = this.getRooms(users, therapists);
        const roomElements = [];
        for (let i = 0; i < therapists[0].length; i++)
        {
            let lines = this.loadLines(i + 1, therapists[1][i]);
            let percent = Math.floor(Math.random() * 100);
            var color = getColor(percent);
            const therapistEvents = [];
            for(let j = 0; j < this.props.therapistEvents.length; j++) //get therapist events for this therapist
            {
                if (this.props.therapistEvents[j].therapistId == therapists[1][i])
                    therapistEvents.push(this.props.therapistEvents[j]);
            }
            const appointments = [];
            for(let j = 0; j < this.props.appointments.length; j++) //get appointments for this therapist
            {
                if (this.props.appointments[j].therapistId == therapists[1][i])
                    appointments.push(this.props.appointments[j]);
            }
            if (i % 10 == 0)
                roomElements.push(<div class="printHours">{this.loadHours(date)}</div>);
            
            roomElements.push(
                <div class="therapist">
                    <div class="roomLabel">{therapists[0][i]}</div>
                    <div class="therapistMetrics" style={color}>{percent} %</div>
                    {lines}
                    {this.getTherapistEventElements(therapistEvents)}
                    {this.getAppointmentElements(appointments)}
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
            if (this.props.role == "admin" || (this.props.role == "therapist" && sessionStorage.getItem('id') == therapistId))
            {
                if (i % 2)
                {
                    items.push(
                        <div onClick={() => {this.showAddAppointment(time, "00", this.props.date, therapistIndex); this.props.therapistEvent.therapistId = therapistId; this.props.appointment.therapistId = therapistId; this.props.setTimes(this.props.date, time, 0)}} class="halfHour"><div class="hide">+</div></div>
                    );
                    items.push(
                        <div onClick={() => {this.showAddAppointment(time, "30", this.props.date, therapistIndex); this.props.therapistEvent.therapistId = therapistId; this.props.appointment.therapistId = therapistId; this.props.setTimes(this.props.date, time, 30)}} class="halfHour"><div class="hide">+</div></div>
                    );
                }
                else
                {
                    items.push(
                        <div onClick={() => {this.showAddAppointment(time, "00", this.props.date, therapistIndex); this.props.therapistEvent.therapistId = therapistId; this.props.appointment.therapistId = therapistId; this.props.setTimes(this.props.date, time, 0)}} class="halfHour printGrey"><div class="hide">+</div></div>
                    );
                    items.push(
                        <div onClick={() => {this.showAddAppointment(time, "30", this.props.date, therapistIndex); this.props.therapistEvent.therapistId = therapistId; this.props.appointment.therapistId = therapistId; this.props.setTimes(this.props.date, time, 30)}} class="halfHour printGrey"><div class="hide">+</div></div>
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
        while(this.props.date.getDay() != day)
        {
            if (this.props.date.getDay() > day)
                this.props.date.setDate(this.props.date.getDate() - 1);
            else
                this.props.date.setDate(this.props.date.getDate() + 1);
        }

        this.props.getTherapistEvents();
        this.props.getAppointments();
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
    const url = "http://10.29.163.20:8081/api/";
        axios.get(url + "user").then((response) => {
            const userList = response.data;
            this.setState({ userList });
              axios.get("http://10.29.163.20:8081/api/permission").then((response) => {
              const therapistList = [];
              const permissions = response.data;
              this.state.userList.forEach(user =>{
                  permissions.forEach(permission => {
                      if (permission.userId == user.userId && permission.role == "therapist")
                          therapistList.push(user);
                  })
                    this.setState({therapistList});
                });
            });
        });

        axios
        .get("http://10.29.163.20:8081/api/patient")
        .then((response) => {
            const patients = response.data;
            this.setState({ patients });
            axios.get("http://10.29.163.20:8081/api/Location")
                    .then((response) => {
                    const locations = response.data;
                    this.setState({ locations });
                    this.props.getTherapistEvents();
                    this.props.getAppointments();
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
                    names.push(therapist.firstName + " " + therapist.lastName);
                    ids.push(therapist.userId);
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

    getTherapistEventButtons(num, therapistEvent)
    {
        const items = [];
        if (this.props.role == "admin" || (this.props.role == "therapist" && therapistEvent.therapistId == sessionStorage.getItem("id")))
        items.push(
            <button class="editAppointmentButton" id={"deleteAppointmentButton" + num} onClick={() => this.deleteTherapistEvent(therapistEvent.eventId)}>Delete</button>
        )
        return items;
    }

    getAppointmentButtons(num, appointment)
    {
        const items = [];
        if (this.props.role == "admin" || (this.props.role == "therapist" && appointment.therapistId == sessionStorage.getItem("id")))
        items.push(
            <button class="editAppointmentButton" id={"copyAppointmentButton" + num} onClick={() => this.props.copyAppointment(appointment.appointmentId)}>Copy</button>,
            <button class="editAppointmentButton" id={"deleteAppointmentButton" + num} onClick={() => this.deleteAppointment(appointment.appointmentId)}>Delete</button>
        )
        return items;
    }

    deleteTherapistEvent(id)
    {
        axios.delete("http://10.29.163.20:8081/api/therapistevent/" + id).then((response) => {
            console.log(response);
            this.props.getTherapistEvents();
        }).catch((error) => {
            console.log("Error caught");
            console.log(error);
        });
    }

    deleteAppointment(id)
    {
        axios.delete("http://10.29.163.20:8081/api/appointment/" + id).then((response) => {
            console.log(response);
            this.props.getAppointments();
        }).catch((error) => {
            console.log("Error caught");
            console.log(error);
        });
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
        var id = "appointment" + this.numAppointments.toString();
        var num = this.numAppointments.toString();
        var patient = this.getPatientById(appointment.patientId);
        var location = this.getLocationById(appointment.locationId);
        if(patient != null && location != null)
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
                    <div class="name">{patient.firstName + " " + patient.lastName}</div>
                    <div class="name">{location.name} {appointment.roomNumber}</div>
                    <div class="time">{appointment.adl}</div>
                    <div class="notes" id={"notes" + num}>Notes: {appointment.notes}</div>
                    {this.getAppointmentButtons(num, appointment)}
                </div>
            );
            this.numAppointments++;
        }
    });
    return appointmentElements;
}

    getLocationById(id)
    {
        for (let i = 0; i < this.state.locations.length; i++)
        {
            if (this.state.locations[i].locationId == id)
                return this.state.locations[i];
        } 
    }

    getPatientById(id)
    {
        for (let i = 0; i < this.state.patients.length; i++)
        {
            if (this.state.patients[i].patientId == id)
                return this.state.patients[i];
        }
        
    }

    getTherapistEventElements(therapistEvents)
    {
        var therapistEventElements = []; 
        therapistEvents.forEach(therapistEvent => {
        var start = new Date(therapistEvent.startTime);
        var end = new Date(therapistEvent.endTime);
        var position = (start.getHours() - 5) * 52 + start.getMinutes() * 52/60 + 36;
        var style = {
                top: position,
                height: Math.abs(end - start) / 36e5 * 52, 
                minHeight: Math.abs(end - start) / 36e5 * 52
            };
        
        var id = "appointment" + this.numAppointments.toString();
        var num = this.numAppointments.toString();
        therapistEventElements.push(
            <div class="appointment" style={style} id={id} onClick={() => seeNotes(num)}>
                <div class="hidden" id={id + "Height"}>{Math.abs(end - start) / 36e5 * 52}px</div>
                <div class="name">{therapistEvent.activityName}</div>
                <div class="name">{}</div>
                <div class="time">{}</div>
                <div class="notes" id={"notes" + num}>Notes: {therapistEvent.notes}</div>
                {this.getTherapistEventButtons(num, therapistEvent)}
            </div>
        );
        this.numAppointments++;
    });
    return therapistEventElements;
    }

    showAddAppointment(hour, minute, date, therapistIndex)   {
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
        var therapistElements = document.getElementsByClassName("selectTherapist");
        for (let i = 0; i < therapistElements.length; i++)
        {
            if (this.props.role == "admin")
                therapistElements[i].selectedIndex = therapistIndex;
            else if (this.props.role == "therapist")
                therapistElements[i].selectedIndex = 1;
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
                    {this.loadRooms(this.props.date, this.state.userList, this.state.therapistList)}
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
            show metrics {this.props.update}
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
        if (cols[i].getBoundingClientRect().width > 100)
        {
            cols[i].style.minWidth = (cols[i].getBoundingClientRect().width - 10) + "px";
            cols[i].style.width = (cols[i].getBoundingClientRect().width - 10) + "px";
        }
    }
}

export default AllTherapistSchedule;