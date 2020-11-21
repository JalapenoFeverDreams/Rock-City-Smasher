namespace Scripts.Buildings
{
    using System.Linq;

    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="EntertainmentBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class EntertainmentBuilding : BaseBuilding
    {
        [SerializeField] private float m_HouseTimerIntervalDecreaseFactorInPercent = 5;

        private float HouseTimerIntervalDecreaseFactor
        {
            get => 1 - m_HouseTimerIntervalDecreaseFactorInPercent / 100;
        }

        /// <inheritdoc/>
        protected override void UpgradeValues()
        {
            foreach(var house in BuildingManager.Instance.Buildings.Where(x => x.BuildingType == BuildingType.House))
            {
                (house as HouseBuilding).WaitTimerForClicks *= HouseTimerIntervalDecreaseFactor;
            }
        }
    }
}
