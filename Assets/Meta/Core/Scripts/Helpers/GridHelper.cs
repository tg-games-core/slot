using System;
using UnityEngine;

namespace Core
{
    public static class GridHelper
    {
        public static Vector3 GetPosition(int count, int rowCount, int lineCount, Vector3 offset)
        {
            int heightCount = count / (rowCount * lineCount);
            int remainder = count % (rowCount * lineCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < lineCount; j++)
                {
                    if (remainder == 0)
                    {
                        return new Vector3(offset.x * j, offset.y * heightCount, offset.z * i);
                    }

                    remainder--;
                }
            }
            
            return Vector3.zero;
        }
        
        public static Vector2[] GenerateGrid(Vector2 size, Vector2Int gridSize)
        {
            if (gridSize.x <= 0 || gridSize.y <= 0)
            {
                return Array.Empty<Vector2>();
            }

            int totalCells = gridSize.x * gridSize.y;
            Vector2[] positions = new Vector2[totalCells];

            float stepX = size.x;
            float stepY = size.y;
            float offsetX = -stepX * (gridSize.x - 1) / 2f;
            float offsetY = -stepY * (gridSize.y - 1) / 2f;

            int index = 0;
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    float posX = offsetX + stepX * x;
                    float posY = offsetY + stepY * y;
                    positions[index++] = new Vector2(posX, posY);
                }
            }

            return positions;
        }
    }
}