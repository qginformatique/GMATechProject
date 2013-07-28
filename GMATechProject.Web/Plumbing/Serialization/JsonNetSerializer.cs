namespace GMATechProject.Web.Plumbing.Serialization
{
	using System;
	using System.IO;
	
	using Nancy;
	using Nancy.IO;
	using Nancy.Responses;
	
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using Newtonsoft.Json.Serialization;
	
	/// <summary>
	/// Description of JsonNetSerializer.
	/// </summary>
	public class JsonNetSerializer : ISerializer
	{
		private DefaultJsonSerializer _defaultSerializer = new DefaultJsonSerializer();
		
		public bool CanSerialize(string contentType)
		{
			return _defaultSerializer.CanSerialize(contentType);
		}
		
		public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
		{
			try
			{
				var settings = new JsonSerializerSettings();

				settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				settings.Converters.Add(new JsonNullCollectionConverter());
				settings.Converters.Add(new StringEnumConverter());
				settings.Converters.Add (new IsoDateTimeConverter()
				                         {
					DateTimeStyles = System.Globalization.DateTimeStyles.AssumeLocal
				});
			
				var serializer = JsonSerializer.Create(settings);
				
				using (var writer = new JsonTextWriter(new StreamWriter(new UnclosableStreamWrapper(outputStream))))
				{
					serializer.Serialize(writer, model);
				
					writer.Flush();
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		public System.Collections.Generic.IEnumerable<string> Extensions {
			get {
				return _defaultSerializer.Extensions;
			}
		}
	}
}
