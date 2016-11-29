using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using SignalrClient.Model;

namespace SignalrClinet.DAL
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        #region 数据上下文
        private DbSet<T> _dbSet;
        public RepositoryBase()
        {



            _dbSet = Context.Set<T>();
        }

        public bool Committted
        {
            get; set;
        }

        public SignalrClientDbContext Context
        {
            get
            {
                #region 避免额外创建连接的开销 同一线程内共享数据上下文
                var context = CallContext.GetData("dbContext") as SignalrClientDbContext;
                if (context == null)
                {
                    context = new SignalrClientDbContext();
                    //关闭实体跟踪
                    context.Configuration.ValidateOnSaveEnabled = false;
                    CallContext.SetData("dbContext", context);
                }
                #endregion

                return context;
            }
        }

        public DbSet<T> DbSet
        {
            get
            {
                return _dbSet;
            }
        }
        /// <summary>
        /// 事务
        /// </summary>
        private DbContextTransaction _transaction = null;
        public DbContextTransaction Transaction
        {
            get
            {
                if (this._transaction == null)
                {
                    this._transaction = this.Context.Database.BeginTransaction();
                }
                return this._transaction;
            }

            set { this._transaction = value; }
        }
        /// <summary>
        /// 异步锁定
        /// </summary>
        private readonly object sync = new object();
        public void Commit()
        {
            if (!Committted)
            {
                lock (sync)
                {
                    if (this._transaction != null)
                        _transaction.Commit();
                }
                Committted = true;
            }
        }
        public void RollBack()
        {
            Committted = false;
            if (this._transaction != null)
                this._transaction.Rollback();
        }
        public bool Delete(Expression<Func<T, bool>> predicate, bool isCommit = true)
        {
            if (predicate == null) return false;
            IQueryable<T> entry = _dbSet.Where(predicate);
            var list = entry.ToList();
            list.ForEach(c =>
            {
                _dbSet.Remove(c);
            });
            if (isCommit)
                return Context.SaveChanges() > 0;
            else
                return false;
        }
        #endregion
        #region 单模型crud

        /// 是否删除数据
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="isCommit">是否提交(默认提交)</param>
        /// <returns></returns>
        public virtual bool Delete(T entity, bool isCommit = true)
        {
            var t = _dbSet.Attach(entity);
            var entry = Context.Entry(t);
            entry.State = EntityState.Modified;
            if (isCommit)
                return Context.SaveChanges() > 0;
            else
                return false;

        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> predicate, bool isCommit = true)
        {
            return Delete(predicate, isCommit);
        }

        public bool DeleteList(IEnumerable<T> entitys, bool isCommit = true)
        {
            if (entitys == null)
                return false;
            foreach (var item in entitys)
            {
                _dbSet.Remove(item);
            }
            if (isCommit)
                return Context.SaveChanges() > 0;
            else
                return false;
        }

        public async Task<bool> DeleteListAsync(IEnumerable<T> entitys, bool isCommit = true)
        {
            return DeleteList(entitys, isCommit);
        }

        /// <summary>
        /// 根据Lambda表达式获取实体
        /// </summary>
        /// <param name="predicate">Lamda表达式(c=>c.Id=Id)</param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).FirstOrDefault();
        }

        public bool IsExist(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);

        }

        public bool IsExist(string sql, params DbParameter[] para)
        {
            return Context.Database.ExecuteSqlCommand(sql, para) > 0;
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate)
        {
            return IsExist(predicate);
        }

        public async Task<bool> IsExistAsync(string sql, params DbParameter[] para)
        {
            return IsExist(sql, para);
        }

        public IQueryable<T> LoadAll(Expression<Func<T, bool>> predicate)
        {
            return predicate == null ? _dbSet.AsNoTracking() : _dbSet.Where(predicate);
        }

        public async Task<IQueryable<T>> LoadAllAsync(Expression<Func<T, bool>> predicate)
        {
            return LoadAll(predicate);
        }

        public IEnumerable<T> LoadAllBySql(string sql, params DbParameter[] para)
        {
            return _dbSet.SqlQuery(sql, para);
        }

        public async Task<IEnumerable<T>> LoadAllBySqlAsync(string sql, params DbParameter[] para)
        {
            return LoadAllBySql(sql, para);
        }

        public List<T> LoadListAll(Expression<Func<T, bool>> predicate)
        {
            return LoadListAll(predicate).ToList();
        }

        public async Task<List<T>> LoadListAllAsync(Expression<Func<T, bool>> predicate)
        {
            return LoadListAll(predicate).ToList();
        }

        public List<T> LoadListAllBySql(string sql, params DbParameter[] para)
        {
            return LoadAllBySql(sql, para).ToList();
        }

        public async Task<List<T>> LoadListAllBySqlAsync(string sql, params DbParameter[] para)
        {
            return LoadAllBySql(sql, para).ToList();
        }

        public dynamic QueryDynamic<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector, bool IsAsc) where TEntity : class
        {
            List<object> list = QueryObject<TEntity, TOrderBy>
  (where, orderby, selector, IsAsc);
            return "";
        }

        public async Task<dynamic> QueryDynamicAsync<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector, bool IsAsc) where TEntity : class
        {
            return QueryDynamic(where, orderby, selector, IsAsc);
        }

        public List<TResult> QueryEntity<TEntity, TOrderBy, TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Expression<Func<TEntity, TResult>> selector, bool IsAsc)
            where TEntity : class
            where TResult : class
        {
            IQueryable<TEntity> iqueryable = Context.Set<TEntity>();
            if (where != null)
            {
                iqueryable = iqueryable.Where(where);
            }
            if (orderby != null)
            {
                iqueryable = IsAsc ? iqueryable.OrderBy(orderby) : iqueryable.OrderByDescending(orderby);
            }
            if (selector != null)
                return iqueryable.Select(selector).ToList();
            else

                return iqueryable.Cast<TResult>().ToList();

        }

        public async Task<List<TResult>> QueryEntityAsync<TEntity, TOrderBy, TResult>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Expression<Func<TEntity, TResult>> selector, bool IsAsc)
            where TEntity : class
            where TResult : class
        {
            return QueryEntity(where, orderby, selector, IsAsc);
        }

        public List<object> QueryObject<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector, bool IsAsc) where TEntity : class
        {
            IQueryable<TEntity> iqueryable = Context.Set<TEntity>();
            if (where != null)
            {
                iqueryable = iqueryable.Where(where);
            }
            if (orderby != null)
            {
                iqueryable = IsAsc ? iqueryable.OrderBy(orderby) : iqueryable.OrderByDescending(orderby);
            }
            if (selector != null)
                return iqueryable.ToList<object>();
            else

                return selector(iqueryable);
        }

        public async Task<List<object>> QueryObjectAsync<TEntity, TOrderBy>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TOrderBy>> orderby, Func<IQueryable<TEntity>, List<object>> selector, bool IsAsc) where TEntity : class
        {
            return QueryObject(where, orderby, selector, IsAsc);
        }



        public virtual bool Save(T entity, bool isCommit = true)
        {
            _dbSet.Add(entity);
            if (isCommit)
            {
                return Context.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }

        public bool SaveList(IEnumerable<T> entitiys, bool isCommit = true)
        {
            foreach (var item in entitiys)
            {
                _dbSet.Add(item);
            }
            if (isCommit)
            {
                return Context.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SaveListAsnc(IEnumerable<T> entitys, bool isCommit = true)
        {
            return SaveList(entitys, isCommit);
        }

        /// <summary>
        /// 更新或新增一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="isSave">是否新增</param>
        /// <param name="isCommit">是否提交(默认为提交)</param>
        /// <returns></returns>
        public virtual bool SaveOrUpdate(T entity, bool isSave, bool isCommit = true)
        {
            if (isSave)
            {
                _dbSet.Add(entity);
            }
            else
            {
                var t = _dbSet.Attach(entity);
                var entry = Context.Entry(t);
                entry.State = EntityState.Modified;
            }
            return isCommit ? Context.SaveChanges() > 0 : false;
        }
        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="entity">实体模型</param>
        /// <param name="isCommit">是否提交(默认为提交)</param>
        /// <returns></returns>
        public virtual bool Update(T entity, bool isCommit = true)
        {
            var t = _dbSet.Attach(entity);
            var entry = Context.Entry(t);
            entry.State = EntityState.Modified;
            return isCommit ? Context.SaveChanges() > 0 : false;
        }

        public bool UpdateList(IEnumerable<T> entitys, bool isCommit = true)
        {
            foreach (var item in entitys)
            {
                var t = _dbSet.Attach(item);
                var entry = Context.Entry(t);
                entry.State = EntityState.Modified;
            }
            return isCommit ? Context.SaveChanges() > 0 : false;
        }

        public async Task<bool> UpdateListAsync(IEnumerable<T> entitys, bool isCommit = true)
        {
            return UpdateList(entitys, isCommit);
        }


        #endregion
    }
}
