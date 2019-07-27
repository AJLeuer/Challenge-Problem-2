using System;
using System.Collections.Generic;
using System.IO;

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
    }
}