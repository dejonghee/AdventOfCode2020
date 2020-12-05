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

        public IEnumerable<string> GetData()
        {
            string line;
            while ((line = _streamReader.ReadLine()) != null)
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
