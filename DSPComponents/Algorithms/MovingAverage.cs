using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            List<float> s = new List<float>();
            float average;
            for (int i = InputWindowSize / 2; i < InputSignal.Samples.Count() - InputWindowSize / 2; i++)
            {

                average = InputSignal.Samples[i];
                for (int j = 0; j < InputWindowSize / 2; j++)
                {
                    if (i == 0)
                    {
                        average += InputSignal.Samples[i + 1];
                    }
                    else if (i == InputSignal.Samples.Count() - 1)
                    {
                        average += InputSignal.Samples[i - 1];

                    }

                    else
                    {
                        average += InputSignal.Samples[i + 1 + j] + InputSignal.Samples[i - 1 - j];
                    }
                }
                s.Add(average / InputWindowSize);
            }
            OutputAverageSignal = new Signal(s, false);
            //throw new NotImplementedException();
        }
    }
}