using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Totoki.ETL {
	public partial class Program {

		SpriteBatch sbatch;
		SpriteFont font;

		Dictionary<string, string> statistics = new Dictionary<string, string>();

		[Conditional("DEBUG")]
		private void InitializeStatistics() {
			sbatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("StaticticsFont");
			
			ToDisposeContent(font);
		}

		[Conditional("DEBUG")]
		public static void AddStatistics(string name, string format, params object[] args) {
			Instance.statistics[name] = string.Format(format, args);
			//statistics.Add(name, string.Format(format, args));
		}
		[Conditional("DEBUG")]
		public void PrintStatistics() {
			sbatch.Begin();
			int offset = 0;
			foreach (var e in statistics) {
				sbatch.DrawString(font, e.Key + " : " + e.Value, new Vector2(0, offset), new Color(0x80FFFFFF));
				offset += 24;
			}
			statistics.Clear();
			sbatch.End();
		}
	}
}
