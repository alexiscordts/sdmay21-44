import React from "react";
import "../Dashboard.css";
import "../ViewMetrics.css";
import { ResponsiveBar } from '@nivo/bar'

class RoomMetrics extends React.Component {

  render() {

    const data = ([
        {
            "room": "100",
            "percent": 25
        },
        {
            "room": "101",
            "percent": 45
        },
        {
            "room": "102",
            "percent": 75
        },
        {
            "room": "103",
            "percent": 36
        },
        {
            "room": "104",
            "percent": 7
        },
        {
            "room": "105",
            "percent": 60
        },
        {
            "room": "106",
            "percent": 37
        },
        {
            "room": "107",
            "percent": 80
        },
        {
            "room": "108",
            "percent": 10
        },
        {
            "room": "109",
            "percent": 90
        },
        {
            "room": "110",
            "percent": 62
        }
      ]);

    return (
             <div id="chart">
            <ResponsiveBar
                data={data}
                keys={['percent']}
                indexBy="room"
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
                    legend: 'Room',
                    legendPosition: 'middle',
                    legendOffset: 32
                }}
                axisLeft={{
                    tickSize: 5,
                    tickPadding: 5,
                    tickRotation: 0,
                    legend: 'Room Use Percentage',
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
            );
    }
}

export default RoomMetrics;