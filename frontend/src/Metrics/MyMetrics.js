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

    getData()   {
        this.totalHours = 0.0;
        var date = new Date(this.props.date);
        const data = [];
        const days = ["Sunday", 'Monday', "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
        days.forEach(day => {
            var total = 0.0;
            this.props.appointments.forEach(appointment => {
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
                <div class="col">Hours in Therapy: {this.totalHours}</div>
                <div class="col">Percent in Therapy: {(this.totalHours / 40) * 100}%</div>
            </div>
            </div>
    );
    }
}

export default MyMetrics;