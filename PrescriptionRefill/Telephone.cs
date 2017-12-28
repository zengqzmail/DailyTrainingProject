using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace PrescriptionRefill
{
    class Telephone
    {
        public int curStatus;
        public DateTime timerStart;
        public TimeSpan THREE_SECONDS = new TimeSpan(0, 0, 3);
        public int language = ConstValues.Language.ENGLISH;
        public int rxNumber = 0;
        public bool singleTrack = true;
        public int pickUpTime = 0;
        public int keyPressedCount = 0;
        public bool today = true;
        public int[] rxNumberList = {  25177789, 44237908 };
        public FormTelephone form1;
        public Form1 form;

        public Telephone(Form1 form1, FormTelephone formTelephone)
        {
           this.form = form1;
        }
        public void refill()
        {
            curStatus = ConstValues.Status.REFILL;
            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.REFILL_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.REFILL_SP);
            }
            Form1.timerDispose();
        }

        public void enterRxNumber()
        {
            rxNumber = 0;
            curStatus = ConstValues.Status.ENTER_RX_NUMBER;
            
            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.ENTER_RX_NUMBER_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.ENTER_RX_NUMBER_SP);
            }
            Form1.timerDispose();
        }

        public void errorRxNumber()
        {
            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.ERROR_RX_NUMBER_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.ERROR_RX_NUMBER_SP);
            }
        }

        //
        //what... the hell???
        //i'm assuming this returns true if the entered RX number is valid, false otherwise.
        public bool isCorrectRxNumber()
        {
            //for (int i = 0; i < rxNumberList.Length; i++)
            //{
            //    if (rxNumber == rxNumberList[i])
            //    {
            //        //rxNumberList[i] = 012345675; //what the hell is this even supposed to do.....? 
            //        return true;
            //    }
            //}
            //return false;
            Console.WriteLine(rxNumberList.Contains(rxNumber));
            return rxNumberList.Contains(rxNumber);
        }

        //
        //called after eleigible RX number is entered through dialpad
        //plays audio notifying user that RX is eligible for refill and prompting to pick up same day or next day
        //
        public void eligibleRxNumber()
        {
            curStatus = ConstValues.Status.ELIGIBLE_RX_NUMBER;
            
            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.ELIGIBLE_RX_NUMBER_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.ELIGIBLE_RX_NUMBER_SP);
            }
            Form1.timerDispose();
        }

        public void errorInvalidChoice()
        {

            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.ERROR_INVALID_CHOICE_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.ERROR_INVALID_CHOICE_SP);
            }
        }


        public void errorInvalidTime()
        {
            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.ERROR_INVALID_TIME_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.ERROR_INVALID_TIME_SP);
            }
        }

        public void errorPharmacyClosed()
        {
            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.ERROR_PHARMACY_CLOSED_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.ERROR_PHARMACY_CLOSED_SP);
            }
        }
        public void enterPickUpTime()
        {
            keyPressedCount = 0;
            pickUpTime = 0;
            curStatus = ConstValues.Status.PICK_UP_TIME;
           
            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.PICK_UP_TIME_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.PICK_UP_TIME_SP);
            }
            Form1.timerDispose();
        }
        public void askAmOrPm()
        {
            curStatus = ConstValues.Status.AM_OR_PM;
            
            if (language == ConstValues.Language.ENGLISH)
            {
                form.playTrack(ConstValues.AudioFile.AM_OR_PM_EN);
            }
            else
            {
                form.playTrack(ConstValues.AudioFile.AM_OR_PM_SP);
            }
            Form1.timerDispose();
        }
        public void validatePickUpTime()   //has been modified by Qingzhou
        {
            //int hour = pickUpTime / 100;
            int min = pickUpTime % 100;
            
            if(pickUpTime >= 2400 || min > 59 || pickUpTime == 0) {
                errorInvalidTime();
                singleTrack = false;
                enterPickUpTime();
            }
            else if(pickUpTime <= 1159)
            {
                askAmOrPm();
            }
            else if(pickUpTime >= 1200 && pickUpTime < 2400)
            {
                //This is tricky
                curStatus = ConstValues.Status.VALID_PM_TIME;
            }
           /* if (hour > 24 || min > 59)
            {

                errorInvalidTime();
                singleTrack = false;
                enterPickUpTime();
                
            }*/
            /*else if (hour < 13)
            {
                askAmOrPm();                
            }*/
            /*else
            {
                if (hour == 24)
                {
                    hour = 0;
                    pickUpTime -= 2400;
                }
                checkTimeRange();
            } */
            singleTrack = true;
            
        }
        public void convertTo24Hour(bool pm)
        {
            if (pm && pickUpTime < 1200)
            {
                pickUpTime += 1200;
            }
            if (!pm && pickUpTime / 100 == 12)
            {
                pickUpTime -= 1200;
            }
        }
        public void checkTimeRange()
        {
            curStatus = ConstValues.Status.READY_TO_PICK_UP;
            
            if (pickUpTime < 900 || pickUpTime > 2300)
            {
                errorPharmacyClosed();
                singleTrack = false;
                enterPickUpTime();
                singleTrack = true;
            }
            else
            {
                readyToPickUp();
            }
            Form1.timerDispose();
        }

        public void readyToPickUp()
        {
            curStatus = ConstValues.Status.START_OVER;
            
            int hour = pickUpTime / 100;
            int min = pickUpTime % 100;
            string minut;
            if (min < 10)
            {
                minut = "0" + min.ToString();
            }
            else
            {
                minut = min.ToString();
            }
            string amOrPm = " am";
            if (hour >= 12)
            {
                amOrPm = " pm";
                hour = hour == 12 ? 12 : hour - 12;
            }
            form.stopPlayer();
            //String str = "<say-as interpret-as=\"time\">" + hour + ":" + min + amOrPm + "</say-as>";
            string time = hour + ":" + minut + " " + amOrPm;
            //modify string to time format
            String str = "<say-as interpret-as=\"time\" format=\"hms12\">"+time+"</say-as>";
            SpeechSynthesizer speech = new SpeechSynthesizer();
            speech.Volume = 95;
            speech.Rate -= 2;
           
            if (language == ConstValues.Language.ENGLISH)
            {
                PromptBuilder prompt = new PromptBuilder(new System.Globalization.CultureInfo("en-US"));
                prompt.AppendAudio(ConstValues.AudioFile.BUTTON_SOUND);
 
                if (today)
                {
                    prompt.AppendAudio(ConstValues.AudioFile.READY_TO_PICK_UP_EN_Today);

                }
                else
                {
                    prompt.AppendAudio(ConstValues.AudioFile.READY_TO_PICK_UP_EN_Tomorrow);
                }
                //prompt.AppendSsmlMarkup(str);
                speech.Speak(prompt);
                prompt.ClearContent();
                form.playTrack(ConstValues.AudioFile.START_OVER_EN);
            }
            else
            {
                PromptBuilder prompt = new PromptBuilder(new System.Globalization.CultureInfo("es-ES"));
                prompt.AppendAudio(ConstValues.AudioFile.BUTTON_SOUND);
                if (today)
                {
                    prompt.AppendAudio(ConstValues.AudioFile.READY_TO_PICK_UP_TODAY_SP);
                }
                else
                {
                    prompt.AppendAudio(ConstValues.AudioFile.READY_TO_PICK_UP_TOMORROW_SP);
                }

                //prompt.AppendSsmlMarkup(str);
                speech.Speak(prompt);
                prompt.ClearContent();
                form.playTrack(ConstValues.AudioFile.START_OVER_SP);
            }
            singleTrack = true;
            Form1.timerDispose();
        }

        public void goodBye()
        {
            curStatus = ConstValues.Status.END;
           
            PromptBuilder prompt = new PromptBuilder();
            SpeechSynthesizer speech = new SpeechSynthesizer();
            prompt.AppendAudio(ConstValues.AudioFile.BUTTON_SOUND);

            if (language == ConstValues.Language.ENGLISH)
            {
                prompt.AppendAudio(ConstValues.AudioFile.GOOD_BYE_EN);
            }
            else
            {
                prompt.AppendAudio(ConstValues.AudioFile.GOOD_BYE_SP);
            }

            speech.Volume = 10;
            speech.Speak(prompt);
            prompt.ClearContent();
            Form1.timerDispose();
        }
    }
}
