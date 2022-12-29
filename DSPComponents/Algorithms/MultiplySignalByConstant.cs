using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        /// multiply a signal by a constant value

        public override void Run()
        {
            List<float> samples1 = new List<float>();
            OutputMultipliedSignal = new Signal(samples1, false);

            for (int n = 0; n < InputSignal.Samples.Count; n++)
            {
                samples1.Add(InputSignal.Samples[n] * InputConstant);


            }
        }
    }
}
