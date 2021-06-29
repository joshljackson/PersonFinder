using System;
using System.Collections;
using System.Collections.Generic;

namespace PersonFinder.Contracts
{
    public class Person
    {
        public Person() {}

        public Person(int id, string name, int? age, string address = null, string interests = null)
        {
            Id = id;
            Name = name;
            Age = age;
            Address = address;
            Interests = interests;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string Interests { get; set; }
    }
}
