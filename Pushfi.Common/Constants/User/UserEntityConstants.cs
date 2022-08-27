namespace Pushfi.Common.Constants.User
{
    public class UserEntityConstants
    {
        public const int NameMaxLength = 100;
        public const int PhoneNumberMaxLenght = 30;
        public const string PhoneNumberTypeName = $"varchar(30)";
        public const int WebsiteURLMaxLenght = 100;
        public const int TaxMaxLength = 100;

		public static List<string> AvatarColors = new List<string>()
			{
				"#32584F",
				"#7E9E82",
				"#CDAE8D",
				"#DA2D1C",
				"#2042A1",
				"#3655AA",
				"#132861",
				"#0D1A40",
				"#584875",
				"#835A99",
				"#E8A16F",
				"#64847B",
				"#3E3252",
				"#5F5170",
				"#933EB7",
				"#532AA3",
				"#6270EE",
				"#3B438F",
				"#C50100",
				"#E90000",
				"#B6121B",
				"#4B3F34",
				"#D59446",
				"#7F0D13",
				"#49070B",
				"#553B1C",
				"#3A59EF",
				"#4C3ADC",
				"#B031DD",
				"#580292",
				"#000063",
				"#000081",
				"#065A9C",
				"#E64556",
				"#49896F",
				"#5F4958",
				"#3A5448"
			};

		public string GenerateRandomAvatarColor()
        {
			var colorIndex = new Random().Next(AvatarColors.Count);
			return AvatarColors[colorIndex];
		}
	}
}
