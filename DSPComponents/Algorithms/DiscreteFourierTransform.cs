using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;

namespace DSPAlgorithms.Algorithms
{             /// <summary>
/// /  From (Time domain) to (Frequency domain) 
/// </summary>
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public List<Complex> z { get; set; }

        public override void Run()
        {

            int N = InputTimeDomainSignal.Samples.Count();
            z = new List<Complex>(); 
            Complex[] j = new Complex[InputTimeDomainSignal.Samples.Count];

            List<float> amplitude = new List<float>(); 
            List<float> phase = new List<float>();   
            List<float> xaxis = new List<float>();  



            for (int i = 0; i < N; i++)        //loop: (( K ))
            {
                float real = 0; float imaginary = 0;

                for (int n = 0; n < N; n++)     //loop: (( n ))
                {
                    // which:  x(k)= n * e^(-2*pi*k*n/N)  &&&  // which:  e^- = cos (phase) - sin(phase) 

                    double power = (double)(2 * Math.PI * i * n) / N;

                    real +=  (float)(InputTimeDomainSignal.Samples[n] * Math.Cos(power));  // n* cos(2*pi*k*n/N) >>> n*cos(phase)

                    imaginary -= (float)(InputTimeDomainSignal.Samples[n] * Math.Sin(power)); // n* -sin(2*pi*k*n/N)  >>> n* -sin(phase) 
                } 
                     
                j[i] = new Complex(real, imaginary);      //**The result** whish is number consist of( real, imaginary) 
                z.Add(j[i]);

                amplitude.Add((float)j[i].Magnitude);   // Sqrt(real^2 + imaginary^2) 

                phase.Add((float)j[i].Phase);  //ceta::  shift tan (y/x)
                //which: Frequency n = 2*pi/N*T  >>>> // which T = 1/fs = (1 / InputSamplingFrequency)
                xaxis.Add((float)((i) * ((2 * Math.PI) / (N * (1 / InputSamplingFrequency)))));   // The result *(2pi/n*T)     
                Math.Round(xaxis[i], 1);


            }
            OutputFreqDomainSignal = new Signal(false, xaxis, amplitude, phase);  // final: xaxis set & mplitude graph & phase graph
        }
    }
}
