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

namespace Platform
{
    class Actor
    {
        public Model model { get; set; }
        public Texture2D texture { get; set; }
        public Matrix localTransforms { get; set; }
        private Matrix[] BoneTransforms;
        public Actor(Model m, Texture2D t, Matrix lo)
        {
            model = m;
            texture = t;
            BoneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(BoneTransforms);
            localTransforms = lo;
        }
        public void Draw(Matrix world)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["Diffuse"].SetValue(texture);
                    effect.Parameters["World"].SetValue(BoneTransforms[mesh.ParentBone.Index] * localTransforms * world);
                    mesh.Draw();
                }
            }
        }
    }
}
