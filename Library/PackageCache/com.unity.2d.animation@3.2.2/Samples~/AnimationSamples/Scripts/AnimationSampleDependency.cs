using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.U2D.Animation.Sample.Dependency
{
    [ExecuteInEditMode]
    internal class AnimationSampleDependency : MonoBehaviour
    {
        public UnityEngine.UI.Text textField;
        public GameObject gameCanvas;

        void Update()
        {
#if PSDIMPORTER_ENABLED
            textField.enabled = false;
            if(gameCanvas != null)
                gameCanvas.SetActive(true);
#else
            textField.enabled = true;
            if(gameCanvas != null)
                gameCanvas.SetActive(false);
#endif

        }
    }
}

