using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATM
{
    class Action
    {
        String m_Content;
        DateTime m_Time;
        BehaviorType m_BehaviorType;


        public Action(String content, BehaviorType behaviorType)
        {
            this.m_Content = content;
            this.m_Time = DateTime.Now;
            this.m_BehaviorType = behaviorType;
        }

        public String content
        {
            get { return this.m_Content; }
            set { this.m_Content = value; }
        }

        public DateTime time
        {
            get { return this.m_Time; }
            set { this.m_Time = value; }
        }

        public BehaviorType behaviorType
        {
            get { return this.m_BehaviorType; }
            set { this.m_BehaviorType = value; }
        }

        public String ToString()
        {
            return m_Time.ToLongDateString() + " " + m_Time.ToLongTimeString() + "\t" + m_BehaviorType.ToString() + "\t" + m_Content;
        }

        public String ToString(BehaviorType behaviorType)
        {
            if (m_BehaviorType == behaviorType)
            {
                return ToString();
            }
            else
            {
                return "";
            }
        }

        public enum BehaviorType
        {
            normal, error
        };
    }
}
