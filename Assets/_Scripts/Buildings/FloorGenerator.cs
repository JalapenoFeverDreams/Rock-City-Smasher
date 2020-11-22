﻿namespace Scripts.Buildings
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// Defines the <see cref="FloorGenerator"/> class.
    /// </summary>
    public class FloorGenerator : MonoBehaviour
    {
        [SerializeField] private int m_SizeX;
        [SerializeField] private int m_SizeZ;
        [SerializeField] private Tile m_TilePrefab;

        /// <summary>
        /// Gets the Tiles of the floor.
        /// </summary>
        public Tile[,] Tiles { get; private set; }

        /// <summary>
        /// Gets the Size in X-Dimension.
        /// </summary>
        public int SizeX => m_SizeX;

        /// <summary>
        /// Gets the Size in Z-Dimension.
        /// </summary>
        public int SizeZ => m_SizeZ;

        private void Awake()
        {
            GenerateFloor();
        }

        private void GenerateFloor()
        {
            Tiles = new Tile[m_SizeX, m_SizeZ];
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for(int z = 0; z < Tiles.GetLength(1); z++)
                {
                    Tiles[x, z] = Instantiate(m_TilePrefab, new Vector3(x, 0, z), Quaternion.identity);
                    Tiles[x, z].X = x;
                    Tiles[x, z].Z = z;
                    Tiles[x, z].Occupied = false;
                    Tiles[x, z].name = $"Tile (x: {x} | z: {z})";
                    Tiles[x, z].transform.parent = transform;
                    
                }
            }

            foreach(var tile in Tiles)
            {
                AssignNeighbours(tile);
            }
        }

        private void AssignNeighbours(Tile tile)
        {
            if (tile.X > 0 && Tiles[tile.X - 1, tile.Z] != null)
            {
                tile.Neighbours["left"] = Tiles[tile.X - 1, tile.Z];
            }
            if (tile.X < Tiles.GetLength(0) - 1 && Tiles[tile.X + 1, tile.Z] != null)
            {
                tile.Neighbours["right"] = Tiles[tile.X + 1, tile.Z];
            }
            if(tile.Z > 0 && Tiles[tile.X, tile.Z - 1] != null)
            {
                tile.Neighbours["bottom"] = Tiles[tile.X, tile.Z - 1];
            }
            if (tile.Z < Tiles.GetLength(1) - 1 && Tiles[tile.X, tile.Z + 1] != null)
            {
                tile.Neighbours["top"] = Tiles[tile.X, tile.Z + 1];
            }
        }
    }
}
