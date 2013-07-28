namespace ABC.Web
{
	using System;
	using System.Collections.Generic;
	using Nancy;
	using Nancy.Validation;

	public class ActionResult
	{
		#region Constructor

		public ActionResult(bool success = false)
		{
			this.Success = success;
			
			this.Errors = new List<string>();
			this.Warnings = new List<string>();
			this.Infos = new List<string>();
		}

		#endregion

		#region Properties

		public bool Success { get; set; }
		
		public IList<string> Errors { get; set; }
		
		public IList<string> Warnings { get; set; }
		
		public IList<string> Infos { get; set; }

		#endregion		
		
		#region Public Methods
		
		public ActionResult WithInfo(string message)
		{
			this.Infos.Add(message);
			
			return this;
		}
		
		public ActionResult WithWarning(string message)
		{
			this.Warnings.Add(message);
			
			return this;
		}
		
		public ActionResult WithError(string message)
		{
			this.Errors.Add(message);
			
			return this;
		}		
		
		#endregion
		
		#region Public Static Methods
		
		public static ActionResult AsSuccess(params string[] infos)
		{
			var result = new ActionResult(true);
			
			foreach (var info in infos) 
			{
				result.WithInfo(info);
			}
			
			return result;
		}

		public static ActionResult AsError(params string[] errors)
		{
			var result = new ActionResult(true);
			
			foreach (var error in errors) 
			{
				result.WithError(error);
			}
			
			return result;
		}
		
		public static ActionResult AsErrorOnValidation(ModelValidationResult validationResult)
		{
			var result = new ActionResult(false);
			
			foreach (var error in validationResult.Errors) 
			{
				foreach (var member in error.MemberNames) 
				{
					result.WithError(error.GetMessage(member));
				}
			}
		
			return result;
		}
		
		#endregion
	}
	
	public class ActionResult<T> : ActionResult
	{
		#region Constructor

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
		
		public T Data { get; set; }
		
		#endregion
		
		#region Public Static Methods
		
		public static ActionResult<T> AsSuccess(T data, params string[] infos)
		{
			var result = new ActionResult<T>(data, true);
			
			foreach (var info in infos) 
			{
				result.WithInfo(info);
			}
			
			return result;
		}

		#endregion
	}
}
