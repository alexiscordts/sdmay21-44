import axios from "axios";
import React from "react";
import "./TherapistSchedule.css";

class TherapistSchedule extends React.Component {
  constructor(props) {
    super(props);
    this.numAppointments = 0;
    this.lines = {values: this.loadLines()};
    this.hours = {values: loadHours()};
    var d = new Date();
    this.time = {value: loadTimeLine()};
    this.days = {values: getDays(this.props.date)};
    var d = new Date();
    while (d.getDay() != 1) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    this.state = {
        appointments: [],
        therapistList: [],
        userList: [],
        patients: [],
        locations: []
    }
  }

  updateDimensions = () => {
    if (window.innerWidth > 550)
        {
            const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
             days.forEach(day => {
                document.getElementById(day + "Toggle").style.backgroundColor = null;
                document.getElementsByClassName(day)[0].style.display = null;
                document.getElementsByClassName(day)[1].style.display = null;
             });
        }
  };

  componentDidMount() {
    window.addEventListener('resize', this.updateDimensions);
    window.addEventListener('load', this.load);
    this.interval = setInterval(() => this.setState({ time: Date.now() }), 60000); //Render every minute

    const url = "http://10.29.163.20:8081/api/";
        axios.get(url + "user").then((response) => {
            console.log(response);
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
                axios
                .get("http://10.29.163.20:8081/api/patient")
                .then((response) => {
                    const patients = response.data;
                    this.setState({ patients });
                    axios.get("http://10.29.163.20:8081/api/Location")
                    .then((response) => {
                        const locations = response.data;
                        this.setState({ locations });
                        this.props.getTherapistEvents(sessionStorage.getItem("id"));
                        this.props.getAppointments(sessionStorage.getItem("id"));
                    });
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

  loadLines(day)
    {
        const items = [];
        for (var i = 0; i < 15; i++)
        {
            let time = (i + 5);
            if (i % 2)
            {
                items.push(
                    <div onClick={() => { this.setDay(day); this.showAddAppointment(time, "00", this.props.date); this.props.setTimes(this.props.date, time, 0); this.props.therapistEvent.therapistId = sessionStorage.getItem("id"); this.props.appointment.therapistId = sessionStorage.getItem("id");}} class="halfHour"><div class="hide">+</div></div>
                );
                items.push(
                    <div onClick={() => { this.setDay(day); this.showAddAppointment(time, "30", this.props.date); this.props.setTimes(this.props.date, time, 30); this.props.therapistEvent.therapistId = sessionStorage.getItem("id"); this.props.appointment.therapistId = sessionStorage.getItem("id");}} class="halfHour"><div class="hide">+</div></div>
                );
            }
            else
            {
                items.push(
                    <div onClick={() => { this.setDay(day); this.showAddAppointment(time, "00", this.props.date); this.props.setTimes(this.props.date, time, 0); this.props.therapistEvent.therapistId = sessionStorage.getItem("id"); this.props.appointment.therapistId = sessionStorage.getItem("id");}} class="halfHour printGrey"><div class="hide">+</div></div>
                );
                items.push(
                    <div onClick={() => {this.setDay(day); this.showAddAppointment(time, "30", this.props.date); this.props.setTimes(this.props.date, time, 30); this.props.therapistEvent.therapistId = sessionStorage.getItem("id"); this.props.appointment.therapistId = sessionStorage.getItem("id");}} class="halfHour printGrey"><div class="hide">+</div></div>
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
    }

    getAppointmentsForWeek()
    {
        const items = [];
        var date = new Date(this.props.date);
        while (date.getDay() != 0) //get Sunday
        {
            date.setDate(date.getDate() - 1);
        }
        for (let i = 0; i < 7; i++)
        {
            items.push(this.getAppointmentElements(this.getAppointments(date)));
            date.setDate(date.getDate() + 1);
        }
        return items;
    }

    getTherapistEventsForWeek()
    {
        const items = [];
        var date = new Date(this.props.date);
        while (date.getDay() != 0) //get Sunday
        {
            date.setDate(date.getDate() - 1);
        }
        for (let i = 0; i < 7; i++)
        {
            items.push(this.getTherapistEventElements(this.getTherapistEvents(date)));
            date.setDate(date.getDate() + 1);
        }
        return items;
    }

    getAppointments(date) {
        const appointments = [];
        for (let i = 0; i < this.props.appointments.length; i++)
        {
            let appointment = this.props.appointments[i]
            let start = new Date(appointment.startTime);
            if (start.getFullYear() === date.getFullYear() &&
            start.getMonth() === date.getMonth() &&
            start.getDate() === date.getDate())
            appointments.push(this.props.appointments[i]);
        }
        return appointments;   
    }

    getTherapistEvents(date)    {
        const appointments = [];
        for (let i = 0; i < this.props.therapistEvents.length; i++)
        {
            let appointment = this.props.therapistEvents[i]
            let start = new Date(appointment.startTime);
            if (start.getFullYear() === date.getFullYear() &&
            start.getMonth() === date.getMonth() &&
            start.getDate() === date.getDate())
            appointments.push(this.props.therapistEvents[i]);
        }
        return appointments; 
    }

    getAppointmentElements(appointments)   {
        var appointmentElements = []; 
        appointments.forEach(appointment => {
            var start = new Date(appointment.startTime);
            var end = new Date(appointment.endTime);
            var position = (start.getHours() - 5) * 52 + start.getMinutes() * 52/60;
            var style = {
                    top: position,
                    height: Math.abs(end - start) / 36e5 * 52, 
                    minHeight: Math.abs(end - start) / 36e5 * 52
                };
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

    getTherapistEventElements(therapistEvents)
    {
        var therapistEventElements = []; 
        therapistEvents.forEach(therapistEvent => {
        var start = new Date(therapistEvent.startTime);
        var end = new Date(therapistEvent.endTime);
        var position = (start.getHours() - 5) * 52 + start.getMinutes() * 52/60;
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

    getTherapistEventButtons(num, therapistEvent)
    {
        const items = [];
        if (this.props.role == "admin" || (this.props.role == "therapist" && therapistEvent.therapistId == sessionStorage.getItem("id")))
        items.push(
            <button class="editAppointmentButton" id={"deleteAppointmentButton" + num} onClick={() => this.deleteTherapistEvent(therapistEvent.eventId)}>Delete</button>
        )
        return items;
    }

    deleteTherapistEvent(id)
    {
        axios.delete("http://10.29.163.20:8081/api/therapistevent/" + id).then((response) => {
            console.log(response);
            this.props.getTherapistEvents(sessionStorage.getItem("id"));
        }).catch((error) => {
            console.log("Error caught");
            console.log(error);
        });
    }

    deleteAppointment(id)
    {
        axios.delete("http://10.29.163.20:8081/api/appointment/" + id).then((response) => {
            console.log(response);
            this.props.getAppointments(sessionStorage.getItem("id"));
        }).catch((error) => {
            console.log("Error caught");
            console.log(error);
        });
    }

    showAddAppointment(hour, minute, date, role)   {
        document.getElementById("addAppointment").style.display = "block";
        let time = "";
        if (hour < 10)
            time = "0" + hour + ":" + minute;
        else
            time = hour + ":" + minute;
        console.log(time);
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
        var therapistElements = document.getElementsByClassName("selectTherapist");
        for (let i = 0; i < therapistElements.length; i++)
        {
            therapistElements[i].selectedIndex = 1;
        }
        this.props.appointment.therapistId = sessionStorage.getItem("id");
    }

  render() {
    this.time = {value: loadTimeLine()} //Update timeline
    this.days = {values: getDays(this.props.date)};
    const appointments = this.getAppointmentsForWeek();
    const therapistEvents = this.getTherapistEventsForWeek();
    return (
        <div id="therapistSchedule">
            <div id="days">
                <div id="topRow"></div>
                {this.days.values}              
            </div>
            <div id="scheduleContainer">
                {this.time.value}
                <div id="hourColumn">
                    {this.hours.values}
                </div>
                <div class="day Sunday"> 
                    {appointments[0]} 
                    {therapistEvents[0]}       
                    {this.loadLines(0)}
                </div>
                <div class="day Monday">   
                    {appointments[1]} 
                    {therapistEvents[1]}                  
                    {this.loadLines(1)}
                </div>
                <div class="day Tuesday">
                    {appointments[2]}  
                    {therapistEvents[2]} 
                    {this.loadLines(2)}
                </div>
                <div class="day Wednesday">
                    {appointments[3]}  
                    {therapistEvents[3]} 
                    {this.loadLines(3)}
                </div>
                <div class="day Thursday">
                    {appointments[4]}  
                    {therapistEvents[4]} 
                    {this.loadLines(4)}
                </div>
                <div class="day Friday">
                    {appointments[5]}  
                    {therapistEvents[5]} 
                    {this.loadLines(5)}
                </div>
                <div class="day Saturday"> 
                    {appointments[6]}  
                    {therapistEvents[6]} 
                    {this.loadLines(6)}
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
            </div>
        </div>
    );
  }
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

var position = 0;
function getPositionForTimeLine()
{
    var d = new Date();
    var hour = d.getHours() - 5;
    var minute = d.getMinutes();
    return hour * 52 + minute * 52/60;
}

function getDays(newDate)
{
    var d = new Date(newDate);
    while (d.getDay() != 0) //get Monday
    {
        d.setDate(d.getDate() - 1);
    }
    const days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    const dayElements = [];
    for (var i = 0; i < 7; i++)
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

export default TherapistSchedule;