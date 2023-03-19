using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniclerAI
{
    internal class AudioSplitter
    {
        public List<string> SplitAudio(string inputFilePath, string outputDirectory, TimeSpan segmentDuration)
        {
            List<string> splitFilePaths = new List<string>();
            using (var reader = new Mp3FileReader(inputFilePath))
            {
                int frameCount = (int)(segmentDuration.TotalSeconds * reader.WaveFormat.SampleRate * reader.WaveFormat.BlockAlign);
                int segmentNumber = 1;
                int totalBytesRead = 0;
                int bytesRead;
                byte[] buffer = new byte[8192];

                LameMP3FileWriter writer = null;
                try
                {
                    while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (writer == null)
                        {
                            // Create a new output file for the current segment
                            string outputFilePath = Path.Combine(outputDirectory, $"{Guid.NewGuid()}_{segmentNumber}.mp3");
                            writer = new LameMP3FileWriter(outputFilePath, reader.WaveFormat, LAMEPreset.STANDARD_FAST);
                            splitFilePaths.Add(outputFilePath);
                        }

                        int bytesToWrite = Math.Min(bytesRead, frameCount - totalBytesRead);
                        writer.Write(buffer, 0, bytesToWrite);
                        totalBytesRead += bytesToWrite;

                        if (totalBytesRead >= frameCount)
                        {
                            // Close the current segment and move to the next one
                            writer.Dispose();
                            writer = null;
                            segmentNumber++;
                            totalBytesRead = 0;
                        }
                        else if (bytesToWrite < bytesRead)
                        {
                            // If there are remaining bytes in the buffer, adjust the reader position
                            int bytesRemaining = bytesRead - bytesToWrite;
                            reader.Position -= bytesRemaining;
                        }
                    }
                }
                finally
                {
                    // Ensure the writer is properly disposed
                    writer?.Dispose();
                }
            }

            return splitFilePaths;
        }

    }
}
