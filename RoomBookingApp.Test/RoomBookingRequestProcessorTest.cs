
namespace RoomBookingApp.Core;


public class RoomBookingRequestProcessorTest
{
    [Fact]
    public void should_return_room_booking_request()
    {


        // arrange
        var request = new RoomBookingRequest()
        {
            FullName = "John Doe",
            Email = "John.Doe@gmail.com",
            Date = new DateTime(2021, 10, 20)
        };

        var processor = new RoomBookingRequestService();

        // act
        var result = processor.BookRoom(request);

        // assert
        Assert.NotNull(result);
        Assert.Equal(request.FullName, result.FullName);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(request.Date, result.Date);




    }
}