using System;
using NodaMoney;

namespace ChallengeProblem2
{
    public class Person : IEquatable<Person>
    {
        public string Name { get; set; }
        public ushort Age { get; set; }
        public EducationLevel Education { get; set; }
        public Money Income { get; set; }

        public Person(string name, ushort age, EducationLevel education, Money income)
        {
            this.Name = name;
            this.Age = age;
            this.Education = education;
            this.Income = income;
        }

        public static Person CreateFromStringDescription(string personDescription)
        {
            throw new NotImplementedException();
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
    }

    public enum EducationLevel
    {
        GradeSchool,
        HighSchool,
        College
    }
}