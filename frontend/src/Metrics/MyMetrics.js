import React from "react";
import "../Dashboard.css";
import "../ViewMetrics.css";
import { ResponsiveBar } from '@nivo/bar'

class MyMetrics extends React.Component {

  render() {

    const data = ([
        {
            "day": "Sunday",
            "percent": 25
        },
        {
            "day": "Monday",
            "percent": 67
        },
        {
            "day": "Tuesday",
            "percent": 80
        },
        {
            "day": "Wednesday",
            "percent": 73
        },
        {
          "day": "Thursday",
          "percent": 42
        },
        {
            "day": "Friday",
            "percent": 21
        },
        {
            "day": "Saturday",
            "percent": 5
        }
      ]);

    return (
        <div>
        <div id="chart">
            <ResponsiveBar
                data={data}
                keys={['percent']}
                indexBy="day"
                margin={{ top: 50, right: 130, bottom: 50, left: 60 }}
                padding={0.3}
                maxValue={100}
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
                    legend: 'Day',
                    legendPosition: 'middle',
                    legendOffset: 32
                }}
                axisLeft={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: 'Percent of Time in Therapy',
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
                <div class="col">Hours in Therapy: 30</div>
                <div class="col">Percent in Therapy: 75%</div>
                <div class="col">More stats...</div>
            </div>
            </div>
    );
    }
}

export default MyMetrics;