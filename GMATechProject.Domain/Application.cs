using NLog;
using System.Collections.Generic;

namespace GMATechProject.Domain
{
	#region Using Directives

	using System;
	using System.Linq;
	using System.Reflection;
	
	using GMATechProject.Domain.Blog;
	using GMATechProject.Domain.Progress;
	using GMATechProject.Domain.Mailing;
	using GMATechProject.Domain.Members;
	using GMATechProject.Domain.Pages;
	using GMATechProject.Domain.Security;
	
	using FluentValidation;
	
	#endregion
	
	public partial class Application : IApplication
	{		
		private readonly IValidatorFactory _ValidatorFactory;
		private readonly Logger _Logger;
				
		public Application (IPageRepository pageRepository,
		                    IArticleRepository articleRepository, 
		                    IProgressRepository progressRepository,
		                    IMemberRepository memberRepository, 
		                    IIdentityRepository identityRepository, 
		                    IRoleTagsBindingRepository roleTagsBindingRepository,
		                    IMailTemplateRepository mailTemplateRepository,
		                    IPasswordUtility passwordUtility,
		                    IValidatorFactory validatorFactory)
		{
			// Defined in Application.Progress.cs
			this._ProgressRepository = progressRepository;
			// Defined in Application.Blog.cs
			this._ArticleRepository = articleRepository;
			// Defined in Application.Page.cs
			this._PageRepository = pageRepository;
			// Defined in Application.Members.cs
			this._MemberRepository = memberRepository;
			// Defined in Application.Security.cs
			this._IdentityRepository = identityRepository;
			// Defined in Application.Security.cs
			this._RoleTagsBindingRepository = roleTagsBindingRepository;
			// Defined in Application.Security.cs
			this._MailTemplateRepository = mailTemplateRepository;
			// Defined in Application.Security.cs
			this._PasswordUtility = passwordUtility;
			
			this._ValidatorFactory = validatorFactory;

			// Initialize the main application logger
			this._Logger = LogManager.GetLogger("ApplicationLogger");
		}
		
		private IValidator<TEntity> GetValidator<TEntity> ()
		{
			return this._ValidatorFactory.GetValidator<TEntity> ();
		}
				
		private TEntity CreateEntity<TEntity> (TEntity entity, IRepository<TEntity> repository)
			where TEntity : class, IEntity
		{
			TEntity result = null;

			this._Logger.Debug("Entity to create: " + entity);

			// If we have an entity
			if (entity != null) {			
				// Get the validator for this entity
				var validator = this.GetValidator<TEntity> ();
			
				// If we have a validator
				if (validator != null) {
					// Request validation of the entity
					var validation = validator.Validate (entity);
			
					// If the entity is valid
					if (validation.IsValid) {
						// Saves it in our repository
						result = repository.Add (entity);
					} else {
						this._Logger.Warn (typeof(TEntity).Name + " is invalid !");
					}
					// If the entity is not valid, return the default null value
				} else {
					this._Logger.Error (typeof(TEntity).Name + " has no validator !");
				}
				// If we didn't get a validator, return the default null value
			}
			
			// If we didn't have an entity, return the default null value
			
			return result;
		}

		private TEntity UpdateEntity<TEntity> (TEntity entity, IRepository<TEntity> repository)
			where TEntity : class, IEntity
		{
			TEntity result = null;
						
			this._Logger.Debug ("Entity to update: " + entity);

			// If we have an entity
			if (entity != null) {
				// Get the validator for this entity
				var validator = this.GetValidator<TEntity> ();
			
				// If we have a validator
				if (validator != null) {
					// Request validation of the entity
					var validation = validator.Validate (entity);
				
					// If the entity is valid
					if (validation.IsValid) {
						// Saves it in our repository
						result = repository.Update (entity);
					} else {
						this._Logger.Warn (typeof(TEntity).Name + " is invalid !");
					}
					// If the entity is not valid, return the default null value
				} else {
					this._Logger.Error (typeof(TEntity).Name + " has no validator !");
				}
				// If we didn't get a validator, return the default null value
			}
			
			// If we didn't have an entity, return the default null value
			
			return result;
		}

		public void InitializeApplicationData()
		{
			// List all role bindings
			var rolesTagsBindings = this._RoleTagsBindingRepository.All();

			// If not null
			if(rolesTagsBindings != null)
			{
				// List all roles
				var roles = Enum.GetValues(typeof(Roles)).Cast<Roles>();

				foreach (var role in roles) 
				{
					// If no binding already exists
					if(!rolesTagsBindings.Any(binding => binding.Role == role))
					{
						// Create one
						this._RoleTagsBindingRepository.Add(
							new RoleTagsBinding()
							{
								Role = role
							});
					}
				}
			}

			// Try to find a superadmin user
			var superAdminIdentity = this._IdentityRepository.FindByEmail("superadmin@qginformatique.fr");

			// If not found
			if(superAdminIdentity == null)
			{
				superAdminIdentity = this.CreateIdentity(
					"superadmin@qginformatique.fr",
					"QGI@2414",
					Roles.SystemAdministrator);
			}
		}

		/// <summary>
		/// Searchs for publicly accessible contents matching the specified query.
		/// </summary>
		public PaginatedList<SearchContentResult> SearchContents (string query, int pageIndex, int pageSize = 20, Roles role = Roles.None, string[] tags = null)
		{
			PaginatedList<SearchContentResult> result = null;
			var tempResult = new List<SearchContentResult>(50);
			var total = 0;
			
			// Get the static pages matching the query and accessible to this role
			/*
			var pages = this.ListPblishedPages (0, 1000, role, tags);

			if (pages != null
				&& pages.Items != null) {
				// Increment the global total
				total += articles.Total;
				
				tempResult.AddRange (pages.Items.Select (
					item => new SearchContentResult (){
						Id = item.Id,
						Label = item.Title, 
						Description = item.Description, 
						Type = ContentType.Page
				}));
			}
			*/

			// Get the articles matching the query and accessible to this role
			var articles = this.ListPublishedArticles (0, 1000, role, tags);

			if (articles != null
				&& articles.Items != null) {
				// Increment the global total
				total += articles.Total;

				tempResult.AddRange (articles.Items.Select (
					item => new SearchContentResult (){
						Id = item.Id,
						Label = item.Title, 
						Description = item.Description, 
						Type = ContentType.Article
				}));
			}

			result = tempResult
				.OrderBy( item => item.Label)
				.AsQueryable()
				.ToPaginatedList<SearchContentResult>(total, pageIndex, pageSize);

			return result;
		}

	}
}

