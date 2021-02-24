using Bogus;
using Bogus.DataSets;
using InpatientTherapySchedulingProgram.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InpatientTherapySchedulingProgramTests.Fakes
{
    public static class ModelFakes
    {
        public static Faker<User>UserFake { get; set; }
        public static Faker<Patient> PatientFake { get; set; }

    static ModelFakes()
    {
        BuildUserFakes();
        BuildPatientFakes();
    }

        private static void BuildUserFakes()
        {
            UserFake = new Faker<User>();
            UserFake.RuleFor(m => m.Uid, r => r.UniqueIndex);
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
            PatientFake.RuleFor(m => m.Pid, r => r.UniqueIndex);
            PatientFake.RuleFor(m => m.FirstName, r => r.Name.FirstName());
            PatientFake.RuleFor(m => m.MiddleName, r => r.Name.FirstName());
            PatientFake.RuleFor(m => m.LastName, r => r.Name.LastName());
            PatientFake.RuleFor(m => m.Address, r => r.Address.FullAddress());
            PatientFake.RuleFor(m => m.PhoneNumber, r => r.Phone.PhoneNumber());
            PatientFake.RuleFor(m => m.Location, r => r.Company.CompanyName());
            PatientFake.RuleFor(m => m.StartDate, r => r.Date.Past());
            PatientFake.RuleFor(m => m.PmrPhysician, r => r.Name.FullName());
    
        }
    }
    
}
