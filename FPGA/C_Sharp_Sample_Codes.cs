using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.UI;
//using System.Globalization;
using System;



public class NiosTerminal : MonoBehaviour
{
    public GameObject textshow;
    public int i=0;
    
    
    // Start is called before the first frame update
    public Process proc = new Process 
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "D:/nios2eds/Nios II Command Shell.bat",
                    Arguments = "nios2-terminal.exe",
                    //Arguments
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardInput = true
                }
            };
   

    void Start()
    {

        
       
       
        proc.Start();

        // string line = proc.StandardOutput.ReadLine();
        // UnityEngine.Debug.Log(line);
        // while (!proc.StandardOutput.EndOfStream)
        //     {
                
        //         break;
        //         // do something with line
        //     }




        // Process p = new Process();
        // string strCmdText;
        // strCmdText= "";
        // p=System.Diagnostics.Process.Start("D:/nios2eds/Nios II Command Shell.bat", strCmdText);
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
        Text MyText=textshow.GetComponent<Text>();
        string line = proc.StandardOutput.ReadLine();
        i++;
        proc.StandardInput.WriteLine(i.ToString()+"~");
        string trash=proc.StandardOutput.ReadLine();
        proc.StandardInput.WriteLine(i.ToString()+"~");
        // trash=proc.StandardOutput.ReadLine();


       // byte[] buffer = System.Text.Encoding.UTF8.GetBytes(proc.StandardOutput.ReadLine());
        //string message = System.Text.Encoding.UTF8.GetString(buffer);
        //MyText.text=line;
        string[] datas = line.Split(new string[] { "<-->" }, StringSplitOptions.None);
        int direction=Convert.ToInt32(datas[0], 16);
        //MyText.text=direction.ToString();
        if(direction<-10){
            MyText.text="Turning right " + direction.ToString() + "";
        }
        else if(direction>10){
            MyText.text="Turning Left " + direction.ToString() + "";
        }
        else{
            MyText.text="Still " + direction.ToString() + "";

        }
        string paddle=datas[1];
        //int paddle=int.parse(datas[1]);
        if(paddle=="1"){
            UnityEngine.Debug.Log("forward");
        }
        else if(paddle=="2"){
            UnityEngine.Debug.Log("backward");
        }
        else{
            UnityEngine.Debug.Log("still");
        }

        //UnityEngine.Debug.Log();
        
        
    }

   

}
