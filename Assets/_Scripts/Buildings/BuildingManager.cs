namespace Scripts.Buildings
{
    using System.Collections.Generic;
    
    using UnityEngine;
    
    /// <summary>
    /// Defines the <see cref="BuildingManager"/> singleton.
    /// </summary>
    [RequireComponent(typeof(FloorGenerator))]
    public class BuildingManager : MonoBehaviour
    {
        private FloorGenerator m_FloorGenerator;
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

        /// <summary>
        /// Gets or sets a value indicating if Random Upgrades are possible.
        /// </summary>
        public bool RandomUpgradesPossible { get; set; }

        private void Awake()
        {
            m_FloorGenerator = GetComponent<FloorGenerator>();
        }
    }
}

