using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> Out1 = new List<float>();   
            List<float> Out2 = new List<float>();
            int N = InputSignal.Samples.Count;
            for (int i = 1; i < N; i++)
            {
                    // FirstDerivative  : x(n) - x(n-1)
                    Out1.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);  
            }  
            for (int j = 1; j < N; j++)                //begin counter from 1 becuase 0-1 =-1 and that not legal 
            {
                if (j == N - 1)                         //at last index n not allowed for n+1 as in the rule below
                {
                    Out2.Add(0);
                    
                }
                else
                {   // SecondDerivative : x(n+1) - 2* x(n) + x(n-1)
                  
                    Out2.Add(InputSignal.Samples[j + 1] - (2 * InputSignal.Samples[j]) + InputSignal.Samples[j - 1]);
                    
                }
            }
            FirstDerivative = new Signal(Out1, false);
            SecondDerivative = new Signal(Out2, false);
        }
    }
}
