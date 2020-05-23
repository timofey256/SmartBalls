using System.Collections;
using UnityEngine;
using System;
using System.IO;

public class Log : MonoBehaviour
{
    public string MainLogFileName = "MainLog.txt"; // Путь и файл главного лога
    public string MainLogFilePath = "Assets/Log/";

    public string DebugLogFileName = "DebugLog.txt"; // Путь и файл отладочного лога
    public string DebugLogFilePath = "Assets/Log/";

    public string ErrorLogFileName = "ErrorLog.txt"; // Путь и файл лога ошибок
    public string ErrorLogFilePath = "Assets/Log/";

    // Функция записи в главный лог
    public void WriteToMainLog(string Message, bool NewLine=true, bool WithDateTime=true) {
        this.WriteToLog(this.MainLogFilePath+this.MainLogFileName, Message, NewLine); 
    }

    //Функция записи в отладочный лог
    public void WriteToDebugLog(string Message, bool NewLine=true, bool WithDateTime=true) {
        this.WriteToLog(this.DebugLogFilePath+this.DebugLogFileName, Message, NewLine); 
    }

    //функция записи в отладочный лог
    public void WriteToErrorLog(string Message, bool NewLine=true, bool WithDateTime=true) {
        this.WriteToLog(this.ErrorLogFilePath+this.ErrorLogFileName, Message, NewLine); 
    }

    private void WriteToLog(string Log, string Message, bool NewLine=true, bool WithDateTime=true) {
        if(WithDateTime) File.AppendAllText(Log, DateTime.Now + ": " + Message);
        else File.AppendAllText(Log, Message);
        if(NewLine) File.AppendAllText(Log, Environment.NewLine);
    }

    void Start() {
        File.Delete(this.MainLogFilePath+this.MainLogFileName);
        File.Delete(this.DebugLogFilePath+this.DebugLogFileName);
        File.Delete(this.ErrorLogFilePath+this.ErrorLogFileName);
    }

}
