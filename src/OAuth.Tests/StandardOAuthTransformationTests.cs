using NUnit.Framework;

namespace OAuth.Tests
{
	/// <summary>
	/// These test cases are found at <see cref="http://wiki.oauth.net/w/page/12238556/TestCases"/>
	/// </summary>
	[TestFixture]
	public class StandardOAuthTransformationTests
	{
		/// <summary>
		/// Parameter Encoding
		/// </summary>
		[TestCase("abcABC123", Result = "abcABC123")]
		[TestCase("-._~", Result = "-._~")]
		[TestCase("%", Result = "%25")]
		[TestCase("+", Result = "%2B")]
		[TestCase("&=*", Result = "%26%3D%2A")]
		[TestCase("\u000A", Result = "%0A")]
		[TestCase("\u0020", Result = "%20")]
		[TestCase("\u007F", Result = "%7F")]
		[TestCase("\u0080", Result = "%C2%80")]
		[TestCase("\u3001", Result = "%E3%80%81")]
		public string Relexed_parameter_encoding_passes_standard_oauth_test_cases(string input)
		{
			return OAuthTools.UrlEncodeRelaxed(input);
		}

		/// <summary>
		/// Parameter Encoding
		/// </summary>
		[TestCase("abcABC123", Result = "abcABC123")]
		[TestCase("-._~", Result = "-._~")]
		[TestCase("%", Result = "%25")]
		[TestCase("+", Result = "%2B")]
		[TestCase("&=*", Result = "%26%3D%2A")]
		[TestCase("\u000A", Result = "%0A")]
		[TestCase("\u0020", Result = "%20")]
		[TestCase("\u007F", Result = "%7F")]
		[TestCase("\u0080", Result = "%C2%80")]
		[TestCase("\u3001", Result = "%E3%80%81")]
		public string Strict_parameter_encoding_passes_standard_oauth_test_cases(string input)
		{
			return OAuthTools.UrlEncodeStrict(input);
		}

		/// <summary>
		/// HMAC-SHA1
		/// </summary>
		[TestCase("cs", null, "bs", Result = "egQqG5AJep5sJ7anhXju1unge2I=")]
		[TestCase("cs", "ts", "bs", Result = "VZVjXceV7JgPq/dOTnNmEfO0Fv8=")]
		[TestCase("kd94hf93k423kf44", "pfkkdhi9sl3r4s00", "GET&http%3A%2F%2Fphotos.example.net%2Fphotos&file%3Dvacation.jpg%26oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3Dkllo9940pd9333jh%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1191242096%26oauth_token%3Dnnch734d00sl2jdk%26oauth_version%3D1.0%26size%3Doriginal", Result = "tR3+Ty81lMeYAr/Fid0kMTYa/WM=")]
		public string HMAC_SHA1_encoding_passes_standard_oauth_test_cases(string consumerSecret, string tokenSecret, string baseString)
		{
			return OAuthTools.GetSignature(OAuthSignatureMethod.HmacSha1, OAuthSignatureTreatment.Unescaped, baseString, consumerSecret, tokenSecret);
		}
	}
}