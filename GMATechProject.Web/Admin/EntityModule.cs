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
	public abstract class EntityModule<TEntity, TRepository> : SimpleEntityModule<TEntity, TRepository>
		where TEntity : class, IEntity
		where TRepository : class, IRepository<TEntity>
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityModule" />.
		/// </summary>
		/// <param name="repository">The entity's repository</param>
		protected EntityModule (IApplication application, TRepository repository)
			: base(application, repository)
		{						         			
			// Bind the HTTP POST verb to the NewEntity method
            this.Post[this.BaseModulePath + "/"] = CreateNewEntity; 
            
			// Bind the HTTP PUT verb to the UpdateEntity method
            this.Put[this.BaseModulePath + "/"] = UpdateEntity;

			// Bind the HTTP POST verb with /publish/ to the Publish method
            this.Put[this.BaseModulePath + "/publish/{id}"] = Publish;

			// Bind the HTTP POST verb with /unpublish/ to the UnPublish method
			this.Put[this.BaseModulePath + "/unpublish/{id}"] = UnPublish;
        }

		#endregion

		#region Protected Methods

		protected virtual Response CreateNewEntity (dynamic parameters)
		{
			Response result = null;

			// Bind the request parameters to get an entity
			var entity = this.Bind<TEntity> ();

			// If we have a valid entity
			if (entity != null) 
			{
				// If we have a validator for this entity
				if(this.Validator != null)
				{
					// Request validation of the entity
					var validation = this.Validator.Validate (entity);

					// If the entity is valid
					if (validation.IsValid) 
					{
						// Saves it 
						var savedEntity = this.InnerCreateEntity (entity);

						// Return the saved version as JSON
						result = Response.AsJson (ActionResult.AsSuccess (savedEntity));
					} 
					// If the entity is not valid
					else 
					{ 
						// Return a json object with th error messages
						result = Response.AsJson (ActionResult.AsErrorOnValidation (validation));
					}
				}
				// If we have no validator for this entity
				else
				{
					// Saves it 
					var savedEntity = this.InnerUpdateEntity (entity);

					// Return the saved version as JSON
					result = Response.AsJson (ActionResult<TEntity>.AsSuccess (savedEntity));
				}
			}
			else
			{
				result = Response.AsJson(ActionResult.AsGenericError());
			}

			return result;
		}

		protected abstract TEntity InnerCreateEntity (TEntity entity);

		protected virtual Response UpdateEntity (dynamic parameters)
		{
			Response result = null;

			// Bind the request parameters to get an entity
			var entity = this.Bind<TEntity> ();

			try
			{
				// If we have a valid entity
				if (entity != null) 
				{
					// If we have a validator for this entity
					if(this.Validator != null)
					{
						// Request validation of the entity
						var validation = this.Validator.Validate (entity);
						
						// If the entity is valid
						if (validation.IsValid) 
						{
							// Saves it 
							var savedEntity = this.InnerUpdateEntity (entity);
							
							// Return the saved version as JSON
							result = Response.AsJson (ActionResult<TEntity>.AsSuccess (savedEntity));
						} 
						// If the entity is not valid
						else 
						{ 
							// Return a json object with th error messages
							result = Response.AsJson (ActionResult.AsErrorOnValidation (validation));
						}
					}
					// If we have no validator for this entity
					else
					{
						// Saves it 
						var savedEntity = this.InnerUpdateEntity (entity);
						
						// Return the saved version as JSON
						result = Response.AsJson (ActionResult<TEntity>.AsSuccess (savedEntity));
					}
				}
				
				// If we don't have a result yet
				if (result == null) {
					result = Response.AsJson (ActionResult<TEntity>.AsError ("L'élément n'a pas été mis à jour"));
				}
			}
			catch(Exception e)
			{
				var errors = new List<string>();
				errors.Add("Erreur: " + e.Message);
				if(e.InnerException != null)
					errors.Add("Exception interne:" + e.InnerException.Message);

				result = Response.AsJson (ActionResult<TEntity>.AsError (errors.ToArray()));
			}

			return result;
		}

		protected abstract TEntity InnerUpdateEntity (TEntity entity);

		protected virtual Response UnPublish(dynamic parameters)
		{
			Response result = null;
			
			// If we have an id paramater
			if(parameters.id != null)
			{
				try
				{
					// Get the item
					var item = this.Repository.GetById(parameters.id.ToString());
					
					// Try to cast it as a publishable item
					var publishableItem = item as IPublishable;
					
					if (publishableItem != null) 
					{
						// Change the publication state of the item
						publishableItem.PublicationState = PublicationState.Draft;
						
						// Saves it 
						this.InnerUpdateEntity (item);
						
						// Return a successfull action result					
						result = Response.AsJson(ActionResult.AsSuccess("Vos modifications ont été enregistrées."));
					}
				}
				catch(Exception exception)
				{
					this.Logger.ErrorException("Exception while unpublishing " + typeof(TEntity).Name + " with id \"" + parameters.id + "\"", exception);
					
					// Return a failed action result
					result = Response.AsJson(ActionResult.AsError("Une erreur est survenue. Vos modifications n'ont pas été enregistrées."));
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

		protected virtual Response Publish(dynamic parameters)
		{
			Response result = null;
			
			// If we have an id paramater
			if(parameters.id != null)
			{
				try
				{
					// Get the item
					var item = this.Repository.GetById(parameters.id.ToString());

					// Try to cast it as a publishable item
					var publishableItem = item as IPublishable;

					if (publishableItem != null) 
					{
						// Change the publication state of the item
						publishableItem.PublicationState = PublicationState.Published;

						// Saves it 
						this.InnerUpdateEntity (item);
						
						// Return a successfull action result					
						result = Response.AsJson(ActionResult.AsSuccess("Vos modifications ont été enregistrées."));
					}
				}
				catch(Exception exception)
				{
					this.Logger.ErrorException("Exception while publishing " + typeof(TEntity).Name + " with id \"" + parameters.id + "\"", exception);

					// Return a failed action result
					result = Response.AsJson(ActionResult.AsError("Une erreur est survenue. Vos modifications n'ont pas été enregistrées."));
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

		#endregion		
	}
}