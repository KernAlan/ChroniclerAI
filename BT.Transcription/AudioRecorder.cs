using System;
using System.IO;
using NAudio.Wave;
using NAudio.Lame;
using System.Windows;

namespace ChroniclerAI
{
    class AudioRecorder
    {
        private readonly string _outputPath;
        private WaveInEvent? _waveIn;
        private LameMP3FileWriter? _mp3Writer;

        public AudioRecorder(string outputPath)
        {
            _outputPath = outputPath;
        }

        public void StartRecording()
        {
            if (_waveIn != null || _mp3Writer != null)
            {
                MessageBox.Show("Already recording!");
                return;
            }

            _waveIn = new WaveInEvent();
            _waveIn.WaveFormat = new WaveFormat(44100, 1);

            _mp3Writer = new LameMP3FileWriter(_outputPath, _waveIn.WaveFormat, LAMEPreset.ABR_128);

            _waveIn.DataAvailable += (s, e) => _mp3Writer?.Write(e.Buffer, 0, e.BytesRecorded);
            _waveIn.StartRecording();
        }

        public void StopRecording()
        {
            if (_waveIn == null || _mp3Writer == null)
            {
                MessageBox.Show("Not recording!");
                return;
            }

            _waveIn.StopRecording();
            _waveIn.Dispose();
            _waveIn = null;

            _mp3Writer.Dispose();
            _mp3Writer = null;
        }
    }
}
