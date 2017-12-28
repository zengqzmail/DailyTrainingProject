using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormsTask
{
    /**
     * keeps all the scripts used in the form in both English and Spanish
     * keeps all the integer values corresponding to each selected radio button 
     * most of this belongs in .resx files
     **/

    class Constants
    {
        public const string NOT_ANSWERED_STR = "";
        public const int NOT_ANSWERED = -1;
        public const int IDK = 0;
        public const int OTHER = 0;
        public const int MALE = 1;
        public const int FEMALE = 0;
        public const int CAUCASIAN = 1;
        public const int AFRICAN_AMERICAN = 2;
        public const int YES = 1;
        public const int NO = 0;
        public const int MIDDLE_SCHOOL = 1;
        public const int HIGH_SCHOOL = 2;
        public const int HIGH_SCHOOL_DIPLOMA = 3;
        public const int COLLEGE = 4;
        public const int COLLEGE_DEGREE = 5;
        public const int HIGHER = 6;
        public const int ENGLISH = 1;
        public const int SPANISH = 2;
        public const int PART_TIME = 1;
        public const int FULL_TIME = 2;
        public const int UNEMPLOYED = 3;
        public const int RETIRED = 4;
        public const int ONE_MONTH = 1;
        public const int TWO_MONTHS = 2;
        public const int SIX_MONTHS = 3;
        public const int ONE_YEAR = 4;
        public const int TWO_YEAR = 5;
        public const int MORE = 6;
        public const int ALONE = 1;
        public const int FRIEND_OR_FAMILY = 2;
        public const int UNSUPERVISED_FACILITY = 3;
        public const int SUPERVISED_FACILITY = 4;
        public const int PAGE_WELCOME = 0;
        public const int PAGE_GROUP_1 = 1;
        public const int PAGE_GROUP_2 = 2;
        public const int PAGE_GROUP_3 = 3;
        public const int PAGE_END = 4;
        public const int INVALID_INPUT = -999; //should only be used in cases of page skipped

        public class EnglishText
        {
            public const string QUESTION_TITLE_1 = "1. What is your current age?";
            public const string QUESTION_TITLE_2 = "2. What is today’s date?";
            public const string QUESTION_TITLE_3 = "3. What is your Gender?";
            public const string MALE = "Male";
            public const string FEMALE = "Female";
            public const string QUESTION_TITLE_4 = "4. What is your race?";
            public const string CAUCASIAN = "Caucasian"; 
            public const string AFRICAN_AMERICAN = "African American";
            public const string OTHER = "Other";
            public const string OTHER_SPECIFY = "Other (specify)";
            public const string QUESTION_TITLE_5= "5. Do you consider yourself to be Hispanic or Latino?";
            public const string YES = "Yes";
            public const string NO = "No";
            public const string QUESTION_TITLE_6 = "6. What is your highest level of education?";
            public const string MIDDLE_SCHOOL = "Middle school or less";
            public const string HIGH_SCHOOL = "Some high school";
            public const string HIGH_SCHOOL_DIPLOMA  = "High school diploma or equivalent";
            public const string COLLEGE = "Some college";
            public const string COLLEGE_DEGREE = "College degree";
            public const string HIGHER = "Higher than college";
            public const string IDK = "--I don’t know--";
            public const string QUESTION_TITLE_7 = "7.	How many years of formal education have you had?";
            public const string QUESTION_TITLE_8 = "8.	What is your primary language?";
            public const string ENGLISH = "English"; 
            public const string SPANISH = "Spanish";
            public const string QUESTION_TITLE_9 = "9.	What is your current employment status?";
            public const string PART_TIME = "Employed Part time";
            public const string FULL_TIME = "Employed Full time";
            public const string UNEMPLOYED = "Unemployed";
            public const string RETIRED  = "Retired";
            public const string QUESTION_TITLE_10_A = "10.a. How long have you been unemployed?";
            public const string ONE_MONTHS = "Less than 1 month";
            public const string TWO_MONTHS = "2 – 6 months";
            public const string SIX_MONTHS = "6 – 12 months";
            public const string ONE_YEAR = "1 – 2 years";
            public const string TWO_YEAR = "2 – 5 years";
            public const string MORE = "Greater than 5 years";
            public const string QUESTION_TITLE_10_B = "10.b. Have you ever had a job?";
            public const string QUESTION_TITLE_10 = "10. What is your current job?";
            public const string QUESTION_TITLE_11 = "11. What is your living status?";
            public const string ALONE = "live alone";
            public const string FRIEND_OR_FAMILY = "live with a friend or family member";
            public const string UNSUPERVISED_FACILITY= "live in an unsupervised residential facility";
            public const string SUPERVISED_FACILITY = "live in a supervised residential facility";
            public const string PREVIOUS_PAGE = "Previous Page";
            public const string NEXT_PAGE = "Next Page";
            public const string SUBMIT_FORM = "Submit Form";
            public const string END = "End of task";
            public const string Instruction = "Please answer the following questions about yourself using the computer";
        }

        public class SpanishText
        {
            public const string QUESTION_TITLE_1 = "1. Cuántos años tienes?";
            public const string QUESTION_TITLE_2 = "2. Cuál es la fecha de hoy?";
            public const string QUESTION_TITLE_3 = "3. Cuál es su sexo?";
            public const string MALE = "Maculino";
            public const string FEMALE = "Femenino";
            public const string QUESTION_TITLE_4 = "4. Cuál es su raza?";
            public const string CAUCASIAN = "Blanco, Anglosajón";
            public const string AFRICAN_AMERICAN = "Negro, Afro Americano";
            public const string OTHER = "Otro";
            public const string OTHER_SPECIFY = "Otro (Especificar)";
            public const string QUESTION_TITLE_5 = "5. Se considera usted Hispano o Latino?";
            public const string YES = "Si";
            public const string NO = "No";
            public const string QUESTION_TITLE_6 = "6. Cuál es su nivel máximo de educación?";
            public const string MIDDLE_SCHOOL = "Escuela secundaria o menos";
            public const string HIGH_SCHOOL = "Algunos años de bachillerato";
            public const string HIGH_SCHOOL_DIPLOMA = "Título de Bachillerato";
            public const string COLLEGE = "Algunos semestres en la universidad";
            public const string COLLEGE_DEGREE = "Títuto Universitario";
            public const string HIGHER = "Postgrado/Doctorado";
            public const string IDK = "-- Yo no sé --";
            public const string QUESTION_TITLE_7 = "7. Cuántos años de educación formal realizó usted?";
            public const string QUESTION_TITLE_8 = "8. Cuál es su idoma principal?";
            public const string ENGLISH = "Ingles";
            public const string SPANISH = "Español";
            public const string QUESTION_TITLE_9 = "9. Cuál es su estado actual de empleo";
            public const string PART_TIME = "Trabajando de medio tiempo";
            public const string FULL_TIME = "Trabajando de tiempo completo ";
            public const string UNEMPLOYED = "Desempleado";
            public const string RETIRED = "Jubilado/Retirado";
            public const string QUESTION_TITLE_10_A = "10.a. Cuánto tiempo llevas estar desempleado?";
            public const string ONE_MONTHS = "Menos de un mes";
            public const string TWO_MONTHS = "2 – 6 meses";
            public const string SIX_MONTHS = "6 – 12 meses";
            public const string ONE_YEAR = "1 – 2 años";
            public const string TWO_YEAR = "2 – 5 años";
            public const string MORE = "Más de 5 años";
            public const string QUESTION_TITLE_10_B = "10.b. Has tenido trabaja antes?";
            public const string QUESTION_TITLE_10 = "10. Cuál es su empleo actual?";
            public const string QUESTION_TITLE_11 = "11. Cómo vive usted?";
            public const string ALONE = "Vive solo";
            public const string FRIEND_OR_FAMILY = "Vive con un amigo o con un miembro de su familia";
            public const string UNSUPERVISED_FACILITY = "Vive en una casa residencial sin supervisión";
            public const string SUPERVISED_FACILITY = "Vive en una casa residencial bajo supervisión";
            public const string PREVIOUS_PAGE = "Página Anterior";
            public const string NEXT_PAGE = "Página Siguiente";
            public const string SUBMIT_FORM = "Entregar";
            public const string END = "End of task";
            public const string Instruction = "Por favor responder las siguientes preguntas sobre usted y su uso de computadora";
        }


    }
}
