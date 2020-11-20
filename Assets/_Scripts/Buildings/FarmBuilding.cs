namespace Scripts.Buildings
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="FarmBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class FarmBuilding : BaseBuilding
    {
        /// <inheritdoc/>
        protected override void UpgradeValues()
        {
            Debug.Log("Farm built");
        }
    }
}

