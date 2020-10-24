using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Totoki.ETL {
	[Serializable]
	public class Configuration {
		public bool FullScreen { get; set; }
		public int DefaultWidth { get; set; }
		public int DefaultHeight { get; set; }
		public InputSettings Input { get; set; }
		public bool Vsync { get; set; }

		public Configuration() {
			FullScreen = false;
			DefaultWidth = 800;
			DefaultHeight = 600;
			Vsync = false;
			Input = new InputSettings();
		}

		public static Configuration Load(string filePath, Configuration ifFailed) {
			XmlSerializer se = new XmlSerializer(typeof(Configuration));
			try {
				return Load(filePath);
			}
			catch (InvalidCastException) {
				return ifFailed;
			}
			catch (IOException) {
				return ifFailed;
			}
		}

		public static Configuration Load(string filePath) {
			XmlSerializer se = new XmlSerializer(typeof(Configuration));
			using (Stream fs = File.OpenRead(filePath)) {
				return (Configuration)se.Deserialize(fs);
			}
		}

		public void Save(string filePath) {
			XmlSerializer se = new XmlSerializer(typeof(Configuration));
			using (Stream fs = File.OpenWrite(filePath)) {
				se.Serialize(fs, this);
			}
		}
	}
}
