using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ballance.it.for_closure
{
   
    public class Runner
    {
        private RepositoryBase _propertyPersistenceManager;
        public Runner()
        {
            _propertyPersistenceManager = new RepositoryBase(
                "Server=re-db.chtlgfr8b1iu.us-east-1.rds.amazonaws.com;Port=7306;Database=re_db;Uid=re_user;Pwd=vXani82LdScu;");
        }
        
        public void Run()
        {
            // RunAgilityPackAsync("https://sales.hutchenslawfirm.com/NCfcSalesList.aspx",  "//*[@id='SalesListGrid_ctl01']/tbody");
            RunSeleniumWebScraper("https://sales.hutchenslawfirm.com/NCfcSalesList.aspx", String.Empty);
        }  

        private void RunSeleniumWebScraper(string baseUrl, string nodeSelector)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            using (var driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl(baseUrl);
                
                // Page 1
                Screenshot pageInitialScreenshot = driver.GetScreenshot();
                var screenshotInitialByteArray = pageInitialScreenshot.AsByteArray;
                File.WriteAllBytes("screenshot_initial.jpg", screenshotInitialByteArray);

                // Page 2
                var jsToBeExecutedGoPageTwo = "__doPostBack('SalesListGrid$ctl01$ctl03$ctl01$ctl02','')";
                ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecutedGoPageTwo);
                
                var waitPage2 = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                waitPage2.Until(condition: ExpectedConditions.ElementExists(By.Id("form1")));
                Screenshot pageTwoScreenshot = driver.GetScreenshot();
                var screenshotPageTwoByteArray = pageTwoScreenshot.AsByteArray;
                File.WriteAllBytes("screenshot_page2.jpg", screenshotPageTwoByteArray);

                // Page 3
                var jsToBeExecutedGoPageThree = "__doPostBack('SalesListGrid$ctl01$ctl03$ctl01$ctl03','')";
                ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecutedGoPageThree);
                
                var waitPage3 = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                waitPage3.Until(condition: ExpectedConditions.ElementExists(By.Id("form1")));
                Screenshot pageThreeScreenshot = driver.GetScreenshot();
                var screenshotPageThreeByteArray = pageThreeScreenshot.AsByteArray;
                File.WriteAllBytes("screenshot_page3.jpg", screenshotPageThreeByteArray);

                // Page 4
                var jsToBeExecutedGoPageFour = "__doPostBack('SalesListGrid$ctl01$ctl03$ctl01$ctl04','')";
                ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecutedGoPageFour);
                
                var waitPage4 = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                waitPage4.Until(condition: ExpectedConditions.ElementExists(By.Id("form1")));
                Screenshot pageFourScreenshot = driver.GetScreenshot();
                var screenshotPageFourByteArray = pageFourScreenshot.AsByteArray;
                File.WriteAllBytes("screenshot_page4.jpg", screenshotPageFourByteArray);
                
                // Page 5
                var jsToBeExecutedGoPageFive = "__doPostBack('SalesListGrid$ctl01$ctl03$ctl01$ctl05','')";
                ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecutedGoPageFive);
                
                var waitPage5 = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
                waitPage5.Until(condition: ExpectedConditions.ElementExists(By.Id("form1")));
                Screenshot pageFiveScreenshot = driver.GetScreenshot();
                var screenshotPageFiveByteArray = pageFiveScreenshot.AsByteArray;
                File.WriteAllBytes("screenshot_page5.jpg", screenshotPageFiveByteArray);
            }
        }

        private void RunAgilityPackAsync(string baseUrl, string nodeSelector)
        {
            var webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.109 Safari/537.36");
            webClient.DownloadStringCompleted += (sender, e) => DownloadListCompleted(sender, e, nodeSelector);
            webClient.DownloadStringAsync(new Uri(baseUrl));
        }

        private void DownloadListCompleted(object sender, DownloadStringCompletedEventArgs e, string nodeSelector)
        {
            var htmlRaw = e.Result;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlRaw);
            var node = htmlDoc.DocumentNode.SelectSingleNode(nodeSelector);
            
            if (node == null)
            {
                // Sanity check on root node.
                throw new ApplicationException("unable to retrieve root node");
            }

            foreach(var row in node.ChildNodes)
            {
                try
                {
                    if (row.NodeType == HtmlNodeType.Text)
                    {
                        // First row is sometimes not a data row, skip it.
                        continue;
                    }

                    var propertyModel = new PropertyModel()
                    {
                        PropertyId = row.SelectSingleNode("td[1]").InnerText,
                        SpNumber = row.SelectSingleNode("td[2]").InnerText,
                        County = row.SelectSingleNode("td[3]").InnerText,
                        SaleDateTime = row.SelectSingleNode("td[4]").InnerText,
                        Address = row.SelectSingleNode("td[5]").InnerText,
                        CityStateZip = row.SelectSingleNode("td[6]").InnerText,
                        DeedOfTrust = row.SelectSingleNode("td[7]").InnerText,
                        Bid = row.SelectSingleNode("td[8]").InnerText
                    };

                    _propertyPersistenceManager.PersistProperty(propertyModel);

                    // System.Console.WriteLine($"Id: {propertyModel.PropertyId}");
                    // System.Console.WriteLine($"SP#: {propertyModel.SpNumber}");
                    // System.Console.WriteLine($"County: {propertyModel.County}");
                    // System.Console.WriteLine($"Sale Date/Time: {propertyModel.SaleDateTime}");
                    // System.Console.WriteLine($"Address: {propertyModel.Address}");
                    // System.Console.WriteLine($"City / State / ZIP: {propertyModel.CityStateZip}");
                    // System.Console.WriteLine($"Deed of Trust {propertyModel.DeedOfTrust}");
                    // System.Console.WriteLine($"Bid: {propertyModel.Bid}");
                    // System.Console.WriteLine("------");
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(row.NodeType);
                    System.Console.WriteLine("Error selecting node.");
                    System.Console.WriteLine(ex);
                }
            }
        }
    }
}
