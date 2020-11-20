namespace Scripts.Buildings
{
    using System.Collections.Generic;
    
    using UnityEngine;
    
    /// <summary>
    /// Defines the <see cref="BuildingManager"/> singleton.
    /// </summary>
    public class BuildingManager : MonoBehaviour
    {
        private static BuildingManager m_Instance;

        /// <summary>
        /// Gets or private sets the Instance of the <see cref="BuildingManager"/> singleton.
        /// </summary>
        public static BuildingManager Instance
        {
            get
            {
                if(m_Instance == null)
                {
                    m_Instance = FindObjectOfType<BuildingManager>();
                    if( m_Instance == null)
                    {
                        GameObject go = new GameObject {name = "BuildingManager"};
                        m_Instance = go.AddComponent<BuildingManager>();
 
                        DontDestroyOnLoad(go);
                    }
                }
                return m_Instance;
            }
            private set => m_Instance = value;
        }
        
        /// <summary>
        /// Gets or sets the Buildings set in the game.
        /// </summary>
        public List<BaseBuilding> Buildings { get; set; } = new List<BaseBuilding>();

        private void Awake()
        {
            if(Instance == null )
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

