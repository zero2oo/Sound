using System;
using System.Collections.Generic;
using NAudio.Wave;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Text txtIsSupported, txtLog;
    public TMP_Dropdown driverDD, fromChannelDD, toChannelDD;

    private int _numberOfChannels;
    private float[] _samples;
    private readonly int _sample = 44100;
    
    private AsioOut _asioOut;
    private BufferedWaveProvider _bufferedWaveProvider;
    private SavingWaveProvider _savingWaveProvider;
    private WaveFileWriter _writer;
    private WaveFormat _waveFormat = new WaveFormat(44100, 2);

    private readonly string _savePath = @"C:\Test Audio Recorder\Test.wav";
    
    
    private void Awake()
    {
        txtIsSupported.text = "Asio is " + (AsioOut.isSupported() ? "supported" : "not supported");
        driverDD.options.Add(new TMP_Dropdown.OptionData("None"));

        foreach (var driverName in AsioOut.GetDriverNames())
        {
            driverDD.options.Add(new TMP_Dropdown.OptionData(driverName)); 
        }

        driverDD.onValueChanged.AddListener(OnDropdownValueChanged);
        // asioOut.init = new WaveFormat(44100, 24, 2); // Định dạng 24 bit
        // var waveFileWriter = new WaveFileWriter("", null);

    }

    private void OnDropdownValueChanged(int index)
    {
        txtLog.text = "OnDropdownValueChanged: " + index;
        if (index == 0) return;

        try
        {
            var asioOut = new AsioOut(driverDD.options[index].text);
            for (int i = 0; i < asioOut.DriverInputChannelCount; i++)
            {
                fromChannelDD.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
                toChannelDD.options.Add(new TMP_Dropdown.OptionData(i.ToString()));
            }
        }
        catch (Exception e)
        {
            txtLog.text = "OnDropdownValueChanged: " + e.Message;
            throw;
        }
    }

    public void ClickBtnRecord()
    {
        if (_asioOut == null)
        {
            txtLog.text = "Asio out null";
            return;
        }

        _asioOut.InputChannelOffset = fromChannelDD.value;
        var recordChannelCount = toChannelDD.value - fromChannelDD.value + 1;
        
        
        _bufferedWaveProvider = new BufferedWaveProvider(_waveFormat);
        _writer = new WaveFileWriter(_savePath, _waveFormat);
        // _savingWaveProvider = new SavingWaveProvider(_bufferedWaveProvider, _savePath);
        
        _asioOut.InitRecordAndPlayback(_bufferedWaveProvider, recordChannelCount, _sample);
        
        _asioOut.AudioAvailable += OnAsioOutAudioAvailable;
        _asioOut.Play();
        
        
        //SavingWaveProvider
        // var savingWaveProvider = new SavingWaveProvider(_bufferedWaveProvider, _savePath);
    }

    private void OnAsioOutAudioAvailable(object sender, AsioAudioAvailableEventArgs e)
    {
        e.GetAsInterleavedSamples(_samples);
        _writer.WriteSamples(_samples, 0, _samples.Length);

        // _bufferedWaveProvider.AddSamples(, 0, _samples.Length);
        //
        // _bufferedWaveProvider.add
    }

    public void ClickBtnStop()
    {
        if (_asioOut == null)
        {
            txtLog.text = "Asio out null";
            return;
        }
        
        _asioOut.AudioAvailable -= OnAsioOutAudioAvailable;
        _asioOut.Stop();
    }

    private void OnDestroy()
    {
        _asioOut.Dispose();
        _writer.Dispose();
        _writer = null;
    }

    [ContextMenu("AddDriverName")]
    public void AddDriverName()
    {
    }
}

public class SavingWaveProvider : IWaveProvider, IDisposable
{
    private readonly IWaveProvider _sourceWaveProvider;
    private readonly WaveFileWriter _writer;
    private bool _isWriterDisposed;

    public SavingWaveProvider(IWaveProvider sourceWaveProvider, string wavFilePath)
    {
        this._sourceWaveProvider = sourceWaveProvider;
        _writer = new WaveFileWriter(wavFilePath, sourceWaveProvider.WaveFormat);
    }

    public int Read(byte[] buffer, int offset, int count)
    {
        var read = _sourceWaveProvider.Read(buffer, offset, count);
        if (count > 0 && !_isWriterDisposed)
        {
            _writer.Write(buffer, offset, read);
        }
        if (count == 0)
        {
            Dispose(); // auto-dispose in case users forget
        }
        return read;
    }

    public WaveFormat WaveFormat { get { return _sourceWaveProvider.WaveFormat; } }

    public void Dispose()
    {
        if (!_isWriterDisposed)
        {
            _isWriterDisposed = true;
            _writer.Dispose();
        }
    }
}

