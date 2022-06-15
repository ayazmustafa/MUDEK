using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mudek.Models
{
    public class Survey
    {
        [Key]
        public Guid Id { get; set; }

        public string SurveyResult { get; set; }
        public string Mail { get; set; }
        public string SurveyName { get; set; }
        public string Department { get; set; }

        public string SurveyType { get; set; }
        public DateTime SurveyDate { get; set; }
        public DateTime EmailCreationDate { get; set; }
        public DateTime SurveyCompletionDate { get; set; }


    }

}
