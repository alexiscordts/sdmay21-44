/* Mock data to insert into inpatient therapy database. */
INSERT INTO [inpatient_therapy].[dbo].[user] (uid, first_name, middle_name, last_name, address, phone_number, username, password) VALUES (1, 'Albert', 'H.', 'Thomas', '212 Sheldon Ave, Ames, IA 50014', '5153429584', 'albTho', HashBytes('SHA1', 'fakePassword'));
INSERT INTO [inpatient_therapy].[dbo].[user] (uid, first_name, middle_name, last_name, address, phone_number, username, password) VALUES (2, 'Kate', 'D.', 'Johnson', '643 Douglas Ave, Urbandale, IA 50322', '5153484999', 'katJoh', HashBytes('SHA1', 'fakePassword'));
INSERT INTO [inpatient_therapy].[dbo].[user] (uid, first_name, middle_name, last_name, address, phone_number, username, password) VALUES (3, 'James', 'K.', 'Taylor', '9462 Meredith Dr, Urbandale, IA 50322', '5154443232', 'jamTay', HashBytes('SHA1', 'fakePassword'));

INSERT INTO [inpatient_therapy].[dbo].[patient] (pid, first_name, middle_name, last_name, address, phone_number, location, start_date)  VALUES (4, 'Kirk', 'T.', 'Franklin', '325 Lane Rd, Urbandale, IA 50322', '5154908778', 'Methodist', '2019-05-15 00:00:00');
INSERT INTO [inpatient_therapy].[dbo].[patient] (pid, first_name, middle_name, last_name, address, phone_number, location, start_date) VALUES (5, 'Miranda', 'H.', 'Wolfe', '604 Luther Ln, Des Moines, IA 50047', '5153248778', 'Luther', '2020-02-21 00:00:00');
INSERT INTO [inpatient_therapy].[dbo].[patient] (pid, first_name, middle_name, last_name, address, phone_number, location, start_date) VALUES (6, 'John', 'K.', 'White', '0983 MLK PKWY, Des Moines, IA 50047', '5152248328', 'Luther', '2015-10-08 00:00:00');

INSERT INTO [inpatient_therapy].[dbo].[location] (lid, name) VALUES (1, 'Methodist');
INSERT INTO [inpatient_therapy].[dbo].[location] (lid, name) VALUES (2, 'Luther');

INSERT INTO [inpatient_therapy].[dbo].[therapy] (adl, type, abbreviation) VALUES ('Upper Body Dressing', 'OT', 'U');
INSERT INTO [inpatient_therapy].[dbo].[therapy] (adl, type, abbreviation) VALUES ('Grooming', 'OT', 'G');
INSERT INTO [inpatient_therapy].[dbo].[therapy] (adl, type, abbreviation) VALUES ('Lower Body Dressing', 'OT', 'L');

INSERT INTO [inpatient_therapy].[dbo].[therapist_activity] (name, isProductive) VALUES ('UBC', 0);
INSERT INTO [inpatient_therapy].[dbo].[therapist_activity] (name, isProductive) VALUES ('Adaptive Design', 0);
INSERT INTO [inpatient_therapy].[dbo].[therapist_activity] (name, isProductive) VALUES ('Lunch', 0);

INSERT INTO [inpatient_therapy].[dbo].[patient_activity] (name) VALUES ('Peritoneal Dialysis');
INSERT INTO [inpatient_therapy].[dbo].[patient_activity] (name) VALUES ('Hemodialysis');
INSERT INTO [inpatient_therapy].[dbo].[patient_activity] (name) VALUES ('Education');

INSERT INTO [inpatient_therapy].[dbo].[hours_worked] (start_datetime, end_datetime, uid) VALUES ('2020-06-04 08:00:00', '2020-06-04 16:00:00', 1);
INSERT INTO [inpatient_therapy].[dbo].[hours_worked] (start_datetime, end_datetime, uid) VALUES ('2020-06-04 09:00:00', '2020-06-04 17:00:00', 2);
INSERT INTO [inpatient_therapy].[dbo].[hours_worked] (start_datetime, end_datetime, uid) VALUES ('2020-06-04 07:00:00', '2020-06-04 15:00:00', 3);

INSERT INTO [inpatient_therapy].[dbo].[permission] (id, role, type) VALUES (1, 'therapist', 'OT');
INSERT INTO [inpatient_therapy].[dbo].[permission] (id, role) VALUES (2, 'nurse');
INSERT INTO [inpatient_therapy].[dbo].[permission] (id, role) VALUES (3, 'admin');

INSERT INTO [inpatient_therapy].[dbo].[appointment] (start_datetime, end_datetime, therapist_id, patient_id, room_number, adl, lid, therapist_drive_time, notes) VALUES ('2020-06-04 08:00:00', '2020-06-04 09:00:00', 1, 5, 401, 'Upper Body Dressing', 2, 0, '');
INSERT INTO [inpatient_therapy].[dbo].[appointment] (start_datetime, end_datetime, therapist_id, patient_id, room_number, adl, lid, therapist_drive_time, notes) VALUES ('2020-06-04 09:00:00', '2020-06-04 09:30:00', 1, 6, 401, 'Lower Body Dressing', 2, 0, '');
INSERT INTO [inpatient_therapy].[dbo].[appointment] (start_datetime, end_datetime, therapist_id, patient_id, room_number, adl, lid, therapist_drive_time, notes) VALUES ('2020-06-04 10:00:00', '2020-06-04 12:00:00', 1, 4, 402, 'Grooming', 1, 30, '');

INSERT INTO [inpatient_therapy].[dbo].[room_number] (number, lid) VALUES (401, 1);
INSERT INTO [inpatient_therapy].[dbo].[room_number] (number, lid) VALUES (402, 1);
INSERT INTO [inpatient_therapy].[dbo].[room_number] (number, lid) VALUES (401, 2);

INSERT INTO [inpatient_therapy].[dbo].[therapist_event] (start_datetime, end_datetime, therapist_id, activity, notes) VALUES ('2020-06-04 12:00:00', '2020-06-04 12:30:00', 1, 'Lunch', '');

INSERT INTO [inpatient_therapy].[dbo].[patient_event] (start_datetime, end_datetime, patient_id, activity, notes) VALUES ('2020-06-04 12:30:00', '2020-06-04 13:00:00', 4, 'Education', '');
INSERT INTO [inpatient_therapy].[dbo].[patient_event] (start_datetime, end_datetime, patient_id, activity, notes) VALUES ('2020-06-04 09:00:00', '2020-06-04 11:00:00', 5, 'Hemodialysis', '');
INSERT INTO [inpatient_therapy].[dbo].[patient_event] (start_datetime, end_datetime, patient_id, activity, notes) VALUES ('2020-06-04 10:00:00', '2020-06-04 11:00:00', 6, 'Peritoneal Dialysis', '');
