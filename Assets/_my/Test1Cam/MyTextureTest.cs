using System.Collections;
using System.Collections.Generic;
using RenderHeads.Media.AVProMovieCapture;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MyTextureTest : MonoBehaviour
{
    [SerializeField] int _textureWidth = 1024;
    [SerializeField] int _textureHeight = 1024;
    [SerializeField] CaptureFromTexture _movieCapture = null;
    [SerializeField] Camera _camera = null;
    [SerializeField] RawImage _rawImage = null;
    
    // [SerializeField] 
    private RenderTexture _texture;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {
        // RenderTextureReadWrite readWrite = QualitySettings.activeColorSpace == ColorSpace.Gamma ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB;
        // // _texture = new RenderTexture(_textureWidth, _textureHeight, 0, RenderTextureFormat.ARGB32, readWrite);
        // _texture = new RenderTexture(_textureWidth, _textureHeight, 0, RenderTextureFormat, readWrite);
        // _texture.filterMode = FilterMode.Bilinear;
        // _texture.Create();

        // _texture = GetRTWithQuality(_textureWidth, _textureHeight);
        
        if (_camera)
        {
            _camera.targetTexture = _texture;
        }
        
        // _rawImage.texture = _texture;
        
        if (_movieCapture)
        {
            // _movieCapture.SetSourceTexture(_texture);
            _movieCapture.SetSourceTexture(_rawImage.texture);
        }
    }
    
    private void OnDestroy()
    {
        if (_texture != null)
        {
            RenderTexture.Destroy(_texture);
            _texture = null;
        }
    }
    
    public static RenderTexture GetRTWithQuality(int width, int height, bool focusSRGB = false)
    {
        RenderTextureDescriptor des;
        var format = GetAppRenderFormat(SystemInfo.graphicsDeviceType);
        if (QualitySettings.antiAliasing > 0)
        {
            des = new RenderTextureDescriptor(width, height, format, 24)
            {
                dimension = TextureDimension.Tex2D,
                graphicsFormat = GraphicsFormat.R8G8B8A8_SRGB,
                colorFormat = RenderTextureFormat.ARGB32,
                autoGenerateMips = true,
                useMipMap = true,
                sRGB = focusSRGB || QualitySettings.activeColorSpace == ColorSpace.Linear,
                memoryless = RenderTextureMemoryless.None,
                msaaSamples = QualitySettings.antiAliasing
            };
        }
        else
        {
            des = new RenderTextureDescriptor(width, height, format, 24)
            {
                dimension = TextureDimension.Tex2D,
                graphicsFormat = GraphicsFormat.R8G8B8A8_SRGB,
                colorFormat = RenderTextureFormat.ARGB32,
                autoGenerateMips = false,
                useMipMap = false,
                sRGB = focusSRGB || QualitySettings.activeColorSpace == ColorSpace.Linear,
                memoryless = RenderTextureMemoryless.None,
                msaaSamples = 1
            };
        }

        return RenderTexture.GetTemporary(des);
    }

    public static RenderTextureFormat GetAppRenderFormat(GraphicsDeviceType type)
    {
        return IsMacOS ? RenderTextureFormat.ARGB32 : RenderTextureFormat.Default;
    }
    public static bool IsMacOS => Application.platform == RuntimePlatform.OSXEditor ||
                                  Application.platform == RuntimePlatform.OSXPlayer;
}
