using Core.Abstract;
using Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System;
using System.Text;

namespace Web.Api
{
    public class PersonsApiController : BaseApiController
    {
        private readonly IDataService _dataService;

        public PersonsApiController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        public JsonResult UploadPersons()
        {
            List<PersonImportModel> personImportModels = new List<PersonImportModel>();

            if (this.Request.Files.Count <= 0)
                return FailJsonResponse("Не было загружено ни одного файла!");

            for (var i = 0; i < this.Request.Files.Count; i++)
            {
                var file = this.Request.Files[i];
                var reader = new StreamReader(file.InputStream, Encoding.Default);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('\t');

                    if (values.Count() >= 4)
                        personImportModels.Add(new PersonImportModel
                        {
                            FullName = values[0],
                            Birthday = values[1],
                            Email = values[2],
                            PhoneMobile = values[3]
                        });
                }
            }

            _dataService.AddPersons(personImportModels);

            return SuccessJsonResponse(message: "Файлы загружены");
        }

        [HttpPost]
        public JsonResult DeletePerson(int personId)
        {
            _dataService.DeletePerson(personId);
            return SuccessJsonResponse();
        }

        [HttpPost]
        public JsonResult UpdatePerson(PersonViewModel personViewModel)
        {
            string errorMessage = _dataService.ValidatePersonViewModel(ref personViewModel);
            if (string.IsNullOrEmpty(errorMessage))
            {
                _dataService.UpdatePerson(personViewModel);
                return SuccessJsonResponse(personViewModel);
            }
            else
            {
                return FailJsonResponse(errorMessage);
            }
        }

        [HttpGet]
        public FileContentResult DownloadPersonsCsv()
        {
            string csv = _dataService.GetPersonsCsv();
            var data = Encoding.UTF8.GetBytes(csv);
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            return File(result, "text/csv", string.Format("Persons_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));
        }

        [HttpPost]
        public JsonResult AddPerson(PersonViewModel personViewModel)
        {
            string errorMessage = _dataService.ValidatePersonViewModel(ref personViewModel);
            if (string.IsNullOrEmpty(errorMessage))
            {
                _dataService.AddPerson(personViewModel);
                return SuccessJsonResponse(message: "Запись добавлена");
            }
            else
            {
                return FailJsonResponse(errorMessage);
            }
        }
        
    }
}