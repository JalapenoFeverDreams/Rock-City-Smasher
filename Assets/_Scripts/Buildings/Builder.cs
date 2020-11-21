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
                return GetCrossingStreet(street, topNeighbour, rightNeighbour, bottomNeighbour, leftNeighbour);
            }

            // Bottom, Right and Top Neighbours are streets
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetTCrossingStreet(street, new Vector3(0, 0, 0), topNeighbour, rightNeighbour, bottomNeighbour, null);
            }

            // Bottom, Right and Left Neighbours are streets
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetTCrossingStreet(street, new Vector3(0, 90, 0), null, rightNeighbour, bottomNeighbour, leftNeighbour);
            }

            // Bottom, Left and Top Neighbours are streets
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetTCrossingStreet(street, new Vector3(0, 180, 0), topNeighbour, null, bottomNeighbour, leftNeighbour);
            }

            // Right, Left and Top Neighbours are streets
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetTCrossingStreet(street, new Vector3(0, 270, 0), topNeighbour, rightNeighbour, null, leftNeighbour);
            }

            // Bottom Neighbour and Top Neighbour are streets, every one else is something else
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
                && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
                && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
                && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetNormalStreet(street, bottomNeighbour, topNeighbour, new Vector3(0,90,0));
            }
            
            // Left Neighbour and Right Neighbour are streets, every one else is something else
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
                && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
                && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
                && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street))
            {
                return GetNormalStreet(street, leftNeighbour, rightNeighbour, new Vector3(0, 0, 0));
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

        private BaseBuilding GetTCrossingStreet(BaseBuilding street, Vector3 rotation, Tile topNeighbour = null, Tile rightNeighbour = null, Tile bottomNeighbour = null, Tile leftNeighbour = null)
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
                topNeighbour.Building.transform.eulerAngles = rotation + new Vector3(0, 90, 0);
            }

            if(bottomNeighbour != null)
            {
                bottomNeighbour.Building.transform.eulerAngles = rotation + new Vector3(0, 90, 0);
            }

            if (leftNeighbour != null)
            {
                leftNeighbour.Building.transform.eulerAngles = rotation;
            }

            if (rightNeighbour != null)
            {
                rightNeighbour.Building.transform.eulerAngles = rotation;
            }

            return street;
        }

        private BaseBuilding GetCrossingStreet(BaseBuilding street, Tile topNeighbour, Tile rightNeighbour, Tile bottomNeighbour, Tile leftNeighbour)
        {
            if (!street.name.EndsWith("CrossingNormal"))
            {
                Destroy(m_CurrentBuilding?.gameObject);
                m_CurrentBuilding = null;
            }

            street = m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingNormal"));
            topNeighbour.Building.transform.eulerAngles = new Vector3(0, 90, 0);
            bottomNeighbour.Building.transform.eulerAngles = new Vector3(0, 90, 0);
            rightNeighbour.Building.transform.eulerAngles = new Vector3(0, 0, 0);
            leftNeighbour.Building.transform.eulerAngles = new Vector3(0, 0, 0);
            return street;
        }

        private BaseBuilding GetNormalStreet(BaseBuilding street, Tile neighbour1, Tile neighbour2, Vector3 rotation)
        {
            if(!street.name.Equals("Street"))
            {
                Destroy(m_CurrentBuilding?.gameObject);
                m_CurrentBuilding = null;
            }

            street = m_StreetVariations.FirstOrDefault(x => x.name.Equals("Street"));
            street.transform.eulerAngles = rotation;
            neighbour1.Building.transform.eulerAngles = rotation;
            neighbour2.Building.transform.eulerAngles = rotation;
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
                ExecuteBuildingChange(neigbour, new Vector3(0, 270, 0), "Curve");
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
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 180, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only bottom and top Neighbour of the neighbour is a street and the tile is the left neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && rightNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 0, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only right and left neighbour of the neighbour is a street and the tile is the top neighbour
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 270, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only right and left Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
               && bottomNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 90, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
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
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 90, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only bottom and right Neighbour of the neighbour is a street and the tile is the top neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 0, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only bottom and left neighbour of the neighbour is a street and the tile is the top neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 180, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only bottom and left Neighbour of the neighbour is a street and the tile is the right neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (topNeighbour == null || topNeighbour.Building == null || topNeighbour.Building.BuildingType != BuildingType.Street)
               && rightNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 90, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only bottom and left neighbour of the neighbour is a street and the tile is the top neighbour
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && topNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 180, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only top and right Neighbour of the neighbour is a street and the tile is the left neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && leftNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 270, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only top and right Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && (leftNeighbour == null || leftNeighbour.Building == null || leftNeighbour.Building.BuildingType != BuildingType.Street)
               && bottomNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 0, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only top and left Neighbour of the neighbour is a street and the tile is the right neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (bottomNeighbour == null || bottomNeighbour.Building == null || bottomNeighbour.Building.BuildingType != BuildingType.Street)
               && rightNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 270, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Only top and left Neighbour of the neighbour is a street and the tile is the bottom neighbour
            if (topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && (rightNeighbour == null || rightNeighbour.Building == null || rightNeighbour.Building.BuildingType != BuildingType.Street)
               && bottomNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingT")), neigbour.Building.transform.position, Quaternion.identity);
                street.transform.eulerAngles = new Vector3(0, 180, 0);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
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
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingNormal")), neigbour.Building.transform.position, Quaternion.identity);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Top is the tile
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingNormal")), neigbour.Building.transform.position, Quaternion.identity);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Right is the tile
            if (bottomNeighbour != null && bottomNeighbour.Building != null && bottomNeighbour.Building.BuildingType == BuildingType.Street
               && leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingNormal")), neigbour.Building.transform.position, Quaternion.identity);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
                return;
            }

            // Bottom is the tile
            if (leftNeighbour != null && leftNeighbour.Building != null && leftNeighbour.Building.BuildingType == BuildingType.Street
               && rightNeighbour != null && rightNeighbour.Building != null && rightNeighbour.Building.BuildingType == BuildingType.Street
               && topNeighbour != null && topNeighbour.Building != null && topNeighbour.Building.BuildingType == BuildingType.Street
               && bottomNeighbour == tile)
            {
                var street = Instantiate(m_StreetVariations.FirstOrDefault(x => x.name.EndsWith("CrossingNormal")), neigbour.Building.transform.position, Quaternion.identity);
                Destroy(neigbour.Building.gameObject);
                neigbour.Building = street;
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