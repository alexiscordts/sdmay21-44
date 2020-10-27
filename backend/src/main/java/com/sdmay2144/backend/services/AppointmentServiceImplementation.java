package com.sdmay2144.backend.services;


import com.sdmay2144.backend.models.Appointment;
import com.sdmay2144.backend.repositories.AppointmentRepository;
import com.sdmay2144.backend.services.interfaces.AppointmentService;
import org.springframework.context.annotation.Bean;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class AppointmentServiceImplementation implements AppointmentService {

    private AppointmentRepository appointmentRepository;

    @Override
    public List<Appointment> getAllAppointments() { return appointmentRepository.findAll(); }

    @Override
    public Appointment findAppointment(Appointment a) {
        return appointmentRepository.findByTherapistIdAndPatientIdAndEndDateTimeAndStartDateTime(a.getTherapistId(),a.getPatientId(), a.getEndDateTime(), a.getStartDateTime());
    }

    @Override
    public Appointment addAppointment(Appointment a) { return appointmentRepository.save(a); }

    @Override
    public void removeAppointment(Appointment a) {appointmentRepository.delete(a); }

}
