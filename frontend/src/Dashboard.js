import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import TherapistSchedule from "./ScheduleViews/TherapistSchedule";
import RoomSchedule from "./ScheduleViews/RoomSchedule";
import AllTherapistSchedule from "./ScheduleViews/AllTherapistSchedule";
import PatientSchedule from "./ScheduleViews/PatientSchedule";
import AddAppointment from "./AddAppointment";
import CopyAppointment from "./CopyAppointment"
import ReactToPrint from "react-to-print";
import axios from "axios";

class Dashboard extends React.Component {
    constructor(props) {
        super(props);
        if (this.props.role == "therapist")
            this.scheduleHeader = "My Schedule";
        else
            this.scheduleHeader = "Therapist Schedule";
        this.state = {
            locations: [],
            weekChanged: false,
            schedule: 1,
            date: new Date(),
            location: null,
            rooms: [],
            update: 0,
            therapistEvents: [],
            appointments: []
        };
        this.therapistEvent = {
            startTime: new Date(),
            endTime: new Date(),
            therapistId: null,
            active: true,
            activityName: null,
            notes: ""
          };
        this.appointment = {
            startTime: new Date(),
            endTime: new Date(),
            therapistId: null,
            active: true,
            notes: "",
            pmrPhysicianId: null,
            patientId: null,
            roomNumber: null,
            adl: null,
            locationId: null
        }
        this.appointmentCopy = {
            startTime: new Date(),
            endTime: new Date(),
            therapistId: null,
            active: true,
            notes: "",
            pmrPhysicianId: null,
            patientId: null,
            roomNumber: null,
            adl: null,
            locationId: null
        }
        this.copyDates = {
            from: new Date(),
            to: new Date()
        }

        this.getRooms = this.getRooms.bind(this);
        this.setLocation = this.setLocation.bind(this);
        this.getTherapistEvents = this.getTherapistEvents.bind(this);
        this.getAppointments = this.getAppointments.bind(this);
        this.setTimes = this.setTimes.bind(this);
        this.copyAppointment = this.copyAppointment.bind(this);
    }
    
    copyAppointment(id)  
    {
        for(let j = 0; j < this.state.appointments.length; j++) //get appointments for this therapist
        {
            if (this.state.appointments[j].appointmentId == id)
            {
                document.getElementById("copyAppointment").style.display = "block";
                let appointment = this.state.appointments[j]
                console.log(appointment.startTime.substring(11, 16));
                document.getElementById("copyStart").value = appointment.startTime.substring(11, 16);
                console.log(appointment.endTime.substring(11, 16));
                document.getElementById("copyEnd").value = appointment.endTime.substring(11, 16);
                console.log(appointment.startTime.substring(0, 10));
                document.getElementById("copyDate").value = appointment.startTime.substring(0, 10);
                this.appointmentCopy.startTime = new Date(appointment.startTime);
                this.appointmentCopy.endTime = new Date(appointment.endTime);
                this.appointmentCopy.locationId = appointment.locationId;
                this.appointmentCopy.roomNumber = appointment.roomNumber;
                this.appointmentCopy.patientId = appointment.patientId;
                this.appointmentCopy.therapistId = appointment.therapistId;
                this.appointmentCopy.pmrPhysicianId = appointment.pmrPhysicianId;
                this.appointmentCopy.adl = appointment.adl;
                this.appointmentCopy.notes = appointment.notes;
                return;
            }
        }
    }

    setTimes(date, hour, minute)
    {
        this.therapistEvent.startTime = new Date(date);
        this.therapistEvent.endTime = new Date(date);
        this.therapistEvent.startTime.setHours(hour);
        this.therapistEvent.startTime.setMinutes(minute);
        this.therapistEvent.endTime.setHours(hour + 1);
        this.therapistEvent.endTime.setMinutes(minute);
        this.appointment.startTime = new Date(date);
        this.appointment.endTime = new Date(date);
        this.appointment.startTime.setHours(hour);
        this.appointment.startTime.setMinutes(minute);
        this.appointment.endTime.setHours(hour + 1);
        this.appointment.endTime.setMinutes(minute);
    }

    //
    componentDidMount() {
        const url = "http://10.29.163.20:8081/api/Location";
        
        axios
        .get(url)
        // .then((json = {}) => json.data)
        .then((response) => {
            const locations = response.data;
            this.setState({ locations });
            this.setState({location: locations[0]});
        });

        axios
        .get("http://10.29.163.20:8081/api/room")
        .then((response) => {
            const rooms = response.data;
            this.setState({ rooms });
        });

        if (this.props.role == "therapist")
        {
            this.setState({schedule: 1});
        }
        else 
        {
            this.setState({schedule: 2});
        }
        this.weeks = this.getDropdownDates();
    }

    getTherapistEvents(therapistId) {
        if (!therapistId) //get for 3 views
        {
            var start = new Date(this.state.date);
            var end = new Date(this.state.date);
            start.setHours(0);
            start.setMinutes(0);
            start.setMinutes(0);
            end.setHours(15);
            end.setMinutes(0);
            end.setMinutes(0);
            
            const event = ({
                startTime: start,
                endTime: end
            })

            axios
            .post("http://10.29.163.20:8081/api/therapistevent/getTherapistEvent", event)
            .then((response) => {
                console.log(response.data);
                const therapistEvents = response.data;
                this.setState({ therapistEvents });
            })
            .catch((error) => {
                console.log("Error caught");
                console.log(error);
            });      
        } 
        else 
        {
            var start = new Date(this.state.date);
            var end = new Date(this.state.date);
            start.setHours(0);
            start.setMinutes(0);
            start.setMinutes(0);
            end.setHours(15);
            end.setMinutes(0);
            end.setMinutes(0);
            while (start.getDay() != 0) //get Sunday
            {
                start.setDate(start.getDate() - 1);
            }
            while (end.getDay() != 6) //get Sunday
            {
                end.setDate(end.getDate() + 1);
            }
            
            const event = ({
                startTime: start,
                endTime: end,
                therapistId: therapistId
            })

            axios
            .post("http://10.29.163.20:8081/api/therapistevent/getTherapistEventsByTherapistId", event)
            .then((response) => {
                console.log(response.data);
                const therapistEvents = response.data;
                this.setState({ therapistEvents });
            })
            .catch((error) => {
                console.log("Error caught");
                console.log(error);
            });      
        } 
    }

    getAppointments(therapistId) {
        if (!therapistId) //get for 3 views
        {
            var start = new Date(this.state.date);
            var end = new Date(this.state.date);
            start.setHours(0);
            start.setMinutes(0);
            start.setMinutes(0);
            end.setHours(15);
            end.setMinutes(0);
            end.setMinutes(0);

            const event = ({
                startTime: start,
                endTime: end,
                adl: "null"
            })

            axios
            .post("http://10.29.163.20:8081/api/appointment/getAppointments", event)
            .then((response) => {
                console.log(response.data);
                const appointments = response.data;
                this.setState({ appointments });
            })
            .catch((error) => {
                console.log("Error caught");
                console.log(error);
            });      
        }
        else //get for therapistview / my schedule view
        {
            var start = new Date(this.state.date);
            var end = new Date(this.state.date);
            start.setHours(0);
            start.setMinutes(0);
            start.setMinutes(0);
            end.setHours(15);
            end.setMinutes(0);
            end.setMinutes(0);
            while (start.getDay() != 0) //get Sunday
            {
                start.setDate(start.getDate() - 1);
            }
            while (end.getDay() != 6) //get Sunday
            {
                end.setDate(end.getDate() + 1);
            }

            const event = ({
                startTime: start,
                endTime: end,
                therapistId: therapistId,
                adl: "null"
            })

            axios
            .post("http://10.29.163.20:8081/api/appointment/getAppointmentsByTherapistId", event)
            .then((response) => {
                console.log(response.data);
                const appointments = response.data;
                this.setState({ appointments });
            })
            .catch((error) => {
                console.log("Error caught");
                console.log(error);
            });
        } 
    }

    setLocation(locationId)
    {
        this.state.locations.forEach(l => {
        if(l.locationId == locationId)
        {
            if (this.state.schedule == 3 || this.state.schedule == 4)
                this.locationHeader=l.name + ' - ';
            this.setState({location: l});
        }
        });
    }

    getRooms()
    {
        const elements = [];
        this.state.rooms.forEach(room => {
        if (this.state.location != null && room.locationId == this.state.location.locationId)
            elements.push(<option value={room.number}>{room.number}</option>);
        })
        return elements;
    }

    updateAppointments()  {

        if (this.state.schedule != 1)
        {
            this.getAppointments();
            this.getTherapistEvents();
        }
        else
        {
            this.getAppointments(sessionStorage.getItem("id"));
            this.getTherapistEvents(sessionStorage.getItem("id"));
        }
    }
    

    getDropdownDates()  {
        let d = new Date();
        while (d.getDay() != 0) //get Sunday
        {
            d.setDate(d.getDate() - 1);
        }
        this.week = d.toLocaleDateString('en-US')
        var elements = [];
        d.setDate(d.getDate() - 7);
        d.setDate(d.getDate() - 7);
        for(let i = 0; i < 5; i++)
        {
            let newDate = new Date(d);
            elements.push(
                <div class="link" onClick={() => {this.setState({date: new Date(newDate)}); this.week=newDate.toLocaleDateString('en-US'); this.updateAppointments() }}>{newDate.toLocaleDateString('en-US')}</div>
            );
            d.setDate(d.getDate() + 7);
        }
        return elements;
    }
    
    setSchedule(newDate)   {
        console.log("reached");
        if (!newDate)
            newDate = this.state.date;
        this.schedule = <div></div>;
        switch(this.state.schedule) {
            case 1:
                return (<TherapistSchedule date={newDate} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role} appointment={this.appointment} getTherapistEvents={this.getTherapistEvents} therapistEvents={this.state.therapistEvents} getAppointments={this.getAppointments} appointments={this.state.appointments} setTimes={this.setTimes} copyAppointment={this.copyAppointment}/>);
            case 2: 
                return (<AllTherapistSchedule date={newDate} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role} appointment={this.appointment} getTherapistEvents={this.getTherapistEvents} therapistEvents={this.state.therapistEvents} getAppointments={this.getAppointments} appointments={this.state.appointments} setTimes={this.setTimes} copyAppointment={this.copyAppointment}/>);
            case 3: 
                return (<RoomSchedule date={newDate} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role} location={this.state.location} appointment={this.appointment} getAppointments={this.getAppointments} appointments={this.state.appointments} setTimes={this.setTimes} copyAppointment={this.copyAppointment}/>);
            case 4:
                return (<PatientSchedule date={newDate} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role} location={this.state.location} appointment={this.appointment} getAppointments={this.getAppointments} appointments={this.state.appointments} setTimes={this.setTimes} copyAppointment={this.copyAppointment}/>);
        }
    }

    getByRoomLinks()   {
        var byRoomLinks = [];
        this.state.locations.forEach(location => {
            byRoomLinks.push(<div class="link" onClick={() => { this.scheduleHeader = "Room Schedule"; this.setState({location}); this.setState({schedule: 3}); this.locationHeader=location.name + ' - '; this.setState({schedule: 3});}}>{location.name}</div>);
        });
        return byRoomLinks;
    }

    getByPatientLinks()   {
        var byRoomLinks = [];
        this.state.locations.forEach(location => {
            byRoomLinks.push(<div class="link" onClick={() => {this.scheduleHeader = "Patient Schedule"; this.setState({location}); this.setState({schedule: 4}); this.locationHeader=location.name + " - "; this.setState({schedule: 4});}}>{location.name}</div>);
        });
        return byRoomLinks;
    }

    getButtons()
    {
        const items = [];
        if (this.props.role == "admin")
        items.push(
            <button style={{marginRight: "30px"}} class="topbtn" onClick={() => hideCopyDayForm()}>
                <img src={require("./Icons/icons8-duplicate-48.png")} alt="edit" height="30" />
            </button>
        );
        if (this.props.role == "admin" || this.props.role == "therapist")
        items.push(
            <button class="topbtn" onClick={() => showAddAppointment()}>
                <img src={require("./Icons/icons8-add-property-48.png")} alt="edit" height="30" />
            </button>
        );
        return items;
    }

    getSchedule()   {
        console.log(this.props.role);
        if (this.props.role == "therapist")
            return (<div class="link" onClick={() => {this.scheduleHeader = "My Schedule"; this.locationHeader=""; this.setState({schedule: 1});}}>My schedule</div>);
        else return (<div class="link">&nbsp;</div>);
    }

    setDate(event, object)
    {
        object.setFullYear(parseInt(event.substring(0, 4)));
        object.setMonth(parseInt(event.substring(5, 7)) - 1);
        object.setDate(parseInt(event.substring(8, 10)));
    }

    copyDay()   {   //copies all appointments from one day to another day
        var start = new Date(this.copyDates.from);
        var end = new Date(this.copyDates.from);
        start.setHours(0);
        start.setMinutes(0);
        start.setMinutes(0);
        end.setHours(15);
        end.setMinutes(0);
        end.setMinutes(0);

        const event = ({
            startTime: start,
            endTime: end,
            adl: "null"
        })
        axios.post("http://10.29.163.20:8081/api/appointment/getAppointments", event)
        .then((response) => {
            const appointments = response.data;
            console.log(appointments);
            var start = new Date(this.copyDates.to);
            var end = new Date(this.copyDates.to);
            start.setHours(0);
            start.setMinutes(0);
            start.setMinutes(0);
            end.setHours(15);
            end.setMinutes(0);
            end.setMinutes(0);
            appointments.forEach(appointment => {
                appointment.startTime = new Date(appointment.startTime);
                appointment.endTime = new Date(appointment.endTime);
                appointment.startTime.setFullYear(this.copyDates.to.getFullYear());
                appointment.startTime.setMonth(this.copyDates.to.getMonth());
                appointment.startTime.setDate(this.copyDates.to.getDate());
                appointment.endTime.setFullYear(this.copyDates.to.getFullYear());
                appointment.endTime.setMonth(this.copyDates.to.getMonth());
                appointment.endTime.setDate(this.copyDates.to.getDate());
                appointment.startTime.setHours(appointment.startTime.getHours() - 5); //account for timezone
                appointment.endTime.setHours(appointment.endTime.getHours() - 5);
                delete appointment.appointmentId;
                axios.post("http://10.29.163.20:8081/api/appointment", appointment)
                    .then((response) => {
                    console.log("Success");
                    console.log(response);
                    if (this.props.schedule != 1)
                        this.props.getAppointments();
                    else
                        this.props.getAppointments(sessionStorage.getItem("id"));
                    })
                    .catch((error) => {
                    console.log("Error caught");
                    console.log(error);
                    });

            });

        })
        .catch((error) => {
            console.log("Error caught");
            console.log(error);
        }); 
    }

    render() {
        this.schedule = this.setSchedule(this.state.date);
        return (
        <div id="screen">
            <div class="pageContainer">
                <div class="dropdown">
                    <button class="dropbtn">
                        {this.scheduleHeader}
                        <i class="arrow down"></i>
                    </button>
                    <div class="dropdown-content">
                        {this.getSchedule()}
                        <div class="link" onClick={() => {this.scheduleHeader = "Therapist Schedule"; this.locationHeader=""; this.setState({schedule: 2});}}>By therapist</div>
                        <div class="byRoom link">By room
                            <div class="locations">
                                {this.getByRoomLinks()}
                            </div>
                        </div>
                            
                        <div class="byPatient link">By patient
                        <div class="locations">
                                {this.getByPatientLinks()}
                            </div>
                        </div>
                        
                    </div>
                </div>
                <div id="scheduleTitle">
                    {this.locationHeader} Week of  
                    <div class="datedropdown">
                    <button class="datedropbtn">{this.week}<i class="datearrow down"></i></button>
                        <div class="datedropdown-content">
                            {this.weeks}
                        </div>
                    </div>
                </div>
    
                
                {this.getButtons()}
    
                <ReactToPrint trigger={() => <button class="topbtn"><img src={require("./Icons/icons8-print-48.png")} alt="edit" height="30" /></button>} 
                onBeforeGetContent={() => showTimes()}
                onAfterPrint={() => {hideTimes(); this.schedule = this.setSchedule(this.state.date); this.forceUpdate();}}
                documentTitle={this.scheduleHeader}
                content={() => this.schedule} />
                
    
                <div id="copyDayForm">
                    <div class="form-style">
                    <form>
                        <span>Copy from: <span class="required">* </span></span>
                        <input style={{width: "160px", marginBottom: "10px"}} class="input-field" type="date" name="date" onChange={e => {this.setDate(e.target.value, this.copyDates.from); console.log(this.copyDates.from)}}></input>
    
                        <span>Copy to: <span class="required">* </span></span>
                        <input style={{width: "160px", marginBottom: "10px"}} class="input-field" type="date" name="date" onChange={e => {this.setDate(e.target.value, this.copyDates.to); console.log(this.copyDates.to)}}></input>
    
                        
                        <button style={{width: "100%"}} type="button" color="primary" onClick={() => this.copyDay()}>Copy Day</button>
                    </form>
                    </div>
                </div>
                <div>{this.schedule}</div>
    
            </div>
            <AddAppointment therapistEvent={this.therapistEvent} appointment={this.appointment} getRooms={this.getRooms} location={this.state.location} setLocation={this.setLocation} getTherapistEvents={this.getTherapistEvents} getAppointments={this.getAppointments} schedule={this.state.schedule}/> 
            <CopyAppointment getAppointments={this.getAppointments} copyAppointment={this.copyAppointment} appointment={this.appointmentCopy} schedule={this.state.schedule}/> 
        </div>
        );
      }
    }

function showAddAppointment() {
  document.getElementById("addAppointment").style.display = "block";
}

function showTimes() {
  setSizes();
  hideMetrics();
  var x = document.getElementsByClassName("printHours");
  for (var i = 0; i < x.length; i++) {
    x[i].style.width = "75px";
  }
}

function hideMetrics() {
  if (document.getElementById("metricCheck") != null)
    document.getElementById("metricCheck").checked = false;
  var cols = document.getElementsByClassName("therapistMetrics");
  for (var i = 0; i < cols.length; i++) {
    cols[i].style.display = "none";
  }
}

function setSizes() {
  var cols = document.getElementsByClassName("room");
  for (var i = 0; i < cols.length; i++) {
    cols[i].style.minWidth = "100px";
    cols[i].style.width = "100px";
  }
  var cols = document.getElementsByClassName("therapist");
  for (var i = 0; i < cols.length; i++) {
    cols[i].style.minWidth = "100px";
    cols[i].style.width = "100px";
  }
}

function hideTimes() {
  var x = document.getElementsByClassName("printHours");
  for (var i = 0; i < x.length; i++) {
    x[i].style.width = "0px";
  }
}

var copyDayFormIsHidden = true;
function hideCopyDayForm() {
  if (copyDayFormIsHidden) {
    copyDayFormIsHidden = false;
    document.getElementById("copyDayForm").style.display = "block";
  } else {
    copyDayFormIsHidden = true;
    document.getElementById("copyDayForm").style.display = "none";
  }
}

export default Dashboard;
