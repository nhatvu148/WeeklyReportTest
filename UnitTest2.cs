using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace WeeklyReportTest
{

    [TestClass]
    [TestCategory("Locating Web Elements")]
    public class IdentifyWebElements
    {
        public IWebDriver Driver { get; private set; }
        private IWebElement userName;
        private IWebElement password;
        private WebDriverWait wait;

        [TestInitialize]
        public void SetupBeforeEveryTestMethod()
        {
            Driver = new WebDriverFactory().Create(BrowserType.Chrome);
            Driver.Navigate().GoToUrl("http://localhost:3000");
            Driver.Manage().Window.Maximize();

            userName = Driver.FindElement(By.Id("normal_login_username"));
            userName.Clear();
            userName.SendKeys("Akiyama");
            userName.SendKeys(Keys.Tab);

            password = Driver.FindElement(By.Id("normal_login_password"));
            password.Clear();
            password.SendKeys("147258369");
            password.SendKeys(Keys.Enter);

            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        }
        [TestMethod]
        public void Login()
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(text(),'akiyama')]"))).Click();

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            var logoutBtn = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.TagName("button")));
            logoutBtn[4].Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@type='submit']"))).Click();
        }
        [TestMethod]
        public void Drag()
        {
            var actions = new Actions(Driver);

            var addBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@class='MuiFab-label']/..")));

            //wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class='ant-empty']")));
            System.Threading.Thread.Sleep(5000);
            addBtn.Click();
            addBtn.Click();


            var selects = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.XPath("//tr[@data-rbd-draggable-id='0']//div[@class='ant-select-selector']")));
            selects[0].Click();
            var typeIn = actions.SendKeys("999999").SendKeys(Keys.Enter)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .SendKeys(Keys.Tab)
                .Build();
            typeIn.Perform();

            IWebElement sourceElement = Driver.FindElement(By.XPath("//button[@data-rbd-drag-handle-draggable-id='0']/span"));
            IWebElement targetElement = Driver.FindElement(By.XPath("//*[contains(text(),'Save Data')]"));

            actions = new Actions(Driver);
            var drag = actions.ClickAndHold(sourceElement).MoveToElement(targetElement).Release(targetElement).Build();
            drag.Perform();

        }
        [TestMethod]
        public void Droppable()
        {
            Driver.Navigate().GoToUrl("https://demoqa.com/droppable/");
            var actions = new Actions(Driver);

            IWebElement sourceElement = Driver.FindElement(By.Id("draggable"));
            IWebElement targetElement = Driver.FindElement(By.Id("droppable"));
            //actions.DragAndDrop(sourceElement, targetElement).Perform();
            var drag = actions.ClickAndHold(sourceElement).MoveToElement(targetElement).MoveByOffset(0, 10).Release().Build();

            drag.Perform();

            Assert.AreEqual("Dropped!", Driver.FindElement(By.XPath("//*[@id='droppable']/p")).Text);
            Console.WriteLine("Hello");
        }
        [TestCleanup]
        public void CleanupAfterEveryTestMethod()
        {
            Driver.Quit();
        }
    }
}
