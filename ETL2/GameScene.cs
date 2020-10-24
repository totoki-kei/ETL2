using System;
using System.Collections.Generic;
using System.Text;

namespace Totoki.ETL {
	abstract class GameScene : GameObject {
		protected GameScene() : base(GameObject.Priorities.Scene) {}

		public virtual void BeforeDraw() {
			Program.Instance.ClearScreen();
			Program.Instance.SetDefaultDeviceStates();
			Program.Instance.UpdateViewProjectionMatrix();
		}
	}
}
