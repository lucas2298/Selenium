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
using System.Collections.ObjectModel;
using Selenium.Core;

namespace Selenium
{
    public partial class MainWindow
    {
        public void OpenLinkInHomePage(object sender, RoutedEventArgs e)
        {
            // Get all files test
            string[] filePaths = Directory.GetFiles(@"D:\Selenium\TestCase\OpenLinkInHomePage\", "*.json");
            // Create Log
            string Log = "";            
            foreach (var file in filePaths)
            {
                // From Json to Dict
                string TextJson = File.ReadAllText(file);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(TextJson);
                // Open Chrome
                ChromeDriver chrome = new ChromeDriver();
                // Wait
                chrome.Url = "http://soliddevapp.allianceitsc.com/#/login/up";
                chrome.Navigate();
                Thread.Sleep(5000);
                chrome.Manage().Window.Maximize();
                base.Login(chrome, dict["UserName"], dict["Password"]);
                // Change server to local or online
                Log += ChangeServerApi(chrome, dict["UserName"], dict["Password"], dict["Server"]);
                chrome.FindElementById("brandlogo").Click();
                Thread.Sleep(1500);
                var list = chrome.FindElementsByCssSelector("[class='btn btn-link']");
                var stt = 0;
                foreach (var sItem in list)
                {
                    sItem.Click();
                    Thread.Sleep(1000);
                    var LinkList = chrome.FindElementsByTagName("a");
                    Log += "TEST CASE: " + dict["Description"] + "\n\n";
                    stt++;
                    foreach (var sItem1 in LinkList)
                    {
                        string text = sItem1.Text;
                        if (text != "" && !text.Contains("Chào") && sItem1.Displayed && sItem1.Location.X > 0 && sItem1.Location.Y > 0)
                        {
                            sItem1.Click();
                            Thread.Sleep(5000);
                            chrome.SwitchTo().Window(chrome.WindowHandles[1]);
                            string temp = CheckError(chrome, dict["ScreenShotPath"]);
                            if (temp != "") Log += "Project " + stt.ToString() + ": " + text + "\n" + temp + "\n";
                            chrome.Close();
                            chrome.SwitchTo().Window(chrome.WindowHandles[0]);
                            Thread.Sleep(1000);
                            File.WriteAllText(dict["LogPath"] + @"\LOG " + dict["Description"] + ".txt", Log);
                        }
                    }
                    // Reset
                    Thread.Sleep(1000);
                    chrome.FindElementByXPath("//*[@id='root']/div/div[4]/header/button[1]/span").Click();
                    Thread.Sleep(1500);
                    chrome.FindElementByXPath("//*[@id='root']/div/div[4]/header/button[1]/span").Click();
                    Thread.Sleep(1500);
                }
                File.WriteAllText(dict["LogPath"] + @"\LOG " + dict["Description"] + ".txt", Log);
                chrome.Quit();
            }
        }
    }
}
