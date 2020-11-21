namespace Scripts.Buildings
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="StreetBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class StreetBuilding : BaseBuilding
    {
        /// <inheritdoc/>
        protected override void UpgradeValues()
        {
            Debug.Log("Street built.");
        }
    }
}