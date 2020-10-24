using System;

namespace Totoki.ETL {
	/// <summary>
	/// The main class.
	/// </summary>
	public partial class Program {
		public static Program Instance { get; private set; }
		public static Configuration Configuration { get; private set; }

		public const float GameFieldSize = 40f;
		public const float GameFieldDegree = -(float)Math.PI / 6;

		public const int UpdatePerSeconds = 60;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			// load setting
			var conf = Configuration.Load("config.xml", null);
			if (conf == null) {
				conf = new Configuration();
				conf.Save("config.xml");
			}
			Configuration = conf;

			Instance = new Program();
			using (Instance) {
				Instance.Run();
			}
		}
	}

}
