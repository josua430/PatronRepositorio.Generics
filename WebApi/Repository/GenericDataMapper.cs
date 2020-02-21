using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApi.Repository
{
    /// <summary>
    /// Generic data mapper
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public class GenericDataMapper<T> : IDataMapper<T> where T : class
    {
        /// <summary>
        /// Context of entities
        /// </summary>
        private Entity.TestEntities _context = null;

        /// <summary>
        /// DBSet
        /// </summary>
        private IDbSet<T> _dbSet = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Context">Context</param>
        public GenericDataMapper(Entity.TestEntities Context)
        {
            _context = Context;
            _dbSet = Context.Set<T>();
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>List of records</returns>
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// One record by Id
        /// </summary>
        /// <param name="Id">Id of record</param>
        /// <returns></returns>
        public T GetById(object Id)
        {
            return _dbSet.Find(Id);
        }

        /// <summary>
        /// Get records by predicate
        /// </summary>
        /// <param name="Predicate"></param>
        /// <returns></returns>
        public IQueryable<T> GetBy(Expression<Func<T, bool>> Predicate)
        {
            return _dbSet.Where(Predicate);
        }

        /// <summary>
        /// Get count of records by predicate
        /// </summary>
        /// <param name="predicate">predicate</param>
        /// <returns></returns>
        public long GetCountBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Count();
        }

        /// <summary>
        /// Insert new
        /// </summary>
        /// <param name="Entity">Entity</param>
        /// <returns>Id of new record. 0 if error</returns>
        public int Insert(T Entity)
        {
            try
            {
                _dbSet.Add(Entity);
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Update a record
        /// </summary>
        /// <param name="Entity">Entity</param>
        /// <returns></returns>
        public bool Update(object Id, T Entity)
        {
            if (Entity == null)
            {
                return false;
            }

            T existing = _context.Set<T>().Find(Id);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(Entity);
                _context.SaveChanges();
            }
            
            return true;
        }

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="Id">Id of record</param>
        /// <returns>true or false</returns>
        public bool Delete(object Id)
        {
            if (Id == null)
            {
                return false;
            }

            T existing = _context.Set<T>().Find(Id);
            if (existing != null)
            {
                _context.Set<T>().Remove(existing);
                _context.SaveChanges();
            }

            return true;
        }
    }

}