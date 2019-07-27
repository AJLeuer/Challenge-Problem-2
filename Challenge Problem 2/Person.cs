using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NodaMoney;
using Optional;

namespace ChallengeProblem2
{
    public class Person : IEquatable<Person>
    {
        public string Name { get; set; }
        public ushort Age { get; set; }
        public EducationLevel Education { get; set; }
        public Money Income { get; set; }

        public static readonly Dictionary<PersonDescriptionField, String> PersonDescriptionFieldKeys = new Dictionary<PersonDescriptionField, String>
        {
            { PersonDescriptionField.Name, "Name:" }, 
            { PersonDescriptionField.Age, "Age:" }, 
            { PersonDescriptionField.Education ,"Highest Level of Education:"}, 
            { PersonDescriptionField.Income ,"Income:" }
        };

        public Person(string name, ushort age, EducationLevel education, Money income)
        {
            this.Name = name;
            this.Age = age;
            this.Education = education;
            this.Income = income;
        }

        public static Person CreateFromDescription(string personDescription)
        {
            String name = GetFieldValueFromPersonDescription<String>(personDescription, PersonDescriptionFieldKeys[PersonDescriptionField.Name]);
            ushort age = GetFieldValueFromPersonDescription<ushort>(personDescription, PersonDescriptionFieldKeys[PersonDescriptionField.Age]);
            EducationLevel education = GetLevelOfEductionFieldValueFromPersonDescription(personDescription) ?? EducationLevel.Unknown;
            Money income = GetIncomeFieldValueFromPersonDescription(personDescription);
            
            var person = new Person(name, age, education, income);
            return person;
        }

        public static List<Person> CreateMultipleFromDescriptions(Stream personDescriptions)
        {
            var persons = new List<Person>();
            var reader = new StreamReader(personDescriptions);

            while (reader.EndOfStream == false)
            {
                String description = reader.ReadLine();
                Person person = CreateFromDescription(description);
                persons.Add(person);
            }

            return persons;
        }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Age == other.Age && Education == other.Education && Income.Equals(other.Income);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Person) obj);
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Age.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Education;
                hashCode = (hashCode * 397) ^ Income.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator == (Person left, Person right)
        {
            return Equals(left, right);
        }

        public static bool operator != (Person left, Person right)
        {
            return !Equals(left, right);
        }

        public static T GetFieldValueFromPersonDescription<T>(String personDescription, String fieldKey) 
        {
            int startingIndex = personDescription.IndexOf(fieldKey, StringComparison.Ordinal);
            int? startingIndexOfNextField = FindIndexOfNextField(personDescription, startingIndex);
            
            var fieldValueBuilder = new StringBuilder();
            
            for (int i = (startingIndex + fieldKey.Length); i < (startingIndexOfNextField ?? personDescription.Length); i++)
            {
                fieldValueBuilder.Append(personDescription[i]);
            }

            String fieldValue = fieldValueBuilder.ToString();
            fieldValue = fieldValue.CleanEnds(' ', ',');
            
            return (T)((IConvertible) fieldValue).ToType(typeof(T), CultureInfo.CurrentCulture);
        }
        
        public static EducationLevel? GetLevelOfEductionFieldValueFromPersonDescription(String personDescription)
        {
            String educationLevelFieldKey = PersonDescriptionFieldKeys[PersonDescriptionField.Education];
            String educationLevelFieldValue = GetFieldValueFromPersonDescription<String>(personDescription, educationLevelFieldKey);

            EducationLevel? educationLevel = null;
            
            foreach (EducationLevel educationLevelValue in (EducationLevel[]) Enum.GetValues(typeof(EducationLevel)))
            {
                if (Utility.AreEquivalent(educationLevelValue.ToString(), educationLevelFieldValue))
                {
                    educationLevel = educationLevelValue;
                    break;
                }
            }
            
            return educationLevel;
        }

        public static Money GetIncomeFieldValueFromPersonDescription(String personDescription)
        {
            String incomeFieldKey = PersonDescriptionFieldKeys[PersonDescriptionField.Income];
            String incomeFieldValue = GetFieldValueFromPersonDescription<String>(personDescription, incomeFieldKey);
            Money income = Money.Parse(incomeFieldValue);
            return income;
        }

        private static int? FindIndexOfNextField(String personDescription, int startingIndex)
        {
            List<int> startingIndicesForEachField = FindStartingIndicesOfDescriptionFields(personDescription);
            List<int> indicesAfterCurrent = startingIndicesForEachField.Where((int index) => index > startingIndex).ToList();
            indicesAfterCurrent.Sort();

            if (indicesAfterCurrent.Count > 0)
            {
                return indicesAfterCurrent[0];
            }
            else
            {
                return null;
            }
        }
        
        private static List<int> FindStartingIndicesOfDescriptionFields(string personDescription)
        {
            var indices = new List<int>();

            foreach (string fieldKey in Person.PersonDescriptionFieldKeys.Values.ToList())
            {
                int index = personDescription.IndexOf(fieldKey, StringComparison.Ordinal);
                indices.Add(index);
            }

            return indices;
        }

        public enum PersonDescriptionField
        {
            Name, 
            Age, 
            Education,
            Income
        }
    }


    public enum EducationLevel
    {
        Unknown,
        GradeSchool,
        HighSchool,
        College
    }
}