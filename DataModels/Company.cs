using System;

namespace Freee {

	[Serializable]
	public class Company {
		public int id;
		public string name;
		public string name_kana;
		public string display_name;
		public string role;
	}

	[Serializable]
	public class Companies {
		public Company[] companies;
	}
}
