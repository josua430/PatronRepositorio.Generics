using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Repository
{
    /// <summary>
    /// Interface Data Mapper
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IDataMapper<T> where T : class
    {
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="Entity">Entity to insert</param>
        /// <returns></returns>
        int Insert(T Entity);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="Id">Id of entity</param>
        /// <param name="Entity">Entity to update</param>
        /// <returns></returns>
        bool Update(object Id, T Entity);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="Id">Id of entity</param>
        /// <returns></returns>
        bool Delete(object Id);

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>List of records</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get one record
        /// </summary>
        /// <param name="Id">Id of record</param>
        /// <returns>Entity</returns>
        T GetById(object Id);

        /// <summary>
        /// Get by where
        /// </summary>
        /// <param name="Predicate">Where clausule</param>
        /// <returns></returns>
        long GetCountBy(Expression<Func<T, bool>> Predicate);
    }
}
