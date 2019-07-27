using System;
using System.Collections.Generic;
using ChallengeProblem2;
using NUnit.Framework;

namespace ChallengeProblem2Test
{
    public static class UtilityTest
    {
        [Test]
        public static void CleanStringEdgesShouldRemoveUnwantedCharacters()
        {
            String dirty = " ,hi ";
            
            String actualCleanedString = dirty.CleanEnds(' ', ',');
            
            String expectedCleanedString = "hi";
            Assert.AreEqual(expectedCleanedString, actualCleanedString);
        }
        
        [Test]
        public static void ShouldComputeMedianOfOddSizedLists()
        {
            var numbers = new List<ushort> { 23, 46, 72, 21, 96 };
            
            ushort median = Utility.FindMedian(numbers);
            
            Assert.AreEqual(46, median);  
        }
        
        [Test]
        public static void ShouldComputeMedianOfEvenSizedLists()
        {
            var numbers = new List<ushort> { 67, 16, 29, 12, 77, 93, 80, 34 };

            ushort median = Utility.FindMedian(numbers);
            
            Assert.AreEqual(50, median);
        }
    }
}