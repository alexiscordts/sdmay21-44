package com.sdmay2144.backend.models;

import lombok.*;
import lombok.Builder;
import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;
import java.util.Date;


@Entity
@Table(name="appointment")
@Builder
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class Appointment {
    @Id
    @Column(name="start_date_time")
    private Date startDatetime;

    @Id
    @Column(name="end_date_time")
    private Date endDatetime;

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
