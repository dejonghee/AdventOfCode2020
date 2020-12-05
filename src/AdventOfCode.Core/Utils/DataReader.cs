using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Core.Utils
{
    /// <summary>
    /// Line based data reader.
    /// </summary>
    public class DataReader : IDisposable
    {
        private readonly string _fileName;
        private readonly StreamReader _streamReader;

        public DataReader(string fileName)
        {
            _fileName = fileName;
            _streamReader = File.OpenText(_fileName);
        }

        public async IAsyncEnumerable<string> GetDataAsync()
        {
            string line;
            while ((line = await _streamReader.ReadLineAsync()) != null)
            {
                yield return line;
            }
        }

        public void Dispose()
        {
            _streamReader.Dispose();
        }
    }
}
