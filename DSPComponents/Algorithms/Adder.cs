using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }


        //OutSig = Sig1 + Sig2

        public override void Run()
        {
            List<float> samples = new List<float>();
            OutputSignal = new Signal(samples, false);



            for (int i = 0; i < InputSignals.Count - 1; i++)

            {

                for (int j = 0; j < InputSignals[i].Samples.Count; j++)
                {
                    samples.Add(InputSignals[i].Samples[j] + InputSignals[i + 1].Samples[j]);

                   

                }


            }
        }
    }
}