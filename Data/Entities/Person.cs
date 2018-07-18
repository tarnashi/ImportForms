using System;

namespace Data.Entities
{
    public class Person
    {
        //public Person(string fullName, DateTime? birthday, string email, string phoneMobile)
        //{
        //    FullName = fullName;
        //    Birthday = birthday;
        //    Email = email;
        //    PhoneMobile = phoneMobile;
        //}

        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string PhoneMobile { get; set; }
    }
}
