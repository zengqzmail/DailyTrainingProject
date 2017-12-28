using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLib;
using System.Speech;
using System.Speech.Synthesis;
using System.Threading;

namespace PrescriptionRefill
{
    public partial class FormTelephone : Form
    {
        Telephone telephone;
        int exitKeysCount = 0;

        public FormTelephone()
        {
            telephone = new Telephone(null, this);
            InitializeComponent();
        }

        private void FormTelephone_Load(object sender, EventArgs e)
        {
            playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
            telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;            
            telephone.timerStart = DateTime.Now;
            //this.KeyDown += new KeyEventHandler(FormTelephone_KeyDown);
        }

        void FormTelephone_KeyDown(object sender, KeyEventArgs e)
        {
            if (exitKeysCount == 0 && e.KeyCode == Keys.ControlKey)
            {
                exitKeysCount = 1;
            }
            else if (exitKeysCount == 1 && e.KeyCode == Keys.ShiftKey)
            {
                exitKeysCount = 2;
            }
            else if (exitKeysCount == 2 && e.KeyCode == Keys.X)
            {
               
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "EscapeEntered");
                dictionary1.Add("eventData", "");
                dictionary1.Add("eventSummary", "");
                //taskDataList.Add(dictionary1);
                this.Close();
                Application.Exit();
            }
            else
            {
                exitKeysCount = 0;
            }
        }

        public void playTrack(string trackName)
        {
            if (telephone.singleTrack)
            {
                player.currentPlaylist.clear();
            }                
            IWMPMedia audio = player.newMedia(trackName);
            player.currentPlaylist.appendItem(audio);
            player.Ctlcontrols.playItem(player.currentPlaylist.Item[0]);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {                   

        }

        private void button1_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.SPANISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.enterRxNumber();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 1;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.today = true;
                    telephone.enterPickUpTime();
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 1;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime(); 
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.convertTo24Hour(false);
                    telephone.checkTimeRange();
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.refill();
                    break;
                default:
                    break;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.REFILL;
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 2;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.today = false;
                    telephone.enterPickUpTime();
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 2;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime(); 
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.convertTo24Hour(true);
                    telephone.checkTimeRange();
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 3;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:                    
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 3;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime(); 
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 4;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 4;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime();
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 5;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 5;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime();
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 6;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 6;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime();
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 7;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 7;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime();
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 8;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 8;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime();
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 9;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 9;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime();
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void buttonStar_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.errorRxNumber();
                    player.Ctlcontrols.stop();
                    telephone.enterRxNumber();
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.enterPickUpTime();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        telephone.validatePickUpTime();
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
        private void buttonPound_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    label1.Text = "" + telephone.rxNumber;
                    if (telephone.isCorrectRxNumber())
                    {
                        telephone.eligibleRxNumber();
                    }
                    else
                    {
                        telephone.errorRxNumber();
                        telephone.singleTrack = false;
                        telephone.enterRxNumber();
                        telephone.singleTrack = true;
                    }
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.enterPickUpTime();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.AM_OR_PM:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.checkTimeRange();
                    telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
        }
       
        private void buttonRestart_Click(object sender, EventArgs e)
        {
            telephone.rxNumber = 0;
            telephone.pickUpTime = 0;
            playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
            telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;

            telephone.timerStart = DateTime.Now;
        }

        private void buttonHangUp_Click(object sender, EventArgs e)
        {
            telephone.goodBye();
        }

        private void player_StatusChange(object sender, EventArgs e)
        {

        }
            
    }
}
