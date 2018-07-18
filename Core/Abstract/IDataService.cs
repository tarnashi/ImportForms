using Core.Models;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Abstract
{
    public interface IDataService
    {
        #region Person
        List<PersonViewModel> GetPersons();
        void AddPerson(PersonViewModel personViewModel);
        void UpdatePerson(PersonViewModel personViewModel);
        void DeletePerson(int personId);
        void AddPersons(List<PersonImportModel> personImportModels);
        string ValidatePersonViewModel(ref PersonViewModel personViewModel);
        string GetPersonsCsv(string delimiter = "\t");
        #endregion
    }
}
