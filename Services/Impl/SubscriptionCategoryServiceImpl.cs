using System.Collections.Generic;
using System.Linq;
using StreamingSubscriptionTrackerAPI.DTOs;
using StreamingSubscriptionTrackerAPI.Models;
using StreamingSubscriptionTrackerAPI.Models.Context;

namespace StreamingSubscriptionTrackerAPI.Services.Impl
{
    public class SubscriptionCategoryServiceImpl : ISubscriptionCategoryService
    {
        private MSSQLContext _context;

        public SubscriptionCategoryServiceImpl(MSSQLContext context)
        {
            _context = context;
        }

        //GET
        static private SubscriptionCategoryResponseDTO ToResponseDTO(SubscriptionCategory subscriptionCategory) => new SubscriptionCategoryResponseDTO
        {
            Id = subscriptionCategory.Id,
            Name = subscriptionCategory.Name
        };

        public SubscriptionCategoryResponseDTO GetById(long id)
        {
            var subscriptionCategory = _context.SubscriptionCategories.FirstOrDefault(sc => sc.Id == id);

            if(subscriptionCategory == null)
                throw new ArgumentException("Subscription category not found");

            return ToResponseDTO(subscriptionCategory);
        }

        public SubscriptionCategoryResponseDTO GetByName(string name)
        {
            var subscriptionCategory = _context.SubscriptionCategories.FirstOrDefault(sc => sc.Name == name);

            if(subscriptionCategory == null)
                throw new ArgumentException("Subscription category not found");

            return ToResponseDTO(subscriptionCategory);
        }

        public List<SubscriptionCategoryResponseDTO> GetAll()
        {
            return _context.SubscriptionCategories
                .Select(sc => ToResponseDTO(sc))
                .ToList();
        }

        public bool ExistsByName(string name) =>
            _context.SubscriptionCategories.Any(sc => sc.Name == name);

        //POST
        public SubscriptionCategoryResponseDTO Create(SubscriptionCategoryRequestDTO dto, long userId)
        {
            var subscriptionCategory = new SubscriptionCategory
            {
                Name = dto.Name,
                IdUser = userId
            };

            _context.SubscriptionCategories.Add(subscriptionCategory);
            _context.SaveChanges();

            return ToResponseDTO(subscriptionCategory);
        }

        //PUT
        public SubscriptionCategoryResponseDTO Update(long id, SubscriptionCategoryRequestDTO dto)
        {
            var existingSubscriptionCategory = _context.SubscriptionCategories.Find(id);

            if (existingSubscriptionCategory == null)
                throw new ArgumentException("Subscription category not found");

            existingSubscriptionCategory.Name = dto.Name;

            _context.SaveChanges();

            return ToResponseDTO(existingSubscriptionCategory);
        }

        //DELETE
        public void Delete(long id)
        {
            var existingSubscriptionCategory = _context.SubscriptionCategories.Find(id);

            if (existingSubscriptionCategory == null)
                throw new ArgumentException("Subscription category not found");

            _context.SubscriptionCategories.Remove(existingSubscriptionCategory);
            _context.SaveChanges();
        }
    }
}
