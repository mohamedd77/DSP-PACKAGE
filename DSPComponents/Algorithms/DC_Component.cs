using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

       /// Removing the mean of the signal display the result.

        public override void Run()
        {
            
            float mean = 0; float sum = 0;
            List<float> output = new List<float>();

            /// mean= sum / count
            
            for (int n = 0; n < InputSignal.Samples.Count; n++)
            {
                sum = sum + InputSignal.Samples[n];
            }
            mean = sum / InputSignal.Samples.Count; //which is avarage

            
            for (int n = 0; n < InputSignal.Samples.Count; n++)
            {
                output.Add(InputSignal.Samples[n] - mean);  ///Removing the mean 
            }
            OutputSignal = new Signal(output, false);
        }
    }
}
