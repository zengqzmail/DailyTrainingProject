using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormsTask
{/**
  * Keeps the answer provided by the subject
  * This can later be a database entity
  */
    class Answer
    {
        public int formLanguage;
        public int age;
        public int gender;
        public int race;
        public string otherRace;
        public int latino;
        public int education;
        public int educationDuration;
        public int primaryLanguage;
        public string primaryLanguageOther;
        public int employmentStatus;
        public int unemploymentDuration;
        public int everWorked;
        public string currentJob;
        public int livingStatus;
        public string livingStatusOther;

        public Answer()
        {
            formLanguage = Constants.ENGLISH;
            age = Constants.NOT_ANSWERED;
            gender = Constants.NOT_ANSWERED;
            race = Constants.NOT_ANSWERED;
            latino = Constants.NOT_ANSWERED;
            education = Constants.NOT_ANSWERED;
            educationDuration = Constants.NOT_ANSWERED;
            primaryLanguage = Constants.NOT_ANSWERED;
            employmentStatus = Constants.NOT_ANSWERED;
            unemploymentDuration = Constants.NOT_ANSWERED;
            everWorked = Constants.NOT_ANSWERED;
            livingStatus = Constants.NOT_ANSWERED;
            livingStatusOther = Constants.NOT_ANSWERED_STR;
            otherRace = Constants.NOT_ANSWERED_STR;
            primaryLanguageOther = Constants.NOT_ANSWERED_STR;
            currentJob = Constants.NOT_ANSWERED_STR;
        }

    }
}
