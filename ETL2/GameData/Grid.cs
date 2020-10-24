using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Totoki.ETL.GameData {
	class Grid : GameObject{
		//Buffer<VertexPositionColor> vb;
		//VertexInputLayout vil;
		VertexBuffer vb;

		const int Density = 32;
		const float Ext = Program.GameFieldSize;

		public Grid() : base(0x7fffffff) {
			this.Enabled = false;
			this.Visible = true;

			Color gridColor1 = new Color(1f, 1f, 0f);
			Color gridColor2 = new Color(1f, 0f, 1f);
			List<VertexPositionColor> vs = new List<VertexPositionColor>();
			for (int i = 0; i <= Density; i++) {
				float offset = i / (float)Density - 0.5f;

				vs.Add(new VertexPositionColor() {
					Position = new Vector3(offset, 0, 0.5f) * Ext,
					Color = gridColor1,
				});
				vs.Add(new VertexPositionColor() {
					Position = new Vector3(offset, 0, -0.5f) * Ext,
					Color = gridColor2,
				});
				vs.Add(new VertexPositionColor() {
					Position = new Vector3(0.5f, 0, offset) * Ext,
					Color = gridColor1,
				});
				vs.Add(new VertexPositionColor() {
					Position = new Vector3(-0.5f, 0, offset) * Ext,
					Color = gridColor2,
				});
			}
			//vb = TKBuffer.Vertex.New(Program.Instance.GraphicsDevice, vs.ToArray());
			vb = new VertexBuffer(Program.Instance.GraphicsDevice, typeof(VertexPositionColor), vs.Count, BufferUsage.None);
			vb.SetData(vs.ToArray());
			//vil = VertexInputLayout.FromBuffer(0, vb);

		}

		public override void Update(GameTime gameTime) {
//			throw new NotImplementedException();
		}

		public override void Draw(GameTime gameTime) {
			var device = Program.Instance.GraphicsDevice;

			device.SetVertexBuffer(vb);
			//device.SetVertexInputLayout(vil);

			Program.Instance.Effect.World = Matrix.Identity;
			Program.Instance.Effect.CurrentTechnique.Passes[0].Apply();

			device.DrawPrimitives(PrimitiveType.LineList, 0, vb.VertexCount);
		}
	}
}
