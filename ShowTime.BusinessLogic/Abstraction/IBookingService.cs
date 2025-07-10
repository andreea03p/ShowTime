using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Abstraction;

public interface IBookingService
{
    Task<BookingCreateDto> CreateBookingAsync(BookingCreateDto bookingCreateDto);
    Task<IList<BookingGetDto>> GetBookingsByUserIdAsync(int userId);
    Task<IList<BookingGetDto>> GetBookingsByFestivalIdAsync(int festivalId);
    Task<IEnumerable<BookingGetDto>> GetAllBookingsAsync();

    //Task DeleteBookingAsync(int bookingId);
}
