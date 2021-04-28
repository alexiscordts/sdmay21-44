import React from "react";
import "../Dashboard.css";
import "../ViewMetrics.css";
import axios from "axios";
import { ResponsiveBar } from '@nivo/bar'

class PatientMetrics extends React.Component {
constructor(props) {
    super(props);
    this.state = {
        appointments: [],
        patients: []
    }
}

componentDidMount() {

        axios
        .get("http://10.29.163.20:8081/api/patient")
        .then((response) => {
            const patients = [];
            response.data.forEach(patient => {
                    patients.push(patient);
            });
            this.setState({ patients });
        });
}

    getData()   {
        const data = [];
        this.state.patients.forEach(patient => {
            if (this.props.location == patient.locationId)
            {
                const dataEntry = {
                    patient: patient.firstName + " " + patient.lastName,
                    Sunday: 0,
                    Monday: 0,
                    Tuesday: 0,
                    Wednesday: 0,
                    Thursday: 0,
                    Friday: 0,
                    Saturday: 0,
                }
                this.props.appointments.forEach(appointment => {
                    if (appointment.patientId == patient.patientId)
                    {
                        let start = new Date(appointment.startTime);
                        let end = new Date(appointment.endTime);
                        switch (start.getDay())
                        {
                            case 0:
                                dataEntry.Sunday += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                                break;
                            case 1:
                                dataEntry.Monday += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                                break;
                            case 2:
                                dataEntry.Tuesday += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                                break;
                            case 3:
                                dataEntry.Wednesday += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                                break;
                            case 4:
                                dataEntry.Thursday += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                                break;
                            case 5:
                                dataEntry.Friday += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                                break;
                            case 6:
                                dataEntry.Saturday += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                                break;
                        }
                    }
                });
                data.push(dataEntry);
        }});
        console.log(data);
        return data;
    }

  render() {
    const data = this.getData();
      const style = {
          minWidth: 200 * data.length
      }

    return (   
        <div id="chart">
        <div id="innerChartContainer" style={style}>
        <ResponsiveBar
        data={data}
        keys={[ 'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday' ]}
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
    </div>
    );
    }
}

export default PatientMetrics;