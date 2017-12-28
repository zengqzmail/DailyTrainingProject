using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrescriptionRefill
{
    class ConstValues
    {
       public class Status
        {
           public const int LANGUAGE_SELECTION = 0;
           public const int REFILL = 10;
           public const int ENTER_RX_NUMBER = 20;
           public const int ELIGIBLE_RX_NUMBER = 30;
           public const int PICK_UP_TIME = 40;
           public const int AM_OR_PM = 50;
           public const int READY_TO_PICK_UP = 60;
           public const int START_OVER = 70;
           public const int END = 80;
           public const int VALID_PM_TIME = 90;

        }

       public class Language
       {
           public const int ENGLISH = 0;
           public const int SPANISH = 1;
       }

       public class AudioFile
       {
           public const string INTRODUCTION = "Resources/PRT-INTRO.m4a";
           public const string LANGUAGE_SELECTION = "Resources/PRT-EN01-2.m4a";
           public const string REFILL_EN = "Resources/PRT-EN02.m4a";
           public const string REFILL_SP = "Resources/PRT-SP02.m4a";
           public const string ENTER_RX_NUMBER_EN = "Resources/PRT-EN03.m4a";
           public const string ENTER_RX_NUMBER_SP = "Resources/PRT-SP03.m4a";
           public const string ELIGIBLE_RX_NUMBER_EN = "Resources/PRT-EN04.m4a";
           public const string ELIGIBLE_RX_NUMBER_SP = "Resources/PRT-SP04.m4a";
           public const string PICK_UP_TIME_EN = "Resources/PRT-EN05.m4a";
           public const string PICK_UP_TIME_SP = "Resources/PRT-SP05.m4a";
           public const string AM_OR_PM_EN = "Resources/PRT-EN06.m4a";
           public const string AM_OR_PM_SP = "Resources/PRT-SP06.m4a";
           public const string READY_TO_PICK_UP_EN_Today = "Resources/PRT-EN07a.wav";
           public const string READY_TO_PICK_UP_EN_Tomorrow = "Resources/PRT-EN07b.wav";
           public const string READY_TO_PICK_UP_TODAY_SP = "Resources/PRT-SP07a.wav";
           public const string READY_TO_PICK_UP_TOMORROW_SP = "Resources/PRT-SP07b.wav";
         
           public const string START_OVER_EN = "Resources/PRT-EN08.m4a";
           public const string START_OVER_SP = "Resources/PRT-SP08.m4a";
           public const string GOOD_BYE_EN = "Resources/PRT-EN09.wav";
           public const string GOOD_BYE_SP = "Resources/PRT-SP09.wav";
           public const string ERROR_RX_NUMBER_EN = "Resources/PRT-EN-ERR02.m4a";
           public const string ERROR_RX_NUMBER_SP = "Resources/PRT-SP-ERR02.m4a";
           public const string ERROR_INVALID_CHOICE_EN = "Resources/PRT-EN-ERR03.m4a";
           public const string ERROR_INVALID_CHOICE_SP = "Resources/PRT-SP-ERR03.m4a";
           public const string ERROR_INVALID_TIME_EN = "Resources/PRT-EN-ERR04.m4a";
           public const string ERROR_INVALID_TIME_SP = "Resources/PRT-SP-ERR04.m4a";
           public const string ERROR_PHARMACY_CLOSED_EN = "Resources/PRT-EN-ERR05.m4a";
           public const string ERROR_PHARMACY_CLOSED_SP = "Resources/PRT-sp-ERR05.m4a";
           public const string BUTTON_SOUND = "Resources/buttonSound.wav";
    
       }
    }
}
