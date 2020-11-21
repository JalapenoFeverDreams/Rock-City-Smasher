namespace Scripts.Buildings
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    
    /// <summary>
    /// Defines the <see cref="Builder"/> class.
    /// </summary>
    public class Builder : MonoBehaviour
    {
        [SerializeField] private LayerMask m_RaycastLayerMask;
        [SerializeField] private List<BaseBuilding> m_StreetVariations;

        private BaseBuilding m_CurrentBuilding;

        public void GetBuilding(BaseBuilding building)
        {
            GameManager.instance.SetBuildingCost(building);
            if (GameManager.instance.BuyBuilding(building))
            {
                if (m_CurrentBuilding)
                {
                    Destroy(m_CurrentBuilding.gameObject);
                }

                m_CurrentBuilding = Instantiate(building);
            }
        }

        private void Update()
        {
            Build();
        }

        private void Build()
        {
            if (m_CurrentBuilding)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(m_CurrentBuilding.gameObject);
                    GameManager.instance.Money += m_CurrentBuilding.Cost;
                    m_CurrentBuilding = null;
                    return;
                }

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, m_RaycastLayerMask))
                {
                    var tile = hit.transform.GetComponent<Tile>();
                    if(!tile.Occupied)
                    {
                        
                        if(m_CurrentBuilding.BuildingType == BuildingType.Street)
                        {
                            m_CurrentBuilding = Instantiate(GetCorrectStreet(tile));
                        }

                        m_CurrentBuilding.transform.position = hit.transform.position + new Vector3(0, 1, 0);

                        if (Input.GetMouseButtonDown(0))
                        {
                            m_CurrentBuilding.PlaceBuilding();
                            GameManager.instance.SetBuildingCost(m_CurrentBuilding);
                            tile.Occupied = true;
                            tile.Building = m_CurrentBuilding;
                            m_CurrentBuilding = null;
                        }
                    }
                }
            }
        }

        private BaseBuilding GetCorrectStreet(Tile tile)
        {
            BaseBuilding street = m_CurrentBuilding;

            var leftNeighbour = tile.Neighbours["left"];
            var rightNeighbour = tile.Neighbours["right"];
            var topNeighbour = tile.Neighbours["top"];
            var bottomNeighbour = tile.Neighbours["bottom"];

            if(bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street)
            {
                if(!street.name.EndsWith("DeadEnd"))
                {
                    Destroy(m_CurrentBuilding.gameObject);
                    m_CurrentBuilding = null;
                }

                street = m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("DeadEnd"));
            }

            if (!street.name.Equals("Street"))
            {
                Destroy(m_CurrentBuilding.gameObject);
                m_CurrentBuilding = null;
            }

            return m_StreetVariations.FirstOrDefault(x => x.name.Equals("Street"));
        }
    }
}

