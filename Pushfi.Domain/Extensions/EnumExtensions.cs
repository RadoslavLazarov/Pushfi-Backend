using Pushfi.Domain.Resources;
using System.Reflection;

namespace Pushfi.Domain.Extensions
{
    public static class EnumExtensions
    {
		public static List<EnumNameValue> AllEnumsAsJson()
		{
			var enumTypes = Assembly.GetExecutingAssembly()
									.GetTypes()
									.Where(x => x.IsEnum);

			List<EnumNameValue> results = new List<EnumNameValue>();
			foreach (var enumType in enumTypes)
			{
				results.Add(EnumToJson(enumType));
			}

			return results;
		}

		public static EnumNameValue EnumToJson(this Type type)
		{
			if (!type.IsEnum)
			{
				throw new InvalidOperationException(Strings.SpecifiedTypeIsNotEnum);
			}

			var results = Enum.GetValues(type)
							  .Cast<object>()
							  .ToDictionary(enumValue => enumValue.ToString(), enumValue => (int)enumValue);

			return new EnumNameValue()
			{
				Name = type.Name,
				Values = results
			};
		}
	}
}
