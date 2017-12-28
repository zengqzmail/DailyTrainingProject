using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ATM
{
    class IO
    {
        public static List<User> loadUserlist(String filepath)
        {
            List<User> userlist = new List<User>();
            Assembly _assembly = Assembly.GetExecutingAssembly();    
            StreamReader sr = new StreamReader(_assembly.GetManifestResourceStream("ATM." + filepath));

            String line = sr.ReadLine();
            String[] items;
            userlist.Add(User.getInstance());
            while (line != null)
            {
                if (line.StartsWith("%") || line == "")
                {
                    line = sr.ReadLine();
                    continue;
                }
                items = line.Split('\t');
                Checking checking = new Checking(items[3], Double.Parse(items[4]));
                Saving saving = new Saving(items[5], Double.Parse(items[6]));
                User user = new User(items[0], items[1], items[2], checking, saving, items[7]);
                userlist.Add(user);
                line = sr.ReadLine();
            }
            sr.Close();
            return userlist;
        }

        public static void storeUserlist(List<User> userlist, String filepath)
        {
            StreamWriter sw = new StreamWriter(new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write));
            sw.WriteLine("%ID\tPIN\tname\tchecking.accountID\tchecking.amount\tsaving.accountID\tsaving.amount\tlanguage");
            foreach (User user in userlist)
            {
                sw.WriteLine(user.ID + "\t" + user.PIN + "\t" + user.name + "\t" + user.checking.accountID
                    + "\t" + user.checking.amount.ToString() + "\t" + user.saving.accountID
                    + "\t" + user.saving.amount.ToString() + "\t" + user.language);
            }
            sw.Close();
        }

        public static void storeContent(String content, String filepath, bool append)
        {
            StreamWriter sw;
            if (new FileInfo(filepath).Exists)
            {
                sw = new StreamWriter(new FileStream(filepath, FileMode.Append, FileAccess.Write));
            }
            else
            {
                sw = new StreamWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write));
            }
            sw.WriteLine(content);
            sw.Close();
        }
    }
}
