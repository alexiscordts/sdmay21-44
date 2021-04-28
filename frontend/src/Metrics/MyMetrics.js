import React from "react";
import "../Dashboard.css";
import "../ViewMetrics.css";
import axios from "axios";
import { ResponsiveBar } from '@nivo/bar'

class MyMetrics extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            appointments: []
        }
    }

    componentDidMount() {
            var start = new Date(this.props.date);
            var end = new Date(this.props.date);
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
                therapistId: sessionStorage.getItem("id"),
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

    getData()   {
        this.totalHours = 0.0;
        var date = new Date(this.props.date);
        const data = [];
        const days = ["Sunday", 'Monday', "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        days.forEach(day => {
            var total = 0.0;
            this.state.appointments.forEach(appointment => {
                let start = new Date(appointment.startTime);
                let end = new Date(appointment.endTime);
                if (start.getFullYear() === date.getFullYear() &&
                start.getMonth() === date.getMonth() &&
                start.getDate() === date.getDate())
                {
                    total += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                    this.totalHours += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                }
                    
            });
            date.setDate(date.getDate() + 1);
            data.push({"day": day, "percent": total})
        });
        return data;
    }

  render() {
    const data = this.getData();

    return (
        <div>
        <div id="chart">
            <ResponsiveBar
                data={data}
                keys={['percent']}
                indexBy="day"
                margin={{ top: 50, right: 130, bottom: 50, left: 60 }}
                padding={0.3}
                maxValue={12}
                valueScale={{ type: 'linear' }}
                indexScale={{ type: 'band', round: true }}
                colors={{ scheme: 'paired' }}

                defs={[
                    {
                        id: 'dots',
                        type: 'patternDots',
                        background: 'inherit',
                        color: '#38bcb2',
                        size: 4,
                        padding: 1,
                        stagger: true
                    },
                    {
                        id: 'lines',
                        type: 'patternLines',
                        background: 'inherit',
                        color: '#38bcb2',
                        rotation: -45,
                        lineWidth: 6,
                        spacing: 10
                    }
                ]}
                borderColor={{ from: 'color', modifiers: [ [ 'darker', 1.6 ] ] }}
                axisTop={null}
                axisRight={null}
                axisBottom={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: '',
                    legendPosition: 'middle',
                    legendOffset: 32
                }}
                axisLeft={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: 'Hours in Therapy',
                    legendPosition: 'middle',
                    legendOffset: -40
                }}
                labelSkipWidth={12}
                labelSkipHeight={12}
                labelTextColor={{ from: 'color', modifiers: [ [ 'darker', 1.6 ] ] }}
                animate={true}
                motionStiffness={90}
                motionDamping={15}
            />
            </div>
            <div id="mystats">
                <h3>My Stats</h3>
                <div class="col">Hours in Therapy: {this.totalHours.toFixed(2)}</div>
                <div class="col">Percent in Therapy: {((this.totalHours / 40) * 100).toFixed(2)}%</div>
            </div>
            </div>
    );
    }
}

export default MyMetrics;