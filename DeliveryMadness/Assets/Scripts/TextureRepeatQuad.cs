using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToolBoxEngine
{
    [ExecuteAlways]
    public class TextureRepeatQuad : MonoBehaviour
    {
        private Vector3 _currentLocalScale = Vector3.zero;
        private Renderer _renderer = null;
        private MeshFilter _meshFilter = null;

        private void Start()
        {
            _currentLocalScale = transform.localScale;
            _renderer = GetComponent<Renderer>();
            _meshFilter = GetComponent<MeshFilter>();
            _RefreshMesh();
        }

        private void Update()
        {
            if (transform.localScale != _currentLocalScale)
            {
                _RefreshMesh();
                _currentLocalScale = transform.localScale;
            }
        }

        private void _RefreshMesh()
        {
            //Update Texture Wrap Mode to Repeat
            if (null != _renderer.sharedMaterial)
            {
                if (null != _renderer.sharedMaterial.mainTexture)
                {
                    if (_renderer.sharedMaterial.mainTexture.wrapMode != TextureWrapMode.Repeat)
                    {
                        _renderer.sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
                    }
                }
            }

            //Pickup Mesh according to PlayMode
            //In EditMode we cannot pickup the mesh copy directly
            //We need to clone shared mesh to update it

            Mesh mesh = null;
            if (Application.isPlaying)
            {
                mesh = _meshFilter.mesh;
            }
            else
            {
                if (null != _meshFilter)
                {
                    if (null != _meshFilter.sharedMesh)
                    {
                        string meshId = "Mesh" + GetInstanceID();
                        if (_meshFilter.sharedMesh.name != meshId)
                        {
                            Mesh meshCopy = Instantiate(_meshFilter.sharedMesh);
                            meshCopy.name = meshId;
                            _meshFilter.sharedMesh = meshCopy;
                        }
                        else
                        {
                            mesh = _meshFilter.sharedMesh;
                        }
                    }
                }
            }

            //Update Mesh UVs according to GameObject scale
            if (null != mesh)
            {
                mesh.uv = SetupUVMap();
            }
        }

        private Vector2[] SetupUVMap()
        {
            float scaleX = transform.localScale.x;
            float scaleY = transform.localScale.y;

            Vector2[] meshUvs = new Vector2[4];
            meshUvs[0] = new Vector2(0f, 0f);
            meshUvs[1] = new Vector2(scaleX, 0f);
            meshUvs[2] = new Vector2(0f, scaleY);
            meshUvs[3] = new Vector2(scaleX, scaleY);

            return meshUvs;
        }
    }
}
