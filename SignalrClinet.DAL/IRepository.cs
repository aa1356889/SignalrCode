using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SignalrClinet.DAL
{
   public interface IRepository<T>where T:class
    {
        #region 数据对象操作
        /// <summary>
        /// 数据模型操作
        /// </summary>
        DbSet<T> DbSet { get; }
        /// <summary>
        /// EF事物
        /// </summary>
        DbContextTransaction Transaction { get; set; }

        /// <summary>
        /// 事物提交结果
        /// </summary>
        bool Committted { get; set; }

        /// <summary>
        /// 提交事物
        /// </summary>
        void Commit();


        /// <summary>
        /// 回滚事物
        /// </summary>
        void RollBack(); 
        #endregion

        #region 单模型crud操作
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="isCommIT">是否提交(默认为提交)</param>
        /// <returns></returns>
        bool Save(T entity, bool isCommit = true);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="isCommit">是否提交(默认为提交)</param>
        /// <returns></returns>
        bool Update(T entity, bool isCommit = true);


        /// <summary>
        /// 更新或新增一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="isSave">是否新增</param>
        /// <param name="isCommit">是否提交(默认为提交)</param>
        /// <returns></returns>
        bool SaveOrUpdate(T entity, bool isSave, bool isCommit = true);


        /// <summary>
        /// 根据Lambda表达式获取实体
        /// </summary>
        /// <param name="predicate">Lamda表达式(c=>c.Id=Id)</param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 是否删除数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="isCommit">是否提交(默认提交)</param>
        /// <returns></returns>
        bool Delete(T entity, bool isCommit = true);
        #endregion

        #region 多模型操作

        /// <summary>
        /// 增加多条记录,同一类型模型
        /// </summary>
        /// <param name="entitiys">实体模型集合</param>
        /// <param name="IsCommit">是否提交</param>
        /// <returns></returns>
        bool SaveList(IEnumerable<T> entitiys, bool isCommit = true);


        /// <summary>
        /// 增加多条记录,同一类型模型（异步方式）
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="IsCommit"></param>
        /// <returns></returns>
        Task<bool> SaveListAsnc(IEnumerable<T> entitys, bool isCommit = true);



        /// <summary>
        /// 更新多条记录,同一类型模型
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="isCommit"></param>
        /// <returns></returns>
        bool UpdateList(IEnumerable<T> entitys, bool isCommit = true);


        /// 更新多条记录，同一模型（异步方式）
        /// </summary>
        /// <param name="T1">实体模型集合</param>
        /// <param name="IsCommit">是否提交（默认提交）</param>
        /// <returns></returns>
        Task<bool> UpdateListAsync(IEnumerable<T> entitys, bool isCommit = true);


        /// <summary>
        /// 删除多条记录,同一模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys">实体模型集合</param>
        /// <param name="isCommit">是否提交(默认提交)</param>
        /// <returns></returns>
        bool DeleteList(IEnumerable<T> entitys, bool isCommit = true);


        /// <summary>
        /// 删除多条记录，同一模型(异步方式)
        /// </summary>
        /// <param name="entitys">实体模型集合</param>
        /// <param name="IsCommit">是否提交(默认提交)</param>
        /// <returns></returns>
        Task<bool> DeleteListAsync(IEnumerable<T> entitys, bool isCommit = true);


        /// <summary>
        /// 通过lamda表达示删除数据
        /// </summary>
        /// <param name="Predicate">lamda表示</param>
        /// <param name="isCommit">是否提交(默认提交)</param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> predicate, bool isCommit = true);

        /// <summary>
        /// 通过lamda表达示删除数据(异步方式)
        /// </summary>
        /// <param name="predicate">lamda表达式</param>
        /// <param name="isCommit"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, bool isCommit = true);
        #endregion


        #region 获取多条数据操作
        /// <summary>
        /// 返回IQueryable集合，延时加载数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> LoadAll(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 返回IQueryable集合，延时加载数据（异步方式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IQueryable<T>> LoadAllAsync(Expression<Func<T, bool>> predicate);

        // <summary>
        /// 返回List<T>集合,不采用延时加载
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<T> LoadListAll(Expression<Func<T, bool>> predicate);
        // <summary>
        /// 返回List<T>集合,不采用延时加载（异步方式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<T>> LoadListAllAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// T-Sql方式：返回IQueryable<T>集合
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">Parameters参数</param>
        /// <returns></returns>
        IEnumerable<T> LoadAllBySql(string sql, params DbParameter[] para);
        /// <summary>
        /// T-Sql方式：返回IQueryable<T>集合（异步方式）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">Parameters参数</param>
        /// <returns></returns>
        Task<IEnumerable<T>> LoadAllBySqlAsync(string sql, params DbParameter[] para);

        /// <summary>
        /// T-Sql方式：返回List<T>集合
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">Parameters参数</param>
        /// <returns></returns>
        List<T> LoadListAllBySql(string sql, params DbParameter[] para);
        /// <summary>
        /// T-Sql方式：返回List<T>集合（异步方式）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="para">Parameters参数</param>
        /// <returns></returns>
        Task<List<T>> LoadListAllBySqlAsync(string sql, params DbParameter[] para);

        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回实体对象集合
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <typeparam name="TResult">数据结果，与TEntity一致</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>实体集合</returns>
        List<TResult> QueryEntity<TEntity, TOrderBy, TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Expression<Func<TEntity, TResult>> selector, bool IsAsc)
            where TEntity : class
            where TResult : class;
        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回实体对象集合（异步方式）
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <typeparam name="TResult">数据结果，与TEntity一致</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>实体集合</returns>
        Task<List<TResult>> QueryEntityAsync<TEntity, TOrderBy, TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Expression<Func<TEntity, TResult>> selector, bool IsAsc)
            where TEntity : class
            where TResult : class;

        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回Object对象集合
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>自定义实体集合</returns>
        List<object> QueryObject<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector, bool IsAsc)
            where TEntity : class;
        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回Object对象集合（异步方式）
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>自定义实体集合</returns>
        Task<List<object>> QueryObjectAsync<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector, bool IsAsc)
            where TEntity : class;

        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回动态类对象集合
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>动态类</returns>
        dynamic QueryDynamic<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector, bool IsAsc)
            where TEntity : class;
        /// <summary>
        /// 可指定返回结果、排序、查询条件的通用查询方法，返回动态类对象集合（异步方式）
        /// </summary>
        /// <typeparam name="TEntity">实体对象</typeparam>
        /// <typeparam name="TOrderBy">排序字段类型</typeparam>
        /// <param name="where">过滤条件，需要用到类型转换的需要提前处理与数据表一致的</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="selector">返回结果（必须是模型中存在的字段）</param>
        /// <param name="IsAsc">排序方向，true为正序false为倒序</param>
        /// <returns>动态类</returns>
        Task<dynamic> QueryDynamicAsync<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector, bool IsAsc)
            where TEntity : class;

        #endregion

        #region 验证是否存在

        /// <summary>
        /// 验证当前条件是否存在相同项
        /// </summary>
        bool IsExist(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 验证当前条件是否存在相同项（异步方式）
        /// </summary>
        Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 根据SQL验证实体对象是否存在
        /// </summary>
        bool IsExist(string sql, params DbParameter[] para);
        /// <summary>
        /// 根据SQL验证实体对象是否存在（异步方式）
        /// </summary>
        Task<bool> IsExistAsync(string sql, params DbParameter[] para);

        #endregion
    }
}
