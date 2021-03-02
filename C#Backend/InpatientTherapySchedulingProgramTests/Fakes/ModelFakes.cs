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
        public static Faker<User> UserFake { get; set; }
        public static Faker<PatientActivity> PatientActivityFake { get; set; }

    static ModelFakes()
    {
        BuildUserFakes();
        BuildPatientActivityFakes();
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
        private static void BuildPatientActivityFakes()
        {
            PatientActivityFake = new Faker<PatientActivity>();
            PatientActivityFake.RuleFor(m => m.Name, r => r.Name.FirstName() + r.UniqueIndex);
        }
    }
    
}
