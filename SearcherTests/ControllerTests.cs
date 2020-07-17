using InfoTrackSearcher.Controllers;
using InfoTrackSearcher.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SearcherTests
{
    [TestClass]
    public class ControllerTests
    {
        [TestMethod]
        public void TestSearchGoogleForValid()
        {
            var controller = new SearchController();
            var model = new SearchModel { Keywords = "online title search", URL = "https://www.google.com.au" };
            var result = controller.Index(model);
            Assert.AreEqual("5, 6", ((SearchModel)result.Model).Results);
        }

        [TestMethod]
        public void TestSearchGoogleForInvalid()
        {
            var controller = new SearchController();
            var model = new SearchModel { Keywords = "apple", URL = "https://www.google.com.au" };
            var result = controller.Index(model);
            Assert.AreEqual("0", ((SearchModel)result.Model).Results);
        }

        [TestMethod]
        public void TestEngineGoogleForValid()
        {
            var controller = new EngineController();
            var model = new SearchModel { Keywords = "online title search", SearchEngine = nameof(SearchModel.SearchEngines.Google) };
            var result = controller.Index(model);
            Assert.AreEqual("5, 6", ((SearchModel)result.Model).Results);
        }

        [TestMethod]
        public void TestEngineGoogleForInvalid()
        {
            var controller = new EngineController();
            var model = new SearchModel { Keywords = "apple", SearchEngine = nameof(SearchModel.SearchEngines.Google) };
            var result = controller.Index(model);
            Assert.AreEqual("0", ((SearchModel)result.Model).Results);
        }

        [TestMethod]
        public void TestEngineBingForValid()
        {
            var controller = new EngineController();
            var model = new SearchModel { Keywords = "online title search", SearchEngine = nameof(SearchModel.SearchEngines.Bing) };
            var result = controller.Index(model);
            Assert.AreNotEqual("0",((SearchModel)result.Model).Results);
        }

        [TestMethod]
        public void TestEngineBingForInvalid()
        {
            var controller = new EngineController();
            var model = new SearchModel { Keywords = "apple", SearchEngine = nameof(SearchModel.SearchEngines.Bing) };
            var result = controller.Index(model);
            Assert.AreEqual("0",((SearchModel)result.Model).Results);
        }
    }
}