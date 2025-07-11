using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Enums;
using ShowTime.DataAccess.Repositories.Abstraction;
using System.Collections.Generic;

namespace ShowTime.BusinessLogic.Services;

public class TicketService : ITicketService
{
    private readonly IGenericRepository<Ticket> _ticketRepository;
    private readonly IGenericRepository<Festival> _festivalRepository;

    public TicketService(
        IGenericRepository<Ticket> ticketRepository,
        IGenericRepository<Festival> festivalRepository)
    {
        _ticketRepository = ticketRepository;
        _festivalRepository = festivalRepository;
    }

    public async Task<TicketGetDto> AddTicketAsync(TicketCreateDto ticketCreateDto)
    {
        try
        {
            if (ticketCreateDto.FestivalId <= 0)
            {
                throw new ArgumentException("Please select a valid festival.", nameof(ticketCreateDto.FestivalId));
            }

            if (!ticketCreateDto.Type.HasValue)
            {
                throw new ArgumentException("Please select a ticket type.", nameof(ticketCreateDto.Type));
            }

            var festival = await _festivalRepository.GetByIdAsync(ticketCreateDto.FestivalId);
            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {ticketCreateDto.FestivalId} not found.");
            }

            var existingTickets = await _ticketRepository.GetAllAsync();
            var existingTicket = existingTickets.FirstOrDefault(t =>
                t.FestivalId == ticketCreateDto.FestivalId &&
                t.TicketType == ticketCreateDto.Type.Value);

            if (existingTicket != null)
            {
                throw new InvalidOperationException(
                    $"A ticket of type '{ticketCreateDto.Type.Value}' already exists for festival '{festival.Name}'. " +
                    $"Each festival can only have one ticket per type.");
            }

            if (ticketCreateDto.Price <= 0)
            {
                throw new ArgumentException("Ticket price must be greater than 0.", nameof(ticketCreateDto.Price));
            }

            if (ticketCreateDto.Quantity <= 0)
            {
                throw new ArgumentException("Ticket quantity must be greater than 0.", nameof(ticketCreateDto.Quantity));
            }

            var ticket = new Ticket
            {
                FestivalId = ticketCreateDto.FestivalId,
                TicketType = ticketCreateDto.Type.Value,
                Price = ticketCreateDto.Price,
                TicketsTotal = ticketCreateDto.Quantity,
                TicketsAvailable = ticketCreateDto.Quantity,
                IsAvailable = ticketCreateDto.IsAvailable
            };

            await _ticketRepository.AddAsync(ticket);

            return new TicketGetDto
            {
                Id = ticket.Id,
                FestivalId = ticket.FestivalId,
                Type = ticket.TicketType,
                Price = ticket.Price,
                TicketsTotal = ticket.TicketsTotal,
                TicketsAvailable = ticket.TicketsAvailable,
                IsAvailable = ticket.IsAvailable
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the ticket.", ex);
        }
    }

    public async Task<TicketGetDto> GetTicketByIdAsync(int id)
    {
        try
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);

            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }

            return new TicketGetDto
            {
                Id = ticket.Id,
                FestivalId = ticket.FestivalId,
                Type = ticket.TicketType,
                Price = ticket.Price,
                TicketsTotal = ticket.TicketsTotal,
                TicketsAvailable = ticket.TicketsAvailable,
                IsAvailable = ticket.IsAvailable
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the ticket with ID {id}.", ex);
        }
    }

    public async Task<IList<TicketGetDto>> GetTicketByFestivalIdAsync(int festivalId)
    {
        try
        {
            var tickets = await _ticketRepository.GetAllAsync();
            var festivalTickets = tickets.Where(t => t.FestivalId == festivalId).ToList();

            return festivalTickets.Select(t => new TicketGetDto
            {
                Id = t.Id,
                FestivalId = t.FestivalId,
                Type = t.TicketType,
                Price = t.Price,
                TicketsTotal = t.TicketsTotal,
                TicketsAvailable = t.TicketsAvailable,
                IsAvailable = t.IsAvailable
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving tickets for festival {festivalId}.", ex);
        }
    }

    public async Task<IList<TicketGetDto>> GetAllTicketsAsync()
    {
        try
        {
            var tickets = await _ticketRepository.GetAllAsync();

            return tickets.Select(t => new TicketGetDto
            {
                Id = t.Id,
                FestivalId = t.FestivalId,
                Type = t.TicketType,
                Price = t.Price,
                TicketsTotal = t.TicketsTotal,
                TicketsAvailable = t.TicketsAvailable,
                IsAvailable = t.IsAvailable
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all tickets.", ex);
        }
    }

    public async Task<TicketGetDto> UpdateTicketAsync(TicketUpdateDto ticketUpdateDto, int id)
    {
        try
        {
            var existingTicket = await _ticketRepository.GetByIdAsync(id);

            if (existingTicket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }

            if (ticketUpdateDto.FestivalId <= 0)
            {
                throw new ArgumentException("Please select a valid festival.", nameof(ticketUpdateDto.FestivalId));
            }

            if (!ticketUpdateDto.Type.HasValue)
            {
                throw new ArgumentException("Please select a ticket type.", nameof(ticketUpdateDto.Type));
            }

            if (ticketUpdateDto.FestivalId != existingTicket.FestivalId)
            {
                var festival = await _festivalRepository.GetByIdAsync(ticketUpdateDto.FestivalId);
                if (festival == null)
                {
                    throw new KeyNotFoundException($"Festival with ID {ticketUpdateDto.FestivalId} not found.");
                }

                var existingTickets = await _ticketRepository.GetAllAsync();
                var duplicateTicket = existingTickets.FirstOrDefault(t =>
                    t.FestivalId == ticketUpdateDto.FestivalId &&
                    t.TicketType == ticketUpdateDto.Type.Value &&
                    t.Id != id);

                if (duplicateTicket != null)
                {
                    throw new InvalidOperationException(
                        $"A ticket of type '{ticketUpdateDto.Type.Value}' already exists for the selected festival. " +
                        $"Each festival can only have one ticket per type.");
                }
            }

            if (ticketUpdateDto.Price <= 0)
            {
                throw new ArgumentException("Ticket price must be greater than 0.", nameof(ticketUpdateDto.Price));
            }

            if (ticketUpdateDto.Quantity <= 0)
            {
                throw new ArgumentException("Ticket quantity must be greater than 0.", nameof(ticketUpdateDto.Quantity));
            }

            int ticketsSold = existingTicket.TicketsTotal - existingTicket.TicketsAvailable;

            if (ticketUpdateDto.Quantity < ticketsSold)
            {
                throw new InvalidOperationException(
                    $"Cannot reduce ticket quantity to {ticketUpdateDto.Quantity} because {ticketsSold} tickets have already been sold. " +
                    $"Minimum allowed quantity is {ticketsSold}.");
            }

            existingTicket.FestivalId = ticketUpdateDto.FestivalId;
            existingTicket.TicketType = ticketUpdateDto.Type.Value;
            existingTicket.Price = ticketUpdateDto.Price;
            existingTicket.TicketsTotal = ticketUpdateDto.Quantity;
            existingTicket.TicketsAvailable = ticketUpdateDto.Quantity - ticketsSold;
            existingTicket.IsAvailable = ticketUpdateDto.IsAvailable;

            await _ticketRepository.UpdateAsync(existingTicket);

            return new TicketGetDto
            {
                Id = existingTicket.Id,
                FestivalId = existingTicket.FestivalId,
                Type = existingTicket.TicketType,
                Price = existingTicket.Price,
                TicketsTotal = existingTicket.TicketsTotal,
                TicketsAvailable = existingTicket.TicketsAvailable,
                IsAvailable = existingTicket.IsAvailable
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the ticket.", ex);
        }
    }

    public async Task DeleteTicketAsync(int id)
    {
        try
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {id} not found.");
            }

            int ticketsSold = ticket.TicketsTotal - ticket.TicketsAvailable;
            if (ticketsSold > 0)
            {
                throw new InvalidOperationException(
                    $"Cannot delete ticket because {ticketsSold} tickets have already been sold. " +
                    $"You can only delete tickets that haven't been purchased yet.");
            }

            await _ticketRepository.DeleteAsync(ticket);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the ticket with ID {id}.", ex);
        }
    }
}