using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLBase<T> where T :class ,new()
    {
        BaseDAL<T> baseDAL = new BaseDAL<T>();
        public bool Add(T t)
        {
            return baseDAL.Add(t);

        }
        public IQueryable<T> Details(Expression<Func<T, bool>> whereLambda)
        {
            return baseDAL.Details(whereLambda);
        }

        public bool Edit(T t)
        {
            return baseDAL.Edit(t);
        }

        public bool Delete(int id)
        {
            return baseDAL.Delete(id);
        }

        public List<T> GetAll()
        {
            return baseDAL.GetAll();
        }
        public bool SaveChange()
        {

                return baseDAL.SaveChange();
        }
    }
}
