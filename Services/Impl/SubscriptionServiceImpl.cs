using Microsoft.EntityFrameworkCore;
using StreamingSubscriptionTrackerAPI.DTOs;
using StreamingSubscriptionTrackerAPI.Models;
using StreamingSubscriptionTrackerAPI.Models.Context;
using System.Linq;

namespace StreamingSubscriptionTrackerAPI.Services.Impl
{
    public class SubscriptionServiceImpl : ISubscriptionService
    {
        private MSSQLContext _context;

        public SubscriptionServiceImpl(MSSQLContext context)
        {
            _context = context;
        }

        //GET
        public List<SubscriptionResponseDTO> GetAll() =>
            _context.Subscriptions
                .Include(s => s.Category)
                .Select(s => ToResponseDTO(s))
                .ToList();

        public SubscriptionResponseDTO GetById(long id)
        {
            var subscription = _context.Subscriptions
                .Include(s => s.Category)
                .FirstOrDefault(s => s.Id == id);

            if (subscription == null) throw new ArgumentException($"Subscription with id {id} not found.");

            return ToResponseDTO(subscription);
        }

        public List<SubscriptionResponseDTO> GetSubscriptionFromCategory(long idCategory)
        {
            var subscriptions = _context.Subscriptions
                .Include(s => s.Category)
                .Where(s => s.IdCategory == idCategory)
                .ToList();

            return subscriptions.Select(ToResponseDTO).ToList();
        }

        //POST
        public SubscriptionResponseDTO Create(SubscriptionRequestDTO dto, long userId)
        {
            var subscription = new Subscription
            {
                Name = dto.Name,
                Price = dto.Price,
                DateToPaid = dto.DateToPaid,
                IdCategory = dto.IdCategory,
                IdUser = userId
            };
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();

            // recarrega com a Category incluída antes de mapear
            _context.Entry(subscription).Reference(s => s.Category).Load();

            return ToResponseDTO(subscription);
        }

        //PUT
        public SubscriptionResponseDTO Update(long id, SubscriptionRequestDTO dto)
        {
            var existingSubscription = _context.Subscriptions
                .Include(s => s.Category)
                .FirstOrDefault(s => s.Id == id);

            if (existingSubscription == null) throw new ArgumentException($"Subscription with id {id} not found.");

            existingSubscription.Name = dto.Name;
            existingSubscription.Price = dto.Price;
            existingSubscription.DateToPaid = dto.DateToPaid;
            existingSubscription.IdCategory = dto.IdCategory;

            _context.SaveChanges();

            // recarrega a Category caso o IdCategory tenha mudado
            _context.Entry(existingSubscription).Reference(s => s.Category).Load();

            return ToResponseDTO(existingSubscription);
        }

        //DELETE
        public void Delete(long id)
        {
            var existingSubscription = _context.Subscriptions.Find(id);

            if (existingSubscription == null) throw new ArgumentException($"Subscription with id {id} not found.");

            _context.Subscriptions.Remove(existingSubscription);
            _context.SaveChanges();
        }

        //DTO Utils
        private static SubscriptionResponseDTO ToResponseDTO(Subscription subscription) => new SubscriptionResponseDTO
        {
            Id = subscription.Id,
            Name = subscription.Name,
            Price = subscription.Price,
            DateToPaid = subscription.DateToPaid,
            IdCategory = subscription.IdCategory,
            CategoryName = subscription.Category?.Name
        };
    }
}