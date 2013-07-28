using System;
using System.Collections.Generic;

namespace ABC.Domain
{
	public interface IWithTags
	{
		List<Tag> Tags { get; set; }
	}
}

