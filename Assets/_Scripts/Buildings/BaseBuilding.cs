namespace Scripts.Buildings
{
    using System.Linq;

    using UnityEngine;
    
    /// <summary>
    /// Defines the <see cref="BaseBuilding"/> class. This class can not be initialized. Only inherited from.
    /// </summary>
    public abstract class BaseBuilding : MonoBehaviour
    {
        #region Private Fields
        
        [SerializeField] private int m_InitialCost;
        [SerializeField] private float m_CostIncreaseFactor;
        [SerializeField] private BuildingType m_BuildingType;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Initial Cost of the Building.
        /// </summary>
        public int InitialCost => m_InitialCost;

        /// <summary>
        /// Gets the Cost Increase Factor of the building.
        /// </summary>
        public float CostIncreaseFactor => m_CostIncreaseFactor;
        
        /// <summary>
        /// Gets the Building Cost.
        /// </summary>
        public int Cost => (int)(InitialCost * Mathf.Pow(m_CostIncreaseFactor, BuildingManager.Instance.Buildings.Count(x => x.BuildingType == BuildingType)));

        /// <summary>
        /// Gets the type of the building.
        /// </summary>
        public BuildingType BuildingType => m_BuildingType;

        #endregion

        /// <summary>
        /// This method Upgrades the Player values.
        /// </summary>
        protected virtual void UpgradeValues() { }

        public void PlaceBuilding()
        {
            BuildingManager.Instance.Buildings.Add(this);

            UpgradeValues();
        }
    }
}


