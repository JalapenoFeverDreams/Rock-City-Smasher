namespace Scripts.Buildings
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="MarketplaceBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class MarketplaceBuilding : BaseBuilding
    {
        [SerializeField] private float m_TimeIntervalDecreaseFactorInPercent = 5f;
        [SerializeField] private float m_ChancesOfRandomUpgradeInPercent = 10f;
        
        /// <summary>
        /// Gets the Time Interval Decrease factor for the random upgrades.
        /// </summary>
        public float TimeIntervalDecreaseFactor
        {
            get => 1 - m_TimeIntervalDecreaseFactorInPercent / 100;
        }

        /// <summary>
        /// Gets the Chance of getting a Random upgrade per time interval.
        /// </summary>
        public float ChancesOfRandomUpgrade => m_ChancesOfRandomUpgradeInPercent;

        /// <inheritdoc/>
        protected override void UpgradeValues()
        {
            BuildingManager.Instance.RandomUpgradesPossible = true;
        }
    }
}
