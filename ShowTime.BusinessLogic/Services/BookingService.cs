using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Extras;
using ShowTime.DataAccess.Repositories.Abstraction;

namespace ShowTime.BusinessLogic.Services;

public class BookingService : IBookingService
{
    private readonly IGenericRepository<Booking> _bookingRepository;
    private readonly IGenericRepository<Ticket> _ticketRepository;
    private readonly IGenericRepository<Festival> _festivalRepository;
    private readonly IGenericRepository<User> _userRepository;

    public BookingService(
        IGenericRepository<Booking> bookingRepository,
        IGenericRepository<Ticket> ticketRepository,
        IGenericRepository<Festival> festivalRepository,
        IGenericRepository<User> userRepository)
    {
        _bookingRepository = bookingRepository;
        _ticketRepository = ticketRepository;
        _festivalRepository = festivalRepository;
        _userRepository = userRepository;
    }

    public async Task<BookingCreateDto> CreateBookingAsync(BookingCreateDto bookingCreateDto)
    {
        try
        {
            if (bookingCreateDto == null)
            {
                throw new ArgumentNullException(nameof(bookingCreateDto), "Booking data is required.");
            }

            if (bookingCreateDto.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0.", nameof(bookingCreateDto.Quantity));
            }

            var ticket = await _ticketRepository.GetByIdAsync(bookingCreateDto.TicketId);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {bookingCreateDto.TicketId} not found.");
            }

            var user = await _userRepository.GetByIdAsync(bookingCreateDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {bookingCreateDto.UserId} not found.");
            }

            if (!ticket.IsAvailable)
            {
                throw new InvalidOperationException("This ticket is not available for booking.");
            }

            if (bookingCreateDto.Quantity > ticket.TicketsAvailable)
            {
                throw new InvalidOperationException($"Not enough tickets available. Available: {ticket.TicketsAvailable}, Requested: {bookingCreateDto.Quantity}");
            }

            var booking = new Booking
            {
                TicketId = bookingCreateDto.TicketId,  
                UserId = bookingCreateDto.UserId,
                Quantity = bookingCreateDto.Quantity,
                BookingStatus = Status.Done
            };

            ticket.TicketsAvailable -= bookingCreateDto.Quantity;
            await _ticketRepository.UpdateAsync(ticket);

            await _bookingRepository.AddAsync(booking);

            return bookingCreateDto;
        }
        catch (Exception ex)
        {
            var innerMessage = ex.InnerException?.Message ?? "No inner exception";
            throw new Exception($"Booking creation failed. Error: {ex.Message}. Inner: {innerMessage}", ex);
        }
    }

    public async Task<IList<BookingGetDto>> GetBookingsByUserIdAsync(int userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var allBookings = await _bookingRepository.GetAllAsync();
            var allTickets = await _ticketRepository.GetAllAsync();
            var allFestivals = await _festivalRepository.GetAllAsync();

            var userBookings = allBookings.Where(b => b.UserId == userId).ToList();

            var bookingDtos = new List<BookingGetDto>();

            foreach (var booking in userBookings)
            {
                var ticket = allTickets.FirstOrDefault(t => t.Id == booking.TicketId);
                var festival = ticket != null ? allFestivals.FirstOrDefault(f => f.Id == ticket.FestivalId) : null;

                bookingDtos.Add(new BookingGetDto
                {
                    Id = booking.Id,
                    TicketId = booking.TicketId,
                    UserId = booking.UserId,
                    Quantity = booking.Quantity,
                    UserName = user.Username ?? string.Empty,
                    UserEmail = user.Email ?? string.Empty,
                    FestivalName = festival?.Name ?? "Unknown Festival",
                    TicketType = ticket?.TicketType.ToString() ?? "Unknown Ticket",
                    TicketPrice = ticket?.Price ?? 0,
                    FestivalDate = festival?.StartDate ?? DateTime.Now
                });
            }

            return bookingDtos.OrderByDescending(b => b.Id).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving bookings for user {userId}: {ex.Message}", ex);
        }
    }

    public async Task<IList<BookingGetDto>> GetBookingsByFestivalIdAsync(int festivalId)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(festivalId);
            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {festivalId} not found.");
            }

            var allBookings = await _bookingRepository.GetAllAsync();
            var allTickets = await _ticketRepository.GetAllAsync();
            var allUsers = await _userRepository.GetAllAsync();

            var festivalTickets = allTickets.Where(t => t.FestivalId == festivalId).ToList();
            var festivalTicketIds = festivalTickets.Select(t => t.Id).ToHashSet();

            var festivalBookings = allBookings.Where(b => festivalTicketIds.Contains(b.TicketId)).ToList();

            var bookingDtos = new List<BookingGetDto>();

            foreach (var booking in festivalBookings)
            {
                var ticket = allTickets.FirstOrDefault(t => t.Id == booking.TicketId);
                var user = allUsers.FirstOrDefault(u => u.Id == booking.UserId);

                bookingDtos.Add(new BookingGetDto
                {
                    Id = booking.Id,
                    TicketId = booking.TicketId,
                    UserId = booking.UserId,
                    Quantity = booking.Quantity,
                    UserName = user?.Username ?? "Unknown User",
                    UserEmail = user?.Email ?? string.Empty,
                    FestivalName = festival.Name,
                    TicketType = ticket?.TicketType.ToString() ?? "Unknown Ticket",
                    TicketPrice = ticket?.Price ?? 0,
                    FestivalDate = festival.StartDate
                });
            }

            return bookingDtos.OrderByDescending(b => b.Id).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving bookings for festival {festivalId}: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<BookingGetDto>> GetAllBookingsAsync()
    {
        try
        {
            var allBookings = await _bookingRepository.GetAllAsync();
            var allTickets = await _ticketRepository.GetAllAsync();
            var allFestivals = await _festivalRepository.GetAllAsync();
            var allUsers = await _userRepository.GetAllAsync();

            var bookingDtos = new List<BookingGetDto>();

            foreach (var booking in allBookings)
            {
                var ticket = allTickets.FirstOrDefault(t => t.Id == booking.TicketId);
                var festival = ticket != null ? allFestivals.FirstOrDefault(f => f.Id == ticket.FestivalId) : null;
                var user = allUsers.FirstOrDefault(u => u.Id == booking.UserId);

                bookingDtos.Add(new BookingGetDto
                {
                    Id = booking.Id,
                    TicketId = booking.TicketId,
                    UserId = booking.UserId,
                    Quantity = booking.Quantity,
                    UserName = user?.Username ?? "Unknown User",
                    UserEmail = user?.Email ?? string.Empty,
                    FestivalName = festival?.Name ?? "Unknown Festival",
                    TicketType = ticket?.TicketType.ToString() ?? "Unknown Ticket",
                    TicketPrice = ticket?.Price ?? 0,
                    FestivalDate = festival?.StartDate ?? DateTime.Now
                });
            }

            return bookingDtos.OrderByDescending(b => b.Id).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving all bookings: {ex.Message}", ex);
        }
    }
}