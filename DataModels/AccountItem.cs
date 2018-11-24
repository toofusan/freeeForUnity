using System;

namespace Freee {

	[Serializable]
	public class AccountItem {
		public int id;
		public string name;
		public string shortcut;
		public string shortcut_num;
		public int default_tax_id;
		public string[] categories;
	}
}