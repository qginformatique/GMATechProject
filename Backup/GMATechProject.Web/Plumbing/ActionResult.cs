namespace GMATechProject.Web
{
	#region Using Directives
	
	using System;
	using System.Collections.Generic;
	using Nancy;
	using Nancy.Validation;

	#endregion
	
	/// <summary>
	/// Class returned for all ajax actions which describes the action's result.
	/// </summary>
	public class ActionResult
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of <see cref="ActionResult"/>
		/// </summary>
		/// <param name="success"><see langword="true"/> if the action succeeded; <see langword="false"/> otherwise.</param>
		public ActionResult(bool success = false)
		{
			this.Success = success;
			
			this.Errors = new List<string>();
			this.Warnings = new List<string>();
			this.Infos = new List<string>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// A boolean indicating whether the action succeeded.
		/// </summary>
		public bool Success { get; set; }
		
		/// <summary>
		/// A list of error messages.
		/// </summary>
		public IList<string> Errors { get; set; }
		
		/// <summary>
		/// A list of warning messages.
		/// </summary>
		public IList<string> Warnings { get; set; }
		
		/// <summary>
		/// A list of information messages.
		/// </summary>
		public IList<string> Infos { get; set; }

		#endregion		
		
		#region Public Methods
		
		/// <summary>
		/// Fluent method used to add an information message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>The current <see cref="ActionResult"/>, so that you can chain method calls.</returns>
		public ActionResult WithInfo(string message)
		{
			this.Infos.Add(message);
			
			return this;
		}
		
		/// <summary>
		/// Fluent method used to add a warning message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>The current <see cref="ActionResult"/>, so that you can chain method calls.</returns>
		public ActionResult WithWarning(string message)
		{
			this.Warnings.Add(message);
			
			return this;
		}
		
		/// <summary>
		/// Fluent method used to add an error message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>The current <see cref="ActionResult"/>, so that you can chain method calls.</returns>
		public ActionResult WithError(string message)
		{
			this.Errors.Add(message);
			
			return this;
		}		
		
		#endregion
		
		#region Public Static Methods
		
		/// <summary>
		/// Helper method returning a new <see cref="ActionResult"/> indicating a successfully executed action, with an optional list of 
		/// information messages.
		/// </summary>
		/// <param name="infos">Optional: information messages</param>
		/// <returns>A new <see cref="ActionResult"/>.</returns>
		public static ActionResult AsSuccess(params string[] infos)
		{
			// Creates the action result initializing its success property to true
			var result = new ActionResult(true);
			
			// If some messages were specified
			if(infos != null)
			{
				// Loop over the messages
				foreach (var info in infos) 
				{
					// Add them to the action result
					result.WithInfo(info);
				}
			}
			
			return result;
		}

		/// <summary>
		/// Helper method returning a new <see cref="ActionResult"/> indicating a successfully executed action, with 
		/// the data returned by the action and an optional list of information messages.
		/// </summary>
		/// <param name="data">The data returned by the action.</param>
		/// <param name="infos">Optional: information messages</param>
		/// <returns>A new <see cref="ActionResult"/>.</returns>
		public static ActionResult<T> AsSuccess<T>(T data, params string[] infos)
		{
			// Creates the action result initializing its success property to true
			var result = new ActionResult<T>(data, true);
			
			// If some messages were specified
			if(infos != null)
			{
				// Loop over the messages
				foreach (var info in infos) 
				{
					// Add them to the action result
					result.WithInfo(info);
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// Helper method returning a new <see cref="ActionResult"/> indicating a failed action, with an optional list of 
		/// error messages.
		/// </summary>
		/// <param name="errors">Optional: error messages</param>
		/// <returns>A new <see cref="ActionResult"/>.</returns>
		public static ActionResult AsError(params string[] errors)
		{
			// Creates the action result initializing its success property to false
			var result = new ActionResult(false);
			
			// If some messages were specified
			if(errors != null)
			{
				// Loop over the messages
				foreach (var error in errors) 
				{
					// Add them to the action result
					result.WithError(error);
				}
			}
			
			return result;
		}

		/// <summary>
		/// Helper method returning a new <see cref="ActionResult"/> indicating a failed action, with an optional list of 
		/// error messages.
		/// </summary>
		/// <returns>A new <see cref="ActionResult"/>.</returns>
		public static ActionResult AsGenericError()
		{
			// Creates the action result initializing its success property to false 
			return new ActionResult(false)
				// With a generic error message
				.WithError("Une erreur est survenue, veuillez ré-essayer plus tard.");
		}

		/// <summary>
		/// Helper method returning a new <see cref="ActionResult"/> indicating a failed action, with a list of 
		/// error messages generated from the specified validation result.
		/// </summary>
		/// <param name="validationResult">A validation result.</param>
		/// <returns>A new <see cref="ActionResult"/>.</returns>
		public static ActionResult AsErrorOnValidation(ModelValidationResult validationResult)
		{
			// Creates the action result initializing its success property to false
			var result = new ActionResult(false);
			
			// If we have a validation result
			if(validationResult != null)
			{
				// Loop over its errors
				foreach (var error in validationResult.Errors) 
				{
					// Loop over the member names for the current error
					foreach (var member in error.MemberNames) 
					{
						// Add the error message to the action result
						result.WithError(error.GetMessage(member));
					}
				}
			}
		
			return result;
		}
		
		#endregion
	}
	
	/// <summary>
	/// Generic version of the <see cref="ActionResult" /> class, used when the action also returns some data.
	/// </summary>
	public class ActionResult<T> : ActionResult
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of <see cref="ActionResult"/>
		/// </summary>
		/// <param name="data">The data returned by the action.</param>
		/// <param name="success"><see langword="true"/> if the action succeeded; <see langword="false"/> otherwise.</param>		
		public ActionResult(T data, bool success = true)
		{
			this.Success = success;
			this.Data = data;
			
			this.Errors = new List<string>();
			this.Warnings = new List<string>();
			this.Infos = new List<string>();
		}

		#endregion
		
		#region Properties
		
		/// <summary>
		/// The data returned by the action.
		/// </summary>
		public T Data { get; set; }
		
		#endregion
	}
}
