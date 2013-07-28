using System;

namespace ABC.Domain
{
	/// <summary>
	/// This attribute flags an entity's property as being searchable.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class SearchableAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ABC.Domain.SearchableAttribute"/> class.
		/// </summary>
		public SearchableAttribute ()
		{
		}
	}
}

