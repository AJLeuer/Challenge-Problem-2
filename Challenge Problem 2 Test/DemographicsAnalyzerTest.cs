using System.Collections.Generic;
using ChallengeProblem2;
using NodaMoney;
using NUnit.Framework;

namespace ChallengeProblem2Test
{
    public static class DemographicsAnalyzerTest
    {
        [Test]
        public static void ShouldComputeAverageAgeOfAllPersons()
        {
            var persons = new List<Person>
            {
                new Person(name: "Suzanne Martinez", age: 39, education: EducationLevel.HighSchool, income: Money.USDollar(45000)),
                new Person(name: "Nathan Southern", age: 73, education: EducationLevel.GradeSchool, income: Money.USDollar(33000)),
                new Person(name: "Celeste Willis", age: 46, education: EducationLevel.HighSchool, income: Money.USDollar(60000)),
            };

            decimal averageAge = DemographicsAnalyzer.ComputeAverageAge(persons);
            
            Assert.AreEqual(52.7, averageAge);
        }
    }
}