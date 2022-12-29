using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{/// <summary>
/// /  From (Frequency domain) to (Time domain) 
/// </summary>
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            OutputTimeDomainSignal = new Signal(new List<float>(), InputFreqDomainSignal.Periodic);


            double[] real_part = new double[InputFreqDomainSignal.Frequencies.Count];
            double[] imagine_part = new double[InputFreqDomainSignal.Frequencies.Count];
            double power_of_eta;
            int N = InputFreqDomainSignal.Frequencies.Count;    //samples count
            for (int i = 0; i < N; i++)
            {

                real_part[i] = InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[i]);  //samples input (real part)
                imagine_part[i] = InputFreqDomainSignal.FrequenciesAmplitudes[i] * Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[i]);//samples input (imaginary part)
            }
            for (int k = 0; k < N; k++)      //loop : (n)
            {
                double a = 0; double b = 0;   //which is  a:real and b:imaginary
                for (int n = 0; n < N; n++)     //loop :(k)
                {
                    // which:  x(n)= (1/N)* k * e^(2*pi*k*n/N)  &&&  // which:  e^ = cos (phase) + sin(phase)
                    power_of_eta = (k * n * 2 * Math.PI) / N ;     

                    a = a + (Math.Cos(power_of_eta) * real_part[n]);              //from eta (e)
                    b = b + (Math.Cos(power_of_eta) * imagine_part[n]);           //from sample input >> k(cos)               ///  add real with real and imaginary with imaginary
                    a = a + (Math.Sin(power_of_eta) * real_part[n]);             //from eta (e)
                    b = b + (Math.Sin(power_of_eta) * imagine_part[n] * -1);     //from sample input k(sin)

                }
                float res = (float)(a + b) * 1 / N ;    
                OutputTimeDomainSignal.Samples.Add(res); //x(n)


            }

        }
    }

}