namespace Scripts.Buildings
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="HouseBuilding"/> class. Inherited from <see cref="BaseBuilding"/>.
    /// </summary>
    public class HouseBuilding : BaseBuilding
    {
        [SerializeField] private float m_WaitTimerForClicks = 5;
        [SerializeField] private int m_PeopleLimitIncrease = 5;

        private Coroutine m_Coroutine;

        /// <summary>
        /// Gets or sets the Wait Timer For Clicks.
        /// </summary>
        public float WaitTimerForClicks
        {
            get => m_WaitTimerForClicks;
            set => m_WaitTimerForClicks = value;
        }

        /// <summary>
        /// Gets the People Limit Increase value.
        /// </summary>
        public int PeopleLimitIncrease
        {
            get => m_PeopleLimitIncrease;
        }

        /// <inheritdoc />
        protected override void UpgradeValues()
        {
            GameManager.instance.PeopleLimit += PeopleLimitIncrease;

            m_Coroutine = StartCoroutine(StartAutomaticClicks());
        }

        /// <inheritdoc/>
        protected override void DowngradeValues()
        {
            if(GameManager.instance.PeopleLimit >= PeopleLimitIncrease)
            {
                GameManager.instance.PeopleLimit -= PeopleLimitIncrease;
            }

            if(m_Coroutine != null)
            {
                StopCoroutine(m_Coroutine);
            }
        }

        private IEnumerator StartAutomaticClicks()
        {
            while (true)
            {
                yield return new WaitForSeconds(WaitTimerForClicks);
                GameManager.instance.SmashRocks(1);
            }
        }
    }
}
