using DocplannerTest.Utils;

namespace DocPlannerNTest
{
    public class TestAvailabilityTimeBuilder
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Build_ShouldThrowExceptionWhenNoDate()
        {
            AvailabilityTimeBuilder builder = new();
            builder.SetBeginHour(8).SetEndHour(10).SetSlotDuration(30);

            var ex = Assert.Throws<Exception>(() => builder.Build());
        }

        [Test]
        public void Test_Build_ShouldThrowExceptionWhenNoBeginHour()
        {
            AvailabilityTimeBuilder builder = new();
            builder.SetDay(new DateOnly(2024, 5, 16));
            builder.SetEndHour(10).SetSlotDuration(30);

            var ex = Assert.Throws<Exception>(() => builder.Build());
        }

        [Test]
        public void Test_Build_ShouldThrowExceptionWhenNoEndHour()
        {
            AvailabilityTimeBuilder builder = new();
            builder.SetDay(new DateOnly(2024, 5, 16));
            builder.SetBeginHour(8).SetSlotDuration(30);

            var ex = Assert.Throws<Exception>(() => builder.Build());
        }

        [Test]
        public void Test_Build_ShouldThrowExceptionWhenNoDurationSlot()
        {
            AvailabilityTimeBuilder builder = new();
            builder.SetDay(new DateOnly(2024, 5, 16));
            builder.SetBeginHour(8).SetEndHour(10);

            var ex = Assert.Throws<Exception>(() => builder.Build());
        }

        [Test]
        public void Test_Build_WithoutBusySlots()
        {
            AvailabilityTimeBuilder builder = new();
            builder.SetDay(new DateOnly(2024, 5, 16));
            builder.SetBeginHour(8).SetEndHour(10).SetSlotDuration(30);
            var freeSlots = builder.Build();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(4, freeSlots.Count);
                
                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 0, 0), freeSlots[0].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 30, 0), freeSlots[0].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 30, 0), freeSlots[1].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 0, 0), freeSlots[1].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 0, 0), freeSlots[2].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 30, 0), freeSlots[2].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 30, 0), freeSlots[3].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 10, 0, 0), freeSlots[3].Stop);

            });
        }

        [Test]
        public void Test_Build_WithOneBusySlot_BusySlotCorrespondsToDuration()
        {
            AvailabilityTimeBuilder builder = new();
            builder.SetDay(new DateOnly(2024, 5, 16));
            builder.SetBeginHour(8).SetEndHour(12).SetSlotDuration(30);
            builder.AddBusyTime(new DateTime(2024, 5, 16, 10, 0, 0), new DateTime(2024, 5, 16, 11, 0, 0));

            var freeSlots = builder.Build();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(6, freeSlots.Count);

                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 0, 0), freeSlots[0].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 30, 0), freeSlots[0].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 30, 0), freeSlots[1].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 0, 0), freeSlots[1].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 0, 0), freeSlots[2].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 30, 0), freeSlots[2].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 30, 0), freeSlots[3].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 10, 0, 0), freeSlots[3].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 11, 0, 0), freeSlots[4].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 11, 30, 0), freeSlots[4].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 11, 30, 0), freeSlots[5].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 12, 0, 0), freeSlots[5].Stop);

            });
        }

        [Test]
        public void Test_Build_WithOneBusySlot_BusySlotNotCorrespondsToDuration()
        {
            AvailabilityTimeBuilder builder = new();
            builder.SetDay(new DateOnly(2024, 5, 16));
            builder.SetBeginHour(8).SetEndHour(12).SetSlotDuration(30);
            builder.AddBusyTime(new DateTime(2024, 5, 16, 10, 0, 0), new DateTime(2024, 5, 16, 11, 15, 0));

            var freeSlots = builder.Build();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(5, freeSlots.Count);

                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 0, 0), freeSlots[0].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 30, 0), freeSlots[0].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 8, 30, 0), freeSlots[1].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 0, 0), freeSlots[1].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 0, 0), freeSlots[2].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 30, 0), freeSlots[2].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 30, 0), freeSlots[3].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 10, 0, 0), freeSlots[3].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 11, 30, 0), freeSlots[4].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 12, 0, 0), freeSlots[4].Stop);

            });
        }

        [Test]
        public void Test_Build_TwoBusySlot()
        {
            AvailabilityTimeBuilder builder = new();
            builder.SetDay(new DateOnly(2024, 5, 16));
            builder.SetBeginHour(8).SetEndHour(13).SetSlotDuration(60);
            builder.AddBusyTime(new DateTime(2024, 5, 16, 10, 0, 0), new DateTime(2024, 5, 16, 12, 00, 0));
            builder.AddBusyTime(new DateTime(2024, 5, 16, 8, 0, 0), new DateTime(2024, 5, 16, 9, 0, 0));

            var freeSlots = builder.Build();

            Assert.Multiple(() =>
            {
                Assert.AreEqual(2, freeSlots.Count);

                Assert.AreEqual(new DateTime(2024, 5, 16, 9, 0, 0), freeSlots[0].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 10, 0, 0), freeSlots[0].Stop);

                Assert.AreEqual(new DateTime(2024, 5, 16, 12, 0, 0), freeSlots[1].Start);
                Assert.AreEqual(new DateTime(2024, 5, 16, 13, 0, 0), freeSlots[1].Stop);
            });
        }
    }
}