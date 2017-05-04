using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomRotate
{
    class CsvReader
    {
        public static AtomData[] Read(string filepath)
        {
            List<AtomData> atoms = new List<AtomData>();
            bool isNewData = false;
            bool countMismatch = false;
            using (var r = new StreamReader(filepath))
            {
                while (!r.EndOfStream)
                {
                    var line = r.ReadLine().Split(new[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);
                    if (line.Length == 0)
                    {
                        continue;
                    }
                    if (isNewData)
                    {
                        atoms.Clear();
                        isNewData = false;
                    }
                    if (line.Length == 1)
                    {
                        countMismatch = false;
                        if (atoms.Count != Int32.Parse(line[0]))
                        {
                            countMismatch = true;
                        }
                        isNewData = true;
                        continue;
                    }
                    if (line.Length == 5)
                    {
                        AtomData atom;
                        if (ParseLine(line, out atom))
                        {
                            atoms.Add(atom);
                        }
                    }
                }
            }
            if (countMismatch)
            {
                throw new InvalidDataException("Atom number mismatch");
            }
            return atoms.ToArray();
        }

        private static bool ParseLine(string[] vals, out AtomData ret)
        {
            float[] fvals = new float[5];
            for (int i = 0; i < fvals.Length; ++i)
            {
                if (!Single.TryParse(vals[i], out fvals[i]))
                {
                    ret = new AtomData();
                    return false;
                }
            }
            ret = new AtomData();
            ret.X = fvals[0];
            ret.Y = fvals[1];
            ret.Z = fvals[2];
            ret.Weight = CalculateAtomWeight(fvals[3]);
            ret.Charge = fvals[4];
            return true;
        }

        private static readonly Dictionary<int, float> _Weight = new Dictionary<int, float>()
        {
            { 1, 1 },
            { 6, 12 },
            { 7, 14 },
            { 8, 16 },
        };
        private static float CalculateAtomWeight(float number)
        {
            float ret;
            if (!_Weight.TryGetValue((int)number, out ret))
            {
                throw new InvalidDataException("Unknown atom number: " + number);
            }
            return ret;
        }
    }
}
