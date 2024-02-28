using Exercise1.Cloud;
using Exercise1.Queries;
using Moq;

namespace Exercise1.Tests
{
    public class FetchPayloadQueryTests
	{
        readonly Guid blobId = new();
        private readonly Mock<ICloudClient> _cloudClientMock = new(MockBehavior.Strict);

        [Test]
        public async Task GivenFetchPayloadQuery_WhenBlobIsFound_ThenPayloadReturned()
        {
            //Assign
            var query = new FetchPayloadQuery(blobId);
            WhenBlobIsFound();

            //Act
            var result = await Act(query);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Payload, Is.EqualTo("payload"));
                Assert.That(result.Exists, Is.EqualTo(true));
            });
        }

        [Test]
        public async Task GivenFetchPayloadQuery_WhenBlobIsNotFound_ThenEmptyPayloadReturned()
        {
            //Assign
            var query = new FetchPayloadQuery(blobId);
            WhenBlobIsNotFound();

            //Act
            var result = await Act(query);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Payload, Is.EqualTo(""));
                Assert.That(result.Exists, Is.EqualTo(false));
            });
        }

        private async Task<FetchPayloadQueryResponse> Act(FetchPayloadQuery query)
        {
            var sut = new FetchPayloadQueryHandler(_cloudClientMock.Object);
            return await sut.Handle(query, new CancellationToken());
        }

        private void WhenBlobIsFound()
        {
            _cloudClientMock.Setup(x => x.Fetch(blobId)).ReturnsAsync(("payload", true));
        }

        private void WhenBlobIsNotFound()
        {
            _cloudClientMock.Setup(x => x.Fetch(blobId)).ReturnsAsync(("", false));
        }
    }
}

