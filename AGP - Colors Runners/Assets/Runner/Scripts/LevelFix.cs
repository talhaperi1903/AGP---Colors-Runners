
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
            gameManager.LoadLevel(levelDef);
        }

    }
}
#endif