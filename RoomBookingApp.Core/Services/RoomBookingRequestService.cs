namespace RoomBookingApp.Core;
public class RoomBookingRequestService
{
    public RoomBookingResponse BookRoom(RoomBookingRequest request)
    {
        return new RoomBookingResponse()
        {
            FullName = request.FullName,
            Email    = request.Email,
            Date     = request.Date
        };
    }
}
