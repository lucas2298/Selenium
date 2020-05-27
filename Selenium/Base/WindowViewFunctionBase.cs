using Caliburn.Micro;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace WindowViewBase
{
    public class WindowViewFunctionBase : Window
    {
        public void TakeScreenShot(ChromeDriver driver, String ScreenShotName, string ErrorName, string ScreenShotPath)
        {
            if (ScreenShotPath != "")
            {
                ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                Directory.CreateDirectory(ScreenShotPath + ErrorName + '/' + ScreenShotName);
                screenshot.SaveAsFile(ScreenShotPath + ErrorName + "/" + ScreenShotName + '/' + ScreenShotName + ".jpg");
                Thread.Sleep(1500);
            }
        }
        public string Login(ChromeDriver chrome, string Name, string Pass, string ScreenShotPath)
        {
            // Tìm đối tượng
            // 1. UserName
            var UserName = chrome.FindElementById("ip_username");
            // 2. Password
            var Password = chrome.FindElementById("ip_password");
            // 3. Sign In Button
            var SignIn = chrome.FindElementByCssSelector("[class='btn btn-primary']");
            // Send values
            UserName.SendKeys(Name);
            Password.SendKeys(Pass);
            SignIn.Click();
            Thread.Sleep(5000);
            return VerifyError(chrome, ScreenShotPath);
        }
        // Check error
        public string VerifyError(ChromeDriver chrome, string ScreenShotPath)
        {
            string Result = "";
            try
            {
                var TextError = chrome.FindElementById("error_api").GetAttribute("innerHTML");
                if (TextError != "")
                {
                    string ScreenShotName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Result = "Error_API: " + TextError + "\nScreenShot saved as: " + ScreenShotName + "\n";
                    TakeScreenShot(chrome, ScreenShotName, "Error", ScreenShotPath);
                }
            }
            catch
            {
            }
            try
            {
                var TextError = chrome.FindElementById("error_ui").GetAttribute("innerHTML");
                if (TextError != "")
                {
                    string ScreenShotName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    Result = "Error_UI: " + TextError + "\nScreenShot saved as: " + ScreenShotName + "\n";
                    TakeScreenShot(chrome, ScreenShotName, "Error", ScreenShotPath);
                }
            }
            catch
            {
            }
            return Result;
        }
        // Open tab
        public string BackTracking(ChromeDriver chrome, int n, int MaxN, string ScreenShotPath)
        {
            string Result = "";
            try
            {
                var Arr = chrome.FindElementsByClassName("tablv-" + n.ToString());
                foreach (var sItem in Arr)
                    if (sItem.Text != "" && sItem.Displayed && sItem.Location.X > 0 && sItem.Location.Y > 0)
                    {
                        sItem.Click();
                        Thread.Sleep(5000);
                        Result += VerifyError(chrome, ScreenShotPath);
                        if (n < MaxN) BackTracking(chrome, n + 1, MaxN, ScreenShotPath);
                    }
                return Result;
            }
            catch
            {
                return Result;
            }
        }
        public string OpenAllTabInPage(ChromeDriver chrome, string ScreenShotPath)
        {
            var TabLevel = 5;
            return BackTracking(chrome, 1, TabLevel, ScreenShotPath);
        }
        // Open Expander
        public string OpenExpander(ChromeDriver chrome, string ScreenShotPath)
        {
            string Result = "";
            try
            {
                var ExpanderList = chrome.FindElementsByClassName("btn-outline-link");
                long MaxHeight = chrome.Manage().Window.Size.Height - 500;
                long temp = MaxHeight;
                foreach (var sItem in ExpanderList)
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)chrome;
                    if (sItem.Location.Y >= MaxHeight)
                    {
                        js.ExecuteScript($"window.scrollBy(0, {sItem.Location.Y - 150});");
                        MaxHeight += temp;
                    }
                    Thread.Sleep(1000);
                    sItem.Click();
                    Thread.Sleep(1500);
                    Result += VerifyError(chrome, ScreenShotPath);
                    sItem.Click();
                    Thread.Sleep(1500);
                }
            }
            catch
            {
            }
            return Result;
        }
        public string ChangeServerApi(ChromeDriver chrome, string UserName, string Password, string Server, string ScreenShotPath)
        {
            string Result = "";
            if (Server == "Local")
            {
                chrome.FindElementByXPath("//*[@id='root']/div/div[4]/header/button[2]").Click();
                Thread.Sleep(1000);
                chrome.FindElementByXPath("//*[@id='root']/div/div[4]/div[1]/aside/div/div/div[1]/div[2]/input[1]").Click();
                Thread.Sleep(3000);
                chrome.Navigate().GoToUrl("http://soliddevapp.allianceitsc.com/#/login/up");
                chrome.Navigate().Refresh();
                Thread.Sleep(5000);
                Login(chrome, UserName, Password, ScreenShotPath);
                Result = "Server run on localhost\n";
            }
            else
            {
                Result = "Server run online\n";
            }
            return Result;
        }
        public string CheckInsertDelete(ChromeDriver chrome, string ScreenShotPath)
        {
            string Result = "";
            try
            {
                var ListBtn = chrome.FindElementsByClassName("btn-primary");
                foreach (var sItem in ListBtn)
                    if (sItem.Text == "Tạo mới")
                    {
                        sItem.Click();
                        var ListEdit = chrome.FindElementsByCssSelector("[class=' has-wrap c-itextarea form-control']");
                        //var AttriSum = chrome.FindElementById("attribute-col-sum");
                        var AttributeSum = 5;
                        foreach (var ssItem in ListEdit)
                            if (AttributeSum > 0)
                            {
                                ssItem.SendKeys("ahihi");
                                Thread.Sleep(500);
                                AttributeSum--;
                            }
                            else break;
                        var Delete = chrome.FindElementByCssSelector("[class='btn btn-outline-danger']");
                        Delete.Click();
                        Thread.Sleep(500);
                        chrome.FindElementByCssSelector("[class='m-button m-button-primary m-button-sm']").Click();
                        Thread.Sleep(500);
                    }
                return Result;
            }
            catch
            {
                return Result;
            }
        }
        public string CheckError(ChromeDriver chrome, string ScreenShotPath)
        {
            string Result = "";
            if (chrome.Url.Contains("/#/home"))
            {
                Result += "Error: Link not found.\n";
                string ScreenShotName = DateTime.Now.ToString("yyyyMMddHHmmss");
                TakeScreenShot(chrome, ScreenShotName, "Error", ScreenShotPath);
            }
            // Open all tab in this page
            var temp = OpenAllTabInPage(chrome, ScreenShotPath);
            if (temp == "") Result += VerifyError(chrome, ScreenShotPath);
            else Result += temp;
            // Open all Expander in this page
            Result += OpenExpander(chrome, ScreenShotPath);
            // Check insert, delete and edit
            Result += CheckInsertDelete(chrome, ScreenShotPath);
            return Result;
        }
    }
}
