using Ams;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Model
{
    public class DbHelper<T> where T : class
    {
        public static readonly DbHelper<T> Instance = new DbHelper<T>();

        private DbHelper() { }

        #region Ctx
        //private static DbContext ctx;
        private DbContext ctx;

        public DbContext GetContext()
        {
            //if (ctx == null)
            //{
            ctx = new Model.AMSEntities();
            //}
            return ctx;
        }

        public DbSet<T> DbSet
        {
            get { return GetContext().Set<T>(); }
        }


        #endregion

        #region Query
        public T GetById(string id)
        {
            using (Model.AMSEntities entity = new AMSEntities())
            {
                return entity.Set<T>().Find(id);
            }
            //  return DbSet.Find(id);
        }

        public T GetById(int id)
        {
            using (Model.AMSEntities entity = new AMSEntities())
            {
                return entity.Set<T>().Find(id);
            }
            //  return DbSet.Find(id);
        }

        public IQueryable<T> GetAll() { return DbSet.AsQueryable(); }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }


        public PagerInfo<T> GetByPage(Expression<Func<T, bool>> predicate, PagerInfo<T> pagerInfo)
        {
            var result = predicate == null ?
              DbSet.AsQueryable() : DbSet.Where(predicate);
            return this.GetByPage(result, pagerInfo);
        }


        /// <summary>
        /// 多表查询结果分页
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pagerInfo"></param>
        /// <returns></returns>
        public PagerInfo<T> GetByPage(IQueryable<T> query, PagerInfo<T> pagerInfo)
        {
            if (pagerInfo.PageSize == 0) pagerInfo.PageSize = Global.Instance.PageSize;//默认每页显示条数

            int skipCount = (pagerInfo.PageIndex - 1) * pagerInfo.PageSize;

            if (string.IsNullOrEmpty(pagerInfo.OrderBy))
            {
                pagerInfo.Data = (skipCount == 0 ?
                    query.Take(pagerInfo.PageSize) : query.Skip(skipCount).Take(pagerInfo.PageSize)).ToList();
                pagerInfo.TotalCount = query.Count();
                pagerInfo.TotalPages = pagerInfo.TotalCount / pagerInfo.PageSize + (pagerInfo.TotalCount % pagerInfo.PageSize > 0 ? 1 : 0);
                return pagerInfo;
            }
            else
            {
                pagerInfo.Data = (skipCount == 0 ?
                  query.Sort(pagerInfo.OrderBy, pagerInfo.IsAsc).Take(pagerInfo.PageSize) :
                  query.Sort(pagerInfo.OrderBy, pagerInfo.IsAsc).Skip(skipCount).Take(pagerInfo.PageSize)).ToList();
                pagerInfo.TotalCount = query.Count();
                pagerInfo.TotalPages = pagerInfo.TotalCount / pagerInfo.PageSize + (pagerInfo.TotalCount % pagerInfo.PageSize > 0 ? 1 : 0);
                return pagerInfo;
            }
        }
        #endregion
    }

    public static class Extensions
    {
        /// <summary>
        /// 自定义排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public static IQueryable<TSource> Sort<TSource>(this IQueryable<TSource> source, string propertyName, bool isAsc)
        {
            if (string.IsNullOrEmpty(propertyName)) return source;

            var sourceType = typeof(TSource);
            var propertyInfo = sourceType.GetProperty(propertyName);

            var parameterExpression = Expression.Parameter(sourceType);
            var bodyExpression = Expression.Property(parameterExpression, propertyInfo);
            var selecterExpression = Expression.Lambda(bodyExpression, parameterExpression);

            var methodName = isAsc ? "OrderBy" : "OrderByDescending";
            //var methodExpression = Expression.Call(typeof(IQueryable), methodName, new Type[] { sourceType, propertyInfo.PropertyType }, Expression.Constant(source), selecterExpression);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { sourceType, propertyInfo.PropertyType }, source.Expression, Expression.Quote(selecterExpression));
            return source.Provider.CreateQuery<TSource>(resultExp);
            //return (IOrderedQueryable<TSource>)methodExpression.Method.Invoke(null, new object[] { source, selecterExpression.Compile() });
        }
        // http://www.cnblogs.com/xxfss2/archive/2010/12/13/1905023.html
        //public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string propertyStr) where TEntity : class
        //{
        //    ParameterExpression param = Expression.Parameter(typeof(TEntity), "c");
        //    PropertyInfo property = typeof(TEntity).GetProperty(propertyStr);
        //    Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
        //    LambdaExpression le = Expression.Lambda(propertyAccessExpression, param);
        //    Type type = typeof(TEntity);
        //    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(le));
        //    return source.Provider.CreateQuery<TEntity>(resultExp);
        //}

        public static PagerInfo<T> GetByPage<T>(this DbContext ctx, IQueryable<T> query, PagerInfo<T> pagerInfo) where T : class
        {
            if (pagerInfo.PageSize == 0) pagerInfo.PageSize = Global.Instance.PageSize;//默认每页显示条数

            int skipCount = (pagerInfo.PageIndex - 1) * pagerInfo.PageSize;

            if (string.IsNullOrEmpty(pagerInfo.OrderBy))
            {
                pagerInfo.Data = (skipCount == 0 ?
                    query.Take(pagerInfo.PageSize) : query.Skip(skipCount).Take(pagerInfo.PageSize)).ToList();
                pagerInfo.TotalCount = query.Count();
                pagerInfo.TotalPages = pagerInfo.TotalCount / pagerInfo.PageSize + (pagerInfo.TotalCount % pagerInfo.PageSize > 0 ? 1 : 0);
                return pagerInfo;
            }
            else
            {
                pagerInfo.Data = (skipCount == 0 ?
                  query.Sort(pagerInfo.OrderBy, pagerInfo.IsAsc).Take(pagerInfo.PageSize) :
                  query.Sort(pagerInfo.OrderBy, pagerInfo.IsAsc).Skip(skipCount).Take(pagerInfo.PageSize)).ToList();
                pagerInfo.TotalCount = query.Count();
                pagerInfo.TotalPages = pagerInfo.TotalCount / pagerInfo.PageSize + (pagerInfo.TotalCount % pagerInfo.PageSize > 0 ? 1 : 0);
                return pagerInfo;
            }
        }

    }
}
