using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Totoki.Util;

namespace Totoki.ETL.GameData {
	class TestScene : GameScene {
		public TestScene() {
			this.Enabled = true;
			this.Visible = true;
		}

		int count;
		public override void Update(GameTime gameTime) {
			count++;
#if true
				var b = new Bullet(count % 2);
				b.X = 0;
				b.Y = 0;
				b.Direction = count / 8f + MathUtil.Pi * count % 2;
				b.Speed = (2 - (count % 2)) / 64f;
				AddObject(b);
#elif false
			{
				var b = new Bullet(count % 2);
				b.X = 0;
				b.Y = 10;
				b.Direction = count / 8f + MathUtil.PiOverFour * count;
				b.Speed = (2 - (count % 2)) / 64f;
				AddObject(b);
			}
#elif true
			if (count % 8 == 0) {
				var b = new Bullet(0);
				b.X = 0;
				b.Y = 10;
				b.Direction = 3 * MathUtil.PiOverTwo;
				b.Speed = 0.25f;
				AddObject(b);
			}
#endif
		}

		public override void Draw(GameTime gameTime) {
			
		}

		public override void OnMessage(object sender, Message.MessageType msg, object[] args) {
			if (msg == Message.MessageType.OnAdded) {
				Program.Instance.Services.GetService<Ship>().Activate();
			}
			base.OnMessage(sender, msg, args);
		}
	}
}
