using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;

namespace Totoki.ETL {
	public abstract class GameObject {
		public int Priority { get; private set; }
		public abstract void Update(GameTime gameTime);
		public abstract void Draw(GameTime gameTime);

		public virtual void OnMessage(object sender, Message.MessageType msg, params object[] args) {
			if (msg == Message.MessageType.Kill) {
				this.Enabled = false;
				this.Visible = false;
				RemoveObject(this);
			}
		}
		public virtual bool Enabled { get; protected set; }
		public virtual bool Visible { get; protected set; }

		public GameObject(int priority) {
			this.Priority = priority;
		}

		#region static
		static readonly SortedList<int, HashSet<GameObject>> objectList = new SortedList<int, HashSet<GameObject>>();
		static readonly SortedList<int, HashSet<GameObject>> preObjectList = new SortedList<int, HashSet<GameObject>>();
		public static void AddObject(GameObject obj) {
			int p = obj.Priority;
			if (!preObjectList.TryGetValue(p, out HashSet<GameObject> set)) {
				set = new HashSet<GameObject>();
				preObjectList.Add(p, set);
			}
			set.Add(obj);
			obj.OnMessage(null, Message.MessageType.OnAdded);
		}
		public static void RemoveObject(GameObject obj) {
			int p = obj.Priority;
			HashSet<GameObject> set;
			bool result = false;
			if (objectList.TryGetValue(p, out set)) {
				result = set.Remove(obj);
			}
			if (!result && preObjectList.TryGetValue(p, out set)) {
				result = set.Remove(obj);
			}

			if (result) {
				obj.OnMessage(null, Message.MessageType.OnRemoved);
			}
		}
			
		public static void UpdateAll (GameTime gameTime){
			foreach (var prevObjSet in preObjectList) {
				if (prevObjSet.Value.Count == 0) continue;

				if (objectList.ContainsKey(prevObjSet.Key)) {
					// マージ
					objectList[prevObjSet.Key].UnionWith(prevObjSet.Value);
				}
				else {
					// 追加(元のHashSetはクリアするため新規HashSetインスタンスを作る)
					objectList.Add(prevObjSet.Key, new HashSet<GameObject>(prevObjSet.Value));
				}
				prevObjSet.Value.Clear();
				Debug.Assert(prevObjSet.Value.Count == 0);
			}

			foreach (var objSet in objectList) {
				foreach(var obj in objSet.Value){
					if (obj.Enabled) {
						obj.Update(gameTime);
					}
				}
			}
		}
		public static void DrawAll(GameTime gameTime) {
			foreach (var objSet in objectList) {
				foreach (var obj in objSet.Value) {
					if (obj.Visible) {
						obj.Draw(gameTime);
					}
				}
			}
		}
		public static IEnumerable<GameObject> GetObjects(int id) {
			HashSet<GameObject> set;
			if (!objectList.TryGetValue(id, out set)) {
				// もとから登録されていない
				return null;
			}
			return set;
		}

		// helper

		public static IEnumerable<GameObject> GetScene() => GetObjects(GameObject.Priorities.Scene);


		/// <summary>
		/// ぼんやりと回転する動作に使う回転行列を得る
		/// </summary>
		/// <param name="time">秒</param>
		/// <returns></returns>
		public static Matrix GetRotateMatrix(float time) {
			return GetRotateMatrix(time, 1, 2, 0.7f);
		}
		public static Matrix GetRotateMatrix(float time, float x, float y, float z) {
			return Matrix.CreateRotationX(time * x) * Matrix.CreateRotationY(time * y) * Matrix.CreateRotationZ(time * z);
		}

		// predefined priority
		public static class Priorities {
			public const int Scene = 0x00000000;
		}

		#endregion
	}
}
