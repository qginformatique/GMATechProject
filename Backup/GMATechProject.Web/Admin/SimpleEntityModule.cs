using NLog;

namespace GMATechProject.Web.Admin
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Nancy;
	using Nancy.ModelBinding;
	using Nancy.Validation;
		
	using GMATechProject.Domain;

	#endregion
	
	/// <summary>
	/// Base class for modules which handles entities.
	/// </summary>
	public abstract class SimpleEntityModule<TEntity, TRepository> : BaseAdminModule
		where TEntity : class, IEntity
		where TRepository : class, IRepository<TEntity>
	{
		#region Fields
		
		private readonly TRepository _Repository;
		
		private IModelValidator _Validator;
				
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets the entity's validator.
		/// </summary>
		protected IModelValidator Validator 
		{
			get 
			{
				return this._Validator ?? (this._Validator = this.ValidatorLocator.GetValidator<TEntity>(null));
			}
		}

		/// <summary>
		/// Gets the entity's repository.
		/// </summary>
		protected TRepository Repository
		{
			get 
			{
				return this._Repository;
			}
		}

		/// <summary>
		/// Gets the base path for this module
		/// </summary>
		protected abstract string BaseModulePath 
		{
			get;
		}
		
		#endregion
		
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleEntityModule" />.
		/// </summary>
		/// <param name="repository">The entity's repository</param>
		protected SimpleEntityModule (IApplication application, TRepository repository) : base(application)
		{			
			this._Repository = repository;
			         
			// Bind the HTTP GET verb to the ListEntities method
			this.Get [this.BaseModulePath + "/"] = ListEntities;
            
			// Bind the HTTP GET verb with an id parameter to the GetEntity method
			this.Get [this.BaseModulePath + "/{id}"] = GetEntity;
            
			// Bind the HTTP POST verb with quicksearch to the QuickSearch method
			this.Post [this.BaseModulePath + "/quicksearch"] = QuickSearch;
			            
			// Bind the HTTP DELETE verb to the DeleteEntity method
            this.Delete[this.BaseModulePath + "/{id}"] = DeleteEntity;
        }

		#endregion
	
		#region Protected Methods

		protected virtual Response QuickSearch (dynamic parameters)
		{
			Response result = null;
			
			var searchParameters = this.Bind<QuickSearchWithPaginationRequestParameters> ();
			
			if (searchParameters != null) {
				if (!string.IsNullOrEmpty (searchParameters.Query)) {
					try
					{
						// Request our repository for entities matching the query
						var entities = this.Repository.QuickSearch (searchParameters.Query, searchParameters.PageIndex, searchParameters.PageSize);
						
						// Return a successfull action result with the articles
						result = Response.AsJson (ActionResult.AsSuccess (entities));
					}
					catch(Exception exception){
						this.Logger.LogException(LogLevel.Error, string.Empty,exception);
						result = Response.AsJson(ActionResult.AsGenericError());
					}
				} else {
					result = Response.AsJson (ActionResult.AsError ("Vous n'avez pas spécifié de critères de recherche."));
				}
			} else {
				result = HttpStatusCode.BadRequest;
			}
			
			return result;
		}
		
		protected virtual Response ListEntities(dynamic parameters)
		{
			Response result = null;
			
			// Try to bind that request parameters (querystring and/or form post) to a PaginationRequestParameters class
			var paginationParameters = this.Bind<PaginationRequestParameters>();
			
			// Default values for pageIndex and pageSize
			var pageIndex = 0;
			var pageSize = 10;
						
			// If we got some parameters
			if(paginationParameters != null)
			{
				// Get the pageIndex from them
				pageIndex = paginationParameters.PageIndex;

				// Get the pageSize from them
				pageSize = paginationParameters.PageSize;
			}
			
			// Request our repository for those articles
			var articles = this.QueryAll()
				// Just the specified page
				.ToPaginatedList(pageIndex, pageSize);

			// Return a successfull action result with the articles
			result = Response.AsJson(ActionResult.AsSuccess(articles));
			
			return result;
		}
		
		protected virtual Response GetEntity(dynamic parameters)
		{
			Response result = null;
			
			// If we have an id paramater
			if(parameters.id.HasValue)
			{
				// Try to load the matching article
				var article = this.Repository.GetById(parameters.id.ToString());
				
				// If article found
				if(article != null)
				{
					// Return a successfull action result with this article
					result = FormatterExtensions.AsJson(Response, ActionResult.AsSuccess(article));
				}
			}
			
			// If we don't have a result yet
			if(result == null)
			{
				// NOTE: this could be better
				// Returns a 404
				result = HttpStatusCode.NotFound;
			}
			
			return result;
		}
				
		protected virtual Response DeleteEntity(dynamic parameters)
		{
			Response result = null;
			
			// If we have an id paramater
			if(parameters.id.HasValue)
			{
				try
				{
					// Delete the item with this identifier
					this.Repository.Delete(parameters.id.ToString());
					
					// Return a successfull action result					
					result = Response.AsJson(ActionResult.AsSuccess("L'élément a été supprimé."));
				}
				catch(Exception exception)
				{
					this.Logger.ErrorException("Exception while deleting " + typeof(TEntity).Name + " with id \"" + parameters.id + "\"", exception);

					// TODO: allow message customization
					var errors = new List<string>();
					errors.Add ("Une erreur est survenue. Nous n'avons pas pu supprimer cet élément.");
					errors.Add ("Exception while deleting " + typeof(TEntity).Name + " with id \"" + parameters.id + "\"");
					errors.Add (exception.Message);
					if(exception.InnerException != null)
						errors.Add (exception.InnerException.Message);
					
					// Return a failed action result
					result = Response.AsJson(ActionResult.AsError(errors.ToArray()));
				}
			}
			
			// If we don't have a result yet
			if(result == null)
			{
				// NOTE: this could be better
				// Returns a 404
				result = HttpStatusCode.NotFound;
			}
			
			return result;
		}

		protected virtual IQueryable<TEntity> QueryAll()
		{
			return this.Repository
				// All of them
				.All();
		}

		#endregion		
	}
}
