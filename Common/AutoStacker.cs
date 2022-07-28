using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ObjectData;

namespace AutoStacker.Common
{
    class AutoStackerCommon
    {
        public static int FindChest(int originX, int originY)
        {
            if (originX < 0 || originY < 0)
            {
                return -1;
            }
            Tile tile = Main.tile[originX, originY];
            if (tile == null)
                return -1;

            if (!Chest.IsLocked(originX, originY))
                return Chest.FindChest(originX, originY);
            else
                return -1;
        }

        public static Point16 GetOrigin(int x, int y)
        {

            Tile tile = Main.tile[x, y];
            if (tile == null)
                return new Point16(x, y);

            TileObjectData tileObjectData = TileObjectData.GetTileData(tile.TileType, 0);
            if (tileObjectData == null)
                return new Point16(x, y);

            //OneByOne
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            if (tileObjectData.Width == 1 && tileObjectData.Height == 1)
                return new Point16(x, y);

            //xOffset
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            int xOffset = tile.TileFrameX % tileObjectData.CoordinateFullWidth / tileObjectData.CoordinateWidth;

            //yOffset
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //Rectangle(single)
            int yOffset;
            if (tileObjectData.CoordinateHeights.Distinct().Count() == 1)
            {
                yOffset = tile.TileFrameY % tileObjectData.CoordinateFullHeight / tileObjectData.CoordinateHeights[0];
            }

            //Rectangle(complex)
            else
            {
                yOffset = 0;
                int FullY = tile.TileFrameY % tileObjectData.CoordinateFullHeight;
                for (int i = 0; i < tileObjectData.CoordinateHeights.Length && FullY >= tileObjectData.CoordinateHeights[i]; i++)
                {
                    FullY -= tileObjectData.CoordinateHeights[i];
                    yOffset++;
                }
            }
            return new Point16(x - xOffset, y - yOffset);
        }
    }
}
