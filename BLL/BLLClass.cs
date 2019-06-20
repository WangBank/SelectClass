using Model;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLClass:BLLBase<Class>
    {
        DALClass dALClass = new DALClass();
        public int GetClassFormByName(string className)
        {
            return dALClass.GetClassFormByName(className);
        }
    }
}
