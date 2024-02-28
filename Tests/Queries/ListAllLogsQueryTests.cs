using Exercise1.Cloud;
using Exercise1.Queries.ListAllLogs;
using Moq;

namespace Exercise1.Tests
{
    public class ListAllLogsQueryTests
	{
        private readonly DateTimeOffset from = new(2024, 2, 1, 10, 0, 0, TimeSpan.FromHours(2));
        private readonly DateTimeOffset to = new(2024, 2, 1, 10, 0, 0, TimeSpan.FromHours(2));
        private Mock<ICloudClient> cloudClientMock;

        [SetUp]
        public void SetUp()
        {
            cloudClientMock = new Mock<ICloudClient>(MockBehavior.Strict);
        }

        [Test]
        public async Task GivenListAllLogsQuery_WhenNoLogsArePresent_ThenNoLogsReturnedForSpecifiedPeriod()
        {
            //Assign
            var query = new ListAllLogsQuery(from, to);
            WhenNoLogsArePresent();
            //Act
            var response = await Act(query);
            //Assert
            Assert.That(response.Logs.Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task GivenListAllLogsQuery_WhenLogsArePresent_ThenLogsReturnedForSpecifiedPeriod()
        {
            //Assign
            var query = new ListAllLogsQuery(from, to);
            WhenLogsArePresent();
            //Act
            var response = await Act(query);
            //Assert
            Assert.That(response.Logs.Count(), Is.EqualTo(2));
        }

        private void WhenNoLogsArePresent()
        {
            cloudClientMock.Setup(x => x.FetchLogs(from, to)).ReturnsAsync(Enumerable.Empty<Log>());
        }

        private void WhenLogsArePresent()
        {
            var x = new List<Log>() { new Log(Status.Success, from), new Log(Status.Failure, to) };
            cloudClientMock.Setup(x => x.FetchLogs(from, to)).ReturnsAsync(x);
        }

        private async Task<ListAllLogsResponse> Act(ListAllLogsQuery query)
        {
            var sut = new ListAllLogsHandler(cloudClientMock.Object);
            return await sut.Handle(query, new CancellationToken());
        }
    }
}

