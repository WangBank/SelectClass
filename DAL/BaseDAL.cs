using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL
{
    public class BaseDAL<T> where T : class, new()
    {
        public SelectClassEntities selectClassContext = new SelectClassEntities();
        private T baseT;
        public bool Add(T t)
        {
            try
            {
                selectClassContext.Set<T>().Add(t);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public IQueryable<T> Details(Expression<Func<T, bool>> whereLambda)
        {
            return selectClassContext.Set<T>().Where(whereLambda).AsQueryable();
        }

        public bool Edit(T t)
        {
            try
            {
                selectClassContext.Entry(t).State = System.Data.EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                baseT = selectClassContext.Set<T>().Find(id);
                selectClassContext.Set<T>().Remove(baseT);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<T> GetAll()
        {
            return selectClassContext.Set<T>().ToList();
        }

        public bool SaveChange() {
           
                 return selectClassContext.SaveChanges() > 0;
        }
    }
}
