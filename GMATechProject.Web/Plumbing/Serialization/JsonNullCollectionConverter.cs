namespace GMATechProject.Web
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	/// <summary>
	/// A write only JSON.Net converter that converts null collections and arrays into empty JSON arrays.
	/// </summary>
	/// <remarks>
	/// This json converter will only work with hacks done to JSON .Net that allows null values to be processed by converters.  The
	/// CogShift customised version of JSON.Net supports this.
	/// </remarks>
	public class JsonNullCollectionConverter : JsonConverter
	{
		public override bool CanRead { get { return false; } }
		
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteStartArray();
			
			if (value != null)
			{
				var collection = value as IEnumerable;
				
				foreach (var item in collection)
					serializer.Serialize(writer, item);
			}
			
			writer.WriteEndArray();
		}
		
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotSupportedException();
		}
		
		public override bool CanConvert(Type objectType)
		{
			return objectType.IsImplementationOf(typeof(ICollection<>)) || objectType.IsImplementationOf(typeof(ICollection)) || objectType.IsArray;
		}
	}
}

