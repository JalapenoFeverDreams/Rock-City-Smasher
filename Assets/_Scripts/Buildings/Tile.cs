namespace Scripts.Buildings
{
    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="Tile"/> class.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the X index of the Tile.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Z index of the tile.
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the Tile is occupied by another building or not.
        /// </summary>
        public bool Occupied { get; set; }
    }
}

