import React from "react";
import "./FormStyles.css";
import Draggable from 'react-draggable';

class AddAppointment extends React.Component {

    
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

              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "150px"}} class="select-field selectTherapist" name="therapist">
                  <option value="select" selected disabled hidden>Select Therapist</option>
                  <option value="sponge">Philip Adamson</option>
                  <option value="pat">Maegan Daugherty</option>
                  <option value="squid">Katelin Lynn</option>
                  <option value="krab">Lyndon Macdonald</option>
                  <option value="krab">Lacy Silva</option>
                  <option value="krab">Lola-Rose Lopez</option>
                  <option value="krab">Alexandria Small</option>
                  <option value="krab">Omar Needham</option>
                  <option value="krab">Delores Daniels</option>
              </select>

              <span class="required"> * </span>
              <select style={{width: "150px"}} class="select-field" name="patient" id="patient" onChange={() => changeRoomToPatientRoom()}>
                  <option value="select" selected disabled hidden>Select Patient</option>
                  <option value="1">Beatrice Coleman</option>
                  <option value="1">Vivian Allison</option>
                  <option value="1">Marsha Morgan</option>
                  <option value="1">Al Carr</option>
                  <option value="1">Jesus Sutton</option>
                  <option value="1">Pearl Robertson</option>
                  <option value="1">Marion Hammond</option>
                  <option value="1">Jim Chandler</option>
                  <option value="1">Kimberly Rodriquez</option>
                  <option value="1">Joel Ramsey</option>
              </select>

              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "120px"}} class="select-field" name="room" id="room">
                  <option value="select" selected disabled hidden>Select Room</option>
                  <option value="237">237</option>
                  <option value="123">123</option>
                  <option value="283">283</option>
                  <option value="283">111</option>
                  <option value="283">083</option>
                  <option value="283">162</option>
                  <option value="283">298</option>
                  <option value="283">293</option>
                  <option value="283">222</option>
                  <option value="283">105</option>
                  <option value="283">102</option>
                  <option value="283">112</option>
                  <option value="283">101</option>
                  <option value="283">103</option>
              </select>
            </div>

            <div class="AppointmentFormRow">
              <span class="required"> * </span>
              <select style={{width: "200px"}} class="select-field" name="type">
                  <option value="select" selected disabled hidden>Select Therapy Type</option>
                  <option value="sponge">Physical</option>
                  <option value="squid">Occupational</option>
                  <option value="pat">Speech</option>
                  <option value="rec">Recreational</option>
              </select>

              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "200px"}} class="select-field" name="therapist">
                  <option value="select" selected disabled hidden>Select ADL</option>
                  <option value="sponge">Upper body dressing</option>
                  <option value="squid">Grooming</option>
                  <option value="pat">Lower body dressing</option>
                  <option value="squid">Spinal cord education</option>
                  <option value="pat">Strength group</option>
              </select>
            </div>

            <div class="AppointmentFormRow">
              <span>Start <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field startTime" type="time" name="appt" min="05:00" max="19:00" ></input>

              <span>&nbsp; End <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field endTime" type="time" name="appt" min="05:00" max="19:00" ></input> 

              <span class="required">&nbsp; * </span>
              <input style={{width: "140px"}} class="input-field date" type="date" name="date"></input>
            </div>

            <label for="notes"><span>Notes </span>
            <textarea class="textarea-field" style={{width: "100%"}} name="notes" rows="4" cols="100">
            </textarea>
            </label>
            

            <div class="buttonContainer">
              <input type="button" value="Save + Copy"/>
              <input type="submit" value="Save" />
            </div>
          </form>

          <form id="addOtherForm" action="" method="post">
            <div class="AppointmentFormRow">
              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "150px"}} class="select-field selectTherapist" name="therapist">
                  <option value="select" selected disabled hidden>Select Therapist</option>
                  <option value="sponge">Philip Adamson</option>
                  <option value="pat">Maegan Daugherty</option>
                  <option value="squid">Katelin Lynn</option>
                  <option value="krab">Lyndon Macdonald</option>
                  <option value="krab">Lacy Silva</option>
                  <option value="krab">Lola-Rose Lopez</option>
                  <option value="krab">Alexandria Small</option>
                  <option value="krab">Omar Needham</option>
                  <option value="krab">Delores Daniels</option>
              </select>
            </div>

            <div class="AppointmentFormRow">
              <span>Start <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field startTime" type="time" name="appt" min="05:00" max="19:00" ></input>

              <span>&nbsp; End <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field endTime" type="time" name="appt" min="05:00" max="19:00" ></input> 

              <span class="required">&nbsp; * </span>
              <input style={{width: "140px"}} class="input-field date" type="date" name="date"></input>
            </div>

            <label for="notes"><span>Title <span class="required">* </span></span>
            <input type="text" class="input-field"  name="title" rows="4" cols="50">
            </input>
            </label>

            <label for="notes"><span>Notes </span>
            <textarea class="textarea-field" style={{width: "100%"}} name="notes" rows="4" cols="50">
            </textarea>
            </label>
            

            <div class="buttonContainer">
              <input type="button" value="Save + Copy"/>
              <input type="submit" value="Save" />
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

function changeRoomToPatientRoom()  {
  document.getElementById("room").selectedIndex = document.getElementById("patient").selectedIndex;
}

export default AddAppointment;