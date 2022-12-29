using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public override void Run()
        {
            List<float> outputt= new List<float>();
            FIR fir = new FIR();
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.InputCutOffFrequency = 1500;
            if (L != 0 && M == 0)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    outputt.Add(InputSignal.Samples[i]);
                    if (i == InputSignal.Samples.Count - 1)
                    { break; }
                    for (int j = 0; j < L - 1; j++) //
                    { outputt.Add(0); }                   
                }
                fir.InputTimeDomainSignal = new Signal(outputt, false);
                fir.Run();
                OutputSignal = fir.OutputYn;
            }
            ///...............................................................
            else if (L == 0 && M != 0)
            {
                FIR low_filter = new FIR();
                low_filter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                low_filter.InputFS = 8000;
                low_filter.InputStopBandAttenuation = 50;
                low_filter.InputCutOffFrequency = 1500;
                low_filter.InputTransitionBand = 500;
                low_filter.InputTimeDomainSignal = InputSignal;
                low_filter.Run();
                Signal filtered_sig = low_filter.OutputYn;
                List<float> l1 = filtered_sig.Samples;

                for (int i = 0; i < filtered_sig.Samples.Count; i += M) //
                {
                    outputt.Add(filtered_sig.Samples[i]);
                }
                filtered_sig = new Signal(outputt, false);
                OutputSignal = filtered_sig;
            }
            ///................................................................
            else if (L != 0 && M != 0)
            {
                List<float> l1 = InputSignal.Samples;
                for (int i = 0; i < l1.Count; i++)
                {
                    outputt.Add(l1[i]);
                    for (int j = 0; j < L - 1; j++) //
                    { outputt.Add(0); }    
                }
                FIR low_filter = new FIR();
                Signal s = new Signal(outputt, false);
                low_filter.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
                low_filter.InputFS = 8000;
                low_filter.InputStopBandAttenuation = 50;
                low_filter.InputCutOffFrequency = 1500;
                low_filter.InputTransitionBand = 500;
                low_filter.InputTimeDomainSignal = s;
                low_filter.Run();
                Signal filtered_sig = low_filter.OutputYn;
                outputt = new List<float>();

                for (int i = 0; i < filtered_sig.Samples.Count; i += M) //
                {
                    if (filtered_sig.Samples[i] != 0)
                        outputt.Add(filtered_sig.Samples[i]);
                }
                s = new Signal(outputt, false);
                OutputSignal = s;
            }
            ///......................................................................
            else  /*(L == 0 && M == 0)*/
            {
                Console.WriteLine("..............ERROR ............");
            }

        }

    }
}
