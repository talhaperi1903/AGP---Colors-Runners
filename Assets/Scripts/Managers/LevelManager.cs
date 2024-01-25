using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    [ExecuteInEditMode]
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private LevelDefinition m_LevelDefinition;

        private List<Spawnable> m_ActiveSpawnables = new List<Spawnable>();

        public static LevelManager Instance => s_Instance;
        static LevelManager s_Instance;

        public PlayerController PlayerController { get; set; }

        public LevelDefinition LevelDefinition
        {
            get => m_LevelDefinition;
            set
            {
                m_LevelDefinition = value;
                if (m_LevelDefinition != null && PlayerController != null)
                {
                    PlayerController.SetMaxXPosition(m_LevelDefinition.LevelWidth);
                }
            }
        }

        public void AddSpawnable(Spawnable spawnable)
        {
            if (spawnable != null)
            {
                m_ActiveSpawnables.Add(spawnable);
            }
        }

        public void ResetSpawnables()
        {
            foreach (var spawnable in m_ActiveSpawnables)
            {
                spawnable.ResetSpawnable();
            }
        }

        void Awake()
        {
            SetupInstance();
        }

        void OnEnable()
        {
            SetupInstance();
        }
        public void LoadLevel(LevelDefinition levelDefinition, ref GameObject levelGameObject)
        {
            // Set the current level definition
            LevelDefinition = levelDefinition;

            // If a level game object already exists, destroy it
            if (levelGameObject != null)
            {
                Destroy(levelGameObject);
            }

            // Create a new level game object
            levelGameObject = new GameObject("Level");

        }

        void SetupInstance()
        {
            if (s_Instance != null && s_Instance != this)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                return;
            }

            s_Instance = this;
        }
    }
}
