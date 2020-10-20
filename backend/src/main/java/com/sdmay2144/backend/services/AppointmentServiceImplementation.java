package com.sdmay2144.backend.services;


import com.sdmay2144.backend.models.Appointment;
import com.sdmay2144.backend.models.User;
import com.sdmay2144.backend.repositories.AppointmentRepository;
import com.sdmay2144.backend.services.interfaces.AppointmentService;

import java.util.List;

public class AppointmentServiceImplementation implements AppointmentService {

    private AppointmentRepository appointmentRepository;

    @Override
    public List<Appointment> getAllAppointments() { return appointmentRepository.findAll(); }

    @Override
    public Appointment findAppointment(Appointment a) {
        return appointmentRepository.findByTherapistIdAndPatientIdAndEndDatetimeAndStartDatetime(a.getTherapistId(),a.getPatientId(), a.getEndDatetime(), a.getStartDatetime());
    }

    @Override
    public Appointment addAppointment(Appointment a) { return appointmentRepository.save(a); }

    @Override
    public void removeAppointment(Appointment a) {appointmentRepository.delete(a); }

}
