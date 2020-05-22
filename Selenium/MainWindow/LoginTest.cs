using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;

namespace Selenium
{
    public partial class MainWindow
    {
        public void LoginTest(object sender, RoutedEventArgs e)
        {
            // Get all files test
            string[] filePaths = Directory.GetFiles(@"D:\Selenium\TestCase\LoginTest\");
            // File log
            string Log = "";
            string LogPath = @"D:\Selenium\Output\LoginTest";
            foreach (var sItem in filePaths)
            {
                ChromeDriver chrome = new ChromeDriver();
                chrome.Url = "http://soliddevapp.allianceitsc.com/#/login/up";
                chrome.Manage().Window.Maximize();
                chrome.Navigate();
                Thread.Sleep(2000);
                // Read test file
                string Json = File.ReadAllText(sItem);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(Json);
                // Tìm đối tượng
                // 1. UserName
                var UserName = chrome.FindElementById("ip_username");
                // 2. Password
                var Password = chrome.FindElementById("ip_password");
                // 3. Sign In Button
                var SignIn = chrome.FindElementByCssSelector("[class='btn btn-primary']");
                var Description = dict["Description"];
                var ScreenShotPath = dict["ScreenShotPath"];
                // Send values
                UserName.SendKeys(dict["UserName"]);
                Password.SendKeys(dict["Password"]);
                SignIn.Click();
                Thread.Sleep(2000);
                try
                {
                    var CheckLogin = chrome.FindElementByCssSelector("[class='btn btn-success']");
                    Log += "Test case: " + Description + "\nKhông có lỗi xảy ra\n\n";
                }
                catch (Exception ex)
                {
                    TakeScreenShot(chrome, DateTime.Now.ToString("yyyyMMddHHmmss"), "LoginFailed", ScreenShotPath);
                    Log += "Test case: " + Description + "\n" + ex + "\n\n";
                }
                chrome.Quit();
            }
            File.WriteAllText(LogPath + @"\Log.txt", Log);
        }
    }
}
