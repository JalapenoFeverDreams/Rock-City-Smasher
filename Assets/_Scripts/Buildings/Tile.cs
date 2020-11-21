namespace Scripts.Buildings
{
    using System.Collections.Generic;

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

        /// <summary>
        /// Gets or sets the Building that is occupying th Tile.
        /// </summary>
        public BaseBuilding Building { get; set; }

        /// <summary>
        /// Gets or sets the direct Neighbours of the tile.
        /// </summary>
        public Dictionary<string, Tile> Neighbours { get; set; } = new Dictionary<string, Tile>
        {
            { "left", null },
            { "right", null },
            { "top", null },
            { "bottom", null },
        };
    }
}

