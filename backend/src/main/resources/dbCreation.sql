CREATE TABLE `user` (
  `uid` int PRIMARY KEY,
  `first_name` varchar(255),
  `middle_name` varchar(255),
  `last_name` varchar(255),
  `address` varchar(255),
  `phone_number` varchar(255),
  `username` varchar(255),
  `password` varchar(255)
);

CREATE TABLE `patient` (
  `pid` int PRIMARY KEY,
  `first_name` varchar(255),
  `middle_name` varchar(255),
  `last_name` varchar(255),
  `address` varchar(255),
  `phone_number` varchar(255),
  `location` varchar(255),
  `start_date` date
);

CREATE TABLE `permission` (
  `id` int,
  `role` ENUM ('therapist', 'nurse', 'admin'),
  PRIMARY KEY (`id`, `role`)
);

CREATE TABLE `therapy` (
  `adl` varchar(255) PRIMARY KEY,
  `type` varchar(255),
  `abbreviation` varchar(255)
);

CREATE TABLE `location` (
  `lid` int PRIMARY KEY,
  `name` varchar(255)
);

CREATE TABLE `room_number` (
  `number` int,
  `lid` int,
  PRIMARY KEY (`number`, `lid`)
);

CREATE TABLE `appointment` (
  `start_datetime` datetime,
  `end_datetime` datetime,
  `therapist_id` int,
  `patient_id` int,
  `room_number` int,
  `adl` varchar(255),
  `lid` int,
  `therapist_drive_time` int,
  `notes` varchar(255),
  PRIMARY KEY (`start_datetime`, `end_datetime`, `therapist_id`, `patient_id`)
);

CREATE TABLE `therapist_activity` (
  `name` varchar(255) PRIMARY KEY,
  `isProductive` boolean
);

CREATE TABLE `patient_activity` (
  `name` varchar(255) PRIMARY KEY
);

CREATE TABLE `therapist_event` (
  `start_datetime` datetime,
  `end_datetime` datetime,
  `therapist_id` int,
  `activity` varchar(255),
  `notes` varchar(255),
  PRIMARY KEY (`start_datetime`, `end_datetime`, `therapist_id`)
);

CREATE TABLE `patient_event` (
  `start_datetime` datetime,
  `end_datetime` datetime,
  `patient_id` int,
  `activity` varchar(255),
  `notes` varchar(255),
  PRIMARY KEY (`start_datetime`, `end_datetime`, `patient_id`)
);

CREATE TABLE `hours_worked` (
  `start_datetime` datetime,
  `end_datetime` datetime,
  `uid` int,
  PRIMARY KEY (`start_datetime`, `end_datetime`, `uid`)
);

ALTER TABLE `permission` ADD FOREIGN KEY (`id`) REFERENCES `user` (`uid`);

ALTER TABLE `room_number` ADD FOREIGN KEY (`lid`) REFERENCES `location` (`lid`);

ALTER TABLE `appointment` ADD FOREIGN KEY (`therapist_id`) REFERENCES `user` (`uid`);

ALTER TABLE `appointment` ADD FOREIGN KEY (`patient_id`) REFERENCES `patient` (`pid`);

ALTER TABLE `appointment` ADD FOREIGN KEY (`lid`) REFERENCES `location` (`lid`);

ALTER TABLE `appointment` ADD FOREIGN KEY (`adl`) REFERENCES `therapy` (`adl`);

ALTER TABLE `therapist_event` ADD FOREIGN KEY (`therapist_id`) REFERENCES `user` (`uid`);

ALTER TABLE `therapist_event` ADD FOREIGN KEY (`activity`) REFERENCES `therapist_activity` (`name`);

ALTER TABLE `patient_event` ADD FOREIGN KEY (`patient_id`) REFERENCES `patient` (`pid`);

ALTER TABLE `patient_event` ADD FOREIGN KEY (`activity`) REFERENCES `patient_activity` (`name`);

ALTER TABLE `hours_worked` ADD FOREIGN KEY (`uid`) REFERENCES `user` (`uid`);

