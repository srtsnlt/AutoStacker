using System;
using Terraria;
using Terraria.DataStructures;

namespace AutoStacker.Common
{
    public static class MagicStorageAdapter
    {
        private static bool loadState = false;

        private static Type tEStorageCenterType;
        private static System.Reflection.MethodInfo getHeartInfo;

        private static Type tEStorageHeartType;
        private static System.Reflection.MethodInfo depositItemInfo;
        private static System.Reflection.FieldInfo idInfo;

        private static Type netHelperType;
        private static System.Reflection.MethodInfo sendRefreshNetworkItemsInfo;

        private static Type storageGUIType;
        private static System.Reflection.MethodInfo refreshItems;

        static MagicStorageAdapter()
        {
            if (AutoStacker.modMagicStorage == null)
            {
                return;
            }
            tEStorageCenterType = Type.GetType("MagicStorage.Components.TEStorageCenter, MagicStorage");
            if (tEStorageCenterType == null)
            {
                return;
            }

            getHeartInfo = tEStorageCenterType.GetMethod("GetHeart");
            if (getHeartInfo == null)
            {
                return;
            }

            tEStorageHeartType = Type.GetType("MagicStorage.Components.TEStorageHeart, MagicStorage");
            if (tEStorageHeartType == null)
            {
                return;
            }

            depositItemInfo = tEStorageHeartType.GetMethod("DepositItem");
            if (getHeartInfo == null)
            {
                return;
            }

            idInfo = tEStorageHeartType.GetField("ID");
            if (idInfo == null)
            {
                return;
            }

            netHelperType = Type.GetType("MagicStorage.NetHelper, MagicStorage");
            if (netHelperType == null)
            {
                return;
            }

            sendRefreshNetworkItemsInfo = netHelperType.GetMethod("SendRefreshNetworkItems");
            if (sendRefreshNetworkItemsInfo == null)
            {
                return;
            }

            storageGUIType = Type.GetType("MagicStorage.StorageGUI, MagicStorage");
            if (storageGUIType == null)
            {
                return;
            }

            refreshItems = storageGUIType.GetMethod("RefreshItems");
            if (refreshItems == null)
            {
                return;
            }

            loadState = true;
        }

        public static bool DepositItem(TileEntity tEStorageCenter, Item item)
        {
            if (!loadState)
            {
                return false;
            }

            int oldstack = item.stack;

            // MagicStorage.Components.TEStorageHeart heart = ((MagicStorage.Components.TEStorageCenter)tEStorageCenter).GetHeart();
            var heart = getHeartInfo.Invoke(tEStorageCenter, null);

            // heart.DepositItem(item);
            object[] parameters = new object[1];
            parameters[0] = item;
            depositItemInfo.Invoke(heart, parameters);

            if (oldstack != item.stack)
            {
                if (Main.netMode == 2)
                {
                    // MagicStorage.NetHelper.SendRefreshNetworkItems(heart.ID);
                    object[] parameters2 = new object[1];
                    int id = (int)idInfo.GetValue(heart);
                    parameters2[0] = id;
                    sendRefreshNetworkItemsInfo.Invoke(null, parameters2);

                }
                else if (Main.netMode == 0)
                {
                    // MagicStorage.StorageGUI.RefreshItems();
                    refreshItems.Invoke(null, null);
                }
            }
            return true;
        }
    }
}
