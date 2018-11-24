using System;

namespace Freee {

	[Serializable]
	public class TimeClock {
		public int id;
		public string date;
		public string type;
		public string datetime;
		public string original_datetime;
		public string note;
	}

    [Serializable]
	public class TimeClockRequest {
		public int company_id;
		public string type;
		public string base_date;
	}

    [Serializable]
	public class TimeClocksResponse {
		public TimeClock[] items;
	}

	[Serializable]
	public class PostTimeClocksResponse {
		public TimeClock employee_time_clock;
	}

    [Serializable]
	public class TimeClocksAvailableTypes {
		public string[] available_types;
	}
}