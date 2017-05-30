using OpenTK;
using System;
using System.IO;

namespace template
{
    // This class reads and stores a PFM HDR image in the memory. 
    // Credits for this class go to Marien Raat, he created it and was kind enough to allow me to use it.
    // This was allowed by Mr. Bikker, he said it was fine considering we could use open libraries.
    public class PFM
    {
        public int Width, Height;
        Vector3[,] pixels;

        public PFM(String filename)
        {
            // First read the metadata
            StreamReader reader = new StreamReader(filename);

            string type = reader.ReadLine();
            string dimens = reader.ReadLine();
            string scale = reader.ReadLine();

            string[] dimensions = dimens.Split(' ');
            Width = int.Parse(dimensions[0]);
            Height = int.Parse(dimensions[1]);
            pixels = new Vector3[Width, Height];

            reader.Close();

            // Now we want to read with a binary reader, but we need
            // to set the position correctly, so we don't read the
            // metadata again.
            long position = type.Length + dimens.Length + scale.Length + 3;
            FileStream stream = new FileStream(filename, FileMode.Open);
            stream.Position = position;
            BinaryReader binaryReader = new BinaryReader(stream);

            // And then just read the floats
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        pixels[x, Height - y - 1][i] = binaryReader.ReadSingle();
                    }
                }
            }
        }

        // Convenience method
        public Vector3 GetPixel(int x, int y)
        {
            return pixels[x, y];
        }
    }
}