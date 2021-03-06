﻿/* 
*   NatRender
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatRenderU.Dispatch {

	using System;
	using System.Threading;
	using Queue = System.Collections.Generic.List<System.Action>;
	
	public class ConcurrentDispatch : MainDispatch {

		#region --Op vars--
		private bool running = true;
		private readonly Thread thread;
		private readonly EventWaitHandle queueSync = new AutoResetEvent(false);
		#endregion


		#region --Dispatcher--

		public ConcurrentDispatch () : base() {
			thread = new Thread(Update);
		}

		public override void Dispatch (Action action) {
			base.Dispatch(action);
			queueSync.Set();
		}

		public override void Dispose () {
			base.Dispose();
			thread.Join();
		}
		#endregion


		#region --Operations--

		protected override void Start () {
			thread.Start();
		}

		protected override void Update () {
			for (;;) {
				queueSync.WaitOne();
				base.Update();
				if (!running)
					break;
			}
		}

		protected override void Stop () {
			running = false;
		}
		#endregion
	}
}

