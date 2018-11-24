using System;

namespace Freee {

	[Serializable]
	public class Section {
		public int id;
		public int company_id;
		public string name;
		public string long_name;
		public string shortcut1;
		public string shortcut2;
		public int indent_count;
		public int parent_id;
	}

	[Serializable]
	public class Sections {
		public Section[] sections;
	}
}