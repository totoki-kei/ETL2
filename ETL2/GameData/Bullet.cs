using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Totoki.Util;

namespace Totoki.ETL.GameData {
	class Bullet : GameObject {
		const int MaxBulletCount = 2048;

		const int BulletPriority = 0x10000000;
		const int BulletDrawerPriority = BulletPriority + 0xFFFF;

		public float X { get; set; }
		public float Y { get; set; }
		public float Direction {
			get { return dir; }
			set {
				dir = value;
				dircos = (float)Math.Cos(dir);
				dirsin = (float)Math.Sin(dir);
			}
		}
		public float Speed { get; set; }

		float dir;
		float dircos;
		float dirsin;

		public int Count { get; private set; }

		class BulletHitArea : HitArea {
			const float BulletSize = 0.125f;

			public BulletHitArea(Bullet p) {
				this.Parent = p;
				this.SelfGroup = Group.Bullet;
				this.TargetGroup = Group.Player;
			}

			public override float X {
				get { return ((Bullet)Parent).X; }
			}

			public override float Y {
				get { return ((Bullet)Parent).Y; }
			}

			public override float Size {
				get { return BulletSize; }
			}
		}

		static int gid = 0;
		int id;
		public Bullet(int type) : base(BulletPriority + type) {
			id = gid++;

			if (bulletDrawer == null) {
				bulletDrawer = new Drawer();
				AddObject(bulletDrawer);
			}

			//GameObject.AddObject(this);
			Enabled = true;
			Visible = true;

			HitArea.AddHitArea(new BulletHitArea(this));
		}

		public override void Update(GameTime gameTime) {
			this.X += dircos * Speed;
			this.Y += dirsin * Speed;
			if (X < -Program.GameFieldSize || Program.GameFieldSize < X || Y < -Program.GameFieldSize || Program.GameFieldSize < Y)
			//if (X < -Program.GameFieldSize / 2 || Program.GameFieldSize / 2 < X || Y < -Program.GameFieldSize / 2 || Program.GameFieldSize / 2 < Y)
				Message.EnqueueMessage(new Message(this, this, Message.MessageType.Kill, null));
			Count++;
		}

		public override void Draw(GameTime gameTime) {
			// 内部行列の更新のみ
			Matrix.CreateTranslation(this.X, 0, this.Y, out matrix);
			switch (this.Priority & 0xffff){
				case 0:
					matrix = GetRotateMatrix(Count / 6f, 0, 0, 1) * Matrix.CreateRotationY(-dir - MathUtil.PiOverTwo) * matrix;
					break;
				case 1:
					matrix = GetRotateMatrix(Count / 16f) * matrix;
					break;
			}
		}

		Matrix matrix;

		public override void OnMessage(object sender, Message.MessageType msg, object[] args) {
			if (msg == Message.MessageType.Collide) {
				Message.EnqueueMessage(new Message(this, this, Message.MessageType.Kill, null));
			}
			else if (msg == Message.MessageType.Kill){
				HitArea.RemoveHitArea(this);
				RemoveObject(this);
			}
			base.OnMessage(sender, msg, args);
		}


		#region drawer
		public class Drawer : GameObject {
			Model[] models;

			public Drawer()
				: base(BulletDrawerPriority) {
				Enabled = false; // 表示専用オブジェクト
				Visible = true;
				if (models == null) {
					models = new Model[2] {
						Program.Instance.Content.Load<Model>("b1"),
						Program.Instance.Content.Load<Model>("b2"),
					};
				}

			}

			public override void Update(GameTime gameTime) {
				throw new NotImplementedException();
			}
#if true // パフォーマンス改善版(定数バッファの切り替え回数などを減らしたつもり)
			public override void Draw(GameTime gameTime) {
				var e = Program.Instance.Effect;
				var graphicsDevice = Program.Instance.GraphicsDevice;
				int n = 0;
				for (int i = 0; i < models.Length; i++) {
					var bullets = GetObjects(BulletPriority + i);
					if (bullets == null) continue;

					models[i].ForEachMeshPart(meshpart => {
						((BasicEffect)meshpart.Effect).View = e.View;
						((BasicEffect)meshpart.Effect).Projection = e.Projection;

						foreach (var o in bullets) {
							var b = o as Bullet;
							((BasicEffect)meshpart.Effect).World = Program.Instance.GlobalWorldOffset * b.matrix;
							meshpart.Effect.CurrentTechnique.Passes[0].Apply();
							graphicsDevice.SetVertexBuffer(meshpart.VertexBuffer, meshpart.VertexOffset);
							graphicsDevice.Indices = meshpart.IndexBuffer;
							graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, meshpart.StartIndex, meshpart.PrimitiveCount);
							n++;
						}
					});
				}
				Program.AddStatistics("Bullet", "{0}", n);
			}
#else
			public override void Draw(GameTime gameTime) {
				var e = Program.Instance.Effect;
				int n = 0;
				for (int i = 0; i < models.Length; i++) {
					var bullets = GetObjects(BulletPriority + i);
					if (bullets == null) continue;

					foreach (var o in bullets) {
						var b = o as Bullet;
						models[i].Draw(Program.Instance.GraphicsDevice, b.matrix, e.View, e.Projection);
						n++;
					}
				}
				Program.AddStatistics("Bullet", "{0}", n);
			}
#endif
		}
		static Drawer bulletDrawer;
		#endregion

		public override string ToString() {
			return base.ToString() + string.Format("(id = {0}", id);
		}
	}
}
