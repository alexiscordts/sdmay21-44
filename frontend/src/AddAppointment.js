import React from "react";
import "./FormStyles.css";
import axios from "axios";
import Draggable from 'react-draggable';

class AddAppointment extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      userList: [],
      permissions: [],
      locations: [],
      therapyTypes: [],
      ADLs: [],
      therapies: [],
      patients: [],
      rooms: [],
    };
    
    this.postTherapistEvent = this.postTherapistEvent.bind(this);
    this.postAppointment = this.postAppointment.bind(this);
    this.clear = this.clear.bind(this);
  }

  componentDidMount() {
    const url = "http://10.29.163.20:8081/api/";
    axios.get(url + "user").then((response) => {
      const userList = response.data;
      this.setState({ userList });
    });

    axios.get("http://10.29.163.20:8081/api/permission").then((response) => {
      this.setState({
        permissions: this.state.permissions.concat(response.data),
      });
    });

    axios
      .get("http://10.29.163.20:8081/api/Location")
      .then((response) => {
        const locations = response.data;
        this.setState({ locations });
      });

      axios
      .get("http://10.29.163.20:8081/api/therapymain")
      .then((response) => {
        const therapyTypes = [];
        response.data.forEach(therapy => {
            therapyTypes.push(therapy.type);
        });
        this.setState({ therapyTypes });
      });

      axios
      .get("http://10.29.163.20:8081/api/therapy/adl")
      .then((response) => {
        const ADLs = response.data;
        this.setState({ ADLs });
      });

      axios
      .get("http://10.29.163.20:8081/api/therapy")
      .then((response) => {
        const therapies = response.data;
        this.setState({ therapies });
      });

      axios
      .get("http://10.29.163.20:8081/api/patient")
      .then((response) => {
        const patients = response.data;
        this.setState({ patients });
      });

      axios
      .get("http://10.29.163.20:8081/api/room")
      .then((response) => {
        const rooms = response.data;
        this.setState({ rooms });
      });
      
  }
    
  getTherapistOptions(users, therapists)
  {
    console.log(sessionStorage.getItem("role"));
    var cols = [];
        if (sessionStorage.getItem("role") == "admin")
          try {
          therapists.forEach(therapist => {
              users.forEach(user => {
                  if (therapist.role == "therapist" && user.userId == therapist.userId)
                      cols.push(<option value={user.userId}>{user.firstName + " " + user.lastName}</option>);
              });
              
          });
          }
          catch (error)
          {
              console.log(error);
          }
          else if (sessionStorage.getItem("role") == "therapist")
          cols.push(<option value={sessionStorage.getItem("id")}>{sessionStorage.getItem("firstname") + " " + sessionStorage.getItem("lastname")}</option>);
        return cols;
  }

  getPhysicianOptions()
  {
    var cols = [];
      try {
      this.state.permissions.forEach(permission => {
          this.state.userList.forEach(user => {
              if (permission.role == "physician" && user.userId == permission.userId)
                  cols.push(<option value={user.userId}>{user.firstName + " " + user.lastName}</option>);
          });
      });
      }
      catch (error)
      {
          console.log(error);
      }
      return cols;
  }

  getLocations(locations)
  {
    var locationElements = [];
    locations.forEach(location => {
      locationElements.push(<option value={location.locationId}>{location.name}</option>);
    }
    )
    return locationElements;
  }

  getTherapyTypes(therapyTypes)
  {
    var therapyElements = [];
    therapyTypes.forEach(therapy => {
      therapyElements.push(<option value={therapy}>{therapy}</option>);
    }
    )
    return therapyElements;
  }

  getADLs(adls)
  {
    var adlElements = [];
    adls.forEach(adl => {
      adlElements.push(<option value={adl}>{adl}</option>);
    })
    return adlElements;
  }

  getPatients(patients)
  {
    const elements = [];
    patients.forEach(patient => {
      elements.push(<option value={patient.patientId}>{patient.firstName + " " + patient.lastName}</option>);
      })
      return elements;
  }

  getRooms()
  {
    const elements = [];
    this.state.rooms.forEach(room => {
      if (room.locationId == this.props.location.locationId)
        elements.push(<option value={room.number}>{room.number}</option>);
      })
      return elements;
  }

  changeADLs(therapyType)  {
    var therapyTypes = [];
    this.state.therapies.forEach(therapy => {
        if (therapy.type == therapyType)
          therapyTypes.push(therapy.adl);
    })
    document.getElementById("ADL").selectedIndex = 0;
    return therapyTypes;
  }

  postTherapistEvent(event)  {
    event.preventDefault();
    var therapistEvent = { //make hard copy
      startTime: new Date(this.props.therapistEvent.startTime),
      endTime: new Date(this.props.therapistEvent.endTime),
      therapistId: this.props.therapistEvent.therapistId,
      active: true,
      activityName: this.props.therapistEvent.activityName,
      notes: this.props.therapistEvent.notes
    }
    therapistEvent.startTime.setHours(therapistEvent.startTime.getHours() - 5); //account for timezone offset
    therapistEvent.endTime.setHours(therapistEvent.endTime.getHours() - 5);
    console.log(therapistEvent);
    console.log(this.props.therapistEvent);
    axios
      .post("http://10.29.163.20:8081/api/therapistevent/", therapistEvent)
      .then((response) => {
        console.log("Success");
        console.log(response);
        if (this.props.schedule != 1)
          this.props.getTherapistEvents();
        else 
          this.props.getTherapistEvents(sessionStorage.getItem("id"));
      })
      .catch((error) => {
        console.log("Error caught");
        console.log(error);
      });
  }

  postAppointment(event)  {
    event.preventDefault();
    console.log(this.props.appointment);
    var appointment = { //make hard copy
      startTime: new Date(this.props.appointment.startTime),
      endTime: new Date(this.props.appointment.endTime),
      therapistId: this.props.appointment.therapistId,
      active: true,
      notes: this.props.appointment.notes,
      pmrPhysicianId: this.props.appointment.pmrPhysicianId,
      patientId: this.props.appointment.patientId,
      roomNumber: this.props.appointment.roomNumber,
      adl: this.props.appointment.adl,
      locationId: this.props.appointment.locationId
    }
    appointment.startTime.setHours(appointment.startTime.getHours() - 5); //account for timezone
    appointment.endTime.setHours(appointment.endTime.getHours() - 5);
    //console.log(appointment);
    console.log(this.props.appointment);
    
    axios
      .post("http://10.29.163.20:8081/api/appointment", appointment)
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
  }

  setStartTime(event, object)
  {
    object.startTime.setHours(parseInt(event.substring(0, 2), 10));
    object.startTime.setMinutes(parseInt(event.substring(3, 5), 10));
  }

  setEndTime(event, object)
  {
    object.endTime.setHours(parseInt(event.substring(0, 2), 10));
    object.endTime.setMinutes(parseInt(event.substring(3, 5), 10));
  }

  setDate(event, object)
  {
    object.startTime.setFullYear(parseInt(event.substring(0, 4)));
    object.startTime.setMonth(parseInt(event.substring(5, 7)) - 1);
    object.startTime.setDate(parseInt(event.substring(8, 10)));
    object.endTime.setFullYear(parseInt(event.substring(0, 4)));
    object.endTime.setMonth(parseInt(event.substring(5, 7)) - 1);
    object.endTime.setDate(parseInt(event.substring(8, 10)));
  }

  changeRoomToPatientRoom(patientId)  {
    console.log(patientId);
    this.state.patients.forEach(patient => {
      if (patient.patientId == patientId)
      {
        document.getElementById("location").value = patient.locationId;
        this.setState({location: patient.locationId});
        console.log(patient.roomNumber);
      }
    })
  }

  setSelectedPatient(room)
  {
    var found = false;
    this.state.patients.forEach(patient => {
      if (patient.locationId == this.props.location.locationId && patient.roomNumber == room)
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
    if(sessionStorage.getItem("role") == "therapist")
    {
        this.props.appointment.therapistId = sessionStorage.getItem("id");
        document.getElementById("appointmentTherapist").selectedIndex = 1;
    }
  }
  
  fillPatientInfo(patientId)
  {
    var found = false;
    this.state.patients.forEach(patient => {
      if (patient.patientId == patientId)
      {
        document.getElementById("appointmentTherapist").value = patient.therapistId;
        this.props.appointment.therapistId = patient.therapistId;
        document.getElementById("physician").value = patient.pmrPhysicianId;
        this.props.appointment.pmrPhysicianId = patient.pmrPhysicianId;
        found = true;
      }
    })
    if (found == false)
    {
      document.getElementById("appointmentTherapist").selectedIndex = 0;
      document.getElementById("physician").selectedIndex = 0;
    }
    if(sessionStorage.getItem("role") == "therapist")
    {
        this.props.appointment.therapistId = sessionStorage.getItem("id");
        document.getElementById("appointmentTherapist").selectedIndex = 1;
    }
  }

  clear() {
    var selectElements = document.getElementsByClassName("select-field");
    for (var i = 0; i < selectElements.length; i++)
    {
        selectElements[i].selectedIndex = 0;
    }
    var startElements = document.getElementsByClassName("startTime");
    for (let i = 0; i < startElements.length; i++)
    {
        startElements[i].value = null;
    }
    var endElements = document.getElementsByClassName("endTime");
    for (let i = 0; i < startElements.length; i++)
    {
        endElements[i].value = null;
    }
    var dateElements = document.getElementsByClassName("date");
    for (let i = 0; i < dateElements.length; i++)
    {
        dateElements[i].value = null;
    }
    this.props.therapistEvent.startTime = new Date();
    this.props.therapistEvent.endTime = new Date();
    this.props.therapistEvent.therapistId = null;
    this.props.therapistEvent.activityName = null;
    this.props.therapistEvent.notes = "";
    this.props.appointment.startTime = new Date();
    this.props.appointment.endTime = new Date();
    this.props.appointment.therapistId = null;
    this.props.appointment.notes = "";
    this.props.appointment.pmrPhysicianId = null;
    this.props.appointment.patientId = null;
    this.props.appointment.roomNumber = null;
    this.props.appointment.adl = null;
    this.props.appointment.locationId = null;
  }

  render() {
    return (
        <div id="addAppointmentContainer">
        <Draggable
        style = "background"
        handle=".handle"
        defaultPosition={{x: 0, y: 0}}
        position={null}
        scale={1}
        onStart={this.handleStart}
        onDrag={this.handleDrag}
        onStop={this.handleStop}>
        <div id="addAppointment">
        <div id="dragMe" class="handle">
        <div id="close" onClick={() => hideAddAppointment()}>X</div>

        </div>
        <div class="form-style">

          <div class="toggle" onClick={() => toggle()}>
            <div class="toggleOption" id="appointmentToggle">Add Appointment</div>
            <div class="toggleOption" id="otherToggle">Add Other</div>
          </div>

          <form id="addApptForm" action="" method="post">
            <div class="AppointmentFormRow">
            <span class="required"> * </span>
              <select style={{width: "150px"}} class="select-field" name="location" id="location" onChange={e => {this.props.setLocation(e.target.value); document.getElementById("room").selectedIndex="0"; this.props.appointment.locationId = parseInt(e.target.value); this.forceUpdate()}}>
                  <option value="select" selected disabled >Select Location</option>
                  {this.getLocations(this.state.locations)}
              </select>

              <span class="required">&nbsp;&nbsp; * </span>
                <select style={{width: "120px"}} class="select-field" name="room" id="room" onChange={e => {this.setSelectedPatient(e.target.value); this.props.appointment.roomNumber = parseInt(e.target.value)}}>
                    <option value="select" selected disabled >Select Room</option>
                    {this.props.getRooms()}
                </select>

                <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "150px"}} class="select-field" name="patient" id="patient" onChange={e => {this.props.appointment.patientId = parseInt(e.target.value); this.fillPatientInfo(e.target.value)}}>
                  <option value="select" selected disabled >Select Patient</option>
                  {this.getPatients(this.state.patients)}
              </select>
            </div>

            <div class="AppointmentFormRow"> 

              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "150px"}} class="select-field selectTherapist" id="appointmentTherapist" name="therapist" onChange={e => {this.props.appointment.therapistId = parseInt(e.target.value); this.props.appointment.adl = e.target.value}}>
                  <option value="select" selected disabled >Select Therapist</option>
                  {this.getTherapistOptions(this.state.userList, this.state.permissions)}
              </select>

              <span class="required"> * </span>
              <select style={{width: "150px"}} class="select-field" name="physician" id="physician" onChange={e => {this.props.appointment.pmrPhysicianId = e.target.value}}>
                  <option value="select" selected disabled>Select Physician</option>
                  {this.getPhysicianOptions()}
              </select>
            </div>

            <div class="AppointmentFormRow">
              <select style={{width: "200px"}} class="select-field" name="type" onChange={e => {this.setState({ADLs: this.changeADLs(e.target.value)});}}>
                  <option value="select" selected disabled >Select Therapy Type</option>
                  {this.getTherapyTypes(this.state.therapyTypes)}
              </select>

              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "200px"}} class="select-field" name="ADL" id="ADL" onChange={e => {this.props.appointment.adl = e.target.value}}>
                  <option value="select" selected disabled >Select ADL</option>
                  {this.getADLs(this.state.ADLs)}
              </select>
            </div>

            <div class="AppointmentFormRow">
              <span>Start <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field startTime" type="time" id="startInput" name="start" min="05:00" max="19:00" onChange={e => this.setStartTime(e.target.value, this.props.appointment)}></input>

              <span>&nbsp; End <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field endTime" type="time" id="endInput" name="end" min="05:00" max="19:00" onChange={e => this.setEndTime(e.target.value, this.props.appointment)}></input> 

              <span class="required">&nbsp; * </span>
              <input style={{width: "140px"}} class="input-field date" type="date" id="dateInput" name="date" onChange={e => this.setDate(e.target.value, this.props.appointment)} required></input>
            </div>

            <label for="notes"><span>Notes </span>
            <textarea class="textarea-field" style={{width: "100%"}} name="notes" rows="4" cols="100" onChange={e => this.props.appointment.notes = e.target.value}>
            </textarea>
            </label>
            

            <div class="buttonContainer">
              <button type="button" color="primary" onClick={this.clear}>Clear</button>
              <button type="button" color="primary" onClick={this.postAppointment}>Save</button>
            </div>
          </form>

          <form id="addOtherForm" action="" method="post">
            <div class="AppointmentFormRow">
              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "150px"}} class="select-field selectTherapist" name="therapist" onChange={e => this.props.therapistEvent.therapistId = e.target.value}>
                  <option value="select" selected disabled >Select Therapist</option>
                  {this.getTherapistOptions(this.state.userList, this.state.permissions)}
              </select>
            </div>

            <div class="AppointmentFormRow">
              <span>Start <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field startTime" type="time" name="appt" min="05:00" max="19:00" onChange={e => this.setStartTime(e.target.value, this.props.therapistEvent)}></input>

              <span>&nbsp; End <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field endTime" type="time" name="appt" min="05:00" max="19:00" onChange={e => this.setEndTime(e.target.value, this.props.therapistEvent)}></input> 

              <span class="required">&nbsp; * </span>
              <input style={{width: "140px"}} class="input-field date" type="date" name="date" onChange={e => this.setDate(e.target.value, this.props.therapistEvent)}></input>
            </div>

            <label for="notes"><span>Title <span class="required">* </span></span>
            <input type="text" class="input-field"  name="title" rows="4" cols="50" onChange={e => this.props.therapistEvent.activityName = e.target.value}>
            </input>
            </label>

            <label for="notes"><span>Notes </span>
            <textarea class="textarea-field" style={{width: "100%"}} name="notes" rows="4" cols="50" onChange={e => this.props.therapistEvent.notes = e.target.value}>
            </textarea>
            </label>
            
            <div class="buttonContainer">
              <button type="button" color="primary" onClick={this.clear}>Clear</button>
              <button type="button" color="primary" onClick={this.postTherapistEvent}>Save</button>
            </div>
          </form>

        </div>
        </div>
    </Draggable>     
    </div>
      );
    }
  }

function hideAddAppointment()   {
    document.getElementById("addAppointment").style.display = "none";
}

var isToggled = true;
function toggle() {
  if (isToggled)
  {
    console.log("toggle");
    isToggled = false;
    document.getElementById("appointmentToggle").style.backgroundColor = "#E6E7E9";
    document.getElementById("otherToggle").style.backgroundColor = "#EA7600";
    document.getElementById("addOtherForm").style.display = "block";
    document.getElementById("addApptForm").style.display = "none";
  }
  else
  {
    document.getElementById("appointmentToggle").style.backgroundColor = "#EA7600";
    document.getElementById("otherToggle").style.backgroundColor = "#E6E7E9";
    document.getElementById("addOtherForm").style.display = "none";
    document.getElementById("addApptForm").style.display = "block";
    isToggled = true;
  }
  
}

export default AddAppointment;