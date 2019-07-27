using System;
using ChallengeProblem2;
using NUnit.Framework;
using NodaMoney;

namespace ChallengeProblem2Test
{
    public class PersonTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldCreatePersonFromDescription()
        {
            String personDescription = "Name: Andrew Burton, Age: 33, Highest Level of Education: High School, Income: $70,000";
            Person actualPerson = Person.CreateFromStringDescription(personDescription);
            
            var expectedPerson = new Person(name: "Andrew Burton", age: 33, education: EducationLevel.HighSchool, income: Money.USDollar(70000));
            
            Assert.AreEqual(expected: expectedPerson, actual: actualPerson);
        }
    }


}