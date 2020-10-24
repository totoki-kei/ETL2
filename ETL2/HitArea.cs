using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totoki.ETL {
	public abstract class HitArea {
		[Flags]
		public enum Group {
			None = 0,
			Bullet = 0x01,
			Enemy = 0x02,
			Player = 0x04,
			PlayerShot = 0x08,
		}

		public GameObject Parent { get; protected set; }
		public Group SelfGroup { get; protected set; }
		public Group TargetGroup { get; protected set; }
		public abstract float X { get; }
		public abstract float Y { get; }
		public abstract float Size { get; }

		#region static
		static Dictionary<Group, HashSet<HitArea>> hitAreaDic = new Dictionary<Group, HashSet<HitArea>>();
		public static void AddHitArea(HitArea a) {
			Group g = a.SelfGroup;
			HashSet<HitArea> ha;
			if (hitAreaDic.TryGetValue(g, out ha)) {
				ha.Add(a);
			}
			else {
				ha = new HashSet<HitArea>();
				ha.Add(a);
				hitAreaDic.Add(g, ha);
			}
		}
		public static void RemoveHitArea(HitArea a) {
			Group g = a.SelfGroup;
			HashSet<HitArea> ha;
			if (hitAreaDic.TryGetValue(g, out ha)) lock(ha) {
				ha.Remove(a);
			}
		}
		public static void RemoveHitArea(GameObject obj) {
			foreach (var s in hitAreaDic) lock(s.Value) {
				s.Value.RemoveWhere(h => h.Parent == obj);
			}
		}
		public static void CheckHitArea() {
			int count = 0;
			int tests = 0;
			foreach (var s in hitAreaDic) lock(s.Value) {
				//s.Value.ForEach(
				Parallel.ForEach(s.Value,
					x => {
						Group target = x.TargetGroup;
						if (!x.Parent.Enabled) return;
						if (target == Group.None) return;
						count++;
						for (int i = 1; i > 0; i <<= 1) {
							Group g = (Group)i;
							if ((target & g) == Group.None) continue;
							if (!hitAreaDic.ContainsKey(g)) continue;
							hitAreaDic[g].ForEach(
								//Parallel.ForEach(hitAreaDic[g], 
								y => {
									if (!y.Parent.Enabled) return;
									tests++;
									if (Intersects(x, y)) {
										Message.EnqueueMessage(new Message(x.Parent, y.Parent, Message.MessageType.Collide, y));
										Message.EnqueueMessage(new Message(y.Parent, x.Parent, Message.MessageType.Collide, x));
									}
								}
								);
						}
					}
					);
			}
			Program.AddStatistics("CheckHitArea", "count = {0}, tests = {1}", count, tests);
		}

		private static bool Intersects(HitArea x, HitArea y) {
			return (x.X - y.X) * (x.X - y.X) + (x.Y - y.Y) * (x.Y - y.Y) <= (x.Size + y.Size);
		}
		#endregion
	}
}
