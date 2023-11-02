using System;
using UnityEngine;
using NAudio.Wave;

public class AudioRecorder : MonoBehaviour
{
    WaveInEvent waveSource = null;
    WaveFileWriter waveFile = null;

    void Start()
    {
        waveSource = new WaveInEvent();
        waveSource.DeviceNumber = 0; // Set the device number to the Line In port number
        waveSource.WaveFormat = new WaveFormat(44100, 1);

        waveSource.DataAvailable += waveSource_DataAvailable;
        waveSource.RecordingStopped += waveSource_RecordingStopped;

        waveFile = new WaveFileWriter(@"C:\Temp\Test.wav", waveSource.WaveFormat);

        waveSource.StartRecording();
    }

    void waveSource_DataAvailable(object sender, WaveInEventArgs e)
    {
        if (waveFile != null)
        {
            waveFile.Write(e.Buffer, 0, e.BytesRecorded);
            waveFile.Flush();
        }
    }

    void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
    {
        if (waveSource != null)
        {
            waveSource.Dispose();
            waveSource = null;
        }

        if (waveFile != null)
        {
            waveFile.Dispose();
            waveFile = null;
        }
    }
}