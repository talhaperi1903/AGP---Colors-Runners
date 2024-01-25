using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HyperCasual.Runner
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance => s_Instance;
        private static GameManager s_Instance;

        [SerializeField]
        private AbstractGameEvent m_WinEvent;

        [SerializeField]
        private AbstractGameEvent m_LoseEvent;

        private LevelDefinition m_CurrentLevel;
        public bool IsPlaying => m_IsPlaying;
        private bool m_IsPlaying;
        private GameObject m_CurrentLevelGO;
        private GameObject m_CurrentTerrainGO;
        private GameObject m_LevelMarkersGO;

        // Dependencies injected through properties or constructor
        public LevelManager LevelManager { get; set; }
        public PlayerController PlayerController { get; set; }
        public CameraManager CameraManager { get; set; }

        private List<Spawnable> m_ActiveSpawnables = new List<Spawnable>();

#if UNITY_EDITOR
        private bool m_LevelEditorMode;
#endif

        void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;

#if UNITY_EDITOR
            if (LevelManager != null)
            {
                StartGame();
                m_LevelEditorMode = true;
            }
#endif
        }

        public void LoadLevel(LevelDefinition levelDefinition, ref GameObject levelGameObject)

        {
            m_CurrentLevel = levelDefinition;
            LevelManager?.LoadLevel(levelDefinition, ref m_CurrentLevelGO);
            CreateTerrain(m_CurrentLevel, ref m_CurrentTerrainGO);
            PlaceLevelMarkers(m_CurrentLevel, ref m_LevelMarkersGO);
            StartGame();
        }

        public void ResetLevel()
        {
            PlayerController?.ResetPlayer();
            CameraManager?.ResetCamera();
            LevelManager?.ResetSpawnables();
        }

        public void UnloadCurrentLevel()
        {
            if (m_CurrentLevelGO != null)
            {
                GameObject.Destroy(m_CurrentLevelGO);
            }

            if (m_LevelMarkersGO != null)
            {
                GameObject.Destroy(m_LevelMarkersGO);
            }

            if (m_CurrentTerrainGO != null)
            {
                GameObject.Destroy(m_CurrentTerrainGO);
            }

            m_CurrentLevel = null;
        }

        private void StartGame()
        {
            ResetLevel();
            m_IsPlaying = true;
        }


        /// <summary>
        /// Creates and instantiates the StartPrefab and EndPrefab defined inside
        /// the levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the start and end prefabs.
        /// </param>
        /// <param name="levelMarkersGameObject">
        /// A new GameObject that is created to be the parent of the start and end prefabs.
        /// </param>
        public static void PlaceLevelMarkers(LevelDefinition levelDefinition, ref GameObject levelMarkersGameObject)
        {
            if (levelMarkersGameObject != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(levelMarkersGameObject);
                }
                else
                {
                    DestroyImmediate(levelMarkersGameObject);
                }
            }

            levelMarkersGameObject = new GameObject("Level Markers");

            GameObject start = levelDefinition.StartPrefab;
            GameObject end = levelDefinition.EndPrefab;

            if (start != null)
            {
                GameObject go = GameObject.Instantiate(start, new Vector3(start.transform.position.x, start.transform.position.y, 0.0f), Quaternion.identity);
                go.transform.SetParent(levelMarkersGameObject.transform);
            }

            if (end != null)
            {
                GameObject go = GameObject.Instantiate(end, new Vector3(end.transform.position.x, end.transform.position.y, levelDefinition.LevelLength), Quaternion.identity);
                go.transform.SetParent(levelMarkersGameObject.transform);
            }
        }

        /// <summary>
        /// Creates and instantiates a Terrain GameObject, built
        /// to the specifications saved in levelDefinition.
        /// </summary>
        /// <param name="levelDefinition">
        /// A LevelDefinition ScriptableObject that defines the terrain size.
        /// </param>
        /// <param name="terrainGameObject">
        /// A new GameObject that is created to hold the terrain.
        /// </param>
        public static void CreateTerrain(LevelDefinition levelDefinition, ref GameObject terrainGameObject)
        {
            TerrainGenerator.TerrainDimensions terrainDimensions = new TerrainGenerator.TerrainDimensions()
            {
                Width = levelDefinition.LevelWidth,
                Length = levelDefinition.LevelLength,
                StartBuffer = levelDefinition.LevelLengthBufferStart,
                EndBuffer = levelDefinition.LevelLengthBufferEnd,
                Thickness = levelDefinition.LevelThickness
            };
            TerrainGenerator.CreateTerrain(terrainDimensions, levelDefinition.TerrainMaterial, ref terrainGameObject);
        }

        public void Win()
        {
            m_WinEvent.Raise();
            HandleEditorMode();
        }

        public void Lose()
        {
            m_LoseEvent.Raise();
            HandleEditorMode();
        }

#if UNITY_EDITOR
        private void HandleEditorMode()
        {
            if (m_LevelEditorMode)
            {
                ResetLevel();
            }
        }
#endif
    }
}