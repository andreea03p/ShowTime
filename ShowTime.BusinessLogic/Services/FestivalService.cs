using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Extras;
using ShowTime.DataAccess.Repositories.Abstraction;
using System.Collections.Generic;

namespace ShowTime.BusinessLogic.Services;

public class FestivalService : IFestivalService
{
    private readonly IGenericRepository<Festival> _festivalRepository;

    public FestivalService(IGenericRepository<Festival> festivalRepository)
    {
        _festivalRepository = festivalRepository;
    }

    public async Task AddFestivalAsync(FestivalCreateDto festivalCreateDto)
    {
        try
        {
            var festival = new Festival
            {
                Name = festivalCreateDto.Name,
                Location = new Location
                {
                    Address = festivalCreateDto.Address,
                    Latitude = festivalCreateDto.Latitude,
                    Longitude = festivalCreateDto.Longitude
                },
                StartDate = festivalCreateDto.StartDate,
                EndDate = festivalCreateDto.EndDate,
                SplashArt = festivalCreateDto.SplashArt,
                Capacity = festivalCreateDto.Capacity
            };

            await _festivalRepository.AddAsync(festival);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while adding the festival.", ex);
        }
    }

    public async Task<IList<FestivalGetDto>> GetAllFestivalsAsync()
    {
        try
        {
            var festivals = await _festivalRepository.GetAllAsync();

            return festivals.Select(f => new FestivalGetDto
            {
                Id = f.Id,
                Name = f.Name,
                Address = f.Location.Address,
                Latitude = f.Location.Latitude,
                Longitude = f.Location.Longitude,
                StartDate = f.StartDate,
                EndDate = f.EndDate,
                SplashArt = f.SplashArt,
                Capacity = f.Capacity
            }).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving all festivals.", ex);
        }
    }

    public async Task<FestivalGetDto?> GetFestivalByIdAsync(int id)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(id);

            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }

            return new FestivalGetDto
            {
                Id = festival.Id,
                Name = festival.Name,
                Address = festival.Location.Address,
                Latitude = festival.Location.Latitude,
                Longitude = festival.Location.Longitude,
                StartDate = festival.StartDate,
                EndDate = festival.EndDate,
                SplashArt = festival.SplashArt,
                Capacity = festival.Capacity
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while retrieving the festival with ID {id}.", ex);
        }
    }

    public async Task UpdateFestivalAsync(FestivalUpdateDto updateDto, int id)
    {
        try
        {
            var existingFestival = await _festivalRepository.GetByIdAsync(id);

            if (existingFestival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }

            existingFestival.Name = updateDto.Name;
            existingFestival.Location.Address = updateDto.Address;
            existingFestival.Location.Latitude = updateDto.Latitude;
            existingFestival.Location.Longitude = updateDto.Longitude;
            existingFestival.StartDate = updateDto.StartDate;
            existingFestival.EndDate = updateDto.EndDate;
            existingFestival.SplashArt = updateDto.SplashArt;
            existingFestival.Capacity = updateDto.Capacity;

            await _festivalRepository.UpdateAsync(existingFestival);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the festival.", ex);
        }
    }

    public async Task DeleteFestivalAsync(int id)
    {
        try
        {
            var festival = await _festivalRepository.GetByIdAsync(id);
            if (festival == null)
            {
                throw new KeyNotFoundException($"Festival with ID {id} not found.");
            }
            await _festivalRepository.DeleteAsync(festival);
        }
        catch (Exception ex)
        {
            throw new Exception($"An error occurred while deleting the festival with ID {id}.", ex);
        }
    }
}