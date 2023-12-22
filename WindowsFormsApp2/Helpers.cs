using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class Helpers
    {
        public static int ParseProjectId(string projectName)
        {
            try
            {
                string projectid = projectName.Substring(projectName.LastIndexOf('(') + 1);
                projectid = projectid.Remove(projectid.LastIndexOf(')'), 1);
                return Convert.ToInt32(projectid);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
