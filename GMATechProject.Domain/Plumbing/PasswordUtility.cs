namespace GMATechProject.Domain
{
	#region Using Directives
	
	using System;
	using System.Security.Cryptography;
	using System.Text;

	#endregion
	
	public class PasswordUtility : IPasswordUtility
	{
		// Define default password length.
		private const int DEFAULT_PASSWORD_LENGTH = 8;
 
		//No characters that are confusing: i, I, l, L, o, O, 0, 1, u, v
 
		private const string PASSWORD_CHARS_Alpha = 
                                   "GMATechProjectdefghjkmnpqrstwxyzGMATechProjectDEFGHJKMNPQRSTWXYZ";
		private const string PASSWORD_CHARS_NUMERIC = "23456789";
		private const string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";
 
		/// <summary>
		/// Generates a random password.
		/// </summary>
		/// <returns>Randomly generated password.</returns>
		public string Generate (int passwordLength = DEFAULT_PASSWORD_LENGTH, 
                                      RandomPasswordOptions option = RandomPasswordOptions.AlphaNumericSpecial)
		{
			if (passwordLength < 0)
				return null;
 
			var passwordChars = GetCharacters (option);
 
			if (string.IsNullOrEmpty (passwordChars))
				return null;
 
			var password = new char[passwordLength];
 
			var random = GetRandom ();
 
			for (int i = 0; i < passwordLength; i++) {
				var index = random.Next (passwordChars.Length);
				var passwordChar = passwordChars [index];
 
				password [i] = passwordChar;
			}
 
			return new string (password);
		}
 
		public string HashPassword (string password)
		{
			string result = null;
			
			MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider ();
			
			byte[] data = Encoding.ASCII.GetBytes (password);
			data = hasher.ComputeHash (data);
			
			result = Encoding.ASCII.GetString (data);
			
			return result;
		}		
		
		/// <summary>
		/// Gets the characters selected by the option
		/// </summary>
		/// <returns></returns>
		private string GetCharacters (RandomPasswordOptions option)
		{
			switch (option) {
			case RandomPasswordOptions.Alpha:
				return PASSWORD_CHARS_Alpha;
			case RandomPasswordOptions.Numeric:
				return PASSWORD_CHARS_NUMERIC;
			case RandomPasswordOptions.AlphaNumeric:
				return PASSWORD_CHARS_Alpha + PASSWORD_CHARS_NUMERIC;
			case RandomPasswordOptions.AlphaNumericSpecial:
				return PASSWORD_CHARS_Alpha + PASSWORD_CHARS_NUMERIC + 
                                 PASSWORD_CHARS_SPECIAL;
			default:
				break;
			}
			return string.Empty;
		}
        
		/// <summary>
		/// Gets a random object with a real random seed
		/// </summary>
		/// <returns></returns>
		private Random GetRandom ()
		{
			// Use a 4-byte array to fill it with random bytes and convert it then
			// to an integer value.
			byte[] randomBytes = new byte[4];
 
			// Generate 4 random bytes.
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider ();
			rng.GetBytes (randomBytes);
 
			// Convert 4 bytes into a 32-bit integer value.
			int seed = (randomBytes [0] & 0x7f) << 24 |
                        randomBytes [1] << 16 |
                        randomBytes [2] << 8 |
                        randomBytes [3];
 
			// Now, this is real randomization.
			return new Random (seed);
		}
 
 
	}
}

