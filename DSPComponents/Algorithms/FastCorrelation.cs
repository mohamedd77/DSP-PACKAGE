using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;


namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            //DiscreteFourierTransform dft = new DiscreteFourierTransform();
            //InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            //.................................. DFT  ........................................//
            List<Complex> compx1 = new List<Complex>();
            List<Complex> compx2 = new List<Complex>();
            float s1 = 0;
            float s2 = 0;
            float normalize;
            double power_of_eta;
            int N1 = InputSignal1.Samples.Count;
            //int N2 = InputSignal2.Samples.Count;
            for (int i = 0; i < N1; i++)  // k   ......>>                                                            ///for signal1
            {
                float p1_real_ = 0; float p2_imagine_ = 0;
                for (int j = 0; j < N1; j++)  // n   
                {
                    power_of_eta = (double)(i * 2 * Math.PI * j) / N1;
                    p1_real_ += (float)Math.Cos(power_of_eta) * InputSignal1.Samples[j];
                    p2_imagine_ += -(float)Math.Sin(power_of_eta) * InputSignal1.Samples[j];
                }

                Complex c1 = new Complex(p1_real_, p2_imagine_);  ///1
                compx1.Add(c1);
            }


            if (InputSignal2 != null)   // .....>> cross corr  (2 signal)
            {
                int N2 = InputSignal2.Samples.Count;
                for (int i = 0; i < N2; i++)  // k   ......>>     //DFT                                                   /// for signal2
                {
                    float p1_real_ = 0;  float p2_imagine_ = 0;                  
                    for (int n = 0; n < N2; n++)  // n
                    {
                        power_of_eta = (double)(i * 2 * Math.PI * n) / N2;
                        p1_real_ += (float)Math.Cos(power_of_eta) * InputSignal2.Samples[n];
                        p2_imagine_ += -(float)Math.Sin(power_of_eta) * InputSignal2.Samples[n];
                    }
                    Complex c2 = new Complex(p1_real_, p2_imagine_);  ///2
                    compx2.Add(c2);
                }
            }
            //.................................................................................//
            int N3 = InputSignal1.Samples.Count;
            List<Complex> complex_list = new List<Complex>();
            for (int i = 0; i < N3; i++)
            {                
                s1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
            }
            if (InputSignal2 == null)  /// (auto corr) 1 sig
            {
                for (int i = 0; i < N3; i++)
                    complex_list.Add(Complex.Multiply(compx1[i], Complex.Conjugate(compx1[i])));                                       ////مرافق بيفير اشارة التخيلى فقط  
               // sum1 += (float)Math.Pow(InputSignal1.Samples[i], 2);// sum2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
                normalize = (float)Math.Sqrt(s1 * s1) / InputSignal1.Samples.Count();
            }

            else                      ///(cross corr) 2 sig
            {
                int N4 = InputSignal2.Samples.Count;
                for (int i = 0; i < N4; i++)
                    complex_list.Add(Complex.Multiply(compx2[i], Complex.Conjugate(compx1[i])));
                for (int i = 0; i < N4 ; i++)
                    s2 += (float)Math.Pow(InputSignal2.Samples[i], 2);

                normalize = (float)Math.Sqrt(s1 * s2) / N4;
            }

            //...........................................  IDFT  .............................................//

            List<float> out_put = new List<float>();
            List<float> normalized = new List<float>();
            for (int i = 0; i < N3; i++)  // n 
            {
               // int N = InputSignal1.Samples.Count;
                Complex res = 0;
                Complex comx = new Complex(0, 1);
                float Result_sample = 0;   
                for (int j = 0; j < N3; j++)  // k              
                    // float p1 = (Math.Cos(2 * Math.PI * k * i / N); //float p2 = (j * Math.Sin(2 * Math.PI * k * i / N));
                    res += complex_list[j] * (Math.Cos(2 * Math.PI * j * i / N3) + (comx * Math.Sin(2 * Math.PI * j * i / N3)));   /// idft rule

                Result_sample = (float)res.Real / (float)Math.Pow(N3, 2); //((float)Math.Pow(N3, 2) === (N3*N3))
                out_put.Add(Result_sample);
                normalized.Add(Result_sample / normalize);
            }
            OutputNonNormalizedCorrelation = out_put;
            OutputNormalizedCorrelation = normalized;
        }
    }
}
