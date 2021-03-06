using Newtonsoft.Json.Shims;
using System;

namespace Newtonsoft.Json
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=false)]
	[Preserve]
	public sealed class JsonRequiredAttribute : Attribute
	{
		public JsonRequiredAttribute()
		{
		}
	}
}