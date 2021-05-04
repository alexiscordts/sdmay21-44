import React from "react";
import "../Dashboard.css";
import "../ViewMetrics.css";
import axios from "axios";
import { ResponsiveBar } from '@nivo/bar'

class TherapistMetrics extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            appointments: [],
            userList: [],
            therapistList: []
        }
    }

    componentDidMount() {
        const url = process.env.REACT_APP_SERVER_URL + "";
        axios.get(url + "user").then((response) => {
            const userList = response.data;
            this.setState({ userList });
              axios.get(process.env.REACT_APP_SERVER_URL + "permission").then((response) => {
              const therapistList = [];
              const permissions = response.data;
              this.state.userList.forEach(user =>{
                  permissions.forEach(permission => {
                      if (permission.userId == user.userId && permission.role == "therapist")
                          therapistList.push(user);
                  })
                    this.setState({therapistList});
                });
            });
        });
    }

    getData()
    {
        const data = [];
        this.state.therapistList.forEach(therapist => {
            const dataEntry = {
                therapist: therapist.firstName + " " + therapist.lastName,
                Sunday: 0,
                Monday: 0,
                Tuesday: 0,
                Wednesday: 0,
                Thursday: 0,
                Friday: 0,
                Saturday: 0,
            }
            var total = 0;
            this.props.appointments.forEach(appointment => {
                if (appointment.therapistId == therapist.userId)
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
                    total += Math.round((Math.abs(end - start) / 36e5) * 100) / 100;
                }
            });
            dataEntry.therapist += " - " + Math.round(total * 100) / 100;
            data.push(dataEntry);
        });
        return data;
    }

  render() {
    const data = this.getData();

    const style = {
        minWidth: 200 * this.state.therapistList.length
    }

    return (   
        <div id="chart">
        <div id="innerChartContainer" style={style}>
        <ResponsiveBar
        data={data}
        keys={[ 'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday' ]}
        indexBy="therapist"
        margin={{ top: 50, right: 130, bottom: 50, left: 60 }}
        padding={0.3}
        maxValue={12}
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

export default TherapistMetrics;