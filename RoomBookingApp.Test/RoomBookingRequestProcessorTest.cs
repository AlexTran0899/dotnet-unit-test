
using Moq;

namespace RoomBookingApp.Core;


public class RoomBookingRequestProcessorTest
{
    private readonly RoomBookingRequestService _processor;
    private readonly RoomBookingRequest _request;

    private readonly Mock<IRoomBookingRepository> _roomBookingRepositoryMock;

    public RoomBookingRequestProcessorTest()
    {
        // arrange
        _request = new RoomBookingRequest()
        {
            FullName = "John Doe",
            Email = "John.Doe@gmail.com",
            Date = new DateTime(2021, 10, 20)
        };

        _roomBookingRepositoryMock = new Mock<IRoomBookingRepository>();

        _processor = new RoomBookingRequestService(_roomBookingRepositoryMock.Object);


    }


    [Fact]
    public void should_return_room_booking_request()
    {

        var result = _processor.BookRoom(_request);

        // assert
        Assert.NotNull(result);
        Assert.Equal(_request.FullName, result.FullName);
        Assert.Equal(_request.Email, result.Email);
        Assert.Equal(_request.Date, result.Date);
    }


    [Fact]
    public void should_throw_exception_for_null_request()
    {

        var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookRoom(null));

        Assert.Equal("request", exception.ParamName);
    }

    [Fact]
    public void ShouldSaveRoomBookingRequest()
    {
        RoomBooking? savedBooking = null;

        _roomBookingRepositoryMock
            .Setup(x => x.Save(It.IsAny<RoomBooking>()))
            .Callback<RoomBooking>(booking =>
            {
                savedBooking = booking;
            });

        _processor.BookRoom(_request);

        _roomBookingRepositoryMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Once);

        Assert.NotNull(savedBooking);
        Assert.Equal(_request.FullName, savedBooking.FullName);
        Assert.Equal(_request.Email, savedBooking.Email);
        Assert.Equal(_request.Date, savedBooking.Date);

    }
}
