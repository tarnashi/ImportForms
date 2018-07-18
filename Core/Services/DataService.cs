using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Abstract;
using Core.Models;
using Data.DataAccess;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Services
{
    public class DataService : IDataService
    {
        private readonly ImportFormsDbContext _ctx;
        private readonly IMapper _mapper;

        public DataService()
        {
            _ctx = new ImportFormsDbContext();
            _mapper = new MapperConfig().GetMapper();
        }

        #region Persons
        public List<PersonViewModel> GetPersons()
        {
            return _ctx.Persons.ProjectTo<PersonViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public void AddPerson(PersonViewModel personViewModel)
        {
            var person = _mapper.Map<Person>(personViewModel);
            _ctx.Persons.Add(person);
            _ctx.SaveChanges();
        }

        public void UpdatePerson(PersonViewModel personViewModel)
        {
            var person = _ctx.Persons.Find(personViewModel.Id);
            _mapper.Map(personViewModel, person);
            _ctx.SaveChanges();
        }

        public void DeletePerson(int personId)
        {
            var person = _ctx.Persons.Find(personId);
            _ctx.Persons.Remove(person);
            _ctx.SaveChanges();
        }

        public void AddPersons(List<PersonImportModel> personImportModels)
        {
            var persons = GetPersonsFromImportModels(personImportModels);
            _ctx.Persons.AddRange(persons);
            _ctx.SaveChanges();
        }

        private List<Person> GetPersonsFromImportModels(List<PersonImportModel> personImportModels)
        {
            List<Person> persons = new List<Person>();
            foreach (var model in personImportModels)
            {
                //фамилия должна быть указана
                if (string.IsNullOrWhiteSpace(model.FullName))
                    continue;

                DateTime birthday;
                bool birthdayParseResult = DateTime.TryParse(model.Birthday, out birthday);

                string email = ValidateAndCorrectEmail(model.Email);
                string phoneMobile = ValidateAndCorrectPhoneMobile(model.PhoneMobile);

                //должен быть хотя бы один способ связи
                if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(phoneMobile))
                    continue;

                persons.Add(new Person
                {
                    FullName = model.FullName,
                    Birthday = birthdayParseResult ? (DateTime?)birthday : null,
                    Email = email,
                    PhoneMobile = phoneMobile
                });
            }

            return persons;
        }
        #endregion

        private string ValidateAndCorrectEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address;
            }
            catch
            {
                return null;
            }
        }

        private string ValidateAndCorrectPhoneMobile(string phoneMobile)
        {
            Regex digitsOnly = new Regex(@"[^\d]");
            string result = digitsOnly.Replace(phoneMobile, "");
            if (result.Length == 11)
                return result;
            else
                return null;
        }

        public string ValidatePersonViewModel(ref PersonViewModel personViewModel)
        {
            if (string.IsNullOrWhiteSpace(personViewModel.FullName))
                return "Укажите имя";

            if (!string.IsNullOrEmpty(personViewModel.Email))
            {
                var correctEmail = ValidateAndCorrectEmail(personViewModel.Email);
                if (string.IsNullOrEmpty(correctEmail))
                    return "Укажите корректный email";
                personViewModel.Email = correctEmail;
            }

            if (!string.IsNullOrEmpty(personViewModel.PhoneMobile))
            {
                var correctPhone = ValidateAndCorrectPhoneMobile(personViewModel.PhoneMobile);
                if (string.IsNullOrEmpty(correctPhone))
                    return "Укажите корректный телефон";
                personViewModel.PhoneMobile = correctPhone;
            }

            if (string.IsNullOrEmpty(personViewModel.Email) && string.IsNullOrEmpty(personViewModel.PhoneMobile))
                return "Должен быть указан хотя бы один способ связи!";

            return string.Empty;
        }

        public string GetPersonsCsv(string delimiter)
        {
            var persons = _ctx.Persons.ToList().Select(p => string.Format("{0}{4}{1}{4}{2}{4}{3}", p.FullName, p.Birthday?.ToString("dd.MM.yyyy"), p.Email, p.PhoneMobile, delimiter));
            return string.Join(Environment.NewLine, persons);
        }
    }
}
