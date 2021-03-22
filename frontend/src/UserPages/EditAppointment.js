import React from "react";
import "../FormStyles.css";
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
        <div class="form-style-heading">Edit Appointment</div>

        <form action="" method="post">
            <label for="patient"><span>Patient <span class="required">*</span></span>
            <select class="select-field" name="patient">
                <option value="sponge">Spongebob Squarepants</option>
                <option value="squid">Squidward Tentacles</option>
                <option value="pat">Patrick Star</option>
            </select>
            </label>

            <label for="therapist"><span>Therapist <span class="required">*</span></span>
            <select class="select-field" name="therapist">
                <option value="sponge">Spongebob Squarepants</option>
                <option value="squid">Squidward Tentacles</option>
                <option value="pat">Patrick Star</option>
            </select>
            </label>

            <label for="room"><span>Room <span class="required">*</span></span>
            <select class="select-field" name="therapist">
                <option value="237">237</option>
                <option value="123">123</option>
                <option value="283">283</option>
            </select></label>

            <label for="start"><span>Start <span class="required">*</span></span></label>
            <input class="input-field" type="time" id="appt" name="appt" min="05:00" max="19:00" ></input>
            <label for="end"><span>End <span class="required">*</span></span>
            <input class="input-field" type="time" id="appt" name="appt" min="05:00" max="19:00" ></input>   
            </label>

            <label for="date"><span>Date <span class="required">*</span></span>
            <input class="input-field" type="date" name="date"></input>
            </label>

            <label for="date"><span>Notes </span>
            <textarea class="textarea-field" name="w3review" rows="4" cols="50">
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

function hideAddAppointment()   {
    document.getElementById("addAppointment").style.display = "none";
}

export default AddAppointment;