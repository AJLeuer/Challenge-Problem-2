using System.Collections.Generic;
using ChallengeProblem2;
using NodaMoney;
using NUnit.Framework;

namespace ChallengeProblem2Test
{
    public static class DemographicsAnalyzerTest
    {
        private static List<Person> Persons;

        [SetUp]
        public static void SetUp()
        {
            Persons = new List<Person>
            {
                new Person(name: "Suzanne Martinez", age: 39, education: EducationLevel.HighSchool, income: Money.USDollar(45000)),
                new Person(name: "Melissa Brownell", age: 27, education: EducationLevel.College, income: Money.USDollar(70000)),
                new Person(name: "Nathan Southern", age: 73, education: EducationLevel.GradeSchool, income: Money.USDollar(33000)),
                new Person(name: "Celeste Willis", age: 46, education: EducationLevel.HighSchool, income: Money.USDollar(60000)),
                new Person(name: "Ashley Green", age: 27, education: EducationLevel.College, income: Money.USDollar(100000)),
                new Person(name: "Jennifer Coleman", age: 35, education: EducationLevel.HighSchool, income: Money.USDollar(75000)),
            };
        }
        
        [Test]
        public static void ShouldComputeAverageAgeOfAllPersons()
        {
            decimal averageAge = DemographicsAnalyzer.ComputeAverageAge(Persons);
            
            Assert.AreEqual(41.2, averageAge);
        }
        
        [Test]
        public static void ShouldFindMostCommonHighestLevelOfEducation()
        {
            EducationLevel mostCommonHighestLevelOfEducation = DemographicsAnalyzer.FindMostCommonHighestLevelOfEducation(Persons);
            
            Assert.AreEqual(EducationLevel.HighSchool, mostCommonHighestLevelOfEducation);
        }
    }
}