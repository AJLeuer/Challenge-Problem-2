using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NodaMoney;

namespace ChallengeProblem2
{
    public static class DemographicsAnalyzer
    {
        public static void PrintFullDemographicsAnalysis(Stream input, Stream output)
        {
            List<Person> persons = Person.CreateMultipleFromDescriptions(personDescriptions: input);
            
            var writer = new StreamWriter(output);
            
            writer.WriteLine($"Total Respondents: {persons.Count}");
            writer.WriteLine($"Average Age: {ComputeAverageAge(persons)}");
            writer.WriteLine($"Most Common Highest Level of Education: {FindMostCommonHighestLevelOfEducation(persons).GetDescription()}");
            writer.WriteLine($"Median Income: {ComputeMedianIncome(persons)}");
            writer.WriteLine($"Names of All Respondents: {GetSortedListOfAllNamesAsText(persons)}");
            
            writer.Flush();
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
            
            foreach (Person person in persons)
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

        public static String GetSortedListOfAllNamesAsText(List<Person> persons)
        {
            persons.Sort();
            var allNamesListBuilder = new StringBuilder();

            for (var i = 0; i < persons.Count; i++)
            {
                Person person = persons[i];
                allNamesListBuilder.Append(person.Name);

                if (i < (persons.Count - 1))
                {
                    allNamesListBuilder.Append(", ");
                }
            }

            return allNamesListBuilder.ToString();
        }
    }
}