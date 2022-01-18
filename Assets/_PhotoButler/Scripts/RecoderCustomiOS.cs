using System.Collections;
using System.Collections.Generic;
using pmjo.NextGenRecorder;
using UnityEngine;
using UnityEngine.Rendering;

public class RecoderCustomiOS : Recorder.VideoRecorderBase
{
    public RenderTexture _renderTexture;
    private RenderTexture m_RenderTexture;

    private int _videoWitdh = 1920;
    private int _videoHeight = 1080;

    private void Start()
    {
        CreateVideoTexture();
    }

    private void Update()
    {
        m_RenderTexture = _renderTexture;
    }

    private void CreateVideoTexture()
    {
        if (m_RenderTexture != null)
        {
            gameObject.GetComponent<Camera>().targetTexture = null;

            DestroyImmediate(m_RenderTexture);
        }

        m_RenderTexture = new RenderTexture(_videoWitdh, _videoHeight, 24, RenderTextureFormat.Default);
        m_RenderTexture.Create();

        RecordingTexture = m_RenderTexture;

        gameObject.GetComponent<Camera>().targetTexture = m_RenderTexture;

        CommandBuffer myCommandBuffer = new CommandBuffer();
        CommandBufferBlitRecordingTexture(myCommandBuffer);
        gameObject.GetComponent<Camera>().AddCommandBuffer(CameraEvent.AfterEverything, myCommandBuffer);
    }
    
    public void SetTextureDimensions(int width, int height)
    {
        _videoWitdh = width;
        _videoHeight = height;

        CreateVideoTexture();
    }

}
