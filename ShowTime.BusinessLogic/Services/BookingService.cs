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
        ArgumentNullException.ThrowIfNull(bookingCreateDto);

        if (bookingCreateDto.Quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than 0.", nameof(bookingCreateDto.Quantity));
        }

        var ticket = await _ticketRepository.GetByIdAsync(bookingCreateDto.TicketId);
        if (ticket == null)
        {
            throw new KeyNotFoundException($"Ticket with ID {bookingCreateDto.TicketId} not found.");
        }

        // Validate user exists - DO NOT load navigation properties
        var user = await _userRepository.GetByIdAsync(bookingCreateDto.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {bookingCreateDto.UserId} not found.");
        }

        if (!ticket.IsAvailable)
        {
            throw new InvalidOperationException("This ticket is not available for booking.");
        }

        var availableQuantity = await GetAvailableTicketQuantityAsync(ticket.Id);
        if (bookingCreateDto.Quantity > availableQuantity)
        {
            throw new InvalidOperationException($"Not enough tickets available. Available: {availableQuantity}, Requested: {bookingCreateDto.Quantity}");
        }


        var booking = new Booking
        {
            TicketId = bookingCreateDto.TicketId,
            UserId = bookingCreateDto.UserId,
            Quantity = bookingCreateDto.Quantity,
            BookingStatus = Status.Done
        };

        try
        {
            await _bookingRepository.AddAsync(booking);
            Console.WriteLine($"Successfully created booking - TicketId: {booking.TicketId}, UserId: {booking.UserId}, Quantity: {booking.Quantity}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating booking: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            throw;
        }

        return bookingCreateDto;
    }

    public async Task<IList<BookingGetDto>> GetBookingsByUserIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

        var bookings = await _bookingRepository.GetAllAsync();
        var tickets = await _ticketRepository.GetAllAsync();
        var festivals = await _festivalRepository.GetAllAsync();

        var userBookings = bookings.Where(b => b.UserId == userId);

        return userBookings
            .Select(booking => MapToBookingGetDto(booking, tickets, festivals, user))
            .OrderByDescending(b => b.Id)
            .ToList();
    }

    public async Task<IList<BookingGetDto>> GetBookingsByFestivalIdAsync(int festivalId)
    {
        var festival = await _festivalRepository.GetByIdAsync(festivalId)
            ?? throw new KeyNotFoundException($"Festival with ID {festivalId} not found.");

        var bookings = await _bookingRepository.GetAllAsync();
        var tickets = await _ticketRepository.GetAllAsync();
        var users = await _userRepository.GetAllAsync();

        var festivalTicketIds = tickets.Where(t => t.FestivalId == festivalId).Select(t => t.Id);
        var festivalBookings = bookings.Where(b => festivalTicketIds.Contains(b.TicketId));

        return festivalBookings
            .Select(booking => MapToBookingGetDto(booking, tickets, new[] { festival }, users))
            .OrderByDescending(b => b.Id)
            .ToList();
    }

    public async Task<IEnumerable<BookingGetDto>> GetAllBookingsAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        var tickets = await _ticketRepository.GetAllAsync();
        var festivals = await _festivalRepository.GetAllAsync();
        var users = await _userRepository.GetAllAsync();

        return bookings
            .Select(booking => MapToBookingGetDto(booking, tickets, festivals, users))
            .OrderByDescending(b => b.Id)
            .ToList();
    }

    public async Task DeleteBookingAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId)
            ?? throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

        if (booking.BookingStatus == Status.Done)
        {
            throw new InvalidOperationException("Cannot delete a confirmed booking. Please cancel it first.");
        }

        await _bookingRepository.DeleteAsync(booking);
    }

    public async Task UpdateBookingStatusAsync(int bookingId, Status newStatus)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId)
            ?? throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");

        if (booking.BookingStatus == Status.Done && newStatus == Status.Pending)
        {
            throw new InvalidOperationException("Cannot change confirmed booking back to pending.");
        }

        if (newStatus == Status.Done && booking.BookingStatus == Status.Pending)
        {
            var availableQuantity = await GetAvailableTicketQuantityAsync(booking.TicketId, bookingId);
            if (booking.Quantity > availableQuantity)
            {
                throw new InvalidOperationException($"Cannot confirm booking. Not enough tickets available. Available: {availableQuantity}, Required: {booking.Quantity}");
            }
        }

        booking.BookingStatus = newStatus;
        await _bookingRepository.UpdateAsync(booking);
    }

    public async Task UpdateBookingAsync(BookingUpdateDto bookingUpdateDto, int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Booking with ID {id} not found.");

        if (booking.BookingStatus == Status.Done && bookingUpdateDto.BookingStatus == Status.Pending)
        {
            throw new InvalidOperationException("Cannot change confirmed booking back to pending.");
        }

        if (booking.Quantity != bookingUpdateDto.Quantity)
        {
            var availableQuantity = await GetAvailableTicketQuantityAsync(booking.TicketId, id);
            if (bookingUpdateDto.Quantity > availableQuantity)
            {
                throw new InvalidOperationException($"Cannot update booking. Not enough tickets available. Available: {availableQuantity}, Requested: {bookingUpdateDto.Quantity}");
            }
        }

        booking.Quantity = bookingUpdateDto.Quantity;
        booking.BookingStatus = bookingUpdateDto.BookingStatus;

        await _bookingRepository.UpdateAsync(booking);
    }

    private async Task<int> GetAvailableTicketQuantityAsync(int ticketId, int excludeBookingId = 0)
    {
        var ticket = await _ticketRepository.GetByIdAsync(ticketId)
            ?? throw new KeyNotFoundException($"Ticket with ID {ticketId} not found.");

        var bookings = await _bookingRepository.GetAllAsync();
        var bookedQuantity = bookings
            .Where(b => b.TicketId == ticketId &&
                       b.Id != excludeBookingId &&
                       (b.BookingStatus == Status.Done || b.BookingStatus == Status.Pending))
            .Sum(b => b.Quantity);

        return ticket.TicketsTotal - bookedQuantity;
    }

    private static BookingGetDto MapToBookingGetDto(
        Booking booking,
        IEnumerable<Ticket> tickets,
        IEnumerable<Festival> festivals,
        IEnumerable<User> users)
    {
        var ticket = tickets.FirstOrDefault(t => t.Id == booking.TicketId);
        var festival = ticket != null ? festivals.FirstOrDefault(f => f.Id == ticket.FestivalId) : null;
        var user = users.FirstOrDefault(u => u.Id == booking.UserId);

        return new BookingGetDto
        {
            Id = booking.Id,
            TicketId = booking.TicketId,
            UserId = booking.UserId,
            Quantity = booking.Quantity,
            BookingStatus = booking.BookingStatus,
            UserName = user?.Username ?? string.Empty,
            UserEmail = user?.Email ?? string.Empty,
            FestivalName = festival?.Name ?? string.Empty,
            TicketType = ticket?.TicketType.ToString() ?? string.Empty,
            TicketPrice = ticket?.Price ?? 0,
            TotalAmount = (ticket?.Price ?? 0) * booking.Quantity,
            FestivalDate = festival?.StartDate ?? DateTime.Now
        };
    }

    private static BookingGetDto MapToBookingGetDto(
        Booking booking,
        IEnumerable<Ticket> tickets,
        IEnumerable<Festival> festivals,
        User user)
    {
        return MapToBookingGetDto(booking, tickets, festivals, new[] { user });
    }
}