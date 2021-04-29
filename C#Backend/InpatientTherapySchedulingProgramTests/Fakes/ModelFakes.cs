using Bogus;
using InpatientTherapySchedulingProgram.Models;
using System.Collections.Generic;

namespace InpatientTherapySchedulingProgramTests.Fakes
{
    public static class ModelFakes
    {
        public static Faker<User> UserFake { get; set; }
        public static Faker<TherapyMain> TherapyMainFake { get; set; }
        public static Faker<Therapy> TherapyFake { get; set; }
        public static Faker<Location> LocationFake { get; set; }
        public static Faker<HoursWorked> HoursWorkedFake { get; set; }
        public static Faker<TherapistEvent> TherapistEventFake { get; set; }
        public static Faker<Permission> PermissionFake { get; set; }
        public static Faker<Patient> PatientFake { get; set; }

        public static Faker<Room> RoomFake { get; set; }


        static ModelFakes()
        {
            BuildUserFakes();
            BuildTherapyMainFakes();
            BuildTherapyFakes();
            BuildLocationFakes();
            BuildHoursWorkedFakes();
            BuildTherapistEventFakes();
            BuildPermissionFakes();
            BuildPatientFakes();
            BuildRoomFakes();
        }

        private static void BuildTherapyMainFakes()
        {
            TherapyMainFake = new Faker<TherapyMain>();
            TherapyMainFake.RuleFor(m => m.Type, r => r.UniqueIndex + r.Random.String2(10));
            TherapyMainFake.RuleFor(m => m.Abbreviation, r => r.UniqueIndex + r.Random.String2(10));
            TherapyMainFake.RuleFor(m => m.Active, true);
        }

        private static void BuildTherapyFakes()
        {
            TherapyFake = new Faker<Therapy>();
            TherapyFake.RuleFor(m => m.Adl, r => r.UniqueIndex + r.Company.CompanyName());
            TherapyFake.RuleFor(m => m.Type, r => r.UniqueIndex + r.Random.String2(10));
            TherapyFake.RuleFor(m => m.Abbreviation, r => r.UniqueIndex + r.Random.String2(3));
            TherapyFake.RuleFor(m => m.Active, true);
        }

        private static void BuildUserFakes()
        {
            UserFake = new Faker<User>();
            UserFake.RuleFor(m => m.FirstName, r => r.Name.FirstName());
            UserFake.RuleFor(m => m.MiddleName, r => r.Name.FirstName());
            UserFake.RuleFor(m => m.LastName, r => r.Name.LastName());
            UserFake.RuleFor(m => m.Address, r => r.Address.FullAddress());
            UserFake.RuleFor(m => m.PhoneNumber, r => r.Phone.PhoneNumber());
            UserFake.RuleFor(m => m.Username, r => r.IndexGlobal + r.Person.UserName);
            UserFake.RuleFor(m => m.Password, r => r.Internet.Password());
            UserFake.RuleFor(m => m.Active, true);
        }
        private static void BuildPatientFakes()
        {
            PatientFake = new Faker<Patient>();
            PatientFake.RuleFor(m => m.FirstName, r => r.Name.FirstName());
            PatientFake.RuleFor(m => m.MiddleName, r => r.Name.FirstName());
            PatientFake.RuleFor(m => m.LastName, r => r.Name.LastName());
            PatientFake.RuleFor(m => m.Address, r => r.Address.FullAddress());
            PatientFake.RuleFor(m => m.PhoneNumber, r => r.Phone.PhoneNumber());
            PatientFake.RuleFor(m => m.RoomNumber, r => r.Random.Int(400, 1000));
            PatientFake.RuleFor(m => m.LocationId, r => r.Random.Int(0, 10000));
            PatientFake.RuleFor(m => m.StartDate, r => r.Date.Past());
            PatientFake.RuleFor(m => m.PmrPhysicianId, r => r.Random.Int(0, 10000));
            PatientFake.RuleFor(m => m.Active, true);

        }
        private static void BuildLocationFakes()
        {
            LocationFake = new Faker<Location>();
            LocationFake.RuleFor(m => m.Name, r => r.UniqueIndex + r.Random.String2(10));
            LocationFake.RuleFor(m => m.Address, r => r.Address.FullAddress());
            LocationFake.RuleFor(m => m.PhoneNumber, r => r.Phone.PhoneNumber());
            LocationFake.RuleFor(m => m.Active, true);
        }
        private static void BuildHoursWorkedFakes()
        {
            HoursWorkedFake = new Faker<HoursWorked>();
            HoursWorkedFake.RuleFor(m => m.HoursWorkedId, r => r.UniqueIndex);
            HoursWorkedFake.RuleFor(m => m.StartTime, r => r.Date.Past());
            HoursWorkedFake.RuleFor(m => m.EndTime, r => r.Date.Past());
            HoursWorkedFake.RuleFor(m => m.UserId, r => r.UniqueIndex);
            HoursWorkedFake.RuleFor(m => m.Active, r => true);
            //HoursWorkedFake.RuleFor(m => m.User, BuildUserFakes());
        }

        private static void BuildTherapistEventFakes()
        {
            TherapistEventFake = new Faker<TherapistEvent>();
            TherapistEventFake.RuleFor(m => m.TherapistId, r => r.Random.Int());
            TherapistEventFake.RuleFor(m => m.StartTime, r => r.Date.Recent());
            TherapistEventFake.RuleFor(m => m.EndTime, r => r.Date.Soon());
            TherapistEventFake.RuleFor(m => m.ActivityName, r => r.IndexGlobal + r.Random.AlphaNumeric(10));
            TherapistEventFake.RuleFor(m => m.Notes, r => r.Commerce.ProductDescription());
            TherapistEventFake.RuleFor(m => m.Active, true);
        }
        
        private static void BuildPermissionFakes()
        {
            List<string> roles = new List<string>
            {
                "therapist",
                "nurse",
                "admin",
                "physician"
            };

            PermissionFake = new Faker<Permission>();
            PermissionFake.RuleFor(m => m.UserId, r => r.UniqueIndex);
            PermissionFake.RuleFor(m => m.Role, r => roles[r.Random.Int(0, 2)]);
        }

        private static void BuildRoomFakes() {
            RoomFake = new Faker<Room>();
            RoomFake.RuleFor(m => m.Number, r => r.Random.Int());
            RoomFake.RuleFor(m => m.LocationId, r => r.Random.Int(0,1000));
            RoomFake.RuleFor(m => m.Active, true);
        }
    }
    
}
