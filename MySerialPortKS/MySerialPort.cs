﻿using System;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace MySerialPortKS
{    
    //Clase encargada de definir la comunicacion serial, recibir y enviar tramas.
    public class MySerialPort
    {    
        
        private const int BAUDIOS = 57600;
        private SerialPort serialPort;
        private int speedBaudios;
        private string portName;
        private string folderReceivePath;
                
        private Thread receiveMessageThread;
        private bool statusThreadReceiveMessage;

        private string smsToRecieve;
        private byte[] dataFrame;

        public delegate void HandlerReceiveMessage(object oo, string message);
        public event HandlerReceiveMessage messageIsHere;

        public delegate void HandlerNotifyProgress(string path,string key,float progress,float tramas);
        public HandlerNotifyProgress tramaHire;
        public HandlerNotifyProgress sendingProgress;
        public HandlerNotifyProgress notifyName;

        public Thread theBufferIsReady;
        public bool processeThreadBuffer;

        public bool bufferStatus;

        /*
         * Nuevo codigo         
         */
        private SentFileController mysentFileController;
        private ReceiveFileController myReceiveFileController;

        public MySerialPort(string portName,string folder_receivePath)
        {                        
            this.portName = portName;
            this.folderReceivePath = folder_receivePath;
            this.myReceiveFileController = new ReceiveFileController();
            this.mysentFileController = new SentFileController(this.portName);
            this.mysentFileController.frameToSend += new SentFileController.EncodedTrame(frameReadyToSend);
            this.mysentFileController.notifyFileProgress += new SentFileController.HandlerNotifyFileProgress(this.notifyFileProgress);
            this.mysentFileController.notifyFile += new SentFileController.HandlerNotifyFileProgress(this.notifyFileName);
            try
            {
                if (!(Directory.Exists(this.folderReceivePath + @"\Received_file_" + this.portName)))
                {
                    Directory.CreateDirectory(this.folderReceivePath + @"\Received_file_" + this.portName);
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
        public MySerialPort(string portName,int speedBaudios, string folder_receivePath)
            :this(portName, folder_receivePath)
        {
            this.speedBaudios = speedBaudios;
            
        }
        public bool Connect(){
            try{
                serialPort = new SerialPort(portName, this.speedBaudios, Parity.Even, 8, StopBits.Two);              
                serialPort.ReadBufferSize= 10420;
                serialPort.ReadTimeout = 500;
                serialPort.WriteTimeout = 500;                                      
            }
            catch(Exception ee){
                throw new Exception(ee.Message);
            }
            try{
                serialPort.Open();
                this.bufferStatus = true;
                this.processeThreadBuffer = true;
                this.theBufferIsReady = new Thread(isBufferReady);
                this.theBufferIsReady.Start();

                this.statusThreadReceiveMessage = true;
                this.receiveMessageThread = new Thread(this.receivingData);
                this.receiveMessageThread.Start();
                return true;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }           
        }        
       
        public bool Disconnect()
        {
            try{
                this.processeThreadBuffer = false;
                this.statusThreadReceiveMessage = false;
                this.mysentFileController.Close();
                this.myReceiveFileController.CloseSession();
                serialPort.Close();
                return true;
            }catch{
                return false;
            }
        }
        public void notifyFileName(string key, string path, float totalFrames, float progress)
        {
            if (notifyName != null) this.notifyName(path, key, progress, totalFrames);
        }
        public void notifyFileProgress(string key, string path, float totalFrames, float progress)
        {
            if (sendingProgress != null) this.sendingProgress(path, key, progress, totalFrames);
        }

        //Recopera la longitud del mensaje codificada en la cabecera de la trama
        private int recoveryNumberOfProgress()
        {
            int start = Frame.FRAME_TYPE + Frame.FRAME_KEY+ Frame.FRAME_LENGTH_DATA + Frame.FRAME_NUM_FRAMES;
            string lengthMessage = this.smsToRecieve.Substring(start, Frame.FRAME_NUMBER);
            return (int)Int64.Parse(lengthMessage);
        }

        private int recoveryNumberOfFrames()
        {
            int start = Frame.FRAME_TYPE + Frame.FRAME_KEY + Frame.FRAME_LENGTH_DATA;
            string lengthMessage = this.smsToRecieve.Substring(start, Frame.FRAME_NUM_FRAMES);
            return (int)Int64.Parse(lengthMessage);
        }

        private int recoveryLengthMessageReceived()
        {
            int start = Frame.FRAME_TYPE + Frame.FRAME_KEY;
            string lengthMessage = this.smsToRecieve.Substring(start,Frame.FRAME_LENGTH_DATA);
            return (int)Int64.Parse(lengthMessage);
        }               
        private string recoveryFileKey()
        {
            int start = Frame.FRAME_TYPE;
            string key = this.smsToRecieve.Substring(start, Frame.FRAME_KEY);           
            return key;
        }


        //Recopera el mensaje codificado en la trama
        private byte[] RecoveryBytesFromFrameData()
        {
            byte[] newData = new byte[this.recoveryLengthMessageReceived()];
            Array.Copy(this.dataFrame,newData,this.recoveryLengthMessageReceived());
            return newData;
        }
        private string RecoveryFrameData()
        {
            return this.smsToRecieve.Substring(Frame.getFrameHeadLength(),recoveryLengthMessageReceived());
        }
        
        ///--------------------------------------------------------------------------------------------------
        ///
        ///--------------------------------------------------------------------------------------------------

        //Ejecuta el el procedimiento messageIsHire, que se encuentra un nivel por encima.
        //Es controlado por el hander de la clase.
        public virtual void onMessageIsHere()
        {
            if (this.messageIsHere != null){
                this.messageIsHere(this, RecoveryFrameData());
            }
        }     

        //Procedimiento de envio de las tramas
        public string SendMessage(string message)
        {                           
            Resource rec = new Resource(false);
            rec.setMessage(message);
            this.mysentFileController.addStoreResource(rec);         
            return "Message was sent ";
           
        }
        public string SendFiles(List<String> paths)
        {
            
            foreach(string path in paths) { 
                Resource rec = new Resource(true);
                KFile file = KFile.NewKFile(path, KFile.READ_MODE);
                rec.SetFile(file);
                this.mysentFileController.addStoreResource(rec);
            }
            return "Files was sent ";
        }
        
        //Ejecucion de enviado de tramas en un hilo. 
        //Verifica si el puerto esta abierto
        public bool isOpen()
        {
            return this.serialPort.IsOpen;
        } 
        public void isBufferReady()
        {
            while (processeThreadBuffer)
            {
                this.bufferStatus = this.serialPort.BytesToWrite == 0;
                Thread.Sleep(5);
            }
        }
    
       
        //Se ejecuta cuando el evento DataReceived se desencadena. 
        private  void receivingData()
        {
            while (this.statusThreadReceiveMessage){
                Boolean _loockTaken = false;
                Monitor.Enter(serialPort,ref _loockTaken);
                try
                {
                    Thread.Sleep(1);
                    if (!serialPort.IsOpen) return;
                    if (serialPort.BytesToRead>0)
                    {
                        byte[] receivedMessage = new byte[Frame.getFrameLength()];
                        this.serialPort.Read(receivedMessage, 0, Frame.getFrameLength());
                        this.smsToRecieve = ASCIIEncoding.UTF8.GetString(receivedMessage, 0, Frame.getFrameLength());

                        if (receivedMessage[0] == Frame.TYPE_MESSAGE)
                        {
                            this.onMessageIsHere();
                        }
                        if (receivedMessage[0] == Frame.TYPE_FILE)
                        {
                            this.dataFrame = new byte[Frame.FRAME_DATA];
                            Array.Copy(receivedMessage, Frame.getFrameHeadLength(), this.dataFrame, 0, Frame.FRAME_DATA);
                            int totalFrames = this.recoveryNumberOfFrames();
                            int frameProgress = this.recoveryNumberOfProgress();                            
                            string key = this.recoveryFileKey();
                            byte[] data = this.RecoveryBytesFromFrameData();
                            int lengthData = this.recoveryLengthMessageReceived();
                            if (frameProgress == 0)
                            {
                                string name = this.RecoveryFrameData();
                                string nameFile = this.folderReceivePath+@"\Received_file_" + this.portName + "\\" + name.Trim();
                                this.myReceiveFileController.InitFileStream(nameFile, key);
                                if (this.tramaHire != null) this.tramaHire(name,key, frameProgress, totalFrames);
                            }
                            else
                            {
                                this.myReceiveFileController.WriteFile(key, data, lengthData, totalFrames, frameProgress);
                                if (this.tramaHire != null) this.tramaHire("...null...",key, frameProgress, totalFrames);
                            }
                        }
                    }
                    else Thread.Sleep(100);
                }
                finally {
                    if (_loockTaken)
                    {
                        Monitor.Exit(serialPort);
                    }
                }
            }
           
        }

        private  void frameReadyToSend(byte[] frame)
        {
            try
            {
                Boolean _lockTeken = false;
                Monitor.Enter(serialPort,ref _lockTeken);
                try { 
                    while (!this.bufferStatus) { }
                    if(serialPort.IsOpen)
                      serialPort.Write(frame, 0, Frame.getFrameLength());
                }
                finally {
                    if (_lockTeken)
                    {
                        Monitor.Exit(serialPort);               
                    }
                }
            }
            catch
            {                
                //throw new Exception("Error to write message");
            }
         
        }
       
    }
}
