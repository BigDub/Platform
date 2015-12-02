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
    class Tileset
    {
        Actor[] actors;
        public int Length
        {
            get
            {
                return actors.Length;
            }
        }
        public void Draw(byte act, Matrix world)
        {
            actors[act].Draw(world);
        }
        public static Tileset LoadTileset(String filename)
        {
            Tileset ts = new Tileset();
            LinkedList<Actor> act = new LinkedList<Actor>();
            System.IO.StreamReader stream = new System.IO.StreamReader(filename);
            String dir = stream.ReadLine();
            String test = stream.ReadLine();
            Model m = global.content.Load<Model>("Wall");
            Texture2D tex = global.content.Load<Texture2D>("Blank");
            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = global.e;
                }
            }
            act.AddLast(new Actor(m, tex, Matrix.CreateTranslation(-1.5f * Vector3.UnitZ)));
            act.AddLast(new Actor(m, tex, Matrix.CreateTranslation(1.5f * Vector3.UnitZ)));
            while (!stream.EndOfStream)
            {
                String[] arr = stream.ReadLine().Split(',');
                int variations = int.Parse(arr[1]);
                for (int i = 0; i < variations; i++)
                {
                    m = global.content.Load<Model>(dir + "\\" + arr[0] + i);
                    tex = global.content.Load<Texture2D>(dir + "\\" + arr[0] + i + "tex");
                    foreach (ModelMesh mesh in m.Meshes)
                    {
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            part.Effect = global.e;
                        }
                    }
                    act.AddLast(new Actor(m, tex, Matrix.Identity));
                    act.AddLast(new Actor(m, tex, Matrix.CreateRotationZ(MathHelper.PiOver2)));
                    act.AddLast(new Actor(m, tex, Matrix.CreateRotationZ(MathHelper.Pi)));
                    act.AddLast(new Actor(m, tex, Matrix.CreateRotationZ(3 * MathHelper.PiOver2)));
                }
            }
            stream.Close();
            ts.actors = act.ToArray();
            return ts;
        }
    }
    class Level
    {
        private String file;
        public Tileset tileset { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Byte[,] data;
        public void Draw()
        {
            for (int x = 0; x < Width; x++)
                for (int y = Height - 1; y >= 0; y--)
                    tileset.Draw(data[x, y], Matrix.CreateTranslation(new Vector3(x, Height - y, 0)));
        }
        public Level(Tileset t, int w, int h)
        {
            tileset = t;
            Width = w;
            Height = h;
            data = new Byte[Width, Height];
        }
        public static Level LoadLevel(String filename, Tileset tiles)
        {
            System.IO.StreamReader stream = new System.IO.StreamReader(filename);
            int Wi = int.Parse(stream.ReadLine());
            int He = int.Parse(stream.ReadLine());
            Level l = new Level(tiles, Wi, He);
            l.file = filename;
            for (int y = 0; y < He; y++)
            {
                String[] arr = stream.ReadLine().Split(',');
                for (int x = 0; x < Wi; x++)
                    l.data[x, y] = byte.Parse(arr[x]);
            }
            stream.Close();
            return l;
        }
        public void Save()
        {
            if (file == null)
                return;
            System.IO.StreamWriter stream = new System.IO.StreamWriter(file);
            stream.WriteLine(Width);
            stream.WriteLine(Height);
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    stream.Write(data[x, y]);
                    if (x < Width - 1)
                        stream.Write(',');
                    else
                        stream.WriteLine();
                }
            stream.Close();
        }
    }
}
