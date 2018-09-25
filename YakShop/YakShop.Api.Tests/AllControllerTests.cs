using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using FluentAssertions;
using YakShop.Api.Controllers;
using YakShop.Common.Models;

namespace YakShop.Api.Tests
{
    [Collection("YakShopContext")]
    public class AllControllerTests
    {
        InMemoryContextTestFixture _context;

        public AllControllerTests(InMemoryContextTestFixture context)
        {
            _context = context;
        }

        [Fact(DisplayName = "LoadController - Post(herdList) Should Reset Content")]
        public void LoadController_Post_HerdList_Should_Reset_Content()
        {
            var controller = new LoadController(_context.Context);

            var herdList = GetDefaultLoadContent();
            var result = controller.Post(herdList);

            Assert.NotNull(result);
            var apiResult = result.Should().BeOfType<StatusCodeResult>().Subject;
            Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status205ResetContent);
            Assert.True(_context.Context.Herds.Count() == 3);
        }

        [Fact(DisplayName = "LoadController - Post(herdList) with Invalid object should fail")]
        public void LoadController_Post_HerdList_With_Invalid_Object_Should_Fail()
        {
            var controller = new LoadController(_context.Context);

            var count = _context.Context.Herds.Count();
            var result = controller.Post(null);

            Assert.NotNull(result);
            var apiResult = result.Should().BeOfType<ObjectResult>().Subject;
            Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError);
            Assert.True(_context.Context.Herds.Count() == count);
        }

        [Fact(DisplayName = "LoadController - Post(herdList) Invalid Herd List should fail")]
        public void LoadController_Post_HerdList_With_Invalid_Herd_List_Should_Fail()
        {
            var controller = new LoadController(_context.Context);

            var count = _context.Context.Herds.Count();
            var herdList = new HerdList() {
                Herds = null
            };
            var result = controller.Post(herdList);

            Assert.NotNull(result);
            var apiResult = result.Should().BeOfType<ObjectResult>().Subject;
            Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError);
            Assert.True(_context.Context.Herds.Count() == count);
        }

        [Fact(DisplayName = "LoadController - Post(days) Should Set Elapsed Days")]
        public void LoadController_Post_Days_Should_Set_Elapsed_Days()
        {
            var controller = new LoadController(_context.Context);

            int elapsedDays = 23;
            var result = controller.Post(elapsedDays);

            Assert.NotNull(result);
            var apiResult = result.Should().BeOfType<StatusCodeResult>().Subject;
            Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status205ResetContent);
            Assert.True(_context.Context.Settings.FirstOrDefault() != null);
            Assert.True(_context.Context.Settings.FirstOrDefault().ElapsedDays == elapsedDays);
        }

        [Fact(DisplayName = "LoadController Get Should Return Elapsed Days")]
        public void LoadController_Get_Should_Return_Elapsed_Days()
        {
            var controller = new LoadController(_context.Context);

            var settings = _context.Context.Settings.FirstOrDefault();
            var result = controller.Get();

            Assert.NotNull(result);
            if (settings != null)
            {
                var apiResult = result.Should().BeOfType<OkObjectResult>().Subject;
                Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status200OK);
                Assert.NotNull(apiResult.Value);
                Assert.True(Convert.ToInt32(apiResult.Value) == settings.ElapsedDays);
            }
            else
            {
                var apiResult = result.Should().BeOfType<StatusCodeResult>().Subject;
                Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
            }
        }

        [Fact(DisplayName = "HerdController Get(days) Should Return Herd view for the day")]
        public void HerdController_Get_Should_Return_Herd_View_For_The_Days()
        {
            var loadController = new LoadController(_context.Context);

            var herdList = GetDefaultLoadContent();
            var result = loadController.Post(herdList);
            var controller = new HerdController(_context.Context);

            int elapsedDays = 12;
            result = controller.Get(elapsedDays);

            Assert.NotNull(result);
            var apiResult = result.Should().BeOfType<OkObjectResult>().Subject;
            Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status200OK);
            var herdDataList = apiResult.Value as HerdDataList;
            Assert.True(herdList.Herds.Count() == herdDataList.Herd.Count());

        }

        [Fact(DisplayName = "HerdController Get(days) Should Return Empty view for the day without data Loaded")]
        public void HerdController_Get_Should_Return_Empty_View_For_The_Days_Without_Data_Loaded()
        {
           var controller = new HerdController(_context.FreshContext);

            int elapsedDays = 12;
            var result = controller.Get(elapsedDays);

            Assert.NotNull(result);
            var apiResult = Assert.IsType<NoContentResult>(result);
            Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent);
        }

        [Fact(DisplayName = "HerdController Get(days) Should Return Herd view for the day Age incremented")]
        public void HerdController_Get_Should_Return_Herd_View_For_The_Days_With_Age_Incremented()
        {
            var loadController = new LoadController(_context.Context);

            var herdList = GetDefaultLoadContent();
            var result = loadController.Post(herdList);
            var controller = new HerdController(_context.Context);

            int elapsedDays = 12;
            result = controller.Get(elapsedDays);

            Assert.NotNull(result);
            var apiResult = result.Should().BeOfType<OkObjectResult>().Subject;
            Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status200OK);
            var herdDataList = apiResult.Value as HerdDataList;
            Assert.True(herdList.Herds.Count() == herdDataList.Herd.Count());
 
            foreach(var herd in herdDataList.Herd)
            {
                var item = herdList.Herds.Where(h => h.Name == herd.Name).FirstOrDefault();
                Assert.NotNull(item);
                Assert.True((int)(herd.Age * 100) == (int)(item.Age * 100 + elapsedDays));
            }
        }

        [Fact(DisplayName = "HerdController Get(days) Herd should die after 10 years")]
        public void HerdController_Get_Herd_Should_Die_On_Or_After_10_Years()
        {
            var loadController = new LoadController(_context.Context);

            var herdList = GetDefaultLoadContent();
            var result = loadController.Post(herdList);
            var controller = new HerdController(_context.Context);

            int elapsedDays = 50;
            result = controller.Get(elapsedDays);

            Assert.NotNull(result);
            var apiResult = result.Should().BeOfType<OkObjectResult>().Subject;
            Assert.True(apiResult.StatusCode == Microsoft.AspNetCore.Http.StatusCodes.Status200OK);
            var herdDataList = apiResult.Value as HerdDataList;
            Assert.True(herdList.Herds.Count() - 1 == herdDataList.Herd.Count());

            foreach (var herd in herdList.Herds)
            {
                var item = herdDataList.Herd.Where(h => h.Name == herd.Name).FirstOrDefault();
                if(item == null)
                {
                    Assert.True((int)(herd.Age * 100 + elapsedDays) >= 1000);
                }
            }
        }

        private HerdList GetDefaultLoadContent()
        {
            var items = new HerdList()
            {
                Herds = new List<LabYak>() {
                        new LabYak() {
                            Name = "Betty-1",
                            Age = 4,
                            Sex = "f"
                        },
                        new LabYak() {
                            Name = "Betty-2",
                            Age = 8,
                            Sex = "f"
                        },
                         new LabYak() {
                            Name = "Betty-3",
                            Age = 9.5M,
                            Sex = "f"
                        }
                    }
            };

            return items;
        }
    }
}
