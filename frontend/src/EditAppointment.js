import React from "react";
import "./FormStyles.css";
import Draggable from 'react-draggable';

class EditAppointment extends React.Component {

    
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
        <div id="editAppointment">
        <div id="dragMe" class="handle">
        <div id="close" onClick={() => hideEditAppointment()}>X</div>

        </div>
        <div class="form-style">
        <div class="form-style-heading">Edit Appointment</div>

        <form action="" method="post">
        <div class="AppointmentFormRow">
              <span class="required"> * </span>
              <select style={{width: "150px"}} class="select-field" name="patient">
                  <option value="select" selected disabled hidden>Select Patient</option>
                  <option value="sponge">Spongebob Squarepants</option>
                  <option value="squid">Squidward Tentacles</option>
                  <option value="pat">Patrick Star</option>
              </select>

              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "150px"}} class="select-field" name="therapist">
                  <option value="select" selected disabled hidden>Select Therapist</option>
                  <option value="sponge">Spongebob Squarepants</option>
                  <option value="squid">Squidward Tentacles</option>
                  <option value="pat">Patrick Star</option>
              </select>

              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "120px"}} class="select-field" name="therapist">
                  <option value="select" selected disabled hidden>Select Room</option>
                  <option value="237">237</option>
                  <option value="123">123</option>
                  <option value="283">283</option>
              </select>
            </div>

            <div class="AppointmentFormRow">
              <span class="required"> * </span>
              <select style={{width: "200px"}} class="select-field" name="type">
                  <option value="select" selected disabled hidden>Select Therapy Type</option>
                  <option value="sponge">Physical</option>
                  <option value="squid">Occupational</option>
                  <option value="pat">Speech</option>
              </select>

              <span class="required">&nbsp;&nbsp; * </span>
              <select style={{width: "200px"}} class="select-field" name="therapist">
                  <option value="select" selected disabled hidden>Select Subtype</option>
                  <option value="sponge">Upper body dressing</option>
                  <option value="squid">Grooming</option>
                  <option value="pat">Lower body dressing</option>
                  <option value="squid">Spinal cord education</option>
                  <option value="pat">Strength group</option>
              </select>
            </div>

            <div class="AppointmentFormRow">
              <span>Start <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field" type="time" id="appt" name="appt" min="05:00" max="19:00" ></input>

              <span>&nbsp; End <span class="required">* </span></span>
              <input style={{width: "110px"}} class="input-field" type="time" id="appt" name="appt" min="05:00" max="19:00" ></input> 

              <span class="required">&nbsp; * </span>
              <input style={{width: "140px"}} class="input-field" type="date" name="date"></input>
            </div>

            <label for="notes"><span>Notes </span>
            <textarea class="textarea-field" name="notes" rows="4" cols="50">
            </textarea>
            </label>
            

            <div class="buttonContainer">
              <input type="button" value="Save + Copy"/>
              <input type="submit" value="Save" />
              <input id="delete" type="submit" value="Delete" />
            </div>
          </form>
        </div>
        </div>
    </Draggable>     
    </div>
      );
    }
  }

function hideEditAppointment()   {
    document.getElementById("editAppointment").style.display = "none";
}

export default EditAppointment;