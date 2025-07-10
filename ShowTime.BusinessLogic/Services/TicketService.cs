using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Extras;
using ShowTime.DataAccess.Repositories.Abstraction;

namespace ShowTime.BusinessLogic.Services;

public class TicketService : ITicketService
{
    private readonly IGenericRepository<Ticket> _ticketRepository;
    private readonly IGenericRepository<Festival> _festivalRepository;
    private readonly IGenericRepository<Booking> _bookingRepository;

    public TicketService(
        IGenericRepository<Ticket> ticketRepository,
        IGenericRepository<Festival> festivalRepository,
        IGenericRepository<Booking> bookingRepository)
    {
        _ticketRepository = ticketRepository;
        _festivalRepository = festivalRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<TicketGetDto> GetTicketByIdAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);

        var availableQuantity = await GetAvailableTicketQuantityAsync(id);
        return MapToTicketGetDto(ticket, availableQuantity);
    }

    public async Task<IList<TicketGetDto>> GetTicketByFestivalIdAsync(int festivalId)
    {
        await _festivalRepository.GetByIdAsync(festivalId);

        var tickets = await _ticketRepository.GetAllAsync();
        var festivalTickets = tickets.Where(t => t.FestivalId == festivalId).ToList();

        var ticketDtos = new List<TicketGetDto>();
        foreach (var ticket in festivalTickets)
        {
            var availableQuantity = await GetAvailableTicketQuantityAsync(ticket.Id);
            ticketDtos.Add(MapToTicketGetDto(ticket, availableQuantity));
        }

        return ticketDtos.OrderBy(t => t.Price).ToList();
    }

    public async Task<IList<TicketGetDto>> GetAllTicketsAsync()
    {
        var tickets = await _ticketRepository.GetAllAsync();
        var ticketDtos = new List<TicketGetDto>();

        foreach (var ticket in tickets)
        {
            var availableQuantity = await GetAvailableTicketQuantityAsync(ticket.Id);
            ticketDtos.Add(MapToTicketGetDto(ticket, availableQuantity));
        }

        return ticketDtos
            .OrderBy(t => t.FestivalId)
            .ThenBy(t => t.Price)
            .ToList();
    }

    public async Task AddTicketAsync(TicketCreateDto ticketCreateDto)
    {
        var festival = await _festivalRepository.GetByIdAsync(ticketCreateDto.FestivalId)
            ?? throw new KeyNotFoundException($"Festival with ID {ticketCreateDto.FestivalId} not found.");

        var ticket = new Ticket
        {
            FestivalId = ticketCreateDto.FestivalId,
            TicketType = ticketCreateDto.Type,
            Price = ticketCreateDto.Price,
            TicketsTotal = ticketCreateDto.Quantity,
            IsAvailable = ticketCreateDto.IsAvailable
        };

        await _ticketRepository.AddAsync(ticket);
    }

    public async Task DeleteTicketAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Ticket with ID {id} not found.");

        var bookings = await _bookingRepository.GetAllAsync();
        var hasConfirmedBookings = bookings.Any(b =>
            b.TicketId == id && b.BookingStatus == Status.Done);

        if (hasConfirmedBookings)
        {
            throw new InvalidOperationException("Cannot delete ticket with confirmed bookings.");
        }

        await _ticketRepository.DeleteAsync(ticket);
    }

    public async Task UpdateTicketAsync(TicketUpdateDto ticketUpdateDto, int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Ticket with ID {id} not found.");

        await _festivalRepository.GetByIdAsync(ticketUpdateDto.FestivalId);

        ticket.FestivalId = ticketUpdateDto.FestivalId;
        ticket.TicketType = ticketUpdateDto.Type;
        ticket.Price = ticketUpdateDto.Price;
        ticket.TicketsTotal = ticketUpdateDto.Quantity;
        ticket.IsAvailable = ticketUpdateDto.IsAvailable;

        await _ticketRepository.UpdateAsync(ticket);
    }

    private async Task<int> GetAvailableTicketQuantityAsync(int ticketId)
    {
        var bookings = await _bookingRepository.GetAllAsync();
        var bookedQuantity = bookings
            .Where(b => b.TicketId == ticketId &&
                       (b.BookingStatus == Status.Done || b.BookingStatus == Status.Pending))
            .Sum(b => b.Quantity);

        var ticket = await _ticketRepository.GetByIdAsync(ticketId);
        return (ticket?.TicketsTotal ?? 0) - bookedQuantity;
    }

    private static TicketGetDto MapToTicketGetDto(Ticket ticket, int availableQuantity)
    {
        return new TicketGetDto
        {
            Id = ticket.Id,
            FestivalId = ticket.FestivalId,
            Type = ticket.TicketType,
            Price = ticket.Price,
            TicketsTotal = ticket.TicketsTotal,
            AvailableQuantity = availableQuantity,
            IsAvailable = ticket.IsAvailable && availableQuantity > 0
        };
    }
}