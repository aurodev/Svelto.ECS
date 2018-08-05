using Svelto.DataStructures;
using Svelto.Utilities;

namespace Svelto.ECS
{
    public interface IEntitiesDB
    {
        /// <summary>
        /// All the EntityView related methods are left for back compatibility, but
        /// shouldn't be used anymore. Always pick EntityViewStruct or EntityStruct
        /// over EntityView
        /// </summary>
        ReadOnlyCollectionStruct<T> QueryEntityViews<T>() where T : class, IEntityStruct;
        /// <summary>
        /// All the EntityView related methods are left for back compatibility, but
        /// shouldn't be used anymore. Always pick EntityViewStruct or EntityStruct
        /// over EntityView
        /// </summary>
        ReadOnlyCollectionStruct<T> QueryEntityViews<T>(int group) where T : class, IEntityStruct;
        /// <summary>
        /// All the EntityView related methods are left for back compatibility, but
        /// shouldn't be used anymore. Always pick EntityViewStruct or EntityStruct
        /// over EntityView
        /// </summary>
        bool TryQueryEntityView<T>(EGID egid, out T entityView) where T : class, IEntityStruct;
        /// <summary>
        /// All the EntityView related methods are left for back compatibility, but
        /// shouldn't be used anymore. Always pick EntityViewStruct or EntityStruct
        /// over EntityView
        /// </summary>
        T QueryEntityView<T>(EGID egid) where T : class, IEntityStruct;
        /// <summary>
        /// Fast and raw (therefore not safe) return of entities buffer
        /// Modifying a buffer would compromise the integrity of the whole DB
        /// so they are meant to be used only in performance critical path
        /// </summary>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T[] QueryEntities<T>(out int count) where T : IEntityStruct;
        T[] QueryEntities<T>(int group, out int count) where T : IEntityStruct;       
        /// <summary>
        /// this version returns a mapped version of the entity array so that is possible to find the
        /// index of the entity inside the returned buffer through it's EGID
        /// However mapping can be slow so it must be used for not performance critical paths
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="mapper"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        EGIDMapper<T> QueryMappedEntities<T>(int groupID) where T : IEntityStruct;
        EGIDMapper<T> QueryMappedEntities<T>() where T : IEntityStruct;
        /// <summary>
        /// Execute an action on entities. Be sure that the action is not capturing variables
        /// otherwise you will allocate memory which will have a great impact on the execution performance.
        /// ExecuteOnEntities can be used to iterate safely over entities, several checks are in place
        /// to be sure that everything will be done correctly.
        /// Cache friendliness is guaranteed if only Entity Structs are used, but 
        /// </summary>
        /// <param name="egid"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        void ExecuteOnEntities<T>(int groupID, ActionRef<T> action) where T : IEntityStruct;
        void ExecuteOnEntities<T>(ActionRef<T> action) where T : IEntityStruct;
        void ExecuteOnEntities<T, W>(int groupID, ref W value, ActionRef<T, W> action) where T : IEntityStruct;
        void ExecuteOnEntities<T, W>(ref W value, ActionRef<T, W> action) where T : IEntityStruct;
        /// <summary>
        /// This specialized version allows to execute actions on multiple entity views or entity structs
        /// Safety checks are in place. This function doesn't guarantee cache
        /// friendliness even if just EntityStructs are used. 
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        void ExecuteOnEntities<T, T1>(int groupID, ActionRef<T, T1> action) where T : IEntityStruct where T1 : IEntityStruct;
        void ExecuteOnEntities<T, T1>(ActionRef<T, T1> action) where T : IEntityStruct where T1 : IEntityStruct;
        
        void ExecuteOnEntities<T, T1, W>(int groupID, ref W value, ActionRef<T, T1, W> action) where T : IEntityStruct where T1 : IEntityStruct;
        void ExecuteOnEntities<T, T1, W>(ref W value, ActionRef<T, T1, W> action) where T : IEntityStruct where T1 : IEntityStruct;
        void ExecuteOnEntities<T, T1, W>(W value, ActionRef<T, T1, W> action) where T : IEntityStruct where T1 : IEntityStruct;
        /// <summary>
        /// Execute an action on ALL the entities regardless the group. This function doesn't guarantee cache
        /// friendliness even if just EntityStructs are used. 
        /// Safety checks are in place 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        void ExecuteOnAllEntities<T>(ActionRef<T> action) where T : IEntityStruct;
        void ExecuteOnAllEntities<T, W>(ref W  value, ActionRef<T, W> action) where T : IEntityStruct;
        /// <summary>
        /// ECS is meant to work on a set of Entities. Working on a single entity is sometime necessary, but using
        /// the following functions inside a loop would be a mistake as performance can be significantly impacted
        /// return the buffer and the index of the entity inside the buffer using the input EGID 
        /// </summary>
        /// <param name="entityGid"></param>
        /// <param name="index"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T[] QueryEntitiesAndIndex<T>(EGID entityGid, out uint index) where T : IEntityStruct;
        bool TryQueryEntitiesAndIndex<T>(EGID entityGid, out uint index, out T[] array) where T : IEntityStruct;
        /// <summary>
        /// ECS is meant to work on a set of Entities. Working on a single entity is sometime necessary, but using
        /// the following functions inside a loop would be a mistake as performance can be significantly impacted
        /// Execute an action on a specific Entity. Be sure that the action is not capturing variables
        /// otherwise you will allocate memory which will have a great impact on the execution performance
        /// </summary>
        /// <param name="egid"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        void ExecuteOnEntity<T>(EGID egid, ActionRef<T> action) where T : IEntityStruct;
        void ExecuteOnEntity<T>(int     id,   ActionRef<T> action) where T : IEntityStruct;
        void ExecuteOnEntity<T>(int     id,   int          groupid, ActionRef<T>    action) where T : IEntityStruct;
        void ExecuteOnEntity<T, W>(EGID egid, ref W        value,   ActionRef<T, W> action) where T : IEntityStruct;
        void ExecuteOnEntity<T, W>(int  id,   ref W        value,   ActionRef<T, W> action) where T : IEntityStruct;
        void ExecuteOnEntity<T, W>(int  id,   int          groupid, ref W           value, ActionRef<T, W> action) where T : IEntityStruct;

        bool Exists<T>(EGID egid) where T : IEntityStruct;
        
        bool HasAny<T>() where T:IEntityStruct;
        bool HasAny<T>(int group) where T:IEntityStruct;
    }
}