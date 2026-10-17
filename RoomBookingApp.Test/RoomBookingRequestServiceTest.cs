
using Moq;

namespace RoomBookingApp.Core;

public class RoomBookingRequestServiceTest
{
    private readonly RoomBookingService _service;
    private readonly RoomBookingRequest _request;

    private readonly Mock<IRoomBookingRepository> _roomBookingRepositoryMock;
    private List<Room> _availableRooms;

    public RoomBookingRequestServiceTest()
    {
        // arrange
        _request = new RoomBookingRequest()
        {
            FullName = "John Doe",
            Email = "John.Doe@gmail.com",
            Date = new DateTime(2021, 10, 20)
        };
        _availableRooms = new List<Room>() { new Room() { Id = 1 } };

        _roomBookingRepositoryMock = new Mock<IRoomBookingRepository>();

        _roomBookingRepositoryMock.Setup(q => q.GetAvaliableRooms(_request.Date))
            .Returns(_availableRooms);

        _service = new RoomBookingService(_roomBookingRepositoryMock.Object);

    }


    [Fact]
    public void should_return_room_booking_request()
    {

        var result = _service.BookRoom(_request);

        // assert
        Assert.NotNull(result);
        Assert.Equal(_request.FullName, result.FullName);
        Assert.Equal(_request.Email, result.Email);
        Assert.Equal(_request.Date, result.Date);
    }


    [Fact]
    public void should_throw_exception_for_null_request()
    {

        var exception = Assert.Throws<ArgumentNullException>(() => _service.BookRoom(null));

        Assert.Equal("request", exception.ParamName);
    }

    [Fact]
    public void should_save_room_booking_request()
    {
        RoomBookingBase? savedBooking = null;

        _roomBookingRepositoryMock
            .Setup(x => x.Save(It.IsAny<RoomBooking>()))
            .Callback<RoomBookingBase>(booking =>
            {
                savedBooking = booking;
            });

        _service.BookRoom(_request);

        _roomBookingRepositoryMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Once);

        Assert.NotNull(savedBooking);
        Assert.Equal(_request.FullName, savedBooking.FullName);
        Assert.Equal(_request.Email, savedBooking.Email);
        Assert.Equal(_request.Date, savedBooking.Date);
        Assert.NotNull(_availableRooms.First());
        Assert.Equal(1, _availableRooms.First().Id);

    }


    [Fact]
    public void should_not_save_room_booking_request_if_none_available()
    {
        _availableRooms.Clear();

        _service.BookRoom(_request);

        _roomBookingRepositoryMock.Verify(
            q => q.Save(It.IsAny<RoomBooking>()),
            Times.Never()
        );
    }

    [Theory]
    [InlineData(BookingSucessflag.Failure, false)]
    [InlineData(BookingSucessflag.Sucess, true)]
    public void should_return_sucess_or_fail_flag_in_result(BookingSucessflag bookingSucessflag, bool isAvailable)
    {
        if (!isAvailable)
        {
            _availableRooms.Clear();
        }


        var result = _service.BookRoom(_request);

        Assert.Equal(result.Flag, bookingSucessflag);
    }


    [Theory]
    [InlineData(1, true)]
    [InlineData(null, false)]
    public void should_return_roomBookingId_in_result(int? roomBookingId, bool isAvailable)
    {
        if (!isAvailable)
        {
            _availableRooms.Clear();
        }
        else
        {
            _roomBookingRepositoryMock
                .Setup(x => x.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    booking.Id = roomBookingId!.Value;
                    booking.RoomBookingId = roomBookingId!.Value;
                });
        }

        var result = _service.BookRoom(_request);

        Assert.Equal(roomBookingId, result.RoomBookingId);
    }
}
