package com.sdmay2144.backend.services.interfaces;

import com.sdmay2144.backend.models.Appointment;
import com.sdmay2144.backend.models.User;

import java.util.List;

public interface AppointmentService {
    List<Appointment> getAllAppointments();
    Appointment findAppointment(Appointment a);
    Appointment addAppointment(Appointment a);
    void removeAppointment(Appointment a);
}
