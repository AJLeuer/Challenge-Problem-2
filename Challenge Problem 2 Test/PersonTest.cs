using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            Person actualPerson = Person.CreateFromDescription(personDescription);
            
            var expectedPerson = new Person(name: "Andrew Burton", age: 33, education: EducationLevel.HighSchool, income: Money.USDollar(70000));
            
            Assert.AreEqual(expected: expectedPerson, actual: actualPerson);
        }
        
        [Test]
        public void ShouldCreateMultiplePersonsFromDescriptionOfMultiplePeople()
        {
            String personDescription = "Name: Suzanne Martinez, Age: 39, Highest Level of Education: High School, Income: $45,000" + '\n' +
                                       "Name: Nathan Southern, Age: 73, Highest Level of Education: Grade School, Income: $33,000" + '\n' +
                                       "Name: Celeste Willis, Age: 46, Highest Level of Education: High School, Income: $60,000";

            var personDescriptionStream = new MemoryStream(Encoding.UTF8.GetBytes(personDescription));
            List<Person> persons = Person.CreateMultipleFromDescriptions(personDescriptionStream);

            var expectedPersons = new List<Person>
            {
                new Person(name: "Suzanne Martinez", age: 39, education: EducationLevel.HighSchool, income: Money.USDollar(45000)),
                new Person(name: "Nathan Southern", age: 73, education: EducationLevel.GradeSchool, income: Money.USDollar(33000)),
                new Person(name: "Celeste Willis", age: 46, education: EducationLevel.HighSchool, income: Money.USDollar(60000)),
            };
            
            Assert.AreEqual(expected: expectedPersons, actual: persons);
        }
    }


}