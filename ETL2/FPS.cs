using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Totoki.ETL {

	public class FPS {
		int count;
		long time;
		long interval;

		// 1tick -> 100nsec
		const long TicksPerSec = TimeSpan.TicksPerSecond;


		public FPS() {
			time = DateTime.Now.Ticks;
			count = 0;
			interval = TicksPerSec / 2;
			Current = 0;
		}

		public double Update() {
			count++;

			long now = DateTime.Now.Ticks;
			if (now > time + interval) {
				Current = count / ((double)(now - time) / TicksPerSec);
				count = 0;
				time = now;
			}
			return Current;
		}

		public double Current { get; private set; }
	}

}


