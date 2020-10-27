package com.sdmay2144.backend.models;

import lombok.*;

import javax.persistence.*;
import java.io.Serializable;
import java.util.Date;


@Entity
@Table(name="appointment")
@Builder
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@IdClass(AppointmentId.class)
public class Appointment implements Serializable {

    @Id
    @Column(name="start_date_time")
    private Date startDateTime;

    @Id
    @Column(name="end_date_time")
    private Date endDateTime;

    @Id
    @Column(name="therapist_id")
    private Integer therapistId;

    @Id
    @Column(name="patient_id")
    private Integer patientId;

    @Column(name="room_number")
    private Integer roomNumber;

    @Column(name="adl")
    private String adl;

    @Column(name="lid")
    private Integer lid;

    @Column(name="therapist_drive_time")
    private Integer therapistDriveTime;

    @Column(name="notes")
    private String notes;
}
