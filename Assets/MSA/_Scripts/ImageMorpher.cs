using UnityEngine;
using System.Collections.Generic;

namespace Morph.Smart.Algorithm
{
    public class ImageMorpher : MonoBehaviour
    {
        public Sprite imageA;
        public Sprite imageB;
        public Material morphMaterial;
        [Range(0f, 1f)]
        public float morphAmount = 0f;


        private Texture2D textureA;
        private Texture2D textureB;
        private Mesh mesh;
        private bool isMorphing = false;

        // Control points
        private List<Vector2> controlPointsA;
        private List<Vector2> controlPointsB;
        private List<Vector2> controlPointsCurrent;

        void Start()
        {
            Initialize();
            isMorphing = true;

        }

        void Update()
        {
            if (isMorphing)
            {
                MorphImages(Mathf.Clamp01(morphAmount));
            }
        }

        public void StartMorph()
        {
            isMorphing = true;
        }

        public void Initialize()
        {
            // Convert Sprites to Texture2D
            textureA = SpriteToTexture2D(imageA);
            textureB = SpriteToTexture2D(imageB);

            // Initialize control points (you can set these manually or use key features)
            InitializeControlPoints();

            // Create a mesh
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            // Assign textures to the material
            morphMaterial.SetTexture("_MainTexA", textureA);
            morphMaterial.SetTexture("_MainTexB", textureB);

            _renderer = GetComponent<Renderer>();
            _renderer.material = morphMaterial;

            // Set initial mesh to imageA
            UpdateMeshWithoutTexture(textureA, controlPointsA);
        }

        private Texture2D SpriteToTexture2D(Sprite sprite)
        {
            // Use textureRect to get the correct width and height
            int width = Mathf.RoundToInt(sprite.textureRect.width);
            int height = Mathf.RoundToInt(sprite.textureRect.height);

            // Create a new Texture2D with the correct dimensions
            Texture2D texture = new Texture2D(
                width,
                height,
                TextureFormat.RGBA32,
                false
            );

            // Extract the pixels from the sprite's texture using textureRect
            Color[] pixels = sprite.texture.GetPixels(
                Mathf.RoundToInt(sprite.textureRect.x),
                Mathf.RoundToInt(sprite.textureRect.y),
                width,
                height
            );

            // Set the pixels to the new texture
            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }


        private void InitializeControlPoints()
        {
            // For simplicity, we'll use the four corners as control points
            controlPointsA = new List<Vector2>
        {
            new Vector2(0, 0),
            new Vector2(textureA.width, 0),
            new Vector2(0, textureA.height),
            new Vector2(textureA.width, textureA.height),
            new Vector2(textureA.width / 2, textureA.height / 2)
        };

            controlPointsB = new List<Vector2>
        {
            new Vector2(0, 0),
            new Vector2(textureB.width, 0),
            new Vector2(0, textureB.height),
            new Vector2(textureB.width, textureB.height),
             new Vector2((textureB.width / 2) + 10, (textureB.height / 2) + 10)
        };

            // Initialize current control points
            controlPointsCurrent = new List<Vector2>(controlPointsA);
        }

        private Renderer _renderer;

        private void MorphImages(float t)
        {
            // Interpolate control points
            for (int i = 0; i < controlPointsA.Count; i++)
            {
                controlPointsCurrent[i] = Vector2.Lerp(controlPointsA[i], controlPointsB[i], t);
            }

            // Update mesh vertices based on control points
            UpdateMeshVertices();

            // Update shader parameter
            _renderer.material.SetFloat("_MorphProgress", t);
        }

        private void UpdateMeshWithoutTexture(Texture2D texture, List<Vector2> controlPoints)
        {
            Vector3[] vertices = new Vector3[4];
            Vector2[] uv = new Vector2[4];
            int[] triangles = new int[6];

            // Set vertices based on control points
            for (int i = 0; i < 4; i++)
            {
                vertices[i] = new Vector3(controlPoints[i].x, controlPoints[i].y, 0);
                uv[i] = new Vector2(
                    controlPoints[i].x / texture.width,
                    controlPoints[i].y / texture.height
                );
            }

            // Define two triangles
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;

            triangles[3] = 2;
            triangles[4] = 3;
            triangles[5] = 1;

            // Assign to mesh
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            // Set texture
            //GetComponent<Renderer>().material.mainTexture = texture;
        }

        private void UpdateMeshVertices()
        {
            Vector3[] vertices = new Vector3[4];

            // Update vertices based on current control points
            for (int i = 0; i < 4; i++)
            {
                vertices[i] = new Vector3(controlPointsCurrent[i].x, controlPointsCurrent[i].y, 0);
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }

    }
}
