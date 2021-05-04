import React from "react";
import "./Dashboard.css";
import "./ViewMetrics.css";
import Nav from "./Nav";
import MyMetrics from './Metrics/MyMetrics';
import TherapistMetrics from './Metrics/TherapistMetrics';
import PatientMetrics from './Metrics/PatientMetrics';
import RoomMetrics from './Metrics/RoomMetrics';
import axios from "axios";

class ViewMetrics extends React.Component {
    constructor(props) {
        super(props);
        this.metrics = (<MyMetrics />);
        this.date = new Date();
        while (this.date.getDay() != 0) //get Sunday
            {
                this.date.setDate(this.date.getDate() - 1);
            }
        this.state = {
            metrics: null,
            locations: [],
            date: new Date(this.date),
            location: null,
            appointments: []
        }
        this.weeks = this.getDropdownDates();
    }
    
    componentDidMount()
    {
        if (sessionStorage.getItem("role") == "therapist")
            this.setState({metrics: 1});
        else if (sessionStorage.getItem("role") == "admin")
            this.setState({metrics: 2});

            axios
            .get(process.env.REACT_APP_SERVER_URL + "Location")
            .then((response) => {
              const locations = response.data;
              this.setState({ locations });
              this.getAppointments(this.state.date);
            });
    }

    getAppointments(date)   {
        var start = new Date(date);
            var end = new Date(date);
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
            while (end.getDay() != 6) //get Saturday
            {
                end.setDate(end.getDate() + 1);
            }
        if (this.state.metrics == 2 || this.state.metrics == 3)
        {
            const event = ({
                startTime: start,
                endTime: end,
                adl: "null"
            })

            axios
            .post(process.env.REACT_APP_SERVER_URL + "appointment/getAppointments", event)
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
        else if (this.state.metrics == 1)
        {
            const event = ({
                startTime: start,
                endTime: end,
                therapistId: sessionStorage.getItem("id"),
                adl: "null"
            })

            axios
            .post(process.env.REACT_APP_SERVER_URL + "appointment/getAppointmentsByTherapistId", event)
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
                <div class="link" onClick={() => {this.setState({date: newDate}); this.week = newDate.toLocaleDateString('en-US'); this.getAppointments(newDate) }}>{newDate.toLocaleDateString('en-US')}</div>
            );
            d.setDate(d.getDate() + 7);
        }
        return elements;
    }

    getLocationOptions()    {
        const items = [];
        this.state.locations.forEach(location => {
            items.push(<div class="link" onClick={() => { this.setState({location: location.locationId}); this.setState({metrics: 3}); }}>{location.name}</div>);
        });
        return items;
    }

    getMetricsOptions()
      {
            const items = [];
            if (sessionStorage.getItem("role") == "therapist")
                return (<div class="link" onClick={() => { this.setState({metrics: 1}) }}>My metrics</div>);
            else if (sessionStorage.getItem("role") == "admin")
            {
                items.push(<div class="link" onClick={() => { this.setState({metrics: 2})}}>By therapist</div>);
                items.push(<div class="byPatient link">By patient
                <div class="metricLocations">
                        {this.getLocationOptions()}
                    </div>
                </div>);
            }
            return items;
      }

      getMetricsView()
      {
        switch(this.state.metrics) {
            case 1: return (<MyMetrics date={this.state.date} appointments={this.state.appointments} />);
            case 2: return (<TherapistMetrics date={this.state.date} appointments={this.state.appointments} />);
            case 3: return (<PatientMetrics date={this.state.date} appointments={this.state.appointments} location={this.state.location}/>);
        }
      }

      getMetricHeader()
      {
        switch(this.state.metrics) {
            case 1: return "My Metrics";
            case 2: return "Therapist Metrics";
            case 3: return "Patient Metrics";
        }
      }

  render() {

    return (

    <div id="screen">
        <div class="pageContainer">
            <div class="dropdown">
                <button class="dropbtn">
                    {this.getMetricHeader()}
                    <i class="arrow down"></i>
                </button>
                <div class="dropdown-content">
                {this.getMetricsOptions()}
                                    
                
                    
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
            {this.getMetricsView()}
        </div>
    </div>
    );
  }
}

export default ViewMetrics;