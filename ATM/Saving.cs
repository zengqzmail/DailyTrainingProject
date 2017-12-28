using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATM
{
    class Saving
    {
        private String m_accountID;
        private double m_amount;

        public Saving(String accountID, double amount)
        {
            this.m_accountID = accountID;
            this.m_amount = amount;
        }

        public String accountID
        {
            get { return this.m_accountID; }
            set { this.m_accountID = value; }
        }

        public double amount
        {
            get { return this.m_amount; }
            set { this.m_amount = value; }
        }
    }
}
