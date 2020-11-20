namespace Scripts.Buildings
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="HouseBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class HouseBuilding : BaseBuilding
    {
        /// <inheritdoc />
        protected override void UpgradeValues()
        {
            Debug.Log("House built!");
        }
    }
}
