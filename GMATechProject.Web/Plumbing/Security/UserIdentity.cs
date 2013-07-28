using System;
using System.Linq;
using Nancy.Security;
using GMATechProject.Domain.Security;
using System.Collections.Generic;

namespace GMATechProject.Web
{
	public class UserIdentity : IUserIdentity
	{
		public UserIdentity (Identity identity)
		{
			this.UserName = identity.ExternalIdentifier;

			var roles = Enum.GetValues(typeof(Roles));
			var userRoles = new List<string>();

			foreach(var role in roles)
			{
				if((Roles)role <= identity.Role)
				{
					userRoles.Add(role.ToString());
				}
			}

			this.Claims = userRoles;
		}

		public override string ToString ()
		{
			return string.Format ("[UserIdentity: UserName={0}, Claims=[{1}]]", UserName, string.Join(", ", Claims.ToArray()));
		}

		#region IUserIdentity implementation

		public string UserName {
			get;
			set;
		}

		public System.Collections.Generic.IEnumerable<string> Claims {
			get;
			set;
		}

		#endregion
	}
}

