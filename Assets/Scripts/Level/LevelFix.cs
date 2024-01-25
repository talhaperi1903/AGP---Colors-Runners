
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    [DefaultExecutionOrder(500)]
    public class LevelFix : MonoBehaviour
    {
        public LevelDefinition levelDef;
        public GameManager gameManager;

        void Start()
        {
            // Create or reference a GameObject that will hold the loaded level
            GameObject levelContainer = new GameObject("LevelContainer");

            // Pass both the LevelDefinition and the reference to the GameObject to LoadLevel
            gameManager.LoadLevel(levelDef, ref levelContainer);
        }
    }
}
#endif