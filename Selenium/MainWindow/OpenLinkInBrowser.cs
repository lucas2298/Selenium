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

namespace Selenium
{
    public partial class MainWindow
    {
        public void OpenLinkInBrowser(object sender, RoutedEventArgs e)
        {
            // Get all files test
            string[] filePaths = Directory.GetFiles(@"D:\Selenium\TestCase\OpenLinkInBrowser\", "*.json");
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
                Login(chrome, dict["UserName"], dict["Password"]);
                // Change server to local or online
                Log += ChangeServerApi(chrome, dict["UserName"], dict["Password"], dict["Server"]);
                var Menu = chrome.FindElementByXPath("//*[@id='root']/div/div[4]/header/button[1]/span");
                Menu.Click();
                var LinkList = chrome.FindElementsByTagName("a");
                List<string> DefaultList = new List<string>();
                Log += "TEST CASE: " + dict["Description"] + "\n\n";
                foreach (var sItem in LinkList)
                {
                    string text = sItem.Text;
                    if (text != "")
                    {
                        DefaultList.Add(text);
                    }
                }
                foreach (var sItem in LinkList)
                {
                    if (sItem.Text != "")
                    {
                        sItem.Click();
                        Thread.Sleep(1000);
                        var NewLinkList = chrome.FindElementsByTagName("a");
                        foreach (var ssItem in NewLinkList)
                        {
                            if (ssItem.Text != "" && !DefaultList.Contains(ssItem.Text))
                            {
                                ssItem.Click();
                                Thread.Sleep(5000);
                                chrome.SwitchTo().Window(chrome.WindowHandles[1]);
                                string temp = CheckError(chrome, dict["ScreenShotPath"]);
                                if (temp != "") Log += ssItem.Text + '\n' + temp + '\n';
                                chrome.Close();
                                chrome.SwitchTo().Window(chrome.WindowHandles[0]);
                                Thread.Sleep(1000);
                                File.WriteAllText(dict["LogPath"] + @"\LOG " + dict["Description"] + ".txt", Log);
                            }
                        }
                        sItem.Click();
                        Thread.Sleep(1500);
                    }
                }
                chrome.Quit();
            }
        }
    }
}

