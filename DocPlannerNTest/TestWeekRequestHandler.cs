using DocplannerTest.DTO;
using DocplannerTest.Handlers;
using DocplannerTest.Model;
using DocplannerTest.Service;
using DocplannerTest.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocPlannerNTest
{
    
    public class TestWeekRequestHandler
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Handle_ShouldThrowExceptionWhenDateHasInvalidFormat()
        {
            // slotService,
            //IAvailabilityTimeDirector

            var mockService = new Mock<IAvailabilityService>();
            var mockDirector = new Mock<IAvailabilityTimeDirector>();
            
            WeekRequestHandler handler = new(mockService.Object, mockDirector.Object);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => handler.Handle("test-test-test"));
        }

        [Test]
        public void Test_Handle_ShouldThrowExceptionWhenDateIsNotMonday()
        {
            var mockService = new Mock<IAvailabilityService>();
            var mockDirector = new Mock<IAvailabilityTimeDirector>();

            WeekRequestHandler handler = new(mockService.Object, mockDirector.Object);

            var ex = Assert.ThrowsAsync<ArgumentException>(() => handler.Handle("20240512"));
        }

        [Test]
        public async Task Test_Handle_NodaysAvailable_NoException()
        {
            var mockService = new Mock<IAvailabilityService>();
            var mockDirector = new Mock<IAvailabilityTimeDirector>();

            WeeklyAvailabilityDTO weekly = new();

            mockService.Setup(service => service.GetWeekly("20240513")).ReturnsAsync(weekly);

            WeekRequestHandler handler = new(mockService.Object, mockDirector.Object);

            var freeSlots = await handler.Handle("20240513");
            Assert.AreEqual(0, freeSlots.Count);
        }

        [Test]
        public async Task Test_Handle_OnlyOneDay()
        {
            var mockService = new Mock<IAvailabilityService>();
            var mockDirector = new Mock<IAvailabilityTimeDirector>();

            WeeklyAvailabilityDTO weekly = new();
            weekly.SlotDurationMinutes = 60;
            weekly.Monday = new();

            string testDay = "20240513";
            DateOnly currentDay = DateOnly.ParseExact(testDay, "yyyyMMdd");

            mockService.Setup(service => service.GetWeekly(testDay)).ReturnsAsync(weekly);

            var freeSlots = new List<Slot>();
            freeSlots.Add(new Slot()
            {
                Start = DateTime.Now,
                Stop = DateTime.Now.AddMinutes(10)
            });

            mockDirector.Setup(s => s.FreeSlotByAvailabilityData(currentDay, weekly.Monday, 60)).Returns(freeSlots);
            WeekRequestHandler handler = new(mockService.Object, mockDirector.Object);

            var res = await handler.Handle(testDay);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(1, res.Count);
                Assert.AreEqual(res["Monday"][0].Start, freeSlots[0].Start);
                Assert.AreEqual(res["Monday"][0].Stop, freeSlots[0].Stop);
            });
        }
    }
}
