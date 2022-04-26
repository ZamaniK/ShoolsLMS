﻿using ShoolsLMS.DataRepos.BLL;
using ShoolsLMS.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoolsLMS.Models.ViewModels
{
    public class ExamTakingModel
    {
        public Question Question
        {
            get
            {
                if (QuestionId != 0)
                {
                    var eManager = new QuizSetManager();

                    return eManager.GetQuestionByID(QuestionId);
                }
                return null;
            }
        }

        public int QuestionId { get; set; }
        public int Choice { get; set; }

        public ExamTakingModel()
        {

            var eManager = new QuizSetManager();

            eManager.GetQuestionByID(QuestionId);
        }
    }
}