namespace Scripts.Buildings
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="EntertainmentBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class EntertainmentBuilding : BaseBuilding
    {
        /// <inheritdoc/>
        protected override void UpgradeValues()
        {
            Debug.Log("Entertainment built.");
        }
    }
}
