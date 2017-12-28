using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATM
{
    class Log
    {
        LinkedList<Action> m_ActionList;
        User m_User;

        public Log(User user)
        {
            this.m_User = user;
            this.m_ActionList = new LinkedList<Action>();
        }

        public User user
        {
            get { return this.m_User; }
            set { this.m_User = value; }
        }

        public LinkedList<Action> actionList
        {
            get { return this.m_ActionList; }
        }

        public void clearActionList()
        {
            this.m_ActionList.Clear();
        }

        public void addAction(String content, Action.BehaviorType behaviorType)
        {
            Action action = new Action(content, behaviorType);
            this.m_ActionList.AddLast(action);
        }

        public String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("User Name: " + m_User.name + "\tID: " + m_User.ID);
            sb.Append("\n");
            foreach (Action action in m_ActionList)
            {
                sb.Append(action.ToString());
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public String ToString(Action.BehaviorType behaviorType)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("User Name: " + m_User.name + "\tID: " + m_User.ID);
            sb.Append("\n");
            String content;
            foreach (Action action in m_ActionList)
            {
                content = action.ToString(behaviorType);
                if (content != "")
                {
                    sb.Append(content);
                    sb.Append("\n");
                }
            }
            return sb.ToString();
        }
    }
}
