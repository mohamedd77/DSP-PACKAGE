using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            List<float> samples1 = new List<float>();
            Signal OutputMultipliedSignal = new Signal(samples1, false);

         ///   first multiply sig2 * -1
            for (int a = 0; a < InputSignal2.Samples.Count; a++)
            {
                samples1.Add(-1 * InputSignal2.Samples[a]);

            }
                    List<float> samples = new List<float>();

            /// second add sig1 + new sig2 

            for (int b = 0; b < InputSignal1.Samples.Count; b++)
            {
                samples.Add(InputSignal1.Samples[b] + OutputMultipliedSignal.Samples[b]);


            }
                 OutputSignal = new Signal(samples, false);


        }
    }
}