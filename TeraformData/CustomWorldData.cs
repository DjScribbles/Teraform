using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using TeraformData;

namespace TeraformData
{
    public class CustomWorldData
    {
        public string _worldName;
        public ItemData[] _itemData;

        public CustomWorldData(string worldName)
        {
            _worldName = worldName;
        }

        public void LoadContent(ContentManager content)
        {
            _itemData = content.Load<ItemData[]>(_worldName + "_Items");
              
        }
    }
}

