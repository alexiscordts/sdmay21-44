CREATE TABLE [user] (
  [uid] int PRIMARY KEY,
  [first_name] nvarchar(255),
  [middle_name] nvarchar(255),
  [last_name] nvarchar(255),
  [address] nvarchar(255),
  [phone_number] nvarchar(255),
  [username] nvarchar(255),
  [password] nvarchar(255)
)
GO

CREATE TABLE [patient] (
  [pid] int PRIMARY KEY,
  [first_name] nvarchar(255),
  [middle_name] nvarchar(255),
  [last_name] nvarchar(255),
  [address] nvarchar(255),
  [phone_number] nvarchar(255),
  [location] nvarchar(255),
  [start_date] date,
  [PMR_physician] nvarchar(255)
)
GO

CREATE TABLE [permission] (
  [id] int,
  [role] nvarchar(255) NOT NULL CHECK ([role] IN ('therapist', 'nurse', 'admin')),
  [type] nvarchar(10),
  PRIMARY KEY ([id], [role])
)
GO

CREATE TABLE [therapy] (
  [adl] nvarchar(255) PRIMARY KEY,
  [type] nvarchar(255),
  [abbreviation] nvarchar(255)
)
GO

CREATE TABLE [location] (
  [lid] int PRIMARY KEY,
  [name] nvarchar(255)
)
GO

CREATE TABLE [room_number] (
  [number] int,
  [lid] int,
  PRIMARY KEY ([number], [lid])
)
GO

CREATE TABLE [appointment] (
  [start_datetime] datetime,
  [end_datetime] datetime,
  [therapist_id] int,
  [patient_id] int,
  [room_number] int,
  [adl] nvarchar(255),
  [lid] int,
  [therapist_drive_time] int,
  [notes] nvarchar(255),
  PRIMARY KEY ([start_datetime], [end_datetime], [therapist_id], [patient_id])
)
GO

CREATE TABLE [therapist_activity] (
  [name] nvarchar(255) PRIMARY KEY,
  [isProductive] bit
)
GO

CREATE TABLE [patient_activity] (
  [name] nvarchar(255) PRIMARY KEY
)
GO

CREATE TABLE [therapist_event] (
  [start_datetime] datetime,
  [end_datetime] datetime,
  [therapist_id] int,
  [activity] nvarchar(255),
  [notes] nvarchar(255),
  PRIMARY KEY ([start_datetime], [end_datetime], [therapist_id])
)
GO

CREATE TABLE [patient_event] (
  [start_datetime] datetime,
  [end_datetime] datetime,
  [patient_id] int,
  [activity] nvarchar(255),
  [notes] nvarchar(255),
  PRIMARY KEY ([start_datetime], [end_datetime], [patient_id])
)
GO

CREATE TABLE [hours_worked] (
  [start_datetime] datetime,
  [end_datetime] datetime,
  [uid] int,
  PRIMARY KEY ([start_datetime], [end_datetime], [uid])
)
GO

ALTER TABLE [permission] ADD FOREIGN KEY ([id]) REFERENCES [user] ([uid])
GO

ALTER TABLE [room_number] ADD FOREIGN KEY ([lid]) REFERENCES [location] ([lid])
GO

ALTER TABLE [appointment] ADD FOREIGN KEY ([therapist_id]) REFERENCES [user] ([uid])
GO

ALTER TABLE [appointment] ADD FOREIGN KEY ([patient_id]) REFERENCES [patient] ([pid])
GO

ALTER TABLE [appointment] ADD FOREIGN KEY ([lid]) REFERENCES [location] ([lid])
GO

ALTER TABLE [appointment] ADD FOREIGN KEY ([adl]) REFERENCES [therapy] ([adl])
GO

ALTER TABLE [therapist_event] ADD FOREIGN KEY ([therapist_id]) REFERENCES [user] ([uid])
GO

ALTER TABLE [therapist_event] ADD FOREIGN KEY ([activity]) REFERENCES [therapist_activity] ([name])
GO

ALTER TABLE [patient_event] ADD FOREIGN KEY ([patient_id]) REFERENCES [patient] ([pid])
GO

ALTER TABLE [patient_event] ADD FOREIGN KEY ([activity]) REFERENCES [patient_activity] ([name])
GO

ALTER TABLE [hours_worked] ADD FOREIGN KEY ([uid]) REFERENCES [user] ([uid])
GO

