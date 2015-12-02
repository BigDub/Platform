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
using Microsoft.Xna.Framework.Content.Pipeline;

namespace ContentPipelineExtension
{
    class Tileset
    {
        Texture2D texture;
        Actor[] Tiles;
    }

    [ContentImporter(".pts", DisplayName = "Tileset Importer")]
    class TilesetImporter : ContentImporter<Tileset>
    {
        public override Tileset Import(string filename, ContentImporterContext context)
        {
            //System.IO.File.ReadAllText(filename);
            LinkedList<Actor> Actors = new LinkedList<Actor>();
            

            return new Tileset();
        }
    }
}
