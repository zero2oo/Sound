using UnityEngine;
using NAudio.Wave;

public class AudioRecorder : MonoBehaviour
{
    // private BufferedWaveProvider bufferedWaveProvider;
    // private WaveInEvent waveSource;
    // private WaveOutEvent waveOut;
    // private WaveFileWriter waveFile;
    //
    // void Start()
    // {
    //     waveSource = new WaveInEvent();
    //     waveSource.DeviceNumber = 0; // Set the device number to the Line In port number
    //     waveSource.WaveFormat = new WaveFormat(44100, 1);
    //
    //     bufferedWaveProvider = new BufferedWaveProvider(waveSource.WaveFormat);
    //     bufferedWaveProvider.DiscardOnBufferOverflow = true;
    //
    //     waveOut = new WaveOutEvent();
    //     waveOut.Init(bufferedWaveProvider);
    //     waveOut.Play();
    //
    //     waveFile = new WaveFileWriter(@"C:\Temp\Test.wav", waveSource.WaveFormat);
    //
    //     waveSource.DataAvailable += (s, a) =>
    //     {
    //         bufferedWaveProvider.AddSamples(a.Buffer, 0, a.BytesRecorded);
    //         waveFile.Write(a.Buffer, 0, a.BytesRecorded);
    //         waveFile.Flush();
    //     };
    //
    //     waveSource.StartRecording();
    // }
    //
    // void OnDestroy()
    // {
    //     waveSource.StopRecording();
    //     waveOut.Stop();
    //
    //     if (waveFile != null)
    //     {
    //         waveFile.Dispose();
    //         waveFile = null;
    //     }
    // }
}