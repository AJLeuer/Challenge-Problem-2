using System;
using System.IO;
using Microsoft.Win32.SafeHandles;

namespace ChallengeProblem2
{
    class Program
    {
        static void Main(string[] args)
        {
            var personsDescriptions = new FileStream("Persons.txt", FileMode.Open);
            DemographicsAnalyzer.PrintFullDemographicsAnalysis(input: personsDescriptions, output: Console.OpenStandardOutput());
        }
    }
}