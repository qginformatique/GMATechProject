namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;

	#endregion

	public enum RandomPasswordOptions
	{
		/// <summary>
		/// Use this value to generate a password containing only letters characters.
		/// </summary>
		Alpha = 1,
		/// <summary>
		/// Use this value to generate a password containing only numeric characters.
		/// </summary>
		Numeric = 2,
		/// <summary>
		/// Use this value to generate a password containing only alphanumeric characters.
		/// </summary>
		AlphaNumeric = Alpha + Numeric,
		/// <summary>
		/// Use this value to generate a password containing alphanumeric and special characters.
		/// </summary>
		AlphaNumericSpecial = 4
	}
	
	public interface IPasswordUtility
	{ 		
		/// <summary>
		/// Generates a random password.
		/// </summary>
		/// <returns>Randomly generated password.</returns>
		/// <param name='passwordLength'>
		/// Password length.
		/// </param>
		/// <param name='option'>
		/// Option.
		/// </param>
		string Generate (int passwordLength = 8, RandomPasswordOptions option = RandomPasswordOptions.AlphaNumericSpecial);
		
		/// <summary>
		/// Determines whether this instance hash password the specified password.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance hash password the specified password; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='password'>
		/// Password.
		/// </param>
		string HashPassword (string password);
	}
}

