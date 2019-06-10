using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace OSMiniProject1
{
    public partial class Form1 : Form
    {
        ProcessGenerator p;
        Process[] pList;
        int ProcessNumber=0;
        double quantum=3;
        double contextSwitch=0;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            p = new ProcessGenerator();

            chart1.ChartAreas[0].Axes[0].Title = "TIME";
            chart1.ChartAreas[0].Axes[1].Title = "PROCESS NUMBER";
          


        }

        private void button1_Click(object sender, EventArgs e)
        {
            pList = p.Generate(ref ProcessNumber);


            button2 .Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;


           MessageBox.Show("Generating done successfully \n  Please check output.txt file  ");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString() != "")
            {
                double.TryParse(textBox1.Text.ToString(), out contextSwitch);
            }

            if (textBox2.Text.ToString() != "")
            {
                double.TryParse(textBox2.Text.ToString(), out quantum);
            }

            Scheduling s = new Scheduling(pList, ProcessNumber, quantum, contextSwitch);

            chart1.Series["Process Number"].Points.Clear();
            chart1.Series["Process Number"].Color = Color.Green;
            List<executedProc> list = s.FCFS();

           
           for (int i = 0; i < list.Count;i++ )
           {
               MessageBox.Show("Sequence = " + (i+1).ToString() + "\n PID = " + list[i].procNumber + "\n Start Time = " + list[i].start + "\n End Time = " + list[i].end);
             //  for (double j = list[i].start; j < list[i].end; j=j+0.01)
              // {
                   //AddXY value in chart1 in series named as Salary  

               //AddXY value in chart1 in series named as Salary  

               chart1.Series["Process Number"].Points.AddXY(list[i].start.ToString(), list[i].procNumber.ToString());
               chart1.Series["Process Number"].Points.AddXY(list[i].end.ToString(), list[i].procNumber.ToString());
              
                 //  chart1.Series["Process Number"].Points.AddXY(Math.Round(j,2).ToString(), list[i].procNumber.ToString());

               //}

           }


          
       

           

        }

        private void chart1_Click(object sender, EventArgs e)
        {
           // this.chart1.SaveImage("C:\\chart.png", ChartImageFormat.Png);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.ToString() != "")
            {
                double.TryParse(textBox1.Text.ToString(), out contextSwitch);
            }

            if (textBox2.Text.ToString() != "")
            {
                double.TryParse(textBox2.Text.ToString(), out quantum);
            }

            Scheduling s = new Scheduling(pList, ProcessNumber, quantum, contextSwitch);
            chart1.Series["Process Number"].Points.Clear();
            chart1.Series["Process Number"].Color = Color.Red;
            List<executedProc> list = s.HPF();



            for (int i = 0; i < list.Count; i++)
            {
                MessageBox.Show("Sequence = " + (i + 1).ToString() + "\n PID = " + list[i].procNumber + "\n Start Time = " + list[i].start + "\n End Time = " + list[i].end);
          
       
                    //AddXY value in chart1 in series named as Salary  
              
                chart1.Series["Process Number"].Points.AddXY(list[i].start.ToString(), list[i].procNumber.ToString());
                chart1.Series["Process Number"].Points.AddXY(list[i].end.ToString(), list[i].procNumber.ToString());
             

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString() != "")
            {
                double.TryParse(textBox1.Text.ToString(), out contextSwitch);
            }

            if (textBox2.Text.ToString() != "")
            {
                double.TryParse(textBox2.Text.ToString(), out quantum);
            }

            Scheduling s = new Scheduling(pList, ProcessNumber, quantum, contextSwitch);
            chart1.Series["Process Number"].Points.Clear();
            chart1.Series["Process Number"].Color = Color.Yellow;
            List<executedProc> list = s.executeRR();



            for (int i = 0; i < list.Count; i++)
            {
                MessageBox.Show("Sequence = " + (i + 1).ToString() + "\n PID = " + list[i].procNumber + "\n Start Time = " + list[i].start + "\n End Time = " + list[i].end);
                   //AddXY value in chart1 in series named as Salary  

                chart1.Series["Process Number"].Points.AddXY(list[i].start.ToString(), list[i].procNumber.ToString());
                chart1.Series["Process Number"].Points.AddXY(list[i].end.ToString(), list[i].procNumber.ToString());
              

            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.ToString() != "")
            {
                double.TryParse(textBox1.Text.ToString(), out contextSwitch);
            }

            if (textBox2.Text.ToString() != "")
            {
                double.TryParse(textBox2.Text.ToString(), out quantum);
            }

            Scheduling s = new Scheduling(pList, ProcessNumber, quantum, contextSwitch);
            chart1.Series["Process Number"].Points.Clear();
            chart1.Series["Process Number"].Color = Color.Blue;
            List<executedProc> list = s.executeSRTN();



            for (int i = 0; i < list.Count; i++)
            {
               MessageBox.Show("Sequence = " + (i + 1).ToString() + "\n PID = " + list[i].procNumber + "\n Start Time = " + list[i].start + "\n End Time = " + list[i].end);

                chart1.Series["Process Number"].Points.AddXY(list[i].start.ToString(), list[i].procNumber.ToString());
                chart1.Series["Process Number"].Points.AddXY(list[i].end.ToString(), list[i].procNumber.ToString());
              

            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }





}
