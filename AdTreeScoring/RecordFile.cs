﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Datastructures
{
    class RecordFile
    {
        public RecordFile()
        {
        }

        public void ReadRecord(string filePath, bool hasHeader = true, char delimiter = ',')
        {
            StreamReader sr;
            try
            {
                sr = new StreamReader(filePath);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            this.hasHeader = hasHeader;
            if (hasHeader && !sr.EndOfStream)
            {
                string header_line = sr.ReadLine();
                header = new List<string>(header_line.Split(delimiter));
            }

            while (sr.EndOfStream == false)
            {
                string read_line = sr.ReadLine();
                Record line = new Record(read_line.Split(delimiter));
                Records.Add(line);
            }
            sr.Close();

        }

        public void Print()
        {
            // ヘッダの出力
            Console.WriteLine(string.Join(",", Header));

            // データの出力
            Records.ForEach(line => Console.WriteLine(string.Join(",", line)));

        }

        public int Size()
        {
            return records.Count;
        }


        private List<string> header = new List<string>();
        public List<string> Header
        {
            get
            {
                return header;
            }
        }

        private List<Record> records = new List<Record>();
        public List<Record> Records
        {
            get
            {
                return records;
            }
        }

        private bool hasHeader = false;
        public bool HasHeader
        {
            get
            {
                return hasHeader;
            }
        }

    }
}
