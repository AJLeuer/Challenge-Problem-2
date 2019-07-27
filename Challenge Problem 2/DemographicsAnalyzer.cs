using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NodaMoney;

namespace ChallengeProblem2
{
    public static class DemographicsAnalyzer
    {
        public static void PrintFullDemographicsAnalysis(Stream output)
        {
            
        }

        public static decimal ComputeAverageAge(List<Person> persons)
        {
            decimal agesSum = 0;
            persons.ForEach((Person person) => { agesSum += person.Age;});
            decimal averageAge = agesSum / persons.Count;
            averageAge = Decimal.Round(averageAge, 1);
            return averageAge;
        }
        
        public static EducationLevel FindMostCommonHighestLevelOfEducation(List<Person> persons)
        {
            var educationLevelOccurences = new SortedDictionary<EducationLevel, uint>()
            {
                { EducationLevel.GradeSchool, 0 },
                { EducationLevel.HighSchool, 0 },
                { EducationLevel.College, 0 }
            };
            
            foreach (var person in persons)
            {
                educationLevelOccurences[person.Education]++;
            }
            
            var sortedEducationLevelOccurences = from educationLevelOccurence in educationLevelOccurences orderby educationLevelOccurence.Value ascending select educationLevelOccurence;

            EducationLevel mostCommonEducationLevel = sortedEducationLevelOccurences.Last().Key;
            return mostCommonEducationLevel;
        }

        public static Money ComputeMedianIncome(List<Person> persons)
        {
            var incomes = new List<Money>();
            
            persons.ForEach((Person person) => { incomes.Add(person.Income); });

            Money medianIncome = Utility.FindMedian(incomes);

            return medianIncome;
        }
    }
}