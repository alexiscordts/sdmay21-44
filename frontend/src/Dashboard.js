import React from "react";
import "./Dashboard.css";
import Nav from "./Nav";

class Dashboard extends React.Component {
  constructor(props) {
    super(props);
    this.hours = {values: loadHours()};
  }



  render() {
    return (
    <div id="screen">
        <Nav />
        <div id="schedule">
            <div id="days">
                <div id="topRow"></div>
                <div class="dayLabel">               
                    <div class="label">
                        Monday<br />
                        10/5
                    </div>
                </div>
                <div class="dayLabel">
                <div class="label">
                    Tuesday<br />
                    10/6
                </div>
                </div> 
                <div class="dayLabel">
                <div class="label">
                    Wednesday<br />
                    10/7
                </div>
                </div> 
                <div class="dayLabel">
                <div class="label">
                    Thursday<br />
                    10/8
                </div>
                </div> 
                <div class="dayLabel">
                <div class="label">
                    Friday<br />
                    10/9
                </div> 
                </div> 
                
            </div>
            <div id="scheduleContainer">
                <div id="hourColumn">
                    <div class="hour">
                        5 AM
                    </div>
                    <div class="hour">
                        6 AM
                    </div>
                    <div class="hour">
                        7 AM
                    </div>
                    <div class="hour">
                        8 AM
                    </div>
                    <div class="hour">
                        9 AM
                    </div>
                    <div class="hour">
                        10 AM
                    </div>
                    <div class="hour">
                        11 AM
                    </div>
                    <div class="hour">
                        12 PM
                    </div>
                    <div class="hour">
                        1 PM
                    </div>
                    <div class="hour">
                        2 PM
                    </div>
                    <div class="hour">
                        3 PM
                    </div>
                    <div class="hour">
                        4 PM
                    </div>
                    <div class="hour">
                        5 PM
                    </div>
                    <div class="hour">
                        6 PM
                    </div>
                    <div class="hour">
                        7 PM
                    </div>
                </div>
                <div class="day">                   
                    {this.hours.values}
                    <div class="appointment">
                        <div class="name">Morning Therapy</div>
                        <div class="time">6 AM - 7 AM</div>
                    </div>
                </div>
                <div class="day">
                    {this.hours.values}
                </div>
                <div class="day">
                    {this.hours.values}
                </div>
                <div class="day">
                    {this.hours.values}
                </div>
                <div class="day">
                    {this.hours.values}
                </div>
            </div>
        </div>           
    </div>
    );
  }
}

function loadHours()
{
    const items = [];
    for (var i = 0; i < 30; i++)
    {
        items.push(
            <div class="halfHour"></div>
        );
    }


    return items;
}

export default Dashboard;
