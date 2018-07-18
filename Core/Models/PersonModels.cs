using System;

namespace Core.Models
{
    public class PersonImportModel
    {
        public string FullName { get; set; }
        public string Birthday { get; set; }
        public string Email { get; set; }
        public string PhoneMobile { get; set; }
    }

    public class PersonViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string PhoneMobile { get; set; }
    }
}
