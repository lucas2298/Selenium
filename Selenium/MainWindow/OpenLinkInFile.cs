using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Net;
using OpenQA.Selenium.Remote;
using System.Net.Http;

namespace Selenium
{
    public partial class MainWindow
    {
        public void OpenLinkInFile(object sender, RoutedEventArgs e)
        {
            // Get all files test
            string[] filePaths = Directory.GetFiles(@"D:\Selenium\TestCase\OpenLinkInFile\", "*.json");
            // Create Log
            string Log = "";
            foreach (var sItem in filePaths)
            {
                // Read file contains link
                string Links = File.ReadAllText(sItem);
                // Create dictionary
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Links);
                var temp = File.ReadAllText(dict["LinkFile"]);
                temp = temp.Replace("\r", string.Empty);
                var arr = temp.Split('\n');
                Log += "TEST CASE: " + dict["Description"] + "\n";
                // Open Chrome
                ChromeDriver chrome = new ChromeDriver();
                // Wait
                chrome.Navigate().GoToUrl("http://soliddevapp.allianceitsc.com/#/login/up");
                Thread.Sleep(5000);
                chrome.Manage().Window.Maximize();
                Login(chrome, dict["UserName"], dict["Password"]);
                // Change server to local or online
                Log += ChangeServerApi(chrome, dict["UserName"], dict["Password"], dict["Server"]);
                foreach (var Item in arr)
                {
                    chrome.Url = Item;
                    chrome.Navigate().Refresh();
                    Thread.Sleep(5000);
                    string temp1 = CheckError(chrome, dict["ScreenShotPath"]);
                    if (temp1 != "") Log += "LINK: " + Item + "\n" + temp1 + '\n';
                    File.WriteAllText(dict["LogPath"] + @"\LOG " + dict["Description"] + ".txt", Log);
                }
                chrome.Quit();
            }            
        }
    }
}
