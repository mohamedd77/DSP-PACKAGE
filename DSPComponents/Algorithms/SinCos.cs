using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
           
            samples = new List<float>();
            float f = AnalogFrequency / SamplingFrequency;   // normalized f = f/fs

            if (SamplingFrequency >= 2 * AnalogFrequency)
            {
                if (type == "sin")
                {

                    for (int n = 0; n < SamplingFrequency; n++)
                    {

                        /// A sin(2pi*n*f + PhaseShift) or A sin(2pi*n*(f/fs) + PhaseShift) 
                        float res =    (A * (float)Math.Sin(2 * Math.PI * n * (f) + PhaseShift));
                        samples.Add(res);
                    }


                }

                else if (type == "cos")
                {
                    for (int n = 0; n < SamplingFrequency; n++)
                    {
                        /// A Cos(2pi*n*f + PhaseShift) or A Cos(2pi*n*(f/fs) + PhaseShift) 
                        /// 
                        float res = (A * (float)Math.Cos(2 * Math.PI * n * (f) + PhaseShift));
                        samples.Add(res);

                    }
                }
            }
        }
    }
}
