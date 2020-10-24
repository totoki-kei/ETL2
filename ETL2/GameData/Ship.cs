using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Totoki.ETL.GameData {
	class Ship : GameObject {
		Model model;

		public Ship() : base(0x20000000) {
			//model = Program.Instance.Content.Load<Model>("block");
			model = Program.Instance.Content.Load<Model>("paper");

			//GameObject.AddObject(this);
			this.Enabled = true;
			this.Visible = true;

			HitArea.AddHitArea(new ShipHitArea(this));
		}

		float x = 0f;
		float y = 0f;

		const float MaxRoll = 1 / 4f;
		const float Speed     = 8f / 16f;
		const float SlowSpeed = 3f / 16f;

		float roll = 0f;

		public override void Update(GameTime gameTime) {
			float vx = 0, vy = 0;
			//vx -= Input.IsPushed(InputUnit.Left) ? 1 : 0;
			//vx += Input.IsPushed(InputUnit.Right) ? 1 : 0;
			//vy += Input.IsPushed(InputUnit.Up) ? 1 : 0;
			//vy -= Input.IsPushed(InputUnit.Down) ? 1 : 0;
			vx = Input.GetLeftRightAxisValue();
			vy = Input.GetUpDownAxisValue();

			if (Input.IsPushed(InputUnit.Subaction)) {
				x += vx * SlowSpeed;
				y += vy * SlowSpeed;
			}
			else {
				x += vx * Speed;
				y += vy * Speed;
			}

			if (x < -Program.GameFieldSize / 2) x = -Program.GameFieldSize / 2;
			if (x > Program.GameFieldSize / 2) x = Program.GameFieldSize / 2;
			if (y < -Program.GameFieldSize / 2) y = -Program.GameFieldSize / 2;
			if (y > Program.GameFieldSize / 2) y = Program.GameFieldSize / 2;

			float targetRoll = -vx * MaxRoll;
			roll = (2 * roll + targetRoll) / 3;

			worldMatrix = Matrix.CreateRotationZ((float)Math.PI * roll) * Matrix.CreateTranslation(x, 0, y);			
		}

		Matrix worldMatrix;
		Matrix coreRotateMatrix;

		int time = 0;
		public override void Draw(GameTime gameTime) {
			var be = Program.Instance.Effect;
			coreRotateMatrix = GetRotateMatrix(time / 10f);
			time++;
			model.ForEachMeshPart((part, meshIndex, partIndex) => {
				BasicEffect e = (BasicEffect)part.Effect;

				if (meshIndex == 0 && partIndex >= 2) {
					// コア部分：手前に表示
					Program.Instance.DepthEnabled = false;
					e.World = coreRotateMatrix * this.worldMatrix;
				}
				else {
					Program.Instance.DepthEnabled = true;
					e.World = this.worldMatrix;
				}
				e.World = Program.Instance.GlobalWorldOffset * e.World;
				e.View = be.View;
				e.Projection = be.Projection;
				e.EnableDefaultLighting();
				e.CurrentTechnique.Passes[0].Apply();

				var graphicsDevice = Program.Instance.GraphicsDevice;
				graphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
				graphicsDevice.Indices = part.IndexBuffer;
				graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, part.StartIndex, part.PrimitiveCount);
			});
			
			// motoni modosu
			Program.Instance.DepthEnabled = true;
		}

		class ShipHitArea : HitArea {
			const float ShipSize = 0.1f;

			public ShipHitArea(Ship s) {
				this.Parent = s;
				this.SelfGroup = Group.Player;
				this.TargetGroup = Group.None;
			}

			public override float X {
				get { return ((Ship)Parent).x; }
			}

			public override float Y {
				get { return ((Ship)Parent).y; }
			}

			public override float Size {
				get { return ShipSize; }
			}
		}


	}
}
