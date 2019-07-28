using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
        public static void ShouldOutputFullDemographicsAnalysis()
        {
            String input =
                "Name: Suzanne Martinez, Age: 39, Highest Level of Education: High School, Income: $45,000" + Environment.NewLine +
                "Name: Melissa Brownell, Age: 27, Highest Level of Education: College, Income: $70,000" + Environment.NewLine +
                "Name: Nathan Southern, Age: 73, Highest Level of Education: Grade School, Income: $33,000" + Environment.NewLine +
                "Name: Celeste Willis, Age: 46, Highest Level of Education: High School, Income: $60,000" + Environment.NewLine +
                "Name: Ashley Green, Age: 27, Highest Level of Education: College, Income: $100,000" + Environment.NewLine +
                "Name: Jennifer Coleman, Age: 35, Highest Level of Education: High School, Income: $75,000" + Environment.NewLine;
                
                
            var demographicsAnalysisInputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var demographicsAnalysisOutputStream = new MemoryStream();
            
            DemographicsAnalyzer.PrintFullDemographicsAnalysis(demographicsAnalysisInputStream, demographicsAnalysisOutputStream);
            String demographicsAnalysisOutput = new StreamReader(demographicsAnalysisOutputStream).ReadFromBeginning();
            
            String expectedOutput =
                "Total Respondents: 6" + Environment.NewLine +
                "Average Age: 41.2" + Environment.NewLine +
                "Most Common Highest Level of Education: High School" + Environment.NewLine +
                "Median Income: $65,000.00" + Environment.NewLine +
                "Names of All Respondents: Melissa Brownell, Jennifer Coleman, Ashley Green, Suzanne Martinez, Nathan Southern, Celeste Willis" + Environment.NewLine;
            
            Assert.AreEqual(expectedOutput, demographicsAnalysisOutput);
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

        [Test]
        public static void ShouldComputeMedianIncome()
        {
            Money medianIncome = DemographicsAnalyzer.ComputeMedianIncome(Persons);

            Assert.AreEqual(new Money(65000, "USD"), medianIncome);
        }
    }
}