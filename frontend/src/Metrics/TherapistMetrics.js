import React from "react";
import "../Dashboard.css";
import "../ViewMetrics.css";
import { ResponsiveBar } from '@nivo/bar'

class TherapistMetrics extends React.Component {

  render() {

    const data = ([
        {
        "therapist": "Spongebob Squarepants",
          "sunday": 39,
          "monday": 65,
          "tuesday": 25,
          "wednesday": 47,
          "thursday": 79,
          "friday": 34,
          "saturday": 70,
        },
        {
            "therapist": "Patrick Star",
              "sunday": 64,
              "monday": 25,
              "tuesday": 5,
              "wednesday": 20,
              "thursday": 48,
              "friday": 59,
              "saturday": 80,
        },
        {
            "therapist": "Squidward Tentacles",
              "sunday": 39,
              "monday": 65,
              "tuesday": 25,
              "wednesday": 47,
              "thursday": 79,
              "friday": 34,
              "saturday": 70,
            },
            {
                "therapist": "Sandy Cheeks",
                  "sunday": 64,
                  "monday": 25,
                  "tuesday": 5,
                  "wednesday": 20,
                  "thursday": 48,
                  "friday": 59,
                  "saturday": 80,
            },
            {
                "therapist": "Mr.Krabs",
                  "sunday": 39,
                  "monday": 65,
                  "tuesday": 25,
                  "wednesday": 47,
                  "thursday": 79,
                  "friday": 34,
                  "saturday": 70,
                },
                {
                    "therapist": "Mrs. Puff",
                      "sunday": 64,
                      "monday": 25,
                      "tuesday": 5,
                      "wednesday": 20,
                      "thursday": 48,
                      "friday": 59,
                      "saturday": 80,
                }
      ]);

    return (   
        <div id="chart">
        <ResponsiveBar
        data={data}
        keys={[ 'sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday' ]}
        indexBy="therapist"
        margin={{ top: 50, right: 130, bottom: 50, left: 60 }}
        padding={0.3}
        maxValue={100}
        groupMode="grouped"
        valueScale={{ type: 'linear' }}
        indexScale={{ type: 'band', round: true }}
        colors={{ scheme: 'set2' }}
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
                color: '#eed312',
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
            legend: 'Therapist',
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
        legends={[
            {
                dataFrom: 'keys',
                anchor: 'bottom-right',
                direction: 'column',
                justify: false,
                translateX: 120,
                translateY: 0,
                itemsSpacing: 2,
                itemWidth: 100,
                itemHeight: 20,
                itemDirection: 'left-to-right',
                itemOpacity: 0.85,
                symbolSize: 20,
                effects: [
                    {
                        on: 'hover',
                        style: {
                            itemOpacity: 1
                        }
                    }
                ]
            }
        ]}
        animate={true}
        motionStiffness={90}
        motionDamping={15}
    />
    </div>
    );
    }
}

export default TherapistMetrics;