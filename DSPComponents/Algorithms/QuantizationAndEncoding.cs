using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            OutputIntervalIndices = new List<int>();
            OutputSamplesError = new List<float>();
            OutputEncodedSignal = new List<string>();

            /// We get number of levels & number of bits as follow:

            
            if (InputLevel == 0) //equals zero
            { 
                InputLevel = (int)Math.Pow(2, InputNumBits);  } // levels = log(base2) [number of bits]
            if (InputNumBits == 0) //
            {
                InputNumBits = (int)Math.Log(InputLevel, 2);  } // number of bits = 2 power [number of levels]
            
            float Range = InputSignal.Samples.Max() - InputSignal.Samples.Min();   // x(max) - x(min)
            float delta = Range / InputLevel;         //(1) iesoulation(delta)= x(max)-x(min)/ numer of levels

            List<float> level = new List<float>(); 
            level.Add(InputSignal.Samples.Min());
            for (int n = 1; n <= InputLevel; n++)
            {
                level.Add(level[n - 1] + delta);  //(2) add delta for input samples until reach num of levels
            }
            List<float> midpoint = new List<float>();
            for (int n = 0; n < InputLevel; n++)
            {
                midpoint.Add((level[n] + level[n + 1]) / 2); //(3) midpoint for the interval levels 
            }

            List<float> s = new List<float>();
            for (int k = 0; k < InputSignal.Samples.Count; k++) //samples count  
            {
                for (int x = 0; x < InputLevel; x++)  // levels count 
                {
                    //if input sample lies between level Begin and its End we return its level Midpoint value q(n) ///quantized error
                    if (InputSignal.Samples[k] >= level[x] && InputSignal.Samples[k] <= level[x + 1] + 0.0001)
                    {
                        s.Add(midpoint[x]);
                        OutputEncodedSignal.Add(Convert.ToString(x, 2).PadLeft(InputNumBits, '0'));                          // convert samples of base2 (the EncodedSignal)
                        OutputIntervalIndices.Add(x + 1);                                                                   // which level interval found in
                        break;
                    }
                }
            }
            OutputQuantizedSignal = new Signal(s, false);



            List<float> s1 = new List<float>();
            // eq_error = Q(n) - x(n)     /// means quantized error - sample input

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float q_error = OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i];
                s1.Add(q_error);
            }
            OutputSamplesError = s1; //EQ(n)
           
        }
    }
}