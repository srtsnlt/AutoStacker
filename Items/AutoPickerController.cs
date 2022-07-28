using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AutoStacker.Items
{
    public class AutoPickerController : ModItem
    {
        public Point16 topLeft = new Point16((short)-1, (short)-1);

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Auto Picker Controller");

            String tooltip_str = "Useage \n";
            tooltip_str += "  Click chest           : Select Recever chest\n";
            tooltip_str += "  Right click AutoPicker: Select AutoPicker\n";
            tooltip_str += "  Right click this item : ON/OFF auto pick \n";
            Tooltip.SetDefault(tooltip_str);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (topLeft.X != -1 && topLeft.Y != -1)
            {
                TooltipLine lineH2 = new TooltipLine(Mod, "head2", "ReceverChest [" + topLeft.X + "," + topLeft.Y + "]\n ");
                tooltips.Insert(2, lineH2);
            }
            else
            {
                TooltipLine lineH2 = new TooltipLine(Mod, "head2", "Chest [ none ]\n ");
                tooltips.Insert(2, lineH2);
            }

        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = 100;
            Item.rare = 1;
            Item.useStyle = 1;
            Item.useAnimation = 28;
            Item.useTime = 28;

        }

        public override bool? UseItem(Player player)
        {
            Point16 origin = Common.AutoStackerCommon.GetOrigin(Player.tileTargetX, Player.tileTargetY);
            if (player.altFunctionUse == 0)
            {

                if (
                    (
                        Common.AutoStackerCommon.FindChest(origin.X, origin.Y) != -1
                        && Main.tile[origin.X, origin.Y].TileType != ModContent.TileType<Tiles.AutoPicker>()
                    )
                    // || ((AutoStacker.modMagicStorage != null || AutoStacker.modMagicStorageExtra != null) && callMagicStorageFindHeart(origin))
                    || (AutoStacker.modMagicStorage != null && callMagicStorageFindHeart(origin))
                )
                {
                    topLeft = origin;
                    Main.NewText("Reciever Chest Selected to x:" + origin.X + ", y:" + origin.Y + " !");
                }
                else
                {
                    Main.NewText("No chest to be found.");
                }
            }
            return true;
        }


        private bool callMagicStorageFindHeart(Point16 origin)
        {
            if (Common.MagicStorageConnecter.FindHeart(origin) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(null, "RecieverChestSelector", 1)
                .AddIngredient(ItemID.Wire, 1)
                .AddTile(TileID.WorkBenches)
                .Register();
        }


        public override ModItem Clone(Item item)
        {
            AutoPickerController newItem = (AutoPickerController)base.Clone(item);
            newItem.topLeft = this.topLeft;
            return (ModItem)newItem;
        }

    }
}
