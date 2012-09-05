using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TeraformData;

namespace Teraform
{
    public class ItemCatalog
    {
        private Dictionary<ulong, Item> _items;

        public ItemCatalog(ItemData[] items_data)
        {
            _items = new Dictionary<ulong, Item>(items_data.GetLength(0) * 2);
            
            foreach (ItemData itemData in items_data)
            {
                Item item;
                FileStream textureStream = new FileStream(Environment.SpecialFolder.Resources + "\\Custom\\" + itemData.UniversalTexture, FileMode.Open);
                Texture2D universalTexture = Texture2D.FromStream(Game.GraphicsInstance.GraphicsDevice, textureStream);


                switch (itemData.ClassType)
                {
                    case "Teraform.Platform":
                        item = new Platform(new Point(0, 0), universalTexture, Item.ITEM_STATE.IN_INVENTORY);                        
                        break;
                    case "Teraform.Block":                                            
                        item = new Block(new Point(0,0), universalTexture, Item.ITEM_STATE.IN_INVENTORY);        
                        break;
                    default:
                        throw new NotSupportedException(string.Concat("Error, item type ", itemData.ClassType ," not recognized; check the world\'s .xml file"));
                }
                item._itemId = itemData.Id;
                item._itemName = itemData.Name;               
                _items.Add(itemData.Id, item);
            }
        }

        public Item GetItem(ulong itemId)
        {
            Item item;
            _items.TryGetValue(itemId, out item);

            if (item == null)
                return null;

            return item.ShallowCopy();
        }
    }
}
