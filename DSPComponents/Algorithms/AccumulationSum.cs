using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm  //as prefix sum
    {///calculate every sample behind until it stand
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> s = new List<float>();
            float acc_sum = 0;

            for (int i = 0; i < InputSignal.Samples.Count; i++)   
            {
                if (i == 0)
                {
                    acc_sum = InputSignal.Samples[i];                                                                ///do nothing
                    s.Add(acc_sum);

                }
                else
                {
                    acc_sum = acc_sum + InputSignal.Samples[i];  
                    s.Add(acc_sum);
                }


            }

            OutputSignal = new Signal(s, false);

            
        }
    }
}