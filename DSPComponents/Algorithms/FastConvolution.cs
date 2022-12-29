using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;


namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        public override void Run()
        {
            //DiscreteFourierTransform dft = new DiscreteFourierTransform();
            //InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            //.................................. DFT  ........................................//
            OutputConvolvedSignal = new Signal(new List<float>(), false);
            int N1 = InputSignal1.Samples.Count;   int N2 = InputSignal2.Samples.Count;
            int x = (N1 + N2) - 1;
            List<float> sign1 = new List<float>();
            List<float> sign2 = new List<float>();
            List<Complex> complexlist1 = new List<Complex>();
            List<Complex> complexlist2 = new List<Complex>();
            double power_of_eta_1;  double power_of_eta_2;
            for (int i = N1; i < x; i++)
                InputSignal1.Samples.Add(0);
            for (int i = N2; i < x; i++)
                InputSignal2.Samples.Add(0);

            for (int i = 0; i < x; i++)     // k   ......>>  
            {
                float p1_real_ = 0;  float p1_imagine_ = 0;
                float p2_real_ = 0;  float p2_imagine_ = 0;
                int N3 = InputSignal1.Samples.Count;
                int N4 = InputSignal2.Samples.Count;
                for (int j = 0; j < N3; j++)   // n   ......>>  
                {
                    power_of_eta_1 = (double)(i * 2 * Math.PI * j) / N3;
                    p1_real_ += (float)Math.Cos(power_of_eta_1) * InputSignal1.Samples[j];
                    p1_imagine_ += -(float)Math.Sin(power_of_eta_1) * InputSignal1.Samples[j];
                    //.......................................................................//
                    power_of_eta_2 = (double)(i * 2 * Math.PI * j) / N4;
                    p2_real_ += (float)Math.Cos(power_of_eta_2) * InputSignal2.Samples[j];
                    p2_imagine_ += -(float)Math.Sin(power_of_eta_2) * InputSignal2.Samples[j];
                }
                Complex comx1 = new Complex(p1_real_, p1_imagine_);
                complexlist1.Add(comx1);
                Complex comx2 = new Complex(p2_real_, p2_imagine_);
                complexlist2.Add(comx2);
            }
        //.....................................................................//

            
            List<Complex> compx_list = new List<Complex>();
            float N_count = InputSignal1.Samples.Count;
            for (int i = 0; i < N_count; i++)
                compx_list.Add(Complex.Multiply(complexlist2[i], complexlist1[i])); //.....> multiply the 2 complex
            //...........................................  IDFT  .............................................//
            ///float N2_count = InputSignal1.Samples.Count;
            List<float> out_put = new List<float>();
            List<float> normalized = new List<float>();
            for (int i = 0; i < N_count; i++)  //.....>> n
            {
                
                Complex _res_ = 0;
                Complex _c_ = new Complex(0, 1);
                float Result_sample = 0;
                for (int j = 0; j < N_count; j++)    //......>> k
                {
                    _res_ += compx_list[j] * (Math.Cos(2 * Math.PI * j * i / N_count) + _c_ * Math.Sin(2 * Math.PI * j * i / N_count));          //IDFT rule
                }
                Result_sample = (float)_res_.Real / (N_count);   
                out_put.Add(Result_sample);
            }
            OutputConvolvedSignal.Samples = out_put;


        }
    }
}



