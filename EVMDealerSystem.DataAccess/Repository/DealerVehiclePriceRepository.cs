using EVMDealerSystem.DataAccess.Models;
using EVMDealerSystem.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVMDealerSystem.DataAccess.Repository
{
    public class DealerVehiclePriceRepository : IDealerVehiclePriceRepository
{
    private readonly EVMDealerSystemContext _context;

    public DealerVehiclePriceRepository(EVMDealerSystemContext context)
    {
        _context = context;
    }

    public async Task<DealerVehiclePrice?> GetPriceByDealerAndVehicleAsync(Guid dealerId, Guid vehicleId)
    {
        return await _context.DealerVehiclePrices
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.DealerId == dealerId && p.VehicleId == vehicleId);
    }

    public async Task<IEnumerable<DealerVehiclePrice>> GetAllPricesForVehicleAsync(Guid vehicleId)
    {
        return await _context.DealerVehiclePrices
            .Where(p => p.VehicleId == vehicleId)
            .Include(p => p.Dealer) 
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<DealerVehiclePrice>> GetAllPricesByDealerAsync(Guid dealerId)
    {
        return await _context.DealerVehiclePrices
            .Where(p => p.DealerId == dealerId)
            .Include(p => p.Vehicle) 
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<DealerVehiclePrice> SetOrUpdatePriceAsync(DealerVehiclePrice newPrice)
    {
        var existingPrice = await _context.DealerVehiclePrices
            .FindAsync(newPrice.DealerId, newPrice.VehicleId); 

        if (existingPrice == null)
        {
            newPrice.CreatedAt = DateTime.UtcNow;
            _context.DealerVehiclePrices.Add(newPrice);
        }
        else
        {
            existingPrice.SellingPrice = newPrice.SellingPrice;
            existingPrice.UpdatedAt = DateTime.UtcNow;
            _context.DealerVehiclePrices.Update(existingPrice);
            newPrice = existingPrice; 
        }

        await _context.SaveChangesAsync();
        return newPrice;
    }

    public async Task<bool> DeletePriceAsync(Guid dealerId, Guid vehicleId)
    {
        var price = await _context.DealerVehiclePrices.FindAsync(dealerId, vehicleId);
        if (price == null)
            return false;

        _context.DealerVehiclePrices.Remove(price);
        await _context.SaveChangesAsync();
        return true;
    }
}
}
