using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Totoki.ETL {
	abstract class GameScene : GameObject {
		protected GameScene() : base(GameObject.Priorities.Scene) {}

		public override void Draw(GameTime gameTime) { }

		public virtual void PrepareForDraw() {
			Program.Instance.ClearScreen();
			Program.Instance.SetDefaultDeviceStates();
			Program.Instance.UpdateViewProjectionMatrix();
		}
	}
}
