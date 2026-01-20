using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LargeFileSorter
{
    internal class LargeFileSorter
    {
        string _inputFilePath = "";
        string _outputFilePath = "";
        string _folderName = "";
        long _fileLength;
        long _numChunks;
        int _numMemChunks;

        public LargeFileSorter(string input, string output)
        {
            _inputFilePath = input;
            _outputFilePath = output;

            _fileLength = new FileInfo(_inputFilePath).Length;
            string? folderName = Path.GetDirectoryName(_outputFilePath);
            if (folderName is null)
                throw new ArgumentException($"outputFilePath: {_outputFilePath} is not contains directory!");
            _folderName = folderName;
        }

        public async Task SortFileAsync()
        {
            Configure();

            await SplitFileAndSort();
            if (_numChunks > 1)
                MergeChunks();
        }

        private void Configure()
        {
            if (_fileLength > Constants.DEFAULT_CHUNK_CAPACITY)
            {
                _numChunks = _fileLength / Constants.DEFAULT_CHUNK_CAPACITY + 1;
                if (_numChunks == 0)
                    _numChunks = 1;
            }
            else
                _numChunks = 1;

            int memChunksCount = (int)Math.Ceiling((double)_fileLength / Constants.DEFAULT_MEM_CHUNK_CAPACITY);
            _numMemChunks = memChunksCount > Constants.DEFAULT_TASK_COUNT ? Constants.DEFAULT_TASK_COUNT : memChunksCount;
        }

        private async Task SplitFileAndSort()
        {
            int currentChunk = 0;
            long currentChunkSize = 0;

            Task? lastTask = null;
            MemoryChunks memChunks = new MemoryChunks(_numMemChunks);

            string chunkName = Path.Combine(_folderName, $"{currentChunk}.chnk");
            using (StreamReader reader = new StreamReader(_inputFilePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    Row? row = Row.Create(line);
                    if (row is null)
                        continue;
                    memChunks.Add(row);
                    currentChunkSize += line.Length;
                    if (currentChunkSize >= Constants.DEFAULT_CHUNK_CAPACITY)
                    {
                        if (lastTask != null)
                            await lastTask;
                        lastTask = SortMemoryChunksAsync(memChunks, chunkName);
                        memChunks = new MemoryChunks(_numMemChunks);

                        currentChunk++;
                        currentChunkSize = 0;

                        chunkName = Path.Combine(_folderName, $"{currentChunk}.chnk");
                    }
                }
                _numChunks = currentChunk + 1;
            }
            if (lastTask != null)
                await lastTask;
            await SortMemoryChunksAsync(memChunks, chunkName);
        }

        private async Task SortMemoryChunksAsync(MemoryChunks memChunks, string output)
        {
            SortMemoryChunks sortMemChunks = new SortMemoryChunks(memChunks);
            await sortMemChunks.SortAll();

            if (_numMemChunks > 1)
            {
                MergeMemoryChunks(memChunks, output);
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(_outputFilePath, false, Encoding.ASCII, Constants.BUFF_SIZE))
                {
                    foreach (var row in memChunks[0])
                        writer.WriteLine(row);
                }
            }
        }

        private void MergeMemoryChunks(MemoryChunks memChunks, string outFileName)
        {
            int[] indices = new int[memChunks.NumChunks];
            using (StreamWriter writer = new StreamWriter(outFileName, false, Encoding.ASCII, Constants.BUFF_SIZE))
            {
                while (true)
                {
                    Row? min = null;
                    int minIndex = -1;
                    for (int i = 0; i < memChunks.NumChunks; i++)
                    {
                        if (indices[i] >= memChunks[i].Count)
                            continue;
                        var curr = memChunks[i][indices[i]];
                        if (min is null || curr < min)
                        {
                            min = curr;
                            minIndex = i;
                        }
                    }
                    if (min is null)
                        break;
                    writer.WriteLine(min);
                    indices[minIndex]++;
                }
            }
        }

        private void MergeChunks()
        {
            using (StreamWriter writer = new StreamWriter(_outputFilePath, false, Encoding.ASCII, Constants.BUFF_SIZE))
            {
                RowReader[] readers = new RowReader[_numChunks];
                SetRow rows = new SetRow();
                for (int i = 0; i < _numChunks; i++)
                {
                    string chunkName = Path.Combine(_folderName, $"{i}.chnk");
                    RowReader? reader = RowReader.Create(chunkName, i);
                    if (reader is null) 
                        continue;
                    readers[i] = reader;
                    var nextRow = reader.NextRow();
                    if (nextRow is null) continue;
                    rows.Add(nextRow);
                }

                while (rows.Count > 0)
                {
                    var row = rows.Min();
                    if(row is null) continue;
                    writer.WriteLine(row);
                    long chunk = row.Indexes[0];
                    rows.Remove(row);
                    var nextRow = readers[chunk].NextRow();
                    if (nextRow is null) continue;
                    rows.Add(nextRow);
                }

                for (int i = 0; i < _numChunks; i++)
                {
                    readers[i].Dispose();
                    File.Delete(Path.Combine(_folderName, $"{i}.chnk"));
                }
            }
        }
    }
}
