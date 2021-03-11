using Bogus;
using InpatientTherapySchedulingProgram.Models;
using System.Collections.Generic;

namespace InpatientTherapySchedulingProgramTests.Fakes
{
    public static class ModelFakes
    {
        public static Faker<User>UserFake { get; set; }
        public static Faker<Therapy>TherapyFake { get; set; }
        public static Faker<TherapistActivity>TherapistActivityFake { get; set; }
        public static Faker<Location> LocationFake { get; set; }
        public static Faker<Permission> PermissionFake { get; set; }
        public static Faker<Patient> PatientFake { get; set; }


        static ModelFakes()
        {
            BuildUserFakes();
            BuildTherapyFakes();
            BuildTherapistActivityFakes();
            BuildLocationFakes();
            BuildPermissionFakes();
            BuildPatientFakes();
        }

        private static void BuildTherapistActivityFakes()
        {
            TherapistActivityFake = new Faker<TherapistActivity>();
            TherapistActivityFake.RuleFor(m => m.ActivityName, r => r.IndexGlobal + r.Random.AlphaNumeric(10));
            TherapistActivityFake.RuleFor(m => m.IsProductive, r => r.Random.Bool());
        }

        private static void BuildTherapyFakes()
        {
            TherapyFake = new Faker<Therapy>();
            TherapyFake.RuleFor(m => m.Adl, r => r.IndexGlobal + r.Company.CompanyName());
            TherapyFake.RuleFor(m => m.TherapyType, r => r.Random.String2(10));
            TherapyFake.RuleFor(m => m.Abbreviation, r => r.IndexGlobal + r.Company.CompanySuffix());
        }

        private static void BuildUserFakes()
        {
            UserFake = new Faker<User>();
            UserFake.RuleFor(m => m.UserId, r => r.UniqueIndex);
            UserFake.RuleFor(m => m.FirstName, r => r.Name.FirstName());
            UserFake.RuleFor(m => m.MiddleName, r => r.Name.FirstName());
            UserFake.RuleFor(m => m.LastName, r => r.Name.LastName());
            UserFake.RuleFor(m => m.Address, r => r.Address.FullAddress());
            UserFake.RuleFor(m => m.PhoneNumber, r => r.Phone.PhoneNumber());
            UserFake.RuleFor(m => m.Username, r => r.IndexGlobal + r.Person.UserName);
            UserFake.RuleFor(m => m.Password, r => r.Internet.Password());
        }
        private static void BuildPatientFakes()
        {
            PatientFake = new Faker<Patient>();
            PatientFake.RuleFor(m => m.PatientId, r => r.UniqueIndex);
            PatientFake.RuleFor(m => m.FirstName, r => r.Name.FirstName());
            PatientFake.RuleFor(m => m.MiddleName, r => r.Name.FirstName());
            PatientFake.RuleFor(m => m.LastName, r => r.Name.LastName());
            PatientFake.RuleFor(m => m.Address, r => r.Address.FullAddress());
            PatientFake.RuleFor(m => m.PhoneNumber, r => r.Phone.PhoneNumber());
            PatientFake.RuleFor(m => m.LocationName, r => r.Company.CompanyName());
            PatientFake.RuleFor(m => m.StartDate, r => r.Date.Past());
            PatientFake.RuleFor(m => m.PmrPhysician, r => r.Name.FullName());

        }
        private static void BuildLocationFakes()
        {
            LocationFake = new Faker<Location>();
            LocationFake.RuleFor(m => m.LocationId, r => r.UniqueIndex);
            LocationFake.RuleFor(m => m.Name, r => r.UniqueIndex + r.Random.String2(10));
        }

        private static void BuildPermissionFakes()
        {
            List<string> roles = new List<string>
            {
                "therapist",
                "nurse",
                "admin"
            };

            PermissionFake = new Faker<Permission>();
            PermissionFake.RuleFor(m => m.UserId, r => r.UniqueIndex);
            PermissionFake.RuleFor(m => m.Role, r => roles[r.Random.Int(0, 2)]);
        }
    }
    
}
