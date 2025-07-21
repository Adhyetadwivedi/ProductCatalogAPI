using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Interfaces;
using ProductApi.Models;

namespace ProductApi.Services
{
    public class ProductIdGenerator : IProductIdGenerator
    {
        private readonly AppDbContext _context;
        private static readonly SemaphoreSlim Semaphore = new(1, 1);

        public ProductIdGenerator(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateAsync()
        {
            await Semaphore.WaitAsync();
            try
            {
                var tracker = await _context.ProductIdTrackers.FirstOrDefaultAsync();
                if (tracker == null)
                {
                    tracker = new ProductIdTracker { LastGeneratedId = 100000 };
                    _context.ProductIdTrackers.Add(tracker);
                }
                else
                {
                    tracker.LastGeneratedId++;
                    _context.ProductIdTrackers.Update(tracker);
                }

                await _context.SaveChangesAsync();
                return tracker.LastGeneratedId.ToString();
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}
