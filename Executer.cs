using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomRotate
{
    class Executer
    {
        public static void Run(string fin, string fout, int itr, Action<string> log)
        {
            log("Start.");
            if (!File.Exists(fin))
            {
                log("Input file does not exist: " + fin + ".");
                log("Abort.");
                log("");
                return;
            }
            if (File.Exists(fout))
            {
                log("Output file already exists: " + fout + ". Overriding.");
                File.Delete(fout);
            }
            AtomData[] data;
            {
                log("Reading input file...");
                try
                {
                    data = CsvReader.Read(fin);
                }
                catch (Exception e)
                {
                    log("Input file error: " + e.Message + ".");
                    log("Abort.");
                    return;
                }
                log("Succeed. Atom number = " + data.Length.ToString() + ".");
            }

            try
            {
                using (var w = new StreamWriter(fout, false, Encoding.ASCII))
                {
                    log("Writing file header...");
                    w.WriteLine("a10A1 - Ala10Na+ Glob");
                    w.WriteLine(itr.ToString());
                    w.WriteLine(data.Length.ToString());
                    w.WriteLine("ang");
                    w.WriteLine("calc");
                    w.WriteLine("1.0000");

                    log("Calculation starts... Iteration = " + itr.ToString() + ".");
                    
                    Random r = new Random();
                    for (int i = 0; i < itr; ++i)
                    {
                        var rot = r.NextRotation();
                        for (int j = 0; j < data.Length; ++j)
                        {
                            var atom = data[j];
                            rot.RotateAtom(ref atom);
                            WriteAtomData(w, atom);
                        }
                        if (i != itr - 1)
                        {
                            w.WriteLine(data.Length);
                        }
                    }
                    log("Finished.");
                }
            }
            catch (Exception e)
            {
                log("Error in writing output file. " + e.Message + ".");
                log("Abort.");
                return;
            }
        }

        private static void WriteAtomData(StreamWriter w, AtomData data)
        {
            w.WriteLine("{0:0.0000} {1} {2} {3} {4}", data.X, data.Y, data.Z, data.Weight, data.Charge);
        }
    }
}
