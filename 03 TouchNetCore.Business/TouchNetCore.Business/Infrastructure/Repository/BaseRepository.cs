using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TouchNetCore.Component.Utils.Helper;

namespace TouchNetCore.Business.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        public TouchDbContext dbcontext { get; set; }
        public int Insert(T entity)
        {
            dbcontext.Entry<T>(entity).State = EntityState.Added;
            return dbcontext.SaveChanges();
        }

        public int Insert(List<T> entitys)
        {
            foreach (var entity in entitys)
            {
                dbcontext.Entry<T>(entity).State = EntityState.Added;
            }
            return dbcontext.SaveChanges();
        }

        public int Update(T entity)
        {
            dbcontext.Set<T>().Attach(entity);
            return dbcontext.SaveChanges();
        }

        public int UpdateSelective(T entity)
        {
            dbcontext.Set<T>().Attach(entity);
            PropertyInfo[] props = entity.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetValue(entity, null) != null)
                {
                    if (prop.GetValue(entity, null).ToString() == "&nbsp;")
                        dbcontext.Entry(entity).Property(prop.Name).CurrentValue = null;
                    dbcontext.Entry(entity).Property(prop.Name).IsModified = true;
                }
            }
            return dbcontext.SaveChanges();
        }

        public int Delete(T entity)
        {
            dbcontext.Set<T>().Attach(entity);
            dbcontext.Entry<T>(entity).State = EntityState.Deleted;
            return dbcontext.SaveChanges();
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            var entitys = dbcontext.Set<T>().Where(predicate).ToList();
            entitys.ForEach(m => dbcontext.Entry<T>(m).State = EntityState.Deleted);
            return dbcontext.SaveChanges();
        }

        public T FindEntity(object keyValue)
        {
            return dbcontext.Set<T>().Find(keyValue);
        }

        public T FindEntity(Expression<Func<T, bool>> predicate)
        {
            return dbcontext.Set<T>().FirstOrDefault(predicate);
        }

        public IQueryable<T> IQueryable()
        {
            return dbcontext.Set<T>();
        }

        public IQueryable<T> IQueryable(Expression<Func<T, bool>> predicate)
        {
            return dbcontext.Set<T>().Where(predicate);
        }

        public List<T> FindList(string strSql)
        {
            return dbcontext.Set<T>().FromSql<T>(strSql).ToList<T>();
        }

        public List<T> FindList(string strSql, SqlParameter[] dbParameter)
        {
            return dbcontext.Set<T>().FromSql<T>(strSql, dbParameter).ToList<T>();
        }

        public List<T> FindList(Pagination pagination)
        {
            var tempData = dbcontext.Set<T>().AsQueryable();
            MethodCallExpression resultExp = null;
            if (!string.IsNullOrEmpty(pagination.sidx))
            {
                bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
                string[] _order = pagination.sidx.Split(',');
               

                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool sort = isAsc;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(T), "t");
                    var property = typeof(T).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
            }
            if (resultExp != null)
            {
                tempData = tempData.Provider.CreateQuery<T>(resultExp);
            }
            pagination.records = tempData.Count();
            tempData = tempData.Skip<T>(pagination.rows * (pagination.page - 1)).Take<T>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }

        public List<T> FindList(Expression<Func<T, bool>> predicate, Pagination pagination)
        {
            var tempData = dbcontext.Set<T>().Where(predicate);
            MethodCallExpression resultExp = null;
            if (!string.IsNullOrEmpty(pagination.sidx))
            {
                bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
                string[] _order = pagination.sidx.Split(',');


                foreach (string item in _order)
                {
                    string _orderPart = item;
                    _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                    string[] _orderArry = _orderPart.Split(' ');
                    string _orderField = _orderArry[0];
                    bool sort = isAsc;
                    if (_orderArry.Length == 2)
                    {
                        isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                    }
                    var parameter = Expression.Parameter(typeof(T), "t");
                    var property = typeof(T).GetProperty(_orderField);
                    var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                    var orderByExp = Expression.Lambda(propertyAccess, parameter);
                    resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(T), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
                }
            }
            if (resultExp != null)
            {
                tempData = tempData.Provider.CreateQuery<T>(resultExp);
            }
            pagination.records = tempData.Count();
            tempData = tempData.Skip<T>(pagination.rows * (pagination.page - 1)).Take<T>(pagination.rows).AsQueryable();
            return tempData.ToList();
        }
    }
}
