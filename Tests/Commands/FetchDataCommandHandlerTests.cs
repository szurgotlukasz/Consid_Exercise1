using Exercise1.Cloud;
using Exercise1.Commands.FetchData;
using Microsoft.Extensions.Logging;
using Moq;

namespace Exercise1.Tests;

public class FetchDataCommandHandlerTests
{
    private readonly DateTimeOffset SystemTime = new(2024, 1, 1,11,10,35, TimeSpan.FromHours(2));
    private Mock<IPublicApisClient> _publicApiMock;
    private Mock<ICloudClient> _cloudClient;
    private Mock<ILogger<FetchDataCommandHandler>> _loggerMock;
    private Mock<ISystemTimeProvider> _systemTimeProvider;
    private readonly PublicApisResponse _expectedPayload = new() { Count = 1, Entries = new List<Entry>() };

    [SetUp]
    public void SetUp()
    {
        _publicApiMock = new Mock<IPublicApisClient>(MockBehavior.Strict);
        _publicApiMock.Setup(x => x.GetAsync()).ReturnsAsync(_expectedPayload);
        _cloudClient = new Mock<ICloudClient>(MockBehavior.Strict);
        _cloudClient.Setup(x => x.Log(It.IsAny<Log>())).Returns(Task.CompletedTask);
        _systemTimeProvider = new Mock<ISystemTimeProvider>(MockBehavior.Strict);
        _systemTimeProvider.SetupGet(x => x.Now).Returns(SystemTime);
        _loggerMock = new Mock<ILogger<FetchDataCommandHandler>>();
    }

    [Test]
    public async Task GivenFetchDataCommand_WhenSuccess_ThenLogStoredInTable()
    {
        //Assign
        var command = GivenFetchDataCommand();

        //Act
        await ExecuteCommand(command);

        //Assert
        LogStoredWithStatus(Status.Success);
    }

    [Test]
    public async Task GivenFetchDataCommand_WhenException_ThenLogStoredInTable()
    {
        //Assign
        var command = GivenFetchDataCommand();
        WhenPublicApisThrowsException();

        //Act
        await ExecuteCommand(command);

        //Assert
        LogStoredWithStatus(Status.Failure);
    }


    [Test]
    public async Task GivenFetchDataCommand_ThenPayloadStoredInBlobContainer()
    {
        //Assign
        var command = GivenFetchDataCommand();

        //Act
        await ExecuteCommand(command);

        //Assert
        PayloadStoredInBlobContainer();
    }

    private void WhenPublicApisThrowsException()
    {
        _publicApiMock.Setup(x => x.GetAsync()).ThrowsAsync(new Exception());
    }

    private void LogStoredWithStatus(Status status)
    {
        _cloudClient.Verify(x => x.Log(new Log(status, SystemTime)), Times.Once);
    }

    private void PayloadStoredInBlobContainer()
    {
        _cloudClient.Verify(x => x.Upsert(_expectedPayload), Times.Once);
    }

    private async Task ExecuteCommand(FetchDataCommand command)
    {
        var sut = new FetchDataCommandHandler(_publicApiMock.Object, _cloudClient.Object, _systemTimeProvider.Object, _loggerMock.Object);
        await sut.Handle(command, new CancellationToken());
    }

    private FetchDataCommand GivenFetchDataCommand()
    {
        return new FetchDataCommand();
    }
}
