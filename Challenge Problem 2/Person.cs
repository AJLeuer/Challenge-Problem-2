using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NodaMoney;

namespace ChallengeProblem2 
{
    public class Person : IEquatable<Person>, IComparable<Person>
    {
        public Name           Name      { get; }
        public ushort         Age       { get; }
        public EducationLevel Education { get; }
        public Money          Income    { get; }

        public static readonly Dictionary<PersonDescriptionField, String> PersonDescriptionFieldKeys = new Dictionary<PersonDescriptionField, String>
        {
            { PersonDescriptionField.Name, "Name:" }, 
            { PersonDescriptionField.Age, "Age:" }, 
            { PersonDescriptionField.Education ,"Highest Level of Education:"}, 
            { PersonDescriptionField.Income ,"Income:" }
        };

        public Person(string name, ushort age, EducationLevel education, Money income)
        {
            this.Name = new Name(name);
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
        
        #region Equality
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
        

        #endregion
        
        #region Comparison
        public int CompareTo(Person other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Comparer<Name>.Default.Compare(Name, other.Name);
        }

        public static bool operator <(Person left, Person right)
        {
            return Comparer<Person>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(Person left, Person right)
        {
            return Comparer<Person>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(Person left, Person right)
        {
            return Comparer<Person>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(Person left, Person right)
        {
            return Comparer<Person>.Default.Compare(left, right) >= 0;
        }
        #endregion

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

    public class Name : IEquatable<Name>, IComparable<Name>
    {
        public String First { get; }
        public String Last  { get; }

        public Name(String fullName)
        {
            (String first, String last) = SplitFullNameIntoComponents(fullName);
            this.First = first;
            this.Last = last;
        }

        public override String ToString()
        {
            return $"{First} {Last}";
        }

        private static (String first, String last) SplitFullNameIntoComponents(string fullName)
        {
            String[] nameComponents = fullName.Split(' ');
            (String first, String last) names = (nameComponents[0], nameComponents.Last());
            return names;
        }

        #region Equality
        public bool Equals(Name other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(First, other.First) && string.Equals(Last, other.Last);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Name) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((First != null ? First.GetHashCode() : 0) * 397) ^ (Last != null ? Last.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Name left, Name right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Name left, Name right)
        {
            return !Equals(left, right);
        }
        

        #endregion

        #region Comparison
        public int CompareTo(Name other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return string.Compare(Last, other.Last, StringComparison.Ordinal);
        }

        public static bool operator < (Name left, Name right)
        {
            return Comparer<Name>.Default.Compare(left, right) < 0;
        }

        public static bool operator > (Name left, Name right)
        {
            return Comparer<Name>.Default.Compare(left, right) > 0;
        }

        public static bool operator <= (Name left, Name right)
        {
            return Comparer<Name>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >= (Name left, Name right)
        {
            return Comparer<Name>.Default.Compare(left, right) >= 0;
        }
        #endregion
    }


    public enum EducationLevel
    {
        Unknown,
        
        [Description("Grade School")]
        GradeSchool,
        
        [Description("High School")]
        HighSchool,
        
        [Description("College")]
        College
    }
}