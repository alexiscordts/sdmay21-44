import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import TherapistSchedule from "./ScheduleViews/TherapistSchedule";
import RoomSchedule from "./ScheduleViews/RoomSchedule";
import AllTherapistSchedule from "./ScheduleViews/AllTherapistSchedule";
import PatientSchedule from "./ScheduleViews/PatientSchedule";
import AddAppointment from "./AddAppointment";
import EditAppointment from "./EditAppointment";
import ReactToPrint from "react-to-print";
import axios from "axios";

class Dashboard extends React.Component {
    constructor(props) {
        super(props);
        if (this.props.role == "therapist")
            this.scheduleHeader = "My Schedule";
        else
            this.scheduleHeader = "Room Schedule";
        this.weeks = this.getDropdownDates();
        this.state = {
            locations: [],
            weekChanged: false,
            schedule: 1,
            date: new Date()
        }
        this.therapistEvent = {
            startTime: new Date(),
            endTime: new Date(),
            therapistId: null,
            active: true,
            activityName: null,
            notes: null
          }
    }
    
    //
    componentDidMount() {
        const url = "http://10.29.163.20:8081/api/Location";
        
    axios
      .get(url)
      // .then((json = {}) => json.data)
      .then((response) => {
        console.log(response.data);
        const locations = response.data;
        this.setState({ locations });
      });
      if (this.props.role == "therapist")
        this.schedule = (<TherapistSchedule date={this.state.date} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role}/>);
        else 
        {
            this.state.schedule = 3;
            this.schedule = (<RoomSchedule date={this.state.date} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role}/>);
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
            let newDate = new Date(d.getTime());
            elements.push(
                <div class="link" onClick={() => {this.state.date = newDate; this.schedule = this.setSchedule(newDate); this.week=newDate.toLocaleDateString('en-US'); this.setState({weekChanged:true}); }}>{newDate.toLocaleDateString('en-US')}</div>
            );
            d.setDate(d.getDate() + 7);
        }
        return elements;
    }
    setSchedule(newDate)   {
        this.schedule = <div></div>;
        switch(this.state.schedule) {
            case 1:
                return (<TherapistSchedule date={newDate} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role}/>);
            case 2: 
                return (<AllTherapistSchedule date={newDate} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role}/>);
            case 3: 
                return (<RoomSchedule date={newDate} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role}/>);
            case 4:
                return (<PatientSchedule date={newDate} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role}/>);
        }
    }

    getByRoomLinks()   {
        var byRoomLinks = [];
        this.state.locations.forEach(location => {
            byRoomLinks.push(<div class="link" onClick={() => {this.schedule = (<RoomSchedule date={this.state.date} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role}/>); this.scheduleHeader = "Room Schedule"; this.locationHeader=location.name + ' - '; this.setState({schedule: 3});}}>{location.name}</div>);
        });
        return byRoomLinks;
    }

    getByPatientLinks()   {
        var byRoomLinks = [];
        this.state.locations.forEach(location => {
            byRoomLinks.push(<div class="link" onClick={() => {this.schedule = (<PatientSchedule date={this.state.date} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role}/>); this.scheduleHeader = "Patient Schedule"; this.locationHeader=location.name + " - "; this.setState({schedule: 4});}}>{location.name}</div>);
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
            return (<div class="link" onClick={() => {this.schedule = (<TherapistSchedule date={this.state.date} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role} />); this.scheduleHeader = "My Schedule"; this.locationHeader=""; this.setState({schedule: 1}); console.log(this.schedule)}}>My schedule</div>);
        else return (<div class="link">&nbsp;</div>);
    }

    render() {
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
                        <div class="link" onClick={() => {this.schedule = (<AllTherapistSchedule date={this.state.date} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} role={this.props.role} />); this.scheduleHeader = "Therapist Schedule"; this.locationHeader="";; this.setState({schedule: 2}); console.log(this.schedule)}}>By therapist</div>
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
                        <input style={{width: "160px", marginBottom: "10px"}} class="input-field" type="date" name="date"></input>
    
                        <span>Copy to: <span class="required">* </span></span>
                        <input style={{width: "160px", marginBottom: "10px"}} class="input-field" type="date" name="date"></input>
    
                        <input style={{width: "100%"}} type="submit" value="Copy Day" />
                    </form>
                    </div>
                </div>
                <div>{this.schedule}</div>
    
            </div>
            <AddAppointment therapistEvent={this.therapistEvent} />
            <EditAppointment />
        </div>
        );
      }
    }

function showAddAppointment() {
  document.getElementById("addAppointment").style.display = "block";
  document.getElementById("editAppointment").style.display = "none";
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
