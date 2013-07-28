namespace GMATechProject.Domain.Plumbing
{
	using System;
	using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    public abstract class Repository<T> : IRepository<T>
        where T : IEntity
    {
        /// <summary>
        /// The default key MongoRepository will look for in the App.config or Web.config file.
        /// </summary>
        private const string DefaultConnectionstringName = "MongoServerSettings";

        /// <summary>
        /// Retrieves the default connectionstring from the App.config or Web.config file.
        /// </summary>
        /// <returns>Returns the default connectionstring from the App.config or Web.config file.</returns>
        private static string GetDefaultConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[DefaultConnectionstringName].ConnectionString;
        }
		
        /// <summary>
        /// Determines the collectionname for T and assures it is not empty
        /// </summary>
        /// <typeparam name="T">The type to determine the collectionname for.</typeparam>
        /// <returns>Returns the collectionname for T.</returns>
        private static string GetCollectionName()
        {
            string collectionName;
            if (typeof(T).BaseType.Equals(typeof(object)))
            {
                collectionName = GetCollectioNameFromInterface();
            }
            else
            {
                collectionName = GetCollectionNameFromType(typeof(T));
            }

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }
		
        /// <summary>
        /// Determines the collectionname from the specified type.
        /// </summary>
        /// <typeparam name="T">The type to get the collectionname from.</typeparam>
        /// <returns>Returns the collectionname from the specified type.</returns>
        private static string GetCollectioNameFromInterface()
        {
            string collectionname;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
			var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                collectionname = typeof(T).Name;
            }

            return collectionname;
        }

        /// <summary>
        /// Determines the collectionname from the specified type.
        /// </summary>
        /// <param name="entitytype">The type of the entity to get the collectionname from.</param>
        /// <returns>Returns the collectionname from the specified type.</returns>
        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;

            // Check to see if the object (inherited from Entity) has a CollectionName attribute
            var att = Attribute.GetCustomAttribute(entitytype, typeof(CollectionNameAttribute));
            if (att != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = ((CollectionNameAttribute)att).Name;
            }
            else
            {
                // No attribute found, get the basetype
                while (!entitytype.BaseType.Equals(typeof(Entity)))
                {
                    entitytype = entitytype.BaseType;
                }

                collectionname = entitytype.Name;
            }

            return collectionname;
        }
		
        /// <summary>
        /// MongoCollection field.
        /// </summary>
        private MongoCollection<T> _Collection;
        private MongoServer _Server;
		private MongoDatabase _Database;

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public Repository()
            : this(GetDefaultConnectionString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public Repository (string connectionString)
		{
			var url = new MongoUrl(connectionString);
			this._Server = MongoServer.Create(url);
			this._Database = this._Server.GetDatabase (url.DatabaseName, SafeMode.True);
			
			this._Collection = this._Database.GetCollection<T>(GetCollectionName());
        }

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        public Repository(MongoUrl url)
        {
        }

        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class.
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        [Obsolete("This property will be removed in future releases; for most purposes you can use the MongoRepositoryManager<T>.")]
        public MongoCollection<T> Collection
        {
            get
            {
                return this._Collection;
            }
        }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The string representing the ObjectId of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        public T GetById(string id)
        {
            if (typeof(T).IsSubclassOf(typeof(Entity)))
            {
                return this._Collection.FindOneByIdAs<T>(new ObjectId(id));
            }

            return this._Collection.FindOneByIdAs<T>(id);
        }

        /// <summary>
        /// Returns a single T by the given criteria.
        /// </summary>
        /// <param name="criteria">The expression.</param>
        /// <returns>A single T matching the criteria.</returns>
        public T GetSingle(Expression<Func<T, bool>> criteria)
        {
            return this._Collection.AsQueryable<T>().Where(criteria).FirstOrDefault();
        }

        /// <summary>
        /// Returns the last error.
        /// </summary>
        public GetLastErrorResult GetLastError ()
		{
			return this._Server.GetLastError();
		}
		
        /// <summary>
        /// Returns the list of T where it matches the criteria.
        /// </summary>
        /// <param name="criteria">The expression.</param>
        /// <returns>IQueryable of T.</returns>
        public IQueryable<T> All(Expression<Func<T, bool>> criteria)
        {
            return this._Collection.AsQueryable<T>().Where(criteria);
        }

        /// <summary>
        /// Returns All the records of T.
        /// </summary>
        /// <returns>IQueryable of T.</returns>
        public IQueryable<T> All()
        {
            return this._Collection.AsQueryable<T>();
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public T Add (T entity)
		{
			T result = default(T);
			var operationResult = this._Collection.Insert<T> (entity);

			Console.Write ("Adding " + typeof(T).Name + ": " + operationResult.Ok);
			
			if (!operationResult.Ok) {
				Console.WriteLine (operationResult.ErrorMessage);
			} else {
				result = entity;
			}			
			return result;
        }

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        public void Add(IEnumerable<T> entities)
        {
            this._Collection.InsertBatch<T>(entities);
        }

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The updated entity.</returns>
        public T Update (T entity)
		{
			T result = default(T);
			var operationResult = this._Collection.Save<T> (entity);

			if (operationResult.Ok) {
				result = entity;
			}

            return result;
        }

        /// <summary>
        /// Upserts the entities.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        public void Update(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                this._Collection.Save<T>(entity);
            }
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The string representation of the entity's id.</param>
        public bool Delete(string id)
        {
			SafeModeResult operationResult = null;

            if (typeof(T).IsSubclassOf(typeof(Entity)))
            {
				operationResult = this._Collection.Remove(Query.EQ("_id", new ObjectId(id)));
            }
            else
            {
                operationResult = this._Collection.Remove(Query.EQ("_id", id));
            }

			return operationResult.Ok;
        }

        /// <summary>
        /// Deletes the given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public bool Delete(T entity)
        {
            return this.Delete(entity.Id);
        }

        /// <summary>
        /// Deletes the entities matching the criteria.
        /// </summary>
        /// <param name="criteria">The expression.</param>
        public void Delete(Expression<Func<T, bool>> criteria)
        {
            foreach (T entity in this._Collection.AsQueryable<T>().Where(criteria))
            {
                this.Delete(entity.Id);
            }
        }

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        public void DeleteAll()
        {
            this._Collection.RemoveAll();
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public long Count()
        {
            return this._Collection.Count();
        }

        /// <summary>
        /// Checks if the entity exists for given criteria.
        /// </summary>
        /// <param name="criteria">The expression.</param>
        /// <returns>true when an entity matching the criteria exists, false otherwise.</returns>
        public bool Exists(Expression<Func<T, bool>> criteria)
        {
            return this._Collection.AsQueryable<T>().Any(criteria);
        }

        /// <summary>
        /// Lets the server know that this thread is about to begin a series of related operations that must all occur
        /// on the same connection. The return value of this method implements IDisposable and can be placed in a using
        /// statement (in which case RequestDone will be called automatically when leaving the using statement). 
        /// </summary>
        /// <returns>A helper object that implements IDisposable and calls RequestDone() from the Dispose method.</returns>
        /// <remarks>
        /// Sometimes a series of operations needs to be performed on the same connection in order to guarantee correct
        /// results. This is rarely the case, and most of the time there is no need to call RequestStart/RequestDone.
        /// An example of when this might be necessary is when a series of Inserts are called in rapid succession with
        /// SafeMode off, and you want to query that data in a consistent manner immediately thereafter (with SafeMode
        /// off the writes can queue up at the server and might not be immediately visible to other connections). Using
        /// RequestStart you can force a query to be on the same connection as the writes, so the query won't execute
        /// until the server has caught up with the writes.
        /// A thread can temporarily reserve a connection from the connection pool by using RequestStart and
        /// RequestDone. You are free to use any other databases as well during the request. RequestStart increments a
        /// counter (for this thread) and RequestDone decrements the counter. The connection that was reserved is not
        /// actually returned to the connection pool until the count reaches zero again. This means that calls to
        /// RequestStart/RequestDone can be nested and the right thing will happen.
        /// </remarks>
        public IDisposable RequestStart()
        {
            return this.RequestStart(false);
        }

        /// <summary>
        /// Lets the server know that this thread is about to begin a series of related operations that must all occur
        /// on the same connection. The return value of this method implements IDisposable and can be placed in a using
        /// statement (in which case RequestDone will be called automatically when leaving the using statement). 
        /// </summary>
        /// <returns>A helper object that implements IDisposable and calls RequestDone() from the Dispose method.</returns>
        /// <param name="slaveOk">Whether queries should be sent to secondary servers.</param>
        /// <remarks>
        /// Sometimes a series of operations needs to be performed on the same connection in order to guarantee correct
        /// results. This is rarely the case, and most of the time there is no need to call RequestStart/RequestDone.
        /// An example of when this might be necessary is when a series of Inserts are called in rapid succession with
        /// SafeMode off, and you want to query that data in a consistent manner immediately thereafter (with SafeMode
        /// off the writes can queue up at the server and might not be immediately visible to other connections). Using
        /// RequestStart you can force a query to be on the same connection as the writes, so the query won't execute
        /// until the server has caught up with the writes.
        /// A thread can temporarily reserve a connection from the connection pool by using RequestStart and
        /// RequestDone. You are free to use any other databases as well during the request. RequestStart increments a
        /// counter (for this thread) and RequestDone decrements the counter. The connection that was reserved is not
        /// actually returned to the connection pool until the count reaches zero again. This means that calls to
        /// RequestStart/RequestDone can be nested and the right thing will happen.
        /// </remarks>
        public IDisposable RequestStart(bool slaveOk)
        {
            return this._Collection.Database.RequestStart(slaveOk);
        }

        /// <summary>
        /// Lets the server know that this thread is done with a series of related operations.
        /// </summary>
        /// <remarks>
        /// Instead of calling this method it is better to put the return value of RequestStart in a using statement.
        /// </remarks>
        public void RequestDone()
        {
            this._Collection.Database.RequestDone();
        }
    
		public abstract PaginatedList<T> QuickSearch (string query, int pageIndex, int pageSize);
	}
}
