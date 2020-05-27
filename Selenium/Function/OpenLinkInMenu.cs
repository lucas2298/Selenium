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
        public void OpenLinkInMenu()
        {
            var FileName = @"D:\Selenium\TestCase\OpenLinkInMenu";
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
                var Logo = chrome.FindElementById("brandlogo");
                Logo.Click();
                Thread.Sleep(1500);
                var OpenMenu = chrome.FindElementByXPath("//*[@id='root']/div/div[4]/header/button[1]/span");
                OpenMenu.Click();
                Thread.Sleep(1000);
                var Links = chrome.FindElementsByTagName("a");
                Dictionary<string, bool> Flag = new Dictionary<string, bool>();
                foreach (var sLink in Links)
                    if (sLink.Text != "")
                    {
                        Flag[sLink.Text] = true;
                    }
                foreach (var sLink in Links)
                    if (sLink.Text != "")
                    {
                        sLink.Click();
                        Thread.Sleep(1500);
                        var SubLink = chrome.FindElementsByTagName("a");
                        foreach (var sSubLink in SubLink)
                            if (sSubLink.Text != "" && !Flag.ContainsKey(sSubLink.Text))
                            {
                                sSubLink.Click();
                                Thread.Sleep(5000);
                                chrome.SwitchTo().Window(chrome.WindowHandles[1]);
                                Log += VerifyError(chrome, ScreenShotPath);
                                if (SelectedOption.IsOpenTab)
                                {
                                    Log += OpenAllTabInPage(chrome, ScreenShotPath);
                                }
                                if (SelectedOption.IsOpenExpander)
                                {
                                    Log += OpenExpander(chrome, ScreenShotPath);
                                }
                                chrome.Close();
                                chrome.SwitchTo().Window(chrome.WindowHandles[0]);
                                Thread.Sleep(1000);
                                File.WriteAllText(LogPath + "Log.txt", Log);
                                break;
                            }
                        sLink.Click();
                        Thread.Sleep(1500);
                    }
                Log += '\n';
                File.WriteAllText(LogPath + "Log.txt", Log);
                chrome.Quit();
            }
        }
    }
}
