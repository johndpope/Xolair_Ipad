/* 
*   NatRender
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatRenderU.Dispatch {

    using UnityEngine;
    using System;
    using System.Collections;
    
    [AddComponentMenu("")]
    public sealed class DispatchUtility : MonoBehaviour {

        #region --Op vars--
        public static event Action onFrame, onQuit;
        public static event Action<bool> onPause;
        private static readonly DispatchUtility instance;
        #endregion        


        #region --Operations--

        static DispatchUtility () {
            instance = new GameObject("NatRender Dispatch Utility").AddComponent<DispatchUtility>();
            instance.StartCoroutine(instance.OnFrame());
        }

        void Awake () {
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(this);
        }
        
        void OnApplicationPause (bool paused) {
            if (onPause != null) onPause(paused);
        }
        
        void OnApplicationQuit () {
            if (onQuit != null) onQuit();
        }

        IEnumerator OnFrame () {
            YieldInstruction yielder = new WaitForEndOfFrame();
            for (;;) {
                yield return yielder;
                if (onFrame != null) onFrame();
            }
        }
        #endregion
    }
}