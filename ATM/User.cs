using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//This is a new comment on jan 3 2012
namespace ATM
{
    class User
    {
        private String m_ID;
        private String m_PIN;
        private String m_name;
        private Checking m_checking;
        private Saving m_saving;
        private String m_language;
        private int m_pinTime;

        public User(String ID, String PIN, String name, Checking checking, Saving saving, String language)
        {
            this.m_ID = ID;
            this.m_PIN = PIN;
            this.m_name = name;
            this.m_checking = checking;
            this.m_saving = saving;
            this.m_language = language;
            this.m_pinTime = 0;
        }

        public static User getInstance()
        {
            return new User("00030", "1234", "John", new Checking("00031", 105.92), new Saving("00032", 224.86), "English"); 
        }

        public Checking checking
        {
            get { return this.m_checking; }
            set { m_checking = value; }
        }

        public String ID
        {
            get { return this.m_ID; }
            set { this.m_ID = value; }
        }

        public Saving saving
        {
            get { return this.m_saving; }
            set { m_saving = value; }
        }

        public String name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }

        public String PIN
        {
            get { return this.m_PIN; }
            set { this.m_PIN = value; }
        }

        public String language
        {
            get { return this.m_language; }
            set { this.m_language = value; }
        }

        public int pinTime
        {
            get { return this.m_pinTime; }
            set { this.m_pinTime = value; }
        }
    }
}
