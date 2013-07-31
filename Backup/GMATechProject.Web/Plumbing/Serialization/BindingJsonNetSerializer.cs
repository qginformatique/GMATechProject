using System;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Json;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace GMATechProject.Web
{
	public class BindingJsonNetSerializer : IBodyDeserializer
	{
		public BindingJsonNetSerializer ()
		{
		}

		public bool CanDeserialize (string contentType)
		{
            if (String.IsNullOrEmpty(contentType))
            {
                return false;
            }

            var contentMimeType = contentType.Split(';')[0];

            return contentMimeType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase) ||
                   contentMimeType.Equals("text/json", StringComparison.InvariantCultureIgnoreCase) ||
                  (contentMimeType.StartsWith("application/vnd", StringComparison.InvariantCultureIgnoreCase) &&
                   contentMimeType.EndsWith("+json", StringComparison.InvariantCultureIgnoreCase));
		}

		public object Deserialize (string contentType, System.IO.Stream bodyStream, BindingContext context)
		{
			object result = null;

			try
			{
				var settings = new JsonSerializerSettings();
				settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				settings.Converters.Add(new StringEnumConverter());
				settings.Converters.Add (new IsoDateTimeConverter()
				                         {
					DateTimeStyles = System.Globalization.DateTimeStyles.AssumeLocal
				});
			
				var serializer = JsonSerializer.Create(settings);
				
				bodyStream.Position = 0;
	            string bodyText;
	            using (var bodyReader = new StreamReader(bodyStream))
	            {
	                bodyText = bodyReader.ReadToEnd();
	            }

				result = serializer.Deserialize(new StringReader(bodyText), context.DestinationType);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}

			return result;
		}
	}
}

