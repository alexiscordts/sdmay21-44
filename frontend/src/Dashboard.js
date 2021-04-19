import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";
import TherapistSchedule from "./ScheduleViews/TherapistSchedule";
import RoomSchedule from "./ScheduleViews/RoomSchedule";
import AllTherapistSchedule from "./ScheduleViews/AllTherapistSchedule";
import PatientSchedule from "./ScheduleViews/PatientSchedule";
import AddAppointment from "./AddAppointment";
import EditAppointment from "./EditAppointment"
import ReactToPrint from "react-to-print";
import axios from "axios";

class Dashboard extends React.Component {
    constructor(props) {
        super(props);
        this.date = new Date();  
        this.scheduleHeader = "My Schedule";
        this.weeks = this.getDropdownDates();
        this.state = {
            locations: [],
            weekChanged: false,
            schedule: 1
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
      this.schedule = (<TherapistSchedule date={this.date} ref={(el) => (this.schedule = el)}/>);
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
                <div class="link" onClick={() => {this.date = newDate; this.schedule = this.setSchedule(newDate); this.week=newDate.toLocaleDateString('en-US'); this.setState({weekChanged:true}); }}>{newDate.toLocaleDateString('en-US')}</div>
            );
            d.setDate(d.getDate() + 7);
        }
        return elements;
    }

    setSchedule(newDate)   {
        this.schedule = <div></div>;
        switch(this.state.schedule) {
            case 1:
                return (<TherapistSchedule date={newDate} ref={(el) => (this.schedule = el)} />);
            case 2: 
                return (<AllTherapistSchedule date={this.date} ref={(el) => (this.schedule = el)} therapistEvent={this.therapistEvent} />);
            case 3: 
                return (<RoomSchedule date={this.date} ref={(el) => (this.schedule = el)} />);
            case 4:
                return (<PatientSchedule date={this.date} ref={(el) => (this.schedule = el)} />);
        }
    }

    getByPatientLinks()   {
        var byRoomLinks = [];
        this.state.locations.forEach(location => {
            byRoomLinks.push(<div class="link" onClick={() => {this.schedule = (<RoomSchedule date={this.date} ref={(el) => (this.schedule = el)} />); this.scheduleHeader = "Room Schedule"; this.locationHeader=location.name + ' - '; this.setState({schedule: 3});}}>{location.name}</div>);
        });
        return byRoomLinks;
    }

    getByRoomLinks()   {
        var byRoomLinks = [];
        this.state.locations.forEach(location => {
            byRoomLinks.push(<div class="link" onClick={() => {this.schedule = (<PatientSchedule date={this.date} ref={(el) => (this.schedule = el)} />); this.scheduleHeader = "Patient Schedule"; this.locationHeader=location.name + " - "; this.setState({schedule: 4});}}>{location.name}</div>);
        });
        return byRoomLinks;
    }


  render() {
    
    return (
    <div id="screen">
        <Nav />
        <div class="pageContainer">
            <div class="dropdown">
                <button class="dropbtn">
                    {this.scheduleHeader}
                    <i class="arrow down"></i>
                </button>
                <div class="dropdown-content">
                    <div class="link" onClick={() => {this.schedule = (<TherapistSchedule date={this.date} ref={(el) => (this.schedule = el)} />); this.scheduleHeader = "My Schedule"; this.locationHeader=""; this.setState({schedule: 1}); }}>My schedule</div>
                    <div class="link" onClick={() => {this.schedule = (<AllTherapistSchedule date={this.date} therapistEvent={this.therapistEvent} />); this.scheduleHeader = "Therapist Schedule"; this.locationHeader="";; this.setState({schedule: 2});}}>By therapist</div>
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

            

            <button style={{marginRight: "30px"}} class="topbtn" onClick={() => hideCopyDayForm()}>
                <img src={require("./Icons/icons8-duplicate-48.png")} alt="edit" height="30" />
            </button>

            <button class="topbtn" onClick={() => showAddAppointment()}>
                <img src={require("./Icons/icons8-add-property-48.png")} alt="edit" height="30" />
            </button>

            <ReactToPrint trigger={() => <button class="topbtn"><img src={require("./Icons/icons8-print-48.png")} alt="edit" height="30" /></button>} 
            onBeforeGetContent={() => showTimes()}
            onAfterPrint={() => {hideTimes(); this.schedule = this.setSchedule(this.date); this.forceUpdate();}}
            documentTitle={this.scheduleHeader + " for " + this.date.toLocaleDateString('en-US')}
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

function showAddAppointment()   {
    document.getElementById("addAppointment").style.display = "block";
    document.getElementById("editAppointment").style.display = "none";
}

function showTimes()   {
    setSizes();
    hideMetrics();
    var x = document.getElementsByClassName("printHours");
    for(var i=0; i< x.length;i++){
        x[i].style.width = "75px";
     }
}

function hideMetrics()  {
    if (document.getElementById("metricCheck") != null)
        document.getElementById("metricCheck").checked = false;
    var cols = document.getElementsByClassName("therapistMetrics");
    for (var i = 0; i < cols.length; i++)
    {
        cols[i].style.display = "none";
    }
}

function setSizes() {
    var cols = document.getElementsByClassName("room");
    for (var i = 0; i < cols.length; i++)
    {
            cols[i].style.minWidth = "100px";
            cols[i].style.width = "100px";
    }
    var cols = document.getElementsByClassName("therapist");
    for (var i = 0; i < cols.length; i++)
    {
            cols[i].style.minWidth = "100px";
            cols[i].style.width = "100px";
    }
}

function hideTimes()   {
    var x = document.getElementsByClassName("printHours");
    for(var i=0; i< x.length;i++){
        x[i].style.width = "0px";
     }
}

var copyDayFormIsHidden = true;
function hideCopyDayForm()  {
    if (copyDayFormIsHidden)
    {
        copyDayFormIsHidden = false;
        document.getElementById("copyDayForm").style.display = "block";
    }
    else
    {
        copyDayFormIsHidden = true;
        document.getElementById("copyDayForm").style.display = "none";

    }

}

export default Dashboard;