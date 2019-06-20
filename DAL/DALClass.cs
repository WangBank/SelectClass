using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALClass:BaseDAL<Class>
    {
        public int GetClassFormByName(string className)
        {
            int classForm = int.Parse(className);
            return selectClassContext.Class.Where(c => c.ClassForm == classForm).FirstOrDefault().ClassForm;
        }
    }
}
