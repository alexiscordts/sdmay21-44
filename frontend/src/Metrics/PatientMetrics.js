import React from "react";
import "../Dashboard.css";
import "../ViewMetrics.css";
import { ResponsiveBar } from '@nivo/bar'

class PatientMetrics extends React.Component {

  render() {

    const data = ([
        {
        "patient": "Spongebob Squarepants",
          "sunday": 2,
          "monday": 3,
          "tuesday": 4,
          "wednesday": 3,
          "thursday": 2,
          "friday": 3,
          "saturday": 3,
        },
        {
            "patient": "Patrick Star",
              "sunday": 4,
              "monday": 2,
              "tuesday": 3,
              "wednesday": 0,
              "thursday": 2,
              "friday": 3,
              "saturday": 3,
        },
        {
            "patient": "Squidward Tentacles",
              "sunday": 3,
              "monday": 2,
              "tuesday": 1,
              "wednesday": 6,
              "thursday": 3,
              "friday": 2,
              "saturday": 4,
            },
            {
                "patient": "Sandy Cheeks",
                  "sunday": 5,
                  "monday": 2,
                  "tuesday": 6,
                  "wednesday": 3,
                  "thursday": 7,
                  "friday": 4,
                  "saturday": 2,
            },
            {
                "patient": "Mr.Krabs",
                  "sunday": 1,
                  "monday": 2,
                  "tuesday": 1,
                  "wednesday": 4,
                  "thursday": 3,
                  "friday": 5,
                  "saturday": 4,
                },
                {
                    "patient": "Mrs. Puff",
                      "sunday": 6,
                      "monday": 4,
                      "tuesday": 5,
                      "wednesday": 6,
                      "thursday": 3,
                      "friday": 2,
                      "saturday": 3,
                }
      ]);

    return (   
        <div id="chart">
        <ResponsiveBar
        data={data}
        keys={[ 'sunday', 'monday', 'tuesday', 'wednesday', 'thursday', 'friday', 'saturday' ]}
        indexBy="patient"
        margin={{ top: 50, right: 130, bottom: 50, left: 60 }}
        padding={0.3}
        maxValue={10}
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
            legend: 'Patient',
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

export default PatientMetrics;