namespace Scripts.Buildings
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="MarketplaceBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class MarketplaceBuilding : BaseBuilding
    {
        /// <inheritdoc/>
        protected override void UpgradeValues()
        {
            Debug.Log("Marketplace built!");
        }
    }
}
