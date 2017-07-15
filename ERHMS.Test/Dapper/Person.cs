﻿using System;

namespace ERHMS.Test.Dapper
{
    public class Person
    {
        public string PersonId { get; set; }
        public string GenderId { get; set; }
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }

        public double Bmi
        {
            get { return Weight / (Height * Height * 144.0) * 703.0; }
        }

        public Person()
        {
            PersonId = Guid.NewGuid().ToString();
        }
    }
}
