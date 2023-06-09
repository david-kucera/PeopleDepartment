namespace PeopleDepartment.CommonLibrary
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string? TitleBefore { get; }
        public string? TitleAfter { get; }
        public string? Position { get; set; }
        public string Email { get; set; }
        public List<string> Departments { get; }

        public Person(string firstName, string lastName, string displayName, string? position, string email, List<string> departments)
        {
            FirstName = firstName;
            LastName = lastName;
            DisplayName = displayName;

            // napr. Mgr. Peter Novotný, PhD.
            var data = DisplayName.Split(" ");

            var lastNameWithComma = LastName + ",";

            for (var i = 0; i < data.Length; i++)
            {
                if (data[i].Equals(FirstName))
                {
                    var titlesBefore = "";
                    for (var j = 0; j < i; j++)
                    {
                        titlesBefore += data[j];
                        // If person has more titles
                        if (j != i)
                        {
                            titlesBefore += " ";
                        }
                    }
                    TitleBefore = titlesBefore;
                }
                if (data[i].Equals(lastNameWithComma))
                {
                    var titlesAfter = "";
                    for (var k = i+1; k < data.Length; k++)
                    {
                        titlesAfter += data[k];
                        // If person has more titles
                        if (k != i)
                        {
                            titlesAfter += " ";
                        }
                    }
                    TitleAfter = titlesAfter;
                }
            }


            Position = position;
            Email = email;
            Departments = departments;
        }

        public string ToFormattedString()
        {
            var val = "";
            val += DisplayName;

            var numberOfWhiteSpaces = 40 - val.Length;

            for (var i = 0; i < numberOfWhiteSpaces; i++)
            {
                val += " ";
            }

            val += Email;

            return val;
        }
    }
}