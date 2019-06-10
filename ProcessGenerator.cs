using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OSMiniProject1
{

    public struct Process
    {
        public int PID;             //: process ID
        public double arrivalTime;  //: follows Normal distribution
        public double burstTime;    //: follows Normal distribution
        public double priority;     //: follows Poisson distribution
        public double burstTimeCopy;

    }

    class ProcessGenerator
    {
        int processesNumber;
        float arrivalTimeμ;
        float arrivalTimeσ;
        float burstTimeμ;
        float burstTimeσ;
        float priorityDistributionλ;

        

        public ProcessGenerator()
        {

          
               
        }



        void ReadInputFile()
        {
            try
            {

                string[] lines = System.IO.File.ReadAllLines(@"C:\Users\passe\Desktop\input.txt");
                int i = 0;
                foreach (string line in lines)
                {

                    string[] words = line.Split(' ');
                    if (i == 0)
                    {
                        processesNumber = int.Parse(words[0]);
                    }
                    if (i == 1)
                    {
                        arrivalTimeμ = float.Parse(words[0]);
                        arrivalTimeσ = float.Parse(words[1]);
                    }
                    if (i == 2)
                    {

                        burstTimeμ = float.Parse(words[0]);
                        burstTimeσ = float.Parse(words[1]);

                    }
                    if (i == 3)
                    {

                        priorityDistributionλ = float.Parse(words[0]);
                    }
                    i++;
                }


            }
            catch (Exception e)
            {

                MessageBox.Show(e.GetType().ToString());

            }
        }


    public Process[] Generate(ref int pN)
    {
        ReadInputFile();

        Process[] processList = new Process[processesNumber];


        System.IO.File.WriteAllText(@"C:\Users\passe\Desktop\output.txt", string.Empty);//to clear old contents from output file
        
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\passe\Desktop\output.txt", true))
        {
            file.WriteLine(processesNumber.ToString()); //write number of processes to output file
        }

        pN = processesNumber;
        for (int i = 0; i < processesNumber; i++)
        {


            Random rand = new Random(); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double arrivalTime = arrivalTimeμ + arrivalTimeσ * randStdNormal; //random normal(mean,stdDev^2)

            Random rand2 = new Random(); //reuse this if you are generating many
            double u12 = 1.0 - rand2.NextDouble(); //uniform(0,1] random doubles
            double u22 = 1.0 - rand2.NextDouble();
            double randStdNormal2 = Math.Sqrt(-2.0 * Math.Log(u12)) * Math.Sin(2.0 * Math.PI * u22); //random normal(0,1)
            double burstTime = burstTimeμ + burstTimeσ * randStdNormal2; //random normal(mean,stdDev^2)

         
            processList[i] = new Process();
            processList[i].PID = i + 1; 
            processList[i].arrivalTime =Math.Abs(Math.Round(arrivalTime,1));
            processList[i].burstTime =Math.Abs(Math.Round(burstTime,1));
            processList[i].priority =Math.Abs(Math.Round(RandomPoissonDistribution(),2));
            processList[i].burstTimeCopy = processList[i].burstTime;



            string x = processList[i].PID + " " + processList[i].arrivalTime + " " + processList[i].burstTime + " " + processList[i].priority;

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\Users\passe\Desktop\output.txt", true))
            {
                file.WriteLine(x);
            }

            System.Threading.Thread.Sleep(200);

        }
            return processList;
    
    }

      public double RandomPoissonDistribution()
      {
           Random rNum = new Random();
           int k = rNum.Next(1, 20);

           //(λ^k * e^-λ) / k!  
          
          int kFactorial = Factorial(k);
          
          double numerator = Math.Pow(Math.E, -(double)priorityDistributionλ) * Math.Pow((double)priorityDistributionλ, (double)k);

          double p = (double)numerator *1000/ kFactorial;
          return p;

      }

      private int Factorial(int k)
      {
          int count = k;
          int factorial = 1;
          while (count >= 1)
          {
              factorial = factorial * count;
              count--;
          }
          return factorial;
      }



    
    }
    
}
