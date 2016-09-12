using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests_OldMethod
{
    [TestClass]
    public class UnitTest1
    {
        public ChromeOptions options = new ChromeOptions();
        public IWebDriver driver;
       
        [TestMethod]
        
        // Count the number of BMW 2 Series
        public void CountCars()
        {
            //Get driver
            driver = GetDriver();

            //Click on the Car icon
            driver.FindElement(By.ClassName("car-for-sale-module")).FindElement(By.CssSelector("a[href*='#tab1']")).Click();
            
            //Find the tab1 element
            IWebElement tab = driver.FindElement(By.Id("tab1"));
          
            //Select the items in the filters
            setElement(tab, By.Id("Car-MakesDropdown"), "BMW");
            setElement(tab, By.Id("Car-ModelsDropdown"), "2 Series");
            
            //Click the search button
            tab.FindElement(By.ClassName("btn-wrap")).Click();

            //Wait for the results pages to load
            IWebElement searchResults = waituntil(By.Id("search-results"));

            //Confirm results page has more than 0 entries (doesnt take into account adverts)
            Assert.IsTrue(searchResults.FindElements(By.ClassName("ad-listing")).Count() > 0, "The Actual count is zero");

            //Close driver
            driver.Close();
        }

        [TestMethod]
        // Count the number of Ducati bikes < 10000
        public void CountBikes()
        {
            //Get driver
            driver = GetDriver();

            //Click on the bike icon
            driver.FindElement(By.ClassName("car-for-sale-module")).FindElement(By.CssSelector("a[href*='#tab2']")).Click();

            //Find the tab2 element
            IWebElement tab = driver.FindElement(By.Id("tab2"));

            //Select the items in the filters
            setElement(tab, By.Id("Bike-MakesDropdown"), "Ducati");
            setElement(tab, By.Id("Bike-MaxPrice"), "£10,000");

            //Click the search button
            tab.FindElement(By.ClassName("btn-wrap")).Click();

            //Wait for the results pages to load
            IWebElement searchResults = waituntil(By.Id("search-results"));
            
            //Confirm results page has more than 0 entries (doesnt take into account adverts)
            Assert.IsTrue(searchResults.FindElements(By.ClassName("ad-listing")).Count() > 0, "The Actual count is zero");

            //Identify the dropdown
            IWebElement dropDown = driver.FindElement(By.Id("search-results-left")).FindElement(By.Id("MakesModels")).FindElement(By.Id("MakeDropdown"));
            
            //Get the select item
            SelectElement selectElement = new SelectElement(dropDown);

            //Confirm selected option in dropdown is ducati)
            Assert.IsTrue(selectElement.SelectedOption.Text == "Ducati", "The Make is not set to Ducati");

            //Close driver
            driver.Close();
        }

        // Function to set up the driver - using chrome
        public IWebDriver GetDriver()
        {
            //Maximize page when starting driver
            options.AddArgument("--start-maximized");

            //Start a new chrome driver with associated options
            driver = new ChromeDriver("C:\\temp\\SeleniumWebDriver", options);

            //Goto pistonheads page
            driver.Navigate().GoToUrl("http://www.pistonheads.com");

            //wait for driver to finish loading
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));

            //return the driver
            return driver;
        }

        //Wait for an element to stop loading
        public IWebElement waituntil(By byString)
        {
            try
            {
                //Create the wait settings
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                //check to see if the passed value is found within wait timeout
                return wait.Until(d => d.FindElement(byString));
            }
            catch (WebDriverTimeoutException)
            {
                driver.Close();
            }
            return null;
        }

        // Standard implementation
        public void setElement(IWebElement parent, By byString, String text)
        {
            try
            {
                //Set element item
                new SelectElement(parent.FindElement(byString)).SelectByText(text);
            }
            catch (NoSuchElementException)
            {
                driver.Close();
            }
            catch (Exception ex)
            {
                writeoutexceptions(ex);
                driver.Close();
            }
        }

        // Override #1 for using driver rather than element
        public void setElement(IWebDriver d, By byString, String text)
        {
            try
            {
                new SelectElement(d.FindElement(byString)).SelectByText(text);
            }
            catch (NoSuchElementException)
            {
                driver.Close();
            }
            catch (Exception ex)
            {
                writeoutexceptions(ex);
                driver.Close();
            }
        }

        #region writeoutexceptions
        public static void writeoutexceptions(Exception ex)
        {
            Console.WriteLine("Error: The following error was found");
            Console.WriteLine("\nMessage ---\n{0}", ex.Message);
            Console.WriteLine("\nSource ---\n{0}", ex.Source);
            Console.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
            Console.WriteLine("\nTargetSite ---\n{0}", ex.TargetSite);
        }
        #endregion
    }
}
