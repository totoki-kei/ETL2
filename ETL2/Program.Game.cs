using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Totoki.ETL {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public partial class Program : Game {
		GraphicsDeviceManager deviceManager;
		private VertexBuffer vertices;
		private IndexBuffer indices;

		public Program() {
			EventHandler<EventArgs> dbg(string msg) {
				return new EventHandler<EventArgs>((o, e) => Debug.WriteLine("{0} : {1}", DateTime.Now, msg));
			}

			deviceManager = new GraphicsDeviceManager(this);
			deviceManager.DeviceCreated += dbg("DeviceCreated");
			deviceManager.DeviceDisposing += dbg("DeviceDisposing");
			deviceManager.DeviceReset += dbg("DeviceReset");
			deviceManager.DeviceResetting += dbg("DeviceResetting");

			IsFixedTimeStep = true;
			Content.RootDirectory = "Content";
			TargetElapsedTime = new TimeSpan(TimeSpan.TicksPerSecond / UpdatePerSeconds);
		}

		protected override void Initialize() {
			// WindowはInitializeで参照：コンストラクタではまだ作成されていないので
			Window.Title = "Hoge.";
			Window.AllowUserResizing = !Configuration.FullScreen;

			deviceManager.PreferredBackBufferWidth = Configuration.DefaultWidth;
			deviceManager.PreferredBackBufferHeight = Configuration.DefaultHeight;
			deviceManager.IsFullScreen = Configuration.FullScreen;
			deviceManager.SynchronizeWithVerticalRetrace = Configuration.Vsync;
			deviceManager.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent() {
			Effect = ToDisposeContent(new BasicEffect(GraphicsDevice) {
				VertexColorEnabled = true,
				View = Matrix.CreateLookAt(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.UnitY),
				Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height, 0.1f, 100.0f),
				World = Matrix.Identity
			});

			// Creates vertices for the cube
			InitializeCube();

			InitializeDepthStencil();

			InitializeRasterizer();

			InitializeBlendState();

			InitializeStatistics();

			Input.Initialize();

			ship = new GameData.Ship();
			GameObject.AddObject(ship);
			grid = new GameData.Grid();
			GameObject.AddObject(grid);

			var scene = new GameData.TestScene();
			GameObject.AddObject(scene);

			base.LoadContent();
		}

		DepthStencilState dss;
		DepthStencilState dss_depth;
		DepthStencilState dss_nodepth;
		private void InitializeDepthStencil() {
			dss = DepthStencilState.Default;
			dss_depth = dss;
			dss_nodepth = DepthStencilState.None;
		}

		bool depthEnabled;
		public bool DepthEnabled {
			get { return depthEnabled; }
			set {
				depthEnabled = value;
				dss = (value ? dss_depth : dss_nodepth);
				GraphicsDevice.DepthStencilState = dss;
			}
		}

		BlendState bs;
		private void InitializeBlendState() {
			bs = BlendState.AlphaBlend;
		}

		RasterizerState rs;
		private void InitializeRasterizer() {
			rs = RasterizerState.CullCounterClockwise;
		}


		private void InitializeCube() {
			var verticesArray = new[]{
				new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), new Color(0x800000FF)), // Front
				new VertexPositionColor(new Vector3(-1.0f,  1.0f, -1.0f), new Color(0x800000FF)),
				new VertexPositionColor(new Vector3( 1.0f, -1.0f, -1.0f), new Color(0x800000FF)),
				new VertexPositionColor(new Vector3( 1.0f,  1.0f, -1.0f), new Color(0x800000FF)),

				new VertexPositionColor(new Vector3(-1.0f, -1.0f,  1.0f), new Color(0x80FFFF00)), // BACK
				new VertexPositionColor(new Vector3( 1.0f, -1.0f,  1.0f), new Color(0x80FFFF00)),
				new VertexPositionColor(new Vector3(-1.0f,  1.0f,  1.0f), new Color(0x80FFFF00)),
				new VertexPositionColor(new Vector3( 1.0f,  1.0f,  1.0f), new Color(0x80FFFF00)),

				new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), new Color(0x8000FF00)), // Bottom
				new VertexPositionColor(new Vector3( 1.0f, -1.0f, -1.0f), new Color(0x8000FF00)),
				new VertexPositionColor(new Vector3(-1.0f, -1.0f,  1.0f), new Color(0x8000FF00)),
				new VertexPositionColor(new Vector3( 1.0f, -1.0f,  1.0f), new Color(0x8000FF00)),

				new VertexPositionColor(new Vector3(-1.0f,  1.0f, -1.0f), new Color(0x80FF00FF)), // Top
				new VertexPositionColor(new Vector3(-1.0f,  1.0f,  1.0f), new Color(0x80FF00FF)),
				new VertexPositionColor(new Vector3( 1.0f,  1.0f, -1.0f), new Color(0x80FF00FF)),
				new VertexPositionColor(new Vector3( 1.0f,  1.0f,  1.0f), new Color(0x80FF00FF)),

				new VertexPositionColor(new Vector3(-1.0f, -1.0f, -1.0f), new Color(0x80FF0000)), // Left
				new VertexPositionColor(new Vector3(-1.0f, -1.0f,  1.0f), new Color(0x80FF0000)),
				new VertexPositionColor(new Vector3(-1.0f,  1.0f, -1.0f), new Color(0x80FF0000)),
				new VertexPositionColor(new Vector3(-1.0f,  1.0f,  1.0f), new Color(0x80FF0000)),

				new VertexPositionColor(new Vector3( 1.0f, -1.0f, -1.0f), new Color(0x8000FFFF)), // Right
				new VertexPositionColor(new Vector3( 1.0f,  1.0f, -1.0f), new Color(0x8000FFFF)),
				new VertexPositionColor(new Vector3( 1.0f, -1.0f,  1.0f), new Color(0x8000FFFF)),
				new VertexPositionColor(new Vector3( 1.0f,  1.0f,  1.0f), new Color(0x8000FFFF)),
			};
			vertices = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), verticesArray.Length, BufferUsage.None);
			vertices.SetData(verticesArray);

			var indicesArray = new uint[] {
				 0, 2, 1, 1, 2, 3,
				 4, 6, 5, 5, 6, 7,
				 8,10, 9, 9,10,11,
				12,14,13,13,14,15,
				16,18,17,17,18,19,
				20,22,21,21,22,23,
				 0, 1, 2, 1, 3, 2,
				 4, 5, 6, 5, 7, 6,
				 8, 9,10, 9,11,10,
				12,13,14,13,15,14,
				16,17,18,17,19,18,
				20,21,22,21,23,22,
			};

			indices = new IndexBuffer(GraphicsDevice, IndexElementSize.ThirtyTwoBits, indicesArray.Length, BufferUsage.None);
			indices.SetData(indicesArray);

			ToDisposeContent(vertices);
			ToDisposeContent(indices);
			// Create an input layout from the vertices
			//inputLayout = VertexInputLayout.FromBuffer(0, vertices);
			

			fps = new FPS();


		}

		private GameData.Ship ship;
		private GameData.Grid grid;


		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		FPS fps;
		int updateCount = 0;
		//bool frameSkip = false;

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			// この辺のフレームスキップの作成意図がよく分からないので一旦コメントアウト
			// 戻す場合frameSkipのコメントアウトも戻す
			//if (updateCount++ > 0 && frameSkip) return;
			//if (updateCount++ > 0) return;
			updateCount++;

			//Debug.WriteLine("======== Update Start ========" + string.Format("(ticks = {0})", gameTime.ElapsedGameTime.Ticks));
			Input.UpdateKeyboard();
			Input.UpdateJoystick();
			HitArea.CheckHitArea();
			Message.PostAll();
			GameObject.UpdateAll(gameTime);

			//// Rotate the cube.
			//var time = (float)gameTime.TotalGameTime.TotalSeconds;
			var deg = Matrix.CreateRotationX(GameFieldDegree);
			Effect.View = Matrix.CreateLookAt(
				Vector3.Transform(Vector3.UnitY, deg) * GameFieldSize,
				-Vector3.UnitY * (GameFieldSize / 4),
				Vector3.Transform(Vector3.UnitZ, deg));
			Effect.Projection = Matrix.CreatePerspective(GameFieldSize / 2 * (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height, GameFieldSize / 2, GameFieldSize / 2, 3 * GameFieldSize / 2);

			GlobalWorldOffset = Matrix.CreateScale(-1, 1, -1);

			//Debug.WriteLine("========= Update OK  =========");
			base.Update(gameTime);
			//Debug.WriteLine("========= Update End =========");
		}

		protected override void Draw(GameTime gameTime) {
			AddStatistics("FPS", "{0:#.00}", fps.Update());
			AddStatistics("Updates", "{0}", updateCount);
			updateCount = 0;
			//Debug.WriteLine("======== Draw Start ========");
			GraphicsDevice.Clear(new Color(0, 0, 32));

			GraphicsDevice.DepthStencilState = dss;
			GraphicsDevice.RasterizerState = rs;
			GraphicsDevice.BlendState = bs;

			GameObject.DrawAll(gameTime);
			//Debug.WriteLine("========= Draw OK  =========");

			PrintStatistics();

			base.Draw(gameTime);

			DrawTestCube(gameTime);
			//Debug.WriteLine("========= Draw End =========");
		}

		private void DrawTestCube(GameTime gameTime) {
			GraphicsDevice.SetVertexBuffer(vertices);
			//GraphicsDevice.SetIndexBuffer(indices, true);
			GraphicsDevice.Indices = indices;
			//GraphicsDevice.SetVertexInputLayout(inputLayout);

			Effect.World = Matrix.CreateRotationZ((float)(gameTime.TotalGameTime.TotalSeconds * MathHelper.Pi / 60.0));

			// Apply the basic effect technique and draw the rotating cube
			Effect.Alpha = 0.25f;
			Effect.CurrentTechnique.Passes[0].Apply();
			//GraphicsDevice.Draw(PrimitiveType.TriangleList, vertices.ElementCount);
			GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indices.IndexCount);
		}

		public BasicEffect Effect { get; private set; }
		public Matrix GlobalWorldOffset { get; private set; }

		// ダミー
		private T ToDisposeContent<T>(T t) { return t; }
	}
}
