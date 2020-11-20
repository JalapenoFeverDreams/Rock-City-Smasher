namespace Scripts.Buildings
{
    using UnityEngine;
    
    /// <summary>
    /// Defines the <see cref="Builder"/> class.
    /// </summary>
    public class Builder : MonoBehaviour
    {
        [SerializeField] private LayerMask m_RaycastLayerMask;
        private BaseBuilding m_CurrentBuilding;

        public void GetBuilding(BaseBuilding building)
        {
            if(m_CurrentBuilding)
            {
                Destroy(m_CurrentBuilding.gameObject);
            }

            m_CurrentBuilding = Instantiate(building);
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
                    m_CurrentBuilding = null;
                    return;
                }

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, m_RaycastLayerMask))
                {
                    var tile = hit.transform.GetComponent<Tile>();
                    if(!tile.Occupied)
                    {
                        m_CurrentBuilding.transform.position = hit.transform.position + new Vector3(0, 1, 0);

                        if (Input.GetMouseButtonDown(0))
                        {
                            m_CurrentBuilding.PlaceBuilding();
                            tile.Occupied = true;
                            m_CurrentBuilding = null;
                        }
                    }
                }
            }
        }
    }
}

