using System.Linq;

namespace Scripts.Buildings
{
    using UnityEngine;
    
    public class Builder : MonoBehaviour
    {
        [SerializeField] private LayerMask m_RaycastLayerMask;
        private BaseBuilding m_CurrentBuilding;
        public void GetBuilding(BaseBuilding building)
        {
            var b = Instantiate(building);
            m_CurrentBuilding = b;
        }

        private void Update()
        {
            Build();
        }

        private void Build()
        {
            if (!m_CurrentBuilding)
            {
                return;
            }

            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity,
                m_RaycastLayerMask))
            {
                return;
            }

            m_CurrentBuilding.transform.position = hit.point;
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            BuildingManager.Instance.Buildings.Count(x => x.BuildingType == BuildingType.House);

            m_CurrentBuilding.PlaceBuilding();
            m_CurrentBuilding = null;
        }
    }
}

