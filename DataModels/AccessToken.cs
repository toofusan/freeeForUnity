using System;

namespace Freee {

	[Serializable]
	public class AccessToken {
		public string access_token;
		public string token_type;
		public string expires_in;
		public string refresh_token;
		public string scope;
	}
}