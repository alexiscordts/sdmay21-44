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
        public static Faker<Location>LocationFake { get; set; }

    static ModelFakes()
    {
        BuildUserFakes();
        BuildLocationFakes();
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

        private static void BuildLocationFakes()
        {
            LocationFake = new Faker<Location>();
            LocationFake.RuleFor(m => m.Lid, r => r.UniqueIndex);
            LocationFake.RuleFor(m => m.Name, r => r.UniqueIndex + r.Random.String2(10));
        }
    }
    
}
