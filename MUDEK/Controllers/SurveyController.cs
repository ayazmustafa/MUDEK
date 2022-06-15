using Microsoft.AspNetCore.Mvc;
using Mudek.Extensions;
using Mudek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mudek.Controllers
{
    public class SurveyController : Controller
    {

        private readonly ApplicationDbContext _context;
        public SurveyController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult GraduateSurvey(Guid id)
        {
            var person = _context.Surveys.Where(x => x.Id == id).FirstOrDefault();
            return View(person);
        }
        

        [HttpPost]
        public IActionResult GraduateSurvey(string jsonResult, Guid Id)
        {
            //var a = jsonResult;
            //byte[] jsonPayloadReceivedFromSomewhere = Encoding.UTF8.GetBytes(jsonResult);
            //var result = JsonProcessor.JsonParser(jsonPayloadReceivedFromSomewhere, "Pozisyon");

            var survey = _context.Surveys.Where(x => x.Id == Id).FirstOrDefault();
            survey.SurveyResult = jsonResult;
            survey.SurveyCompletionDate = DateTime.Now;
            _context.Update(survey);
            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        public List<string> GetGraduateQuestions(Guid id)
        {
            var survey = _context.Surveys.Where(x => x.Id == id).FirstOrDefault();

            var surveyQuestions = _context.DepartmentOutcomes
                .Where(x => x.DepartmentName == survey.Department)
                .Select(x => x.Outcome)
                .ToList();

            return surveyQuestions; 
        }



        public IActionResult EmployerSurvey(Guid id)
        {
            var person = _context.Surveys.Where(x => x.Id == id).FirstOrDefault();
            return View(person);
        }


        [HttpPost]
        public IActionResult EmployerSurvey(string jsonResult, Guid Id)
        {
			//var a = jsonResult;
			//byte[] jsonPayloadReceivedFromSomewhere = Encoding.UTF8.GetBytes(jsonResult);
			//var result = JsonProcessor.JsonParser(jsonPayloadReceivedFromSomewhere, "AAA");

			var survey = _context.Surveys.Where(x => x.Id == Id).FirstOrDefault();
            survey.SurveyResult = jsonResult;
            survey.SurveyCompletionDate = DateTime.Now;
            _context.Update(survey);
            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        public List<string> GetEmployerQuestions(Guid id)
        {
            var survey = _context.Surveys.Where(x => x.Id == id).FirstOrDefault();

            var surveyQuestions = _context.DepartmentOutcomes
                .Where(x => x.DepartmentName == survey.Department)
                .Select(x => x.Outcome)
                .ToList();

            return surveyQuestions;
        }


        public IActionResult Result()
        {
            return View();
        }

        public IActionResult Results()
        {
            return View();
        }



        [HttpGet]
        public IEnumerable<object> GetColumnChart(string surveyName)
        {

            // TİPİNİ BULMAK İÇİN
			var surveyInfo = _context.Surveys.Where(x => x.SurveyName == surveyName).FirstOrDefault();

            // SONUÇLAR
			var surveys = _context.Surveys.Where(x => x.SurveyName == surveyName).ToList();

            // DEPARTMENT OUTCOMES
			var questions = _context.DepartmentOutcomes
				.Where(x => x.DepartmentName == surveyInfo.Department)
                .Select(x => x.Outcome).ToList();


			//if (surveyInfo.SurveyType == "Graduate")
			//{
				foreach (var survey in surveys)
				{
					byte[] surveyResult = Encoding.UTF8.GetBytes(survey.SurveyResult);
					foreach (var question in questions)
					{
                        var note = JsonProcessor.JsonParser(surveyResult, question);
                        yield return new { question = question, Note = note };
					}
				}
			//}
			//else if (surveyInfo.SurveyType == "Employer")
			//{

			//}
        }
    }
}

//  ww