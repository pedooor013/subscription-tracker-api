using StreamingSubscriptionTrackerAPI.DTOs;

namespace StreamingSubscriptionTrackerAPI.Services
{
    public interface IUserService
    {
        //GET
        List<UserResponseDTO> GetAll();
        UserResponseDTO GetByUsername(string username);
        UserResponseDTO GetByEmail(string email);
        UserResponseDTO GetById(long id);
        List<UserResponseDTO> GetByActived(bool actived);

        //POST
        UserResponseDTO Create(UserRequestDTO userDto);
        UserLoginResponseDTO Login(string usernameDto, string passwordDto);

        //PUT
        UserResponseDTO Update(long id, UserRequestDTO userDto, long? filterByUserId);
        UserResponseDTO UpdateActived(long id, bool actived, long? filterByUserId);
        UserResponseDTO UpdatePassword(long id, string password, long? filterByUserId);

        //DELETE
        UserResponseDTO Delete(long id, long? filterByUserId);
    }
}
