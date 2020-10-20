package com.sdmay2144.backend.controllers;

import com.sdmay2144.backend.models.Appointment;
import com.sdmay2144.backend.services.interfaces.AppointmentService;
import lombok.AllArgsConstructor;
import org.springframework.web.bind.annotation.*;
import lombok.Builder;

import java.util.Date;
@AllArgsConstructor
@RestController
@RequestMapping("/appointment")
public class AppointmentController {

    private AppointmentService appointmentService;

    @PostMapping(path="/add")
    public @ResponseBody String addAppointment(@RequestParam Date startDateTime,
                                               @RequestParam Date endDateTime,
                                               @RequestParam Integer therapistId,
                                               @RequestParam Integer patientId,
                                               @RequestParam Integer roomNumber,
                                               @RequestParam String adl,
                                               @RequestParam Integer lid,
                                               @RequestParam Integer therapistDriveTime,
                                               @RequestParam String notes) {
        StringBuilder errorMsg = new StringBuilder();
        if (startDateTime == null) {
            errorMsg.append("Start Date Time cannot be null");
        }

        if (endDateTime == null) {
            errorMsg.append("End Date Time cannot be null");
        }

        if (therapistId == null) {
            errorMsg.append("Therapist Id cannot be null");
        }

        if (patientId == null) {
            errorMsg.append("Patient Id cannot be null");
        }

        if (roomNumber == null) {
            errorMsg.append("Room Number cannot be null");
        }

        if (adl == null || adl.isEmpty()) {
            errorMsg.append("ADL cannot be null");
        }

        if (lid == null) {
            errorMsg.append("Location Id cannot be null");
        }

        if (errorMsg.length() > 0) {
            errorMsg.deleteCharAt(errorMsg.length() - 1);//Remove ending space;
            throw new IllegalArgumentException(errorMsg.toString());
        }
        Appointment newAppointment = Appointment
                .builder()
                .startDatetime(startDateTime)
                .endDatetime(endDateTime)
                .therapistId(therapistId)
                .patientId(patientId)
                .roomNumber(roomNumber)
                .adl(adl)
                .lid(lid)
                .therapistDriveTime(therapistDriveTime)
                .notes(notes)
                .build();

        if(appointmentService.addAppointment(newAppointment) != null) {
            return "Success Appointment Created";
        }
        return "Failed to Create Appointment";
    }

    @GetMapping("/all")
    public @ResponseBody Iterable<Appointment> getAllAppointments() { return appointmentService.getAllAppointments(); }

    @PostMapping("/delete")
    public @ResponseBody String deleteAppointment(@RequestParam Appointment appointment){

        appointmentService.removeAppointment(appointment);

        if(appointmentService.findAppointment(appointment) != null){
            return "Failed to Delete Appointment";
        }
        return "Appointment Deleted";
    }
}
