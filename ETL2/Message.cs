using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Totoki.ETL {
	public class Message {
		public GameObject Sender { get; private set; }
		public GameObject Target { get; private set; }
		public MessageType Code { get; private set; }
		public object[] Args { get; private set; }

		public Message(GameObject sender, GameObject target, MessageType code, params object[] args) {
			Sender = sender;
			Target = target;
			Code = code;
			Args = args;
		}

		public void Post() {
			Target.OnMessage(Sender, Code, Args);
		}

		public enum MessageType {
			Null = 0,
			Collide,
			Kill,
		}

		#region static
		static ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
		public static void EnqueueMessage(Message message) {
			messageQueue.Enqueue(message);
		}

		public static void PostAll() {
			Program.AddStatistics("MessageCount", "{0}", messageQueue.Count);
			while (messageQueue.Count > 0) {
				Message m;
				if (messageQueue.TryDequeue(out m)) {
					m.Post();
				}
			}
		}
		#endregion

	}

}
