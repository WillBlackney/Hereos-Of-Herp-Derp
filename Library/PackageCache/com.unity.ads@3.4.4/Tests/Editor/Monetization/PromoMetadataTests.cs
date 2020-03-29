using System;
using NUnit.Framework;

namespace UnityEngine.Monetization.Editor.Tests
{
    [TestFixture]
    public class PromoMetadataTests
    {
        public struct TimeRemainingTestData
        {
            public DateTime firstImpressionDate;
            public TimeSpan offerDuration;
            public TimeSpan timeRemaining;
        }
        private TimeRemainingTestData[] timeRemainingTestData;

        public struct IsExpiredTestData
        {
            public DateTime firstImpressionDate;
            public TimeSpan offerDuration;
            public bool isExpired;
        }
        private IsExpiredTestData[] isExpiredTestData;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.isExpiredTestData = new IsExpiredTestData[]
            {
                new IsExpiredTestData
                {
                    firstImpressionDate = DateTime.Now.Add(-TimeSpan.FromSeconds(100)),
                    offerDuration = TimeSpan.FromSeconds(60),
                    isExpired = true
                },
                new IsExpiredTestData
                {
                    offerDuration = TimeSpan.FromSeconds(100),
                    isExpired = false
                },
                new IsExpiredTestData
                {
                    firstImpressionDate = DateTime.Now.Add(-TimeSpan.FromSeconds(20)),
                    offerDuration = TimeSpan.FromSeconds(60),
                    isExpired = false
                }
            };

            this.timeRemainingTestData = new TimeRemainingTestData[]
            {
                new TimeRemainingTestData
                {
                    firstImpressionDate = DateTime.Now.Add(-TimeSpan.FromSeconds(100)),
                    offerDuration = TimeSpan.FromSeconds(60),
                    timeRemaining = TimeSpan.FromSeconds(-40)
                },
                new TimeRemainingTestData
                {
                    offerDuration = TimeSpan.FromSeconds(100),
                    timeRemaining = TimeSpan.FromSeconds(100)
                },
                new TimeRemainingTestData
                {
                    firstImpressionDate = DateTime.Now.Add(-TimeSpan.FromSeconds(20)),
                    offerDuration = TimeSpan.FromSeconds(60),
                    timeRemaining = TimeSpan.FromSeconds(40)
                }
            };
        }

        [Test]
        public void TestIsExpired()
        {
            foreach (var tt in isExpiredTestData)
            {
                var metadata = new PromoMetadata
                {
                    impressionDate = tt.firstImpressionDate,
                    offerDuration = tt.offerDuration
                };
                Assert.That(metadata.isExpired, Is.EqualTo(tt.isExpired));
            }
        }

        [Test]
        public void TestTimeRemaining()
        {
            foreach (var tt in timeRemainingTestData)
            {
                var metadata = new PromoMetadata
                {
                    impressionDate = tt.firstImpressionDate,
                    offerDuration = tt.offerDuration
                };
                Assert.That(metadata.timeRemaining, Is.EqualTo(tt.timeRemaining).Within(1).Seconds);
            }
        }

        [TestCase("100.gold.coins", true)]
        [TestCase(null, false)]
        public void TestIsPremium(string productId, bool isPremium)
        {
            var metadata = new PromoMetadata
            {
                premiumProduct =
                {
                    productId = productId
                }
            };
            Assert.That(metadata.isPremium, Is.EqualTo(isPremium));
        }
    }
}
