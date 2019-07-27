using System;
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
    }
}