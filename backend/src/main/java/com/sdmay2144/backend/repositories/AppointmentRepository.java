package com.sdmay2144.backend.repositories;

import com.sdmay2144.backend.models.Appointment;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Date;

@Repository
public interface AppointmentRepository extends JpaRepository<Appointment, Integer> {
    Appointment findByTherapistIdAndPatientIdAndEndDateTimeAndStartDateTime(Integer therapistId, Integer patientId, Date endDateTime, Date startDateTime);
}
