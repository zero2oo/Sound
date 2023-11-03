using UnityEngine;
using NAudio.Wave;
using UnityEngine.UI;

public class AudioRecorder : MonoBehaviour
{
    public Text txtDevice, txtDeviceCount;
    private int currentDevice = -1;

    WaveInEvent waveSource = null;
    WaveFileWriter waveFile = null;

    void Start()
    {
        txtDeviceCount.text = WaveInEvent.DeviceCount.ToString();
    }

    public void OnClickBtnNext()
    {
        currentDevice++;

        if (currentDevice < WaveInEvent.DeviceCount)
        {
            SetDeviceInfo(currentDevice);
            return;
        }

        if (WaveInEvent.DeviceCount == 0)
        {
            currentDevice = -1;
            txtDevice.text = "not found";
            return;
        }

        currentDevice = 0;
        SetDeviceInfo(currentDevice);
    }

    private void SetDeviceInfo(int index)
    {
        var info = WaveInEvent.GetCapabilities(currentDevice);
        txtDevice.text = "ProductName: " + info.ProductName + ", NameGuid: " + info.NameGuid + ", ManufacturerGuid: " + info.ManufacturerGuid +
                         ", ProductGuid: " + info.ProductGuid + ", Channels: " + info.Channels;

    }

    public void OnClickBtnRecord()
    {
        if (currentDevice < 0) return;
        
        waveFile = new WaveFileWriter(@"C:\Sound Recorder Test\Test.wav", waveSource.WaveFormat);
        waveSource = new WaveInEvent();
        waveSource.DeviceNumber = currentDevice; // Set the device number to the Line In port number
        waveSource.WaveFormat = new WaveFormat(44100, 1);

        waveSource.DataAvailable += waveSource_DataAvailable;
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

    public void OnClickBtnStop()
    {
        if (waveSource == null) return;
        
        waveSource.RecordingStopped += waveSource_RecordingStopped;
        waveSource.StopRecording();
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