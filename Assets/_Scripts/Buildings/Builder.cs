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

            // Every Neighbour is a street
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street)
            {
                return GetCrossingStreet(street, tile, topNeighbour, rightNeighbour, bottomNeighbour, leftNeighbour);
            }

            // Bottom, Right and Top Neighbours are streets
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetTCrossingStreet(street, new Vector3(0, 0, 0), tile, topNeighbour, rightNeighbour, bottomNeighbour, null);
            }

            // Bottom, Right and Left Neighbours are streets
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetTCrossingStreet(street, new Vector3(0, 90, 0), tile, null, rightNeighbour, bottomNeighbour, leftNeighbour);
            }

            // Bottom, Left and Top Neighbours are streets
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetTCrossingStreet(street, new Vector3(0, 180, 0), tile, topNeighbour, null, bottomNeighbour, leftNeighbour);
            }

            // Right, Left and Top Neighbours are streets
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetTCrossingStreet(street, new Vector3(0, 270, 0), tile, topNeighbour, rightNeighbour, null, leftNeighbour);
            }

            // Bottom Neighbour and Top Neighbour are streets, every one else is something else
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetNormalStreet(street, tile, bottomNeighbour, topNeighbour, new Vector3(0,90,0));
            }
            
            // Left Neighbour and Right Neighbour are streets, every one else is something else
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetNormalStreet(street, tile, leftNeighbour, rightNeighbour, new Vector3(0, 0, 0));
            }

            // Left Neighbour and Top Neighbour are streets, every one else is something else
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetCurveStreet(street, tile, leftNeighbour, topNeighbour, new Vector3(0, 180, 0));
            }

            // Left Neighbour and Bottom Neighbour are streets, every one else is something else
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetCurveStreet(street, tile, bottomNeighbour, leftNeighbour, new Vector3(0, 90, 0));
            }

            // Right Neighbour and Top Neighbour are streets, every one else is something else
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetCurveStreet(street, tile, rightNeighbour, topNeighbour, new Vector3(0, 270, 0));
            }

            // Right Neighbour and Bottom Neighbour are streets, every one else is something else
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetCurveStreet(street, tile, bottomNeighbour, rightNeighbour, new Vector3(0, 0, 0));
            }

            // Bottom Neighbour is street, every one else is something else
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetDeadEnd(street, tile, bottomNeighbour, new Vector3(0, 90, 0));
            }

            // Top Neighbour is street, every one else is something else
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetDeadEnd(street, tile, topNeighbour, new Vector3(0, -90, 0));
            }

            // Left Neighbour is street, every one else is something else
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetDeadEnd(street, tile, leftNeighbour, new Vector3(0, 180, 0));
            }

            // Right Neighbour is street, every one else is something else
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetDeadEnd(street, tile, rightNeighbour, new Vector3(0, 0, 0));
            }

            if (!street.name.Equals("Street"))
            {
                Destroy(m_CurrentBuilding?.gameObject);
                m_CurrentBuilding = null;
            }

            return m_StreetVariations.FirstOrDefault(x => x.name.Equals("Street"));
        }

        private BaseBuilding GetCurveStreet(BaseBuilding street, Tile tile, Tile neighbour1, Tile neighbour2, Vector3 rotation)
        {
            if(!street.name.EndsWith("Curve"))
            {
                Destroy(m_CurrentBuilding?.gameObject);
                m_CurrentBuilding = null;
            }

            street = m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("Curve"));
            street.transform.eulerAngles = rotation;

            ChangeNeighbourBuilding(tile, neighbour1);
            ChangeNeighbourBuilding(tile, neighbour2);

            return street;
        }

        private BaseBuilding GetTCrossingStreet(BaseBuilding street, Vector3 rotation, Tile tile, Tile topNeighbour = null, Tile rightNeighbour = null, Tile bottomNeighbour = null, Tile leftNeighbour = null)
        {
            if (!street.name.EndsWith("CrossingT"))
            {
                Destroy(m_CurrentBuilding?.gameObject);
                m_CurrentBuilding = null;
            }

            street = m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT"));
            street.transform.eulerAngles = rotation;
            if (topNeighbour != null)
            {
                ChangeNeighbourBuilding(tile, topNeighbour);
            }

            if(bottomNeighbour != null)
            {
                ChangeNeighbourBuilding(tile, bottomNeighbour);
            }

            if (leftNeighbour != null)
            {
                ChangeNeighbourBuilding(tile, leftNeighbour);
            }

            if (rightNeighbour != null)
            {
                ChangeNeighbourBuilding(tile, rightNeighbour);
            }

            return street;
        }

        private BaseBuilding GetCrossingStreet(BaseBuilding street, Tile tile, Tile topNeighbour, Tile rightNeighbour, Tile bottomNeighbour, Tile leftNeighbour)
        {
            if (!street.name.EndsWith("CrossingNormal"))
            {
                Destroy(m_CurrentBuilding?.gameObject);
                m_CurrentBuilding = null;
            }

            street = m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingNormal"));

            ChangeNeighbourBuilding(tile, topNeighbour);
            ChangeNeighbourBuilding(tile, bottomNeighbour);
            ChangeNeighbourBuilding(tile, rightNeighbour);
            ChangeNeighbourBuilding(tile, leftNeighbour);

            return street;
        }

        private BaseBuilding GetNormalStreet(BaseBuilding street, Tile tile, Tile neighbour1, Tile neighbour2, Vector3 rotation)
        {
            if(!street.name.Equals("Street"))
            {
                Destroy(m_CurrentBuilding?.gameObject);
                m_CurrentBuilding = null;
            }

            street = m_StreetVariations.FirstOrDefault(x => x.name.Equals("Street"));
            street.transform.eulerAngles = rotation;
            ChangeNeighbourBuilding(tile, neighbour1);
            ChangeNeighbourBuilding(tile, neighbour2);
            return street;
        }

        private BaseBuilding GetDeadEnd(BaseBuilding street, Tile tile, Tile neighbour, Vector3 rotation)
        {
            if (!street.name.EndsWith("DeadEnd"))
            {
                Destroy(m_CurrentBuilding?.gameObject);
                m_CurrentBuilding = null;
            }

            street = m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("DeadEnd"));
            street.transform.eulerAngles = rotation;
            neighbour.Building.transform.eulerAngles = rotation;
            ChangeNeighbourBuilding(tile, neighbour);
            return street;
        }

        private void ChangeNeighbourBuilding(Tile tile, Tile neigbour)
        {
            var leftNeighbour = neigbour.Neighbours["left"];
            var rightNeighbour = neigbour.Neighbours["right"];
            var topNeighbour = neigbour.Neighbours["top"];
            var bottomNeighbour = neigbour.Neighbours["bottom"];

            #region No Neighbour

            // Bottom neighbour is tile
            if ((leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && bottomNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 90, 0), "DeadEnd");
                return;
            }

            // Top neighbour is tile
            if ((leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 270, 0), "DeadEnd");
                return;
            }

            // Left neighbour is tile
            if ((topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && leftNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 180, 0), "DeadEnd");
                return;
            }

            // Right neighbour is tile
            if ((topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && rightNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "DeadEnd");
                return;
            }

            #endregion

            #region One Neighbour

            // Only Left Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street 
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && bottomNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 90, 0), "Curve");
                return;
            }

            // Only Left Neighbour of the neighbour is a street and the tile is the top neighbour
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 180, 0), "Curve");
                return;
            }

            // Only Left Neighbour of the neighbour is a street and the tile is the right neighbour
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && rightNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "Street");
                return;
            }

            // Only right Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && bottomNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "Curve");
                return;
            }

            // Only right Neighbour of the neighbour is a street and the tile is the top neighbour
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 270, 0), "Curve");
                return;
            }

            // Only right Neighbour of the neighbour is a street and the tile is the left neighbour
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && leftNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "Street");
                return;
            }

            // Only top Neighbour of the neighbour is a street and the tile is the right neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && rightNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 270, 0), "Curve");
                return;
            }

            // Only top Neighbour of the neighbour is a street and the tile is the left neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && leftNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 180, 0), "Curve");
                return;
            }

            // Only top Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && bottomNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 90, 0), "Street");
                return;
            }

            // Only bottom Neighbour of the neighbour is a street and the tile is the right neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
                && rightNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "Curve");
                return;
            }

            // Only bottom Neighbour of the neighbour is a street and the tile is the left neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
               && leftNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 90, 0), "Curve");
                return;
            }

            // Only bottom Neighbour of the neighbour is a street and the tile is the top neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 90, 0), "Street");
                return;
            }

            #endregion

            #region Two Opposite site neighbours

            // Only bottom and top Neighbour of the neighbour is a street and the tile is the left neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && leftNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 180, 0), "CrossingT");
                return;
            }

            // Only bottom and top Neighbour of the neighbour is a street and the tile is the left neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && rightNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "CrossingT");
                return;
            }

            // Only right and left neighbour of the neighbour is a street and the tile is the top neighbour
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 270, 0), "CrossingT");
                return;
            }

            // Only right and left Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
               && bottomNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 90, 0), "CrossingT");
                return;
            }

            #endregion

            #region Two Orthogonal neighbours

            // Only bottom and right Neighbour of the neighbour is a street and the tile is the left neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
               && leftNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 90, 0), "CrossingT");
                return;
            }

            // Only bottom and right Neighbour of the neighbour is a street and the tile is the top neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "CrossingT");
                return;
            }

            // Only bottom and left neighbour of the neighbour is a street and the tile is the top neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 180, 0), "CrossingT");
                return;
            }

            // Only bottom and left Neighbour of the neighbour is a street and the tile is the right neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
               && rightNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 90, 0), "CrossingT");
                return;
            }

            // Only bottom and left neighbour of the neighbour is a street and the tile is the top neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 180, 0), "CrossingT");
                return;
            }

            // Only top and right Neighbour of the neighbour is a street and the tile is the left neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && leftNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 270, 0), "CrossingT");
                return;
            }

            // Only top and right Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && bottomNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "CrossingT");
                return;
            }

            // Only top and left Neighbour of the neighbour is a street and the tile is the right neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && rightNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 270, 0), "CrossingT");
                return;
            }

            // Only top and left Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && bottomNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 180, 0), "CrossingT");
                return;
            }

            #endregion

            #region Three neighbours

            // Left is the tile
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "CrossingNormal");
                return;
            }

            // Top is the tile
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "CrossingNormal");
                return;
            }

            // Right is the tile
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "CrossingNormal");
                return;
            }

            // Bottom is the tile
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && bottomNeighbour == tile)
            {
                ExecuteBuildingChange(neigbour, new Vector3(0, 0, 0), "CrossingNormal");
                return;
            }

            #endregion
        }

        private void ExecuteBuildingChange(Tile neighbour, Vector3 rotation, string searchValue)
        {
            var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith(searchValue)), neighbour.Building.transform.position, Quaternion.identity);
            street.transform.eulerAngles = rotation;
            Destroy(neighbour.Building.gameObject);
            neighbour.Building = street;
        }
    }
}