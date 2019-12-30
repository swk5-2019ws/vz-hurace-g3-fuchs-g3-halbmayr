using Hurace.Core.Db.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hurace.Core.DAL
{
    public interface IDataAccessObject<T>
    {
        /// <summary>
        /// Loads a specific <see cref="T"/> from the db, identified by the passed id
        /// </summary>
        /// <param name="id">the identifying id</param>
        /// <returns>a instance of <see cref="T"/>, if a row exists on the db that
        /// has this id</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Loads all instances of <see cref="T"/> that fulfill the optional passed <see cref="IQueryCondition"/>.
        /// If no <see cref="IQueryCondition"/> is provided, all rows from a table are loaded
        /// </summary>
        /// <param name="condition">the optional result-restricting condition</param>
        /// <returns>all instances of <see cref="T"/>, or all instances of <see cref="T"/>
        /// that fulfill the passed <see cref="IQueryCondition"/></returns>
        Task<IEnumerable<T>> GetAllConditionalAsync(IQueryCondition condition = null);

        /// <summary>
        /// Inserts a passed instance of <see cref="T"/>
        /// </summary>
        /// <param name="newInstance">the instance of <see cref="T"/> to insert</param>
        /// <returns>either the id of the newly generated instance, or -1 if the
        /// insert failed</returns>
        Task<int> CreateAsync(T newInstance);

        /// <summary>
        /// Updates a passed instance of <see cref="T"/>. The id of the instance
        /// is not allowed to change, since its the mechanism that links the updated
        /// entity and existing row in the db together.
        /// </summary>
        /// <param name="updatedInstance">the udpated instance of <see cref="T"/></param>
        /// <returns>a boolean that indicates, if the update was successfull or not</returns>
        Task<bool> UpdateAsync(T updatedInstance);

        /// <summary>
        /// Updates all values passed with anonymous-<see cref="object"/> of all rows,
        /// that fulfill a passed <see cref="IQueryCondition"/>.
        /// </summary>
        /// <param name="updatedValues">contains columns with values to update</param>
        /// <param name="condition">describes which rows should be updated</param>
        /// <returns>the number of affected rows</returns>
        Task<int> UpdateAsync(object updatedValues, IQueryCondition condition);

        /// <summary>
        /// Deletes a single row of <see cref="T"/> in the db identified by a passed id.
        /// </summary>
        /// <param name="id">identifies a single row in the db.</param>
        /// <returns>a boolean that indicates, if the delete for a single row was successfull or not</returns>
        Task<bool> DeleteByIdAsync(int id);

        /// <summary>
        /// Deletes all rows of <see cref="T"/> that fullfill a passed <see cref="IQueryCondition"/>.
        /// </summary>
        /// <param name="condition">identifies all rows that should be deleted.</param>
        /// <returns>the number of affected rows</returns>
        Task<int> DeleteAsync(IQueryCondition condition);
    }
}
