using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OSMiniProject1
{
  

    public struct executedProc
    {
        public int procNumber;
        public double start;
        public double end;
        public double arrivalTime;
        public double totalBT;
        public executedProc(int pNo, double st, double en, double AT, double BT)
        {
            procNumber = pNo;
            start = st;
            end = en;
            arrivalTime = AT;
            totalBT = BT;
        }
    }

    public struct procParams
    {
        public int procNumber;
        public double waitingTime;
        public double TA;
        public double wTA;
    }

    class Scheduling
    {
        Process[] temp;
        double quantum ;                    //Just for testing...
        double ContextSwitch;

       
        int processesNumber;

        public List<executedProc> executedList;   //List that carries the execution order of processes 

        public List<Process> readyQueue;     //Queue for the processes that are ready on arrival and/or after finishing quantum
                                             //while still having remaining runtime.

//---------Karim Yasser----------------------------------------------------------------- 
        public Scheduling( Process[] list,int pN,double q=3,double cs=0)
        {
            processesNumber = pN;
            if (q != 0)
            {
                quantum = q;
            }
            else
            {
                quantum = 3;
            }

               ContextSwitch = cs;
           

           temp = new Process[processesNumber];
            for (int i = 0; i < processesNumber; i++)
            {
                temp[i].arrivalTime = list[i].arrivalTime;
                temp[i].burstTime = list[i].burstTime;
                temp[i].PID = list[i].PID;
                temp[i].priority = list[i].priority;
                temp[i].burstTimeCopy = list[i].burstTimeCopy;

          


            }
            MergeSort(temp, 0, processesNumber-1);

            
        }
        public List<executedProc> FCFS()
        {


            executedList = new List<executedProc>();
              double Timer;
              Timer = 0.0;


              for (int i = 0; i < processesNumber; i++)
              {
                  executedProc timings = new executedProc();
                  timings.procNumber = temp[i].PID;
                  timings.arrivalTime = temp[i].arrivalTime;
                  timings.totalBT = temp[i].burstTime;

                  if (temp[i].arrivalTime > Timer)
                  {
                      timings.start = temp[i].arrivalTime;
                  }
                  else
                  {
                      timings.start = Timer ;
                  }


                  timings.end =timings.start + temp[i].burstTime;

                  executedList.Insert(executedList.Count, timings);

                  Timer = timings.end + ContextSwitch;

                
              }
              writeToFile();
            return executedList;

        }
        void MergeSort(Process []list, int start, int end) 
{ 
    if (start < end) 
    { 
     
        int mid = start+((end-start)/2); //middle element
		
        // Sort first and second halves 
        MergeSort(list, start, mid); 
        MergeSort(list, mid+1, end); 
  
		 int i, j, loc; 
    int size1 = mid - start + 1 ; 
    int size2 =  end - mid ; 
  
    // create two temp arrays
	Process []Left =new Process[size1];
	Process []Right =new Process[size2];

    // Copy data to temp arrays Left[] and Right[] 
    for (i = 0; i < size1; i++) 
        Left[i] = list[start + i]; 
    for (j = 0; j < size2; j++) 
        Right[j] = list[mid + 1+ j]; 
  
    // Merge the temp arrays back into the list
    i = 0;
    j = 0; 
    loc = start; // Initial index of merged subarray 
    while (i < size1 && j < size2) 
    { 
        if (Left[i].arrivalTime <= Right[j].arrivalTime) 
        { 
            list[loc] = Left[i]; 
            i++; 
        } 
        else
        { 
            list[loc] = Right[j]; 
            j++; 
        } 
        loc++; 
    } 
  
    // Copy the remaining elements of Left[], if there are any 
    while (i < size1) 
    { 
        list[loc] = Left[i]; 
        i++; 
        loc++; 
    } 
  
    // Copy the remaining elements of Right[], if there are any

    while (j < size2) 
    { 
        list[loc] = Right[j]; 
        j++; 
        loc++; 
    } 
    } 
}
        void PrioritySort(Process[] list, int start, int end)
        {
            if (start < end)
            {

                int mid = start + ((end - start) / 2); //middle element

                // Sort first and second halves 
                PrioritySort(list, start, mid);
                PrioritySort(list, mid + 1, end);

                int i, j, loc;
                int size1 = mid - start + 1;
                int size2 = end - mid;

                // create two temp arrays
                Process[] Left = new Process[size1];
                Process[] Right = new Process[size2];

                // Copy data to temp arrays Left[] and Right[] 
                for (i = 0; i < size1; i++)
                    Left[i] = list[start + i];
                for (j = 0; j < size2; j++)
                    Right[j] = list[mid + 1 + j];

                // Merge the temp arrays back into the list
                i = 0;
                j = 0;
                loc = start; // Initial index of merged subarray 
                while (i < size1 && j < size2)
                {
                    if (Left[i].priority >= Right[j].priority)
                    {
                        list[loc] = Left[i];
                        i++;
                    }
                    else
                    {
                        list[loc] = Right[j];
                        j++;
                    }
                    loc++;
                }

                // Copy the remaining elements of Left[], if there are any 
                while (i < size1)
                {
                    list[loc] = Left[i];
                    i++;
                    loc++;
                }

                // Copy the remaining elements of Right[], if there are any

                while (j < size2)
                {
                    list[loc] = Right[j];
                    j++;
                    loc++;
                }
            }
        }
        public List<executedProc> HPF() //Non-Preemptive Highest Priority First.
        {

           
            executedList= new List<executedProc>();
            double Timer= 0.0;


            Process []arrivedList = new Process [processesNumber];



            Process []temp2 = new Process[processesNumber];
            for (int i = 0; i < processesNumber; i++)
            {
                temp2[i].arrivalTime = temp[i].arrivalTime;
                temp2[i].burstTime = temp[i].burstTime;
                temp2[i].PID = temp[i].PID;
                temp2[i].priority = temp[i].priority;
            }

           

            Timer = temp2[0].arrivalTime;

            int j = 0;

            int done = 0;


            
             while(processesNumber> done)
           {

               
                 bool x = false;
                 for (int k = 0; k < processesNumber; k++)
                {
                    
                    if (temp2[k].arrivalTime <= Timer)
                    {
                        
                        arrivedList[j].arrivalTime = temp2[k].arrivalTime;
                        arrivedList[j].burstTime = temp2[k].burstTime;
                        arrivedList[j].PID = temp2[k].PID;
                        arrivedList[j].priority = temp2[k].priority;
                        temp2[k].arrivalTime = 1000000;
                        j++;
                        x = true;
                    }
                 
                 }

                 if (x == false && j < processesNumber)
                 {
                     for (int k = 0; k < processesNumber; k++)
                     {

                         if (temp2[k].arrivalTime < 1000000)
                         {
                             Timer = temp2[k].arrivalTime;
                             break;
                         }

                     }
                 }
                PrioritySort(arrivedList, 0, j-1);

                if (j >= 0 && j >done)
                {
                     executedProc timings = new executedProc();
                    timings.procNumber = arrivedList[0].PID;
                    timings.arrivalTime = arrivedList[0].arrivalTime;
                    timings.totalBT = arrivedList[0].burstTime;

                    if (arrivedList[0].arrivalTime > Timer)
                    {
                        timings.start = arrivedList[0].arrivalTime;
                        
                    }
                    else
                    {
                        timings.start = Timer ;
                    }

                    timings.end = timings.start + arrivedList[0].burstTime;
                    Timer = timings.end + ContextSwitch;
                    executedList.Insert(executedList.Count, timings);
                    arrivedList[0].priority = -1;
                    done++;
                    
                }

             }
             writeToFile();
           
            return executedList;
        }





//--------------KHALED SAMEH----------------------------------------------------------------
        public void writeToFile()//KHALED
        {

            double totalTA = 0, totalWTA = 0;

            string x;

            System.IO.File.WriteAllText(@"C:\Users\passe\Desktop\output2.txt", string.Empty);//to clear old contents from output file

            for (int k = 1; k < processesNumber + 1; k++)
            {
                for (int l = executedList.Count() - 1; l > -1; l--)
                {
                    if (executedList[l].procNumber == k)
                    {

                        double finishTime = executedList[l].end;

                        double TA = finishTime - executedList[l].arrivalTime;

                        double waitingTime = TA - executedList[l].totalBT;

                        double weightedTA = TA / executedList[l].totalBT;

                        totalTA += TA;

                        totalWTA += weightedTA;

                        x = k+" " +Math.Round(waitingTime,2) + " " + Math.Round(TA,2) + " " + Math.Round(weightedTA,2) + " ";

                        using (System.IO.StreamWriter file =
               new System.IO.StreamWriter(@"C:\Users\passe\Desktop\output2.txt", true))
                        {
                            file.WriteLine(x);
                        }

                        break;
                    }
                }
            }

            x = "ATAT = "+Math.Round(totalTA / processesNumber ,2)+ "  AWTAT = " + Math.Round(totalWTA / processesNumber,2) + " ";

            using (System.IO.StreamWriter file =
   new System.IO.StreamWriter(@"C:\Users\passe\Desktop\output2.txt", true))
            {
                file.WriteLine(x);
            }
        }
        public List<executedProc> executeRR()
        {

            executedList = new List<executedProc>();

            readyQueue = new List<Process>();

            double time;
          //  MergeSort(temp, 0,processesNumber-1);              //Now the procs array is sorted according to arrival time...
        
            int i = 1;

            time = temp[0].arrivalTime;          //In case the arrival time of the 1st proc (after sorting) is not equal to zero

            readyQueue.Insert(0, temp[0]);

            while (readyQueue.Count != 0 || i < processesNumber)
            {

                //check if there is any process that will arrive in this quantum. If so, will insert it at the end of the queue

                while (i < processesNumber && temp[i].arrivalTime > time && temp[i].arrivalTime <= (time + quantum))
                {
                    readyQueue.Insert(readyQueue.Count(), temp[i]);
                    i++;
                }

                double remainingTime = quantum;

                //If the readyQueue is not empty and the quantum is not yet finished
                while (readyQueue.Count > 0 && remainingTime > 0)
                {

                    //Process first process, 1st case if the process's BT is greater than the time of
                    //the quantum, will process it with what time is left, then insert at the rear..
                    if (remainingTime <= readyQueue.First().burstTime)
                    {

                        executedProc ex = new executedProc();

                        ex.procNumber = readyQueue.First().PID;

                        ex.arrivalTime = readyQueue.First().arrivalTime;

                        ex.totalBT = readyQueue.First().burstTimeCopy;

                        ex.start = time;

                        ex.end = time + remainingTime;

                        time += remainingTime;

                        executedList.Insert(executedList.Count, ex);

                        Process proc = new Process();

                        proc.PID = readyQueue.First().PID;

                        proc.burstTime = readyQueue.First().burstTime;

                        proc.priority = readyQueue.First().burstTime;

                        proc.arrivalTime = readyQueue.First().arrivalTime;

                        proc.burstTime -= remainingTime;

                        proc.burstTimeCopy = readyQueue.First().burstTimeCopy;

                        readyQueue.Remove(readyQueue.First());

                        if (proc.burstTime > 0)
                        {
                            readyQueue.Insert(readyQueue.Count, proc);
                        }
                        if (readyQueue.Count() > 1)
                        {
                            time += ContextSwitch;
                        }
                        remainingTime = 0;
                    }


                    //2nd case is when the BT of the queue head is less than the remaining time (quantum)
                    else
                    {
                        remainingTime = 0;

                        executedProc ex = new executedProc();

                        ex.totalBT = readyQueue.First().burstTimeCopy;

                        ex.arrivalTime = readyQueue.First().arrivalTime;

                        ex.procNumber = readyQueue.First().PID;

                        ex.start = time;

                        ex.end = time + readyQueue.First().burstTime;

                        executedList.Insert(executedList.Count, ex);

                        time += readyQueue.First().burstTime;

                        if(readyQueue.Count()!=0)
                        {
                            time += ContextSwitch;
                        }

                        readyQueue.Remove(readyQueue.First());

                    }

                }

            }


            writeToFile();
            return executedList;
        }
        public void adjustSRTN()
        {
            List<executedProc> exProcs = new List<executedProc>();

            int count = executedList.Count();

            int iter = 0;

            while (iter < count)
            {
                int procID = executedList[iter].procNumber;
                double start = executedList[iter].start;
                double AT=executedList[iter].arrivalTime;
                double BT = executedList[iter].totalBT;
                double endt=start+0.1;

                while (executedList[iter].procNumber==procID&&iter<count)
                {
                    if (iter < count)
                    {
                        if (iter+1==count)
                        {
                            endt = executedList[iter].end;
                            iter++;
                            break;
                        }
                        else if (executedList[iter + 1].procNumber != procID)
                        {
                            endt = executedList[iter].end;
                            iter++;
                            break;
                        }
                        iter++;
                    }
                    else break;
                }
                executedProc exP = new executedProc();
                exP.procNumber = procID;
                exP.totalBT = BT;
                exP.start = start;
                exP.end = endt;
                exP.arrivalTime = AT;
                exProcs.Insert(exProcs.Count(), exP);
            }
            executedList = exProcs;
        }
        public List<executedProc> executeSRTN()
        {

            executedList = new List<executedProc>();

            readyQueue = new List<Process>();

     //       iterativeMerge(temp, processesNumber);                //Now the procs array is sorted according to arrival time...

            double time = 0;                                     //Timer will be incremented by 0.01 in each stage

            int i = 0;

            while (readyQueue.Count() != 0 || i < processesNumber)
            {

                //If there are no processes in the ready queue, will take the first on in the array and set the timer to its AT
                if (i < processesNumber && readyQueue.Count() == 0)
                {

                    time = temp[i].arrivalTime;

                    readyQueue.Insert(0, temp[i]);

                    i++;

                    while(time==temp[i].arrivalTime&&i<processesNumber)
                    {
                        Process p = new Process();
                        p.PID = temp[i].PID;
                        p.arrivalTime = temp[i].arrivalTime;
                        p.burstTime = temp[i].burstTime;
                        p.burstTimeCopy = temp[i].burstTimeCopy;
                        p.priority = temp[i].priority;
                        i++;

                        int j = 0;
                        int count = readyQueue.Count();
                        while (i < processesNumber && j < count)
                        {

                            if (p.burstTime < readyQueue[j].burstTime)
                            {
                                //Inserting the process in its place after the element that
                                //has BT greater than itself

                                readyQueue.Insert(j, p);
                                
                                break;

                            }

                            j++;

                            if (j == readyQueue.Count())                       //No more elements ==> Will be inserted
                                readyQueue.Insert(readyQueue.Count(), p);      //at the rear of the queue (Biggest BT left)
                        }
                    }

                }

                if (i < processesNumber)
                {

                    //There are elements in the ready queue and the arrival time of the next process
                    //hasn't been met yet

                    while (readyQueue.Count() != 0 && temp[i].arrivalTime > time)
                    {

                        Process p = new Process();

                        p.PID = readyQueue.First().PID;

                        p.arrivalTime = readyQueue.First().arrivalTime;

                        p.burstTime = readyQueue.First().burstTime;

                        p.burstTimeCopy = readyQueue.First().burstTimeCopy;

                        p.priority = readyQueue.First().priority;

                        p.burstTime -= 0.1;

                        executedProc ex = new executedProc();

                        ex.start = time;

                        ex.end = time + 0.1;

                        ex.arrivalTime = readyQueue.First().arrivalTime;

                        ex.procNumber = readyQueue.First().PID;

                        ex.totalBT = readyQueue.First().burstTimeCopy;

                        executedList.Insert(executedList.Count(), ex);

                        readyQueue.Remove(readyQueue.First());

                        if (p.burstTime > 0)
                        {
                            readyQueue.Insert(0, p);
                        }

                        else if (readyQueue.Count()>0)
                        {
                            time += ContextSwitch;
                        }

                        time =Math.Round(time+ 0.1,1);

                    }

                    //Here we break out of the previous while due to one of 2 reasons:
                    //1- The count of the readyQueue is now zero ====> We know that there are still other processes to execute
                    //2- The arrival time of the next process has been met ===> will check if its BT is less than queue head or not 

                    if (readyQueue.Count() == 0)
                    {

                        time = temp[i].arrivalTime;

                        readyQueue.Insert(0, temp[i]);

                        i++;

                        while (i<processesNumber&&time==temp[i].arrivalTime)  //If more than one element have the same arrival time
                        {
                            Process p = new Process();
                            p.PID = temp[i].PID;
                            p.arrivalTime = temp[i].arrivalTime;
                            p.burstTime = temp[i].burstTime;
                            p.burstTimeCopy = temp[i].burstTimeCopy;
                            p.priority = temp[i].priority;
                            i++;

                            int j = 0;
                            int count = readyQueue.Count();
                            while (j < count)
                            {

                                if (p.burstTime < readyQueue[j].burstTime)
                                {
                                    //Inserting the process in its place after the element that
                                    //has BT greater than itself

                                    readyQueue.Insert(j, p);
                                    
                                    break;

                                }

                                j++;

                                if (j == readyQueue.Count())                       //No more elements ==> Will be inserted
                                    readyQueue.Insert(readyQueue.Count(), p);      //at the rear of the queue (Biggest BT left)
                            }
                        }

                    }
                    else //Here the readyQueue is NOT empty and the next element's arrival time has come
                    {

                        //The readyQueue is sorted according to remaining burst time for all processes...
                        //If a process came and its burst time is less than the burst time of the queue head
                        //Therefore will have to insert it according to BT to maintain the queue

                        Process p = new Process();
                        p.PID = temp[i].PID;
                        p.arrivalTime = temp[i].arrivalTime;
                        p.burstTime = temp[i].burstTime;
                        p.burstTimeCopy = temp[i].burstTimeCopy;
                        p.priority = temp[i].priority;
                        i++;

                        int j = 0;
                        int count = readyQueue.Count();
                        while (j < count)
                        {

                            if (p.burstTime < readyQueue[j].burstTime)
                            {
                                //Inserting the process in its place after the element that
                                //has BT greater than itself

                                readyQueue.Insert(j, p);

                                if (j == 0)                 //element was inserted at the beggining of the queue but the process
                                {                           //in the queue head has not finished processing yet due its larger BT
                                                            //Therefore a context switch must occur
                                    time += ContextSwitch;

                                }
                                break;                    

                            }

                            j++;

                            if (j == readyQueue.Count())                       //No more elements ==> Will be inserted
                                readyQueue.Insert(readyQueue.Count(), p);      //at the rear of the queue (Biggest BT left)
                        }


                    }


                }

                //The process list is empty and now we have a ready queue to empty
                else
                {

                    while (readyQueue.Count() != 0)
                    {
                        Process p = new Process();

                        p.PID = readyQueue.First().PID;

                        p.arrivalTime = readyQueue.First().arrivalTime;

                        p.burstTime = readyQueue.First().burstTime;

                        p.burstTimeCopy = readyQueue.First().burstTimeCopy;

                        p.priority = readyQueue.First().priority;

                        p.burstTime -= 0.1;

                        executedProc ex = new executedProc();

                        ex.start = time;

                        ex.end = time + 0.1;

                        ex.arrivalTime = readyQueue.First().arrivalTime;

                        ex.procNumber = readyQueue.First().PID;

                        ex.totalBT = readyQueue.First().burstTimeCopy;

                        executedList.Insert(executedList.Count(), ex);

                        readyQueue.Remove(readyQueue.First());

                        if (p.burstTime > 0)
                        {
                            readyQueue.Insert(0, p);
                        }

                        else if(readyQueue.Count()>0)
                        {
                            time += ContextSwitch;
                        }
                        

                        time = Math.Round(time + 0.1, 1);

                    }

                }

            }
            adjustSRTN();
            writeToFile();
            return executedList;
        }



    }
}
