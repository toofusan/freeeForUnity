using System;

namespace Freee {

	[Serializable]
	public class Employee {
		public int id;
		public string num;
		public string display_name;
		public string entry_date;
		public string retire_date;
		public int user_id;
		public string email;
	}

    [Serializable]
	public class EmployeesResponse {
		public Employee[] items;
	}
}