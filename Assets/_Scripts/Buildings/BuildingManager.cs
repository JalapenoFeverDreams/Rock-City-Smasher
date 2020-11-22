namespace Scripts.Buildings
{
    using System.Collections.Generic;
    using System.Collections;
    using System.Linq;

    using UnityEngine;
    
    /// <summary>
    /// Defines the <see cref="BuildingManager"/> singleton.
    /// </summary>
    [RequireComponent(typeof(FloorGenerator))]
    public class BuildingManager : MonoBehaviour
    {
        [SerializeField] private List<BaseBuilding> m_BuildPrefabs;
        [SerializeField] private float m_InitialTimerInterval = 15f;

        private FloorGenerator m_FloorGenerator;
        private static BuildingManager m_Instance;
        private Coroutine m_RandomUpgradeRoutine;
        private bool m_RandomUpgradesPossible;

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
        public bool RandomUpgradesPossible 
        { 
            get => m_RandomUpgradesPossible; 
            set
            {
                m_RandomUpgradesPossible = value;
                if(value)
                {
                    if(m_RandomUpgradeRoutine == null)
                    {
                        m_RandomUpgradeRoutine = StartCoroutine(StartRandomUpgradeInterval());
                    }
                }
                else
                {
                    if(m_RandomUpgradeRoutine != null)
                    {
                        StopCoroutine(m_RandomUpgradeRoutine);
                    }                
                }
            } 
        }

        /// <summary>
        /// Gets the initial time interval.
        /// </summary>
        public float InitialTimeInterval => m_InitialTimerInterval;

        private void Awake()
        {
            m_FloorGenerator = GetComponent<FloorGenerator>();
            
        }

        private IEnumerator StartRandomUpgradeInterval()
        {
            while(true)
            {
                var marketplaceBuilding = (Buildings.FirstOrDefault(x => x.BuildingType == BuildingType.Marketplace) as MarketplaceBuilding);
                yield return new WaitForSeconds(InitialTimeInterval * Mathf.Pow(marketplaceBuilding.TimeIntervalDecreaseFactor, Buildings.Count(x => x.BuildingType == BuildingType.Marketplace)));

                var rand = Random.Range(1, 100001);
                
                if(rand <= marketplaceBuilding.ChancesOfRandomUpgrade * 1000)
                {
                    var randomBuildingIndex = Random.Range(0, m_BuildPrefabs.Count * 10000) / 10000;
                    var randomBuilding = m_BuildPrefabs[randomBuildingIndex];

                    int randomXPosition, randomZPosition;

                    do
                    {
                        randomXPosition = Random.Range(0, m_FloorGenerator.SizeX);
                        randomZPosition = Random.Range(0, m_FloorGenerator.SizeZ);
                    } while (m_FloorGenerator.Tiles[randomXPosition, randomZPosition].Occupied);
                    
                    if(randomBuilding != null)
                    {
                        var instance = Instantiate(randomBuilding, m_FloorGenerator.Tiles[randomXPosition, randomZPosition].transform.position + Vector3.up, Quaternion.identity);
                        m_FloorGenerator.Tiles[randomXPosition, randomZPosition].Building = instance;
                        m_FloorGenerator.Tiles[randomXPosition, randomZPosition].Occupied = true;

                        instance.PlaceBuilding();
                        GameManager.instance.SetBuildingCost(instance);
                    }
                }
            }
        }
    }
}

