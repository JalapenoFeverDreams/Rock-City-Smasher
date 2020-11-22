namespace Scripts.Buildings
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="FarmBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class FarmBuilding : BaseBuilding
    {
        [SerializeField] private int m_PeopleCountIncrease = 5;
        [SerializeField] private int m_MaterialMultiplyFactorAsPercent = 2;

        /// <summary>
        /// Gets the People Count increase value.
        /// </summary>
        public int PeopleCountIncrease => m_PeopleCountIncrease;

        /// <summary>
        /// Gets the material multiply factor.
        /// </summary>
        public int MaterialMultiplyFactor => 1 - m_MaterialMultiplyFactorAsPercent / 100;

        /// <inheritdoc/>
        protected override void UpgradeValues()
        {
            GameManager.instance.PeopleCount += PeopleCountIncrease;
        }

        /// <inheritdoc>/>
        protected override void DowngradeValues()
        {
            if(GameManager.instance.PeopleCount >= PeopleCountIncrease)
            {
                GameManager.instance.PeopleCount -= PeopleCountIncrease;
            }
        }
    }
}

