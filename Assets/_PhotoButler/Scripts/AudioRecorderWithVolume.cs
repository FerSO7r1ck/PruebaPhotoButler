using UnityEngine;
using System.Runtime.InteropServices;

namespace pmjo.NextGenRecorder
{
    [AddComponentMenu("Next Gen Recorder/Audio Recorder With Volume")]
    public class AudioRecorderWithVolume : Recorder.AudioRecorderBase
    {
        [Range(0, 1.0f)]
        public float volume = 1.0f;
        private int m_SampleRate = 0;

        void Awake()
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();

            if (config.sampleRate >= 8000)
                m_SampleRate = config.sampleRate;
            else
                Debug.Log("Only sample rate >= 8000 is supported");
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            // Capture audio before volume changed
            if (m_SampleRate > 0)
            {
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

                if (handle.IsAllocated)
                {
                    AppendInterleavedAudio(handle.AddrOfPinnedObject(), data.Length, channels, m_SampleRate);
                    handle.Free();
                }
            }

            // Apply volume to audio listener
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = data[i] * volume;
            }
        }

        public void SetVolume(float newVolume)
        {
            volume = newVolume;
        }
    }
}