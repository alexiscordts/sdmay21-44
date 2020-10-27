package com.sdmay2144.backend.models;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.io.Serializable;
import java.util.Date;


@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class AppointmentId implements Serializable {

    private Date startDateTime;
    private Date endDateTime;
    private Integer patientId;
    private Integer therapistId;

}
