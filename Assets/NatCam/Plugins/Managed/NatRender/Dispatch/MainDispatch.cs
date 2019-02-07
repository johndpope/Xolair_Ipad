﻿/* 
*   NatRender
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatRenderU.Dispatch {

	using System;
	using Queue = System.Collections.Generic.List<System.Action>;

	public class MainDispatch : Dispatcher {

		#region --Op vars--
		private readonly Queue queue = new Queue();
		private readonly object queueLock = new object();
		private static readonly Action SEPPUKU = () => {};
		#endregion


		#region --Dispatcher--

		public MainDispatch () {
			Start();
		}

		public override void Dispatch (Action action) {
			lock (queueLock)
				queue.Add(action);
		}

		public override void Dispose () {
			Dispatch(SEPPUKU);
		}
		#endregion


		#region --Operations--

		protected virtual void Start () {
			DispatchUtility.onFrame += Update;
		}

		protected virtual void Update () {
			Action[] pending;
			lock (queueLock) {
				pending = new Action[queue.Count];
				queue.CopyTo(pending);
				queue.Clear();
			}
			for (var i = 0; i < pending.Length; i++) {
				if (pending [i] != SEPPUKU)
					pending[i]();
				else {
					Stop();
					return;
				}
			}
		}

		protected virtual void Stop () {
			DispatchUtility.onFrame -= Update;
		}
		#endregion
	}
}