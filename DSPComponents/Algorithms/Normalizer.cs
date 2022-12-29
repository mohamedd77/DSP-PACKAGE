using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }


        /// normalize signals from -1 to 1 or 0 to 1
        // equation : (b-a)*( (x - min x) / (max x - min x) ) + a
        public override void Run()
        {
            List<float> samples = new List<float>();

            float n = (InputSignal.Samples.Max() - InputSignal.Samples.Min());


            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {

                samples.Add(((InputMaxRange - InputMinRange) * ((InputSignal.Samples[i] - InputSignal.Samples.Min()) / n)) + InputMinRange);
            }

            OutputNormalizedSignal = new Signal(samples, false);

        }
    }
    }

