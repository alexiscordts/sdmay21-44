import React from "react";
import "./FormStyles.css";
import axios from "axios";
import Draggable from 'react-draggable';

class copyAppointment extends React.Component {
  constructor(props) {
    super(props);
    
    this.postAppointment = this.postAppointment.bind(this);
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
      document.getElementById("copyAppointment").style.display = "block";
  }

  setStartTime(event)
  {
    this.props.appointment.startTime.setHours(parseInt(event.substring(0, 2), 10));
    this.props.appointment.startTime.setMinutes(parseInt(event.substring(3, 5), 10));
  }

  setEndTime(event)
  {
    this.props.appointment.endTime.setHours(parseInt(event.substring(0, 2), 10));
    this.props.appointment.endTime.setMinutes(parseInt(event.substring(3, 5), 10));
  }

  setDate(event)
  {
    this.props.appointment.startTime.setFullYear(parseInt(event.substring(0, 4)));
    this.props.appointment.startTime.setMonth(parseInt(event.substring(5, 7)) - 1);
    this.props.appointment.startTime.setDate(parseInt(event.substring(8, 10)));
    this.props.appointment.endTime.setFullYear(parseInt(event.substring(0, 4)));
    this.props.appointment.endTime.setMonth(parseInt(event.substring(5, 7)) - 1);
    this.props.appointment.endTime.setDate(parseInt(event.substring(8, 10)));
  }

  render() {
    return (
        <Draggable
        style = "background"
        handle=".handle"
        defaultPosition={{x: 0, y: 0}}
        position={null}
        scale={1}
        onStart={this.handleStart}
        onDrag={this.handleDrag}
        onStop={this.handleStop}>
        <div id="copyAppointment">
        <div id="dragMe" class="handle">
        <div id="close" onClick={() => hidecopyAppointment()}>X</div>

        </div>
        <div id="copyFormHeader">Copy Appointment</div>
        <div class="form-style">

        <form id="addApptForm">

        <div class="AppointmentFormRow">
              <span>Start <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field startTime" type="time" id="copyStart" name="start" min="05:00" max="19:00" onChange={e => this.setStartTime(e.target.value)}></input>

              <span>&nbsp; End <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field endTime" type="time" id="copyEnd" name="end" min="05:00" max="19:00" onChange={e => this.setEndTime(e.target.value)}></input> 

              <span class="required">&nbsp; * </span>
              <input style={{width: "140px"}} class="input-field date" type="date" id="copyDate" name="date" onChange={e => this.setDate(e.target.value)} required></input>
            </div>

            <div class="buttonContainer">
              <button type="button" color="primary" onClick={this.postAppointment}>Copy</button>
            </div>
        </form>
        </div>
        </div>
    </Draggable>
      );
    }
  }

function hidecopyAppointment()   {
    document.getElementById("copyAppointment").style.display = "none";
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

export default copyAppointment;