using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public override void Run()
        {   List<float> Finalresult = new List<float>();
            for (int n = 0; n < InputSignal.Samples.Count; n++)            ///InputSignal.Samples.Count ===> N
            {
                double Dct_Equation = 0;
                for (int y = 0; y < InputSignal.Samples.Count; y++)
                {
                    double part1 = Math.Sqrt(2f / (float)InputSignal.Samples.Count);
                    double part2 = (InputSignal.Samples[y] * Math.Cos((((2 * n) - 1) * ((2 * y) - 1) * (Math.PI)) / (4 * InputSignal.Samples.Count)));
                    Dct_Equation += (part1 * part2);
                    //Finalresult.Add(Dct_Equation) += (part1*part2);
                }
                Finalresult.Add((float)(Dct_Equation));
            }
            OutputSignal = new Signal(Finalresult, false);
            /*
            StreamWriter str = new StreamWriter("mSamples.txt", false);
            for (int i = 0; i < 5; i++)
            {
                str.WriteLine("value:" + OutputSignal.Samples[i]);
            }
            str.Close();
            * */
        }
    }
}
