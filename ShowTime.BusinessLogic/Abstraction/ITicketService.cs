using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Abstraction;

public interface ITicketService
{
    Task<TicketGetDto> GetTicketByIdAsync(int id);
    Task<IList<TicketGetDto>> GetTicketByFestivalIdAsync(int festivalId);
    Task<IList<TicketGetDto>> GetAllTicketsAsync();
    Task DeleteTicketAsync(int id);
    Task<TicketGetDto> UpdateTicketAsync(TicketUpdateDto ticketUpdateDto, int id);
    Task<TicketGetDto> AddTicketAsync(TicketCreateDto ticketCreateDto);
}
