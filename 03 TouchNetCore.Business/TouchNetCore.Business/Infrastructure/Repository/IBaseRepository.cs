using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TouchNetCore.Component.Utils.Helper;

namespace TouchNetCore.Business.Infrastructure.Repository
{
    /// <summary>
    /// 基础仓储接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : class, new()
    {
        int Insert(T entity);
        int Insert(List<T> entitys);
        int Update(T entity);
        int UpdateSelective(T entity);
        int Delete(T entity);
        int Delete(Expression<Func<T, bool>> predicate);
        T FindEntity(object keyValue);
        T FindEntity(Expression<Func<T, bool>> predicate);
        IQueryable<T> IQueryable();
        IQueryable<T> IQueryable(Expression<Func<T, bool>> predicate);
        List<T> FindList(string strSql);
        List<T> FindList(string strSql, SqlParameter[] dbParameter);
        List<T> FindList(Pagination pagination);
        List<T> FindList(Expression<Func<T, bool>> predicate, Pagination pagination);
    }
}
