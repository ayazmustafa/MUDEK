using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mudek.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mudek.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            return View();
        }

        //3 tane button var.
        //    1. button = basıldığında sadece doc'u okuyacak ve onları sergileyecek.
        //    2. button = basıldığında database'e soruları kaydedecek ve dropdownları görünür kılacak
        //                ML ile de dropdownları eşleştirecek.
        //    3. button = basıldığında soruları çıktılarla eşleştirip anasayfaya atacak.


        public IActionResult Create(int id)
        {
            ViewBag.ExamId = id;
            HttpContext.Session.SetInt32("ExamId", id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file)
        {
            // kullanici id'si

            if (file == null || file.Length == 0)
            {
                return View();
            }

            string fileExtension = Path.GetExtension(file.FileName);

            if (fileExtension != ".docx")
            {
                return View();
            }

            var fileName = Guid.NewGuid().ToString() + fileExtension;

            ViewBag.FILE = fileName;

            var targetPath = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot", "Trash",
                fileName);

            using (var fileStream = new FileStream(targetPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }


            //var All = "";

            using (var doc = WordprocessingDocument.Open(targetPath, false))
            {
                //int questionCount = doc.MainDocumentPart.Document.Body.Elements().OfType<Paragraph>().Count();
                var allQuestions = new List<string>();

                foreach (var question in doc.MainDocumentPart.Document.Body.Elements().OfType<Paragraph>())
                {
                    if (!string.IsNullOrEmpty(question.InnerText))
                        allQuestions.Add(question.InnerText + Environment.NewLine);
                    //All += question.InnerText + Environment.NewLine;
                    //question.AddAnnotation(question.InnerText + Environment.NewLine);
                }

                ViewBag.AllText = allQuestions;
                // return Content(All);
            }


            //System.IO.File.Delete(fileName);

            return View();
        }

        //[HttpPost]
        //public IActionResult Editor(string editor)
        //{
        //    return Redirect("/Teacher/Index");
        //}

  

        [HttpPost]
        public IActionResult Match(string editor)
        {
            // kullanici id'si
            var examId = HttpContext.Session.GetInt32("ExamId");
            var courseId = HttpContext.Session.GetInt32("CourseId");

            var courseName = _context.Courses.Where(x => x.Id == courseId).Select(x => x.Name).FirstOrDefault();
            string[] rawQuestions = editor.Split(new string[] {"<br />\r\n<br />"}, StringSplitOptions.None);

            HtmlDocument htmlDocument = new HtmlDocument();

            List<Question> questions = new List<Question>();


            foreach (var question in rawQuestions)
            {
                htmlDocument.LoadHtml(question);
                var questionWithoutHtmlTags = htmlDocument.DocumentNode.InnerText;
                questionWithoutHtmlTags = WebUtility.HtmlDecode(questionWithoutHtmlTags);
                //Regex.Replace(word, "<.*?>", String.Empty);

                questions.Add(new Question
                {
                    QuestionText = questionWithoutHtmlTags,
                    ExamId = Convert.ToInt32(examId)
                });
            }

            //course_name = sys.argv[0]
            //language = sys.argv[1]
            //amount = sys.argv[2]
            //questions = []
            // call python here
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\pc\AppData\Local\Programs\Python\Python39\python.exe";

            var script = Path.Combine(
                Directory.GetCurrentDirectory(), "Script", "predict.py");

            // var soruSayisi = "10";
            var language = "turkish";
            psi.Arguments = $"\"{script}\" \"{courseName}\" \"{language}\" \"{rawQuestions.Length}\" ";
            foreach (var item in questions)
            {
                psi.Arguments += $"\"{item.QuestionText.Trim()}\" ";
            }

            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var pythonResult = "";  

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                pythonResult = process.StandardOutput.ReadToEnd();
            }
            // pthon, 0 => c#, 1
            //var pythonOutcomes = pythonResult.Split('|');

            //IEnumerable<Tuple<Question, string>> pthon;

            ViewData["PythonOutcomes"] = pythonResult;

            // burada id alıyor value'sunu da burayı benim tekrar soruid ile eşleştirmem lazım gibi
            ViewData["CourseOutcomeId"] = new SelectList(_context.CourseOutcomes.Include(x => x.Course)
                    .Where(x => x.CourseId == courseId),
                "Id", "OutcomeText"); // 1 => selected value

            //words = words.Take(words.Count() - 1).ToArray();
            return View("Match", questions);
        }


        [HttpPost]
        public async Task<IActionResult> SaveMatches(IEnumerable<Question> questions)
        {
            // onceden sınav soruları girilmisse direkt don
            foreach (var item in questions)
            {
                var doesExist = _context.Questions.Where(x => x.ExamId == item.ExamId).Any();
                if (doesExist)
                    return Redirect("/Teacher/Index");
            }

            var examId = HttpContext.Session.GetInt32("ExamId");
            var courseId = HttpContext.Session.GetInt32("CourseId");

            var students = (from s in _context.Students
                join soc in _context.StudentOpenedCourses on s.Id equals soc.StudentId
                //join p in _context.Points on soc.Id equals p.StudentOpenedCourseId
                where soc.OpenedCourseId == courseId
                select soc).ToList();


            // kullanici id'si
            if (ModelState.IsValid)
            {
                int whichQuestion = 1;
                foreach (var item in questions)
                {
                    item.ExamId = (int) examId;
                    item.WhichQuestion = whichQuestion;
                    whichQuestion++;
                }

                await _context.AddRangeAsync(questions);
                await _context.SaveChangesAsync();


                var points = new List<Point>();
                foreach (var student in students)
                {
                    foreach (var question in questions)
                    {
                        var point = new Point
                        {
                            Score = 0,
                            QuestionId = question.Id,
                            StudentOpenedCourseId = student.Id
                        };
                        points.Add(point);
                        _context.Points.Add(point);
                    }
                }

                await _context.SaveChangesAsync();


                return Redirect("/Teacher/Index");
            }

            return View("");
        }


        //public IActionResult Editor(int id, string editor)
        //{
        //    // kullanici id'si
        //    string[] words = editor.Split(new string[] { "<br />\r\n<br />" }, StringSplitOptions.None);

        //    HtmlDocument htmlDocument = new HtmlDocument();

        //    //List<Question> questions = new List<Question>();
        //    List<string> countries = new List<string>();
        //    foreach (var word in words)
        //    {
        //        htmlDocument.LoadHtml(word);
        //        var questionWithoutHtmlTags = htmlDocument.DocumentNode.InnerText;
        //        questionWithoutHtmlTags = WebUtility.HtmlDecode(questionWithoutHtmlTags);
        //        //Regex.Replace(word, "<.*?>", String.Empty);
        //        countries.Add(questionWithoutHtmlTags);
        //    }
        //    //words = words.Take(words.Count() - 1).ToArray();
        //    return View("Hmm", countries);
        //}
    }
}