using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; } // x
        public Signal InputSignal2 { get; set; } // h
        public Signal OutputConvolvedSignal { get; set; }
        public override void Run()
        {
            List<float> s = new List<float>(); ///samples
            List<int> index1 = new List<int>();

            int min1 = InputSignal1.SamplesIndices.Min();  //min index for x
            int min2 = InputSignal2.SamplesIndices.Min();   //min index for k
            int N1 = InputSignal1.Samples.Count();   
            int N2 = InputSignal2.Samples.Count();  

            int count = (N1 +N2) - 1;     
            for (int n = 0; n < 2 * count; n++)   //n
            {  
                float x = 0;  int c = 0;          
                for (int k = 0; k < N1; k++)      //k
                {
                    if ((n - k) >= 0 && (n - k) < N2)             
                    {
                        x += InputSignal1.Samples[k] * InputSignal2.Samples[n - k]; // x(k) * h(n-k) 
                        c = 1;
                       
                    }

                }
                if (c == 1)

                    s.Add(x);
            }
            for (int i = min1 + min2; i < (count + min1 + min2); i++)  //loop for indces .index
            {
                index1.Add(i);

            }
            OutputConvolvedSignal = new Signal(s, index1, false);

        }
    }
}
