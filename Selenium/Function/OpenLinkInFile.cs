using Selenium.Models;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace Selenium.Views
{
    public partial class TestOptionView
    {
        public void OpenLinkInFile()
        {
            var FileName = @"D:\Selenium\TestCase\OpenLinkInFile";
            var Files = Directory.GetFiles(FileName, "*.json");
            foreach (var sFile in Files)
            {
                var Json = File.ReadAllText(sFile);
                var Dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Json);
                var SelectedOption = (SelectTestModel)StackPanel_SelectOptions.DataContext;
                ChromeDriver chrome = new ChromeDriver();
                chrome.Manage().Window.Maximize();
                chrome.Url = "http://soliddevapp.allianceitsc.com/#/login/up";
                chrome.Navigate();
                Thread.Sleep(5000);
                var ScreenShotPath = Dict["ScreenShotPath"];
                var LogPath = Dict["LogPath"];
                var Log = "TEST CASE: " + Dict["Description"] + '\n';
                string LoginError = Login(chrome, Dict["UserName"], Dict["Password"], Dict["ScreenShotPath"]);
                if (LoginError != "")
                {
                    Log += "Login error, stop testing!\n" + LoginError;
                    File.WriteAllText(Dict["LogPath"] + "Log.txt", Log);
                    chrome.Quit();
                    return;
                }
                Log += ChangeServerApi(chrome, Dict["UserName"], Dict["Password"], Dict["Server"], Dict["ScreenShotPath"]);
                var Links = File.ReadAllLines(@"D:\Selenium\TestCase\OpenLinkInFile\"+Dict["FileName"]);
                foreach (var sLink in Links)
                {
                    chrome.Url = sLink;
                    chrome.Navigate();
                    Thread.Sleep(5000);
                    Log += VerifyError(chrome, ScreenShotPath);
                    if (SelectedOption.IsOpenTab)
                    {
                        Log += OpenAllTabInPage(chrome, ScreenShotPath);
                    }
                    if (SelectedOption.IsOpenExpander)
                    {
                        Log += OpenExpander(chrome, ScreenShotPath);
                    }
                    File.WriteAllText(LogPath + "Log.txt", Log);
                }
                Log += '\n';
                File.WriteAllText(LogPath + "Log.txt", Log);
                chrome.Quit();
            }
        }
        
        public void OpenLinkInProject()
        {

        }
    }
}
